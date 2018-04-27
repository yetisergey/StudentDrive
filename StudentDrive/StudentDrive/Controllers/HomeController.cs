namespace StudentDrive.Controllers
{
    using System;
    using System.Security.Policy;
    using System.Web.Mvc;
    using Utils;
    using Newtonsoft.Json.Linq;
    using Core;
    using Core.DTO;
    using System.Web.Security;
    using System.IO.Compression;
    using System.IO;

    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult Index(Guid? id)
        {
            Guid sessionGuid = new Guid(User.Identity.Name);
            if (id == null)
            {
                return RedirectToAction("Index", "Home", new { id = sessionGuid });
            }

            if (id == sessionGuid)
            {
                return View("Index", new { id = sessionGuid });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { id = sessionGuid });
            }
        }

        [AllowAnonymous]
        public ActionResult Enter()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home", new { id = new Guid(User.Identity.Name) });
            }
            else
            {
                return View("Enter");
            }
        }

        public FileResult File(Guid[] id, string[] name)
        {
            using (var data = new Core())
            {
                using (var compressedFileStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                    {
                        for (var i = 0; i < id.Length; i++)
                        {
                            var caseAttachmentModels = data.GetFile(id[i]);

                            var zipEntry = zipArchive.CreateEntry(name[i]);

                            using (var originalFileStream = new MemoryStream(caseAttachmentModels))
                            {
                                using (var zipEntryStream = zipEntry.Open())
                                {
                                    originalFileStream.CopyTo(zipEntryStream);
                                }
                            }
                        }
                    }
                    return File(compressedFileStream.ToArray(), "application/zip", "Disk_" + DateTime.Now.ToString("H:mm:ss") + ".zip");
                }
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Enter", "Home");
        }
        [AllowAnonymous]
        public ActionResult VkAutorization(string code)
        {
            try
            {
                if (code != null)
                {
                    const string client_id = "";
                    const string client_secret = "";
                    const string redirect_uri = "/Home/VkAutorization";
                    var jsonAccess = GetJSONWebRequest.Get(new Url("https://oauth.vk.com/access_token?client_id=" + client_id + "&client_secret=" + client_secret + "&redirect_uri=" + redirect_uri + "&code=" + code));
                    var vkid = jsonAccess["user_id"].Value<string>();
                    var jsonData = GetJSONWebRequest.Get(new Url("https://api.vk.com/method/users.get?user_ids=" + vkid + "&v=5.63"));
                    var log = jsonData["response"][0]["first_name"].Value<string>();
                    var pass = jsonData["response"][0]["last_name"].Value<string>();
                    using (var core = new Core())
                    {
                        UserDTO usr;
                        try
                        {
                            usr = core.GetUserAuthorizeVk(vkid);
                        }
                        catch
                        {
                            usr = core.AddUser(new UserDTO() { FirstName = log, SecondName = pass, Login = log, Password = pass, VkId = vkid });
                        }
                        FormsAuthentication.SetAuthCookie(usr.Id.ToString(), true);
                        return RedirectToAction("Index", "Home", new { id = usr.Id });
                    }
                }
            }
            catch (Exception e)
            {
                TempData["EnterMessage"] = e.Message;
                return RedirectToAction("Enter", "Home");
            }
            TempData["EnterMessage"] = "Неудачная регистрация";
            return RedirectToAction("Enter", "Home");
        }
    }
}