using Azure.Core;
using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV2.Dtos.ChatHubDtos;
using ChatAppBackEndV2.Dtos.MessageService;
using ChatAppBackEndV2.Dtos.UserService;
using ChatAppBackEndV2.Services.FileStorageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace ChatAppBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ChatAppDbContext _context;
        private static Random _rnd = new Random();
        public FileController(IFileStorageService fileStorageService, ChatAppDbContext context)
        {
            _fileStorageService = fileStorageService;
            _context = context;
        }
        [HttpPost("attachment")]
        public async Task<IActionResult> UploadMessageFile([FromForm] UploadMessageFile request)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(request.File.ContentDisposition).FileName.Trim('"');

            var fileName = $"attachment-{request.ConversationId}-{_rnd.Next().ToString("x")}{_rnd.Next().ToString("x")}{System.IO.Path.GetExtension(originalFileName)}";

            await _fileStorageService.SaveFileAsync(request.File.OpenReadStream(), "attachment", fileName);

            return Ok(System.IO.Path.Combine("attachment", fileName));
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvtar([FromForm] UploadAvatarRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);

            if (user.ProfileImagePath != null)
            {
                await _fileStorageService.DeleteFileAsync(user.ProfileImagePath);
            }

            var originalFileName = ContentDispositionHeaderValue.Parse(request.File.ContentDisposition).FileName.Trim('"');
            var fileName = $"avatar-{request.UserId}{System.IO.Path.GetExtension(originalFileName)}";
            await _fileStorageService.SaveFileAsync(request.File.OpenReadStream(), "user-avatar", fileName);
            var filePath = System.IO.Path.Combine("user-avatar", fileName);

            user.ProfileImagePath = filePath;
            await _context.SaveChangesAsync();

            return Ok(System.IO.Path.Combine("user-avatar", fileName));
        }
    }
}
