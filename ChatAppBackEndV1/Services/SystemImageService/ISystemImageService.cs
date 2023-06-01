using ChatAppBackEndV1.Data.Entities;

namespace ChatAppBackEndV2.Services.SystemImageService
{
    public interface ISystemImageService
    {
        Task<List<SystemImageMessage>> GetSystemImagesAsync(string type);
    }
}
