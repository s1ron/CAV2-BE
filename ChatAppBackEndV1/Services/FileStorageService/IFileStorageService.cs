namespace ChatAppBackEndV2.Services.FileStorageService
{
    public interface IFileStorageService
    {
        Task SaveFileAsync(Stream mediaBinaryStream, string folderName, string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
