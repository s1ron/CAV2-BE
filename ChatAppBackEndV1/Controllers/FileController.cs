using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV2.Dtos.ChatHubDtos;
using ChatAppBackEndV2.Dtos.MessageService;
using ChatAppBackEndV2.Services.FileStorageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ChatAppBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private static Random _rnd = new Random();
        public FileController(IFileStorageService fileStorageService)
        { 
            _fileStorageService= fileStorageService;
        }
        [HttpPost("attachment")]
        public async Task<IActionResult> UploadMessageFile([FromForm]UploadMessageFile resquest)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(resquest.File.ContentDisposition).FileName.Trim('"');

            var fileName = $"attachment-{resquest.ConversationId}-{_rnd.Next().ToString("x")}{_rnd.Next().ToString("x")}{System.IO.Path.GetExtension(originalFileName)}";

            await _fileStorageService.SaveFileAsync(resquest.File.OpenReadStream(), "attachment", fileName);

            return Ok(System.IO.Path.Combine("attachment", fileName));
        }
    }
}
