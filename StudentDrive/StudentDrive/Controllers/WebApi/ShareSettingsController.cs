namespace StudentDrive.Controllers.WebApi
{
    using System;
    using System.Web.Http;
    using Core;
    using Models;
    using System.Collections.Generic;
    using Core.DTO;
    using System.Threading.Tasks;
    using System.Net.Http;
    [Authorize]
    public class ShareSettingsController : ApiController
    {
        // GET: api/ShareSettings
        public IHttpActionResult Get([FromUri]Guid id)
        {
            try
            {
                using (var data = new Core())
                {
                    var resList = new List<RedactShareHelperDto>();
                    var temp = data.GetShare(id);
                    foreach (var item in temp)
                    {
                        resList.Add(new RedactShareHelperDto() { Id = item.ToUserId, FirstName = data.GetUserId(item.ToUserId).FirstName, SecondName = data.GetUserId(item.ToUserId).SecondName, checkedShare = item.IsWrite });
                    }

                    return Ok(resList);
                }
            }
            catch (Exception)
            {
                return Ok();
            }
        }

        // POST: api/ShareSettings
        public IHttpActionResult Post([FromUri]Guid fileId, [FromBody]List<RedactShareHelperDto> value)
        {
            try
            {
                using (var data = new Core())
                {
                    data.RemoveAllShare(fileId);
                    foreach (var item in value)
                    {
                        data.AddShare(new ShareDTO()
                        {
                            FileId = fileId,
                            OwnerId = new Guid(User.Identity.Name),
                            ToUserId = item.Id,
                            IsWrite = item.checkedShare
                        });
                    }
                    return Ok("Успешное редактирование");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // PUT: api/ShareSettings
        public async Task<IHttpActionResult> Put([FromUri]Guid fileId)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return BadRequest();
                }
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                using (var data = new Core())
                {
                    foreach (var file in provider.Contents)
                    {
                        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                        byte[] fileArray = await file.ReadAsByteArrayAsync();

                        var fdto = new FileDTO()
                        {
                            Name = filename,
                            FileSource = fileArray,
                            UserId = data.GetShareToUserOwnerUser(fileId, new Guid(User.Identity.Name)),
                            Size = fileArray.Length
                        };
                        data.OverWriteFile(fileId, fdto);
                        break;
                    }
                }
                return Ok("Успешная перезапись!");
            }
            catch (Exception e)
            {
                return BadRequest("Ошибка перезаписи файла " + e.Message);
            }
        }
    }
}
