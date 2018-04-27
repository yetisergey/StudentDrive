namespace StudentDrive.Controllers.WebApi
{
    using System;
    using Core;
    using System.Web.Http;
    using Core.DTO;
    using System.Web.Security;
    [Authorize]
    public class UserController : ApiController
    {
        // GET: api/User
        public IHttpActionResult GetFio()
        {
            try
            {
                using (var data = new Core())
                {
                    var player = data.GetUserId(new Guid(User.Identity.Name));
                    var stat = data.GetStatisticsByUserId(new Guid(User.Identity.Name));
                    return Ok(new { f = player.SecondName, i = player.FirstName, s = player.DiscUsage, uc = stat.UploadCount, dc = stat.DownloadCount, sc = stat.ShareCount });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/User/5
        public IHttpActionResult Get([FromUri]Guid id)
        {
            try
            {
                using (var data = new Core())
                {
                    var user = data.GetUserId(new Guid(User.Identity.Name));
                    user.Password = "";
                    return Ok(user);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/User/5
        public IHttpActionResult Put([FromUri]Guid id, [FromBody]UserDTO value)
        {
            try
            {
                using (var data = new Core())
                {
                    value.Id = id;
                    data.ChangeUser(value);
                    return Ok("Успешное редактирование");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/User/5
        public IHttpActionResult Delete()
        {
            try
            {
                using (var data = new Core())
                {
                    data.DeleteUser(new Guid(User.Identity.Name));
                    FormsAuthentication.SignOut();
                    return Ok("Успешное удаление");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}