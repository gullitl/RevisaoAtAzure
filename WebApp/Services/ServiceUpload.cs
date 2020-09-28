using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;

namespace WebApp.Services
{
    public interface IServiceUpload
    {
        string Upload(IFormFile formFile);
    }
    public class ServiceUpload : IServiceUpload
    {
        public readonly IConfiguration _configuration;
        public ServiceUpload(IConfiguration configuration) => _configuration = configuration;
        public string Upload(IFormFile formFile)
        {
            var reader = formFile.OpenReadStream();
            var cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("CloudStorageAccount"));
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("post-images");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();
            return destinoDaImagemNaNuvem;
        }
    }
}
