namespace DotNetInterview.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetInterview.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class FileService : IFileService
    {
        public async Task<string> SaveFile(IFormFile file, string fileDirectory)
        {
            string fileName = null;

            if (file != null && file?.FileName != null)
            {
                fileName = this.UniqueFileNameGenerator(file);

                using (var stream = new FileStream(fileDirectory + fileName, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return fileName;
        }

        public void DeleteFile(string fileDiretory, string fileName)
        {
            if (File.Exists(Path.Combine(fileDiretory, fileName)))
            {
                File.Delete(Path.Combine(fileDiretory, fileName));
            }
        }

        private string UniqueFileNameGenerator(IFormFile file)
        {
            var fileExtension = file.FileName
                .Split(".")
                .LastOrDefault();

            var uniqueFileName = Guid.NewGuid().ToString() + "." + fileExtension;

            return uniqueFileName;
        }

        private string ImageUserUrlParser(IEnumerable<Image> images, string imagesPath)
        {
            var imageTitle = images.Where(i => i.IsDeleted == false).Select(i => i.ImageUrl).FirstOrDefault();

            if (imageTitle != null)
            {
                return imagesPath + imageTitle;
            }

            return null;
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
