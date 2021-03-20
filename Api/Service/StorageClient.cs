namespace Api.Service
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Threading.Tasks;

    public interface IStorageClient
    {
        Task Upload(IFormFile file, string name);
    }

    public class StorageClient : IStorageClient
    {
        private readonly IConfiguration _configuration;
        private readonly string _constr;
        private readonly string _container;
        public StorageClient(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _constr = _configuration["StrConnectionStrings"];
            _container = _configuration["ContainerName"];
        }

        public async Task Upload(IFormFile file, string name)
        {
            try
            {
                Microsoft.WindowsAzure.Storage.CloudStorageAccount cloudStorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(_constr);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_container);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        });
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(name);
                cloudBlockBlob.Properties.ContentType = file.ContentType;
                await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

