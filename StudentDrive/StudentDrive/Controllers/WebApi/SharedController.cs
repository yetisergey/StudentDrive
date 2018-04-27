namespace StudentDrive.Controllers.WebApi
{
    using System;
    using System.Web.Http;
    using Core;
    using Models;
    using System.Collections.Generic;
    [Authorize]
    public class SharedController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                using (var data = new Core())
                {
                    var resList = new List<ShareHelperDto>();
                    var temp = data.GetShares(new Guid(User.Identity.Name));
                    foreach (var item in temp)
                    {
                        resList.Add(new ShareHelperDto()
                        {
                            Id = item.Id
                            ,
                            Name = item.Name
                            ,
                            DateOfUpload = item.DateOfUpload
                            ,
                            Size = item.Size
                            ,
                            Share = item.Share
                            ,
                            Rewrite = data.GetShareOverride(item.Id, new Guid(User.Identity.Name))
                        });
                    }
                    return Ok(resList);
                }
            }
            catch
            {
                return Ok();
            }
        }
        public IHttpActionResult Delete([FromUri]Guid[] ids)
        {
            try
            {
                using (var data = new Core())
                {
                    foreach (var id in ids)
                    {
                        data.DeleteShare(new Guid(User.Identity.Name), id);
                    }
                }
                return Ok("Удалено!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IHttpActionResult Put([FromUri]Guid[] ids)
        {
            try
            {
                using (var data = new Core())
                {
                    foreach (var id in ids)
                    {
                        data.RemoveAllShare(id);
                    }
                }
                return Ok("Удалено!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}