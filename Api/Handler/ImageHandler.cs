namespace Api.Handler
{
    using Api.Service;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImageHandler
    {
        Task<string> UploadImage(List<IFormFile> files);
    }

    public class ImageHandler : IImageHandler
    {
        private readonly IStorageClient _storageClient;

        public ImageHandler(IStorageClient storageClient)
        => _storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));

        public async Task<string> UploadImage(List<IFormFile> files)
        {
            var message = "uploaded successfully";
            try
            {
                foreach (var file in files)
                {
                    await _storageClient.Upload(file, file.FileName);
                }
                return message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
