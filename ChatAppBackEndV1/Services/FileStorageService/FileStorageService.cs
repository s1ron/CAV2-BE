using Path = System.IO.Path;

namespace ChatAppBackEndV2.Services.FileStorageService
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _userContentFolder;
        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = webHostEnvironment.WebRootPath;
        }
        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string folderName, string fileName)
        {
            var folferPath = Path.Combine(_userContentFolder, folderName);
            var filePath = Path.Combine(folferPath, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }
    }
}
