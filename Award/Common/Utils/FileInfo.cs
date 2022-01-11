using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Common.Utils
{
    public interface IFileInfo
    {
        Task<string> SaveUploadFile(string basePath, string extendedPath, IFormFile uploadedFile);
    }
    public class FileInfo : IFileInfo
    {
        public async Task<string> SaveUploadFile(string basePath, string extendedPath, IFormFile uploadedFile)
        {
            try
            {
                if (string.IsNullOrEmpty(basePath) || string.IsNullOrEmpty(extendedPath) || uploadedFile == null || uploadedFile.Length < 1)
                {
                    return string.Empty;
                }

                var saveDirPath = Path.Combine(basePath, extendedPath);
                var isDirExists = CreateDirectoryIfNotExists(saveDirPath);
                if (!isDirExists)
                {
                    return string.Empty;
                }

                //var fileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName) ?? string.Empty;
                var extension = Path.GetExtension(uploadedFile.FileName);
                var fullFileName = $"{Guid.NewGuid()}{extension}";
                var fullFilePath = Path.Combine(saveDirPath, fullFileName);

                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                }

                if (uploadedFile.Length > 0)
                {
                    var fileStream = new FileStream(fullFilePath, FileMode.CreateNew);
                    await uploadedFile.CopyToAsync(fileStream);
                    fileStream.Close();
                    //fileStream.Flush();
                    return Path.Combine(extendedPath, fullFileName);
                }
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }
        public bool CreateDirectoryIfNotExists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }

            var directoryInfo = Directory.CreateDirectory(path);
            return directoryInfo.Exists;
        }
    }

}
