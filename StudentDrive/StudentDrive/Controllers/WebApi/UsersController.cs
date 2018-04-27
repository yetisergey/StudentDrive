namespace StudentDrive.Controllers.WebApi
{
    using System;
    using System.Web.Http;
    using Core;
    [Authorize]
    public class UsersController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using (var data = new Core())
                {
                    return Ok(data.GetUsers());
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}