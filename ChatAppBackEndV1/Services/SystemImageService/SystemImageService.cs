using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEndV2.Services.SystemImageService
{
    public class SystemImageService : ISystemImageService
    {
        private readonly ChatAppDbContext _context;
        public SystemImageService(ChatAppDbContext context)
        {
            _context = context;
        }
        public Task<List<SystemImageMessage>> GetSystemImagesAsync(string type)
        {
            return _context.SystemImageMessages.Where(x=>x.Type == type).ToListAsync();
            
        }
    }
}
