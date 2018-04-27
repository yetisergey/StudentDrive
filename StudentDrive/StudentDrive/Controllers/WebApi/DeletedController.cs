namespace StudentDrive.Controllers.WebApi
{
    using System;
    using System.Web.Http;
    using Core;
    [Authorize]
    public class DeletedController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using (var data = new Core())
                {
                    return Ok(data.GetRemoveFiles(new Guid(User.Identity.Name)));
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IHttpActionResult Put([FromUri]Guid[] id)
        {
            try
            {
                using (var data = new Core())
                {
                    foreach (var i in id)
                    {
                        data.RecoveryFile(i);
                    }
                    return Ok("Успешное восстановление");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}