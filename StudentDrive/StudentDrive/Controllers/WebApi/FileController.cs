namespace StudentDrive.Controllers.WebApi
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Core;
    using Core.DTO;
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Models;

    [Authorize]
    public class FileController : ApiController
    {

        public IHttpActionResult GetFiles()
        {
            try
            {
                using (var data = new Core())
                {
                    return Ok(data.GetFiles(new Guid(User.Identity.Name)));
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IHttpActionResult> Post()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return BadRequest();
                }
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                var resultList = new List<FileDTO>();
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
                            UserId = new Guid(User.Identity.Name),
                            Size = fileArray.Length
                        };
                        resultList.Add(data.AddFile(fdto));
                    }
                }
                return Ok(resultList);
            }
            catch (Exception e)
            {
                return BadRequest("Ошибка загрузки файлов " + e.Message);
            }
        }

        public IHttpActionResult Put([FromUri]CreateShareHelperDto[] ids, [FromBody]Guid[] value)
        {
            try
            {
                using (var data = new Core())
                {
                    foreach (var val in value)
                    {
                        foreach (var id in ids)
                        {
                            data.AddShare(new ShareDTO()
                            {
                                FileId = val,
                                OwnerId = new Guid(User.Identity.Name),
                                ToUserId = id.id,
                                IsWrite = id.isRew
                            });
                        }
                    }
                    return Ok("Файлы расшарены!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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

                        data.DeleteFile(id);
                    }
                }
                return Ok("Файлы удалены!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}