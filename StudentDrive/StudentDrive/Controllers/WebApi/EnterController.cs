namespace StudentDrive.Controllers.WebApi
{
    using System;
    using System.Web.Http;
    using System.Web.Security;
    using Core;
    public class EnterController : ApiController
    {
        [AllowAnonymous]
        public IHttpActionResult Get([FromUri]string login, [FromUri] string password)
        {
            try
            {
                using (var core = new Core())
                {
                    var tempUser = core.GetUserAuthorize(login, password);
                    if (tempUser != null)
                    {
                        FormsAuthentication.SetAuthCookie(tempUser.Id.ToString(), true);
                        return Ok(tempUser.Id);
                    }
                    else
                    {
                        return BadRequest("Неправильно введён логин или пароль!");
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}