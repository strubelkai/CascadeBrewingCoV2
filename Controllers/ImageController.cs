using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Threading;

namespace CascadeBrewingCo.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly BlobContainerClient _containerClient;
        private readonly IConfiguration _configuration;

        public ImageController(ILogger<InventoryController> logger, CosmosClient cosmosClient, BlobContainerClient containerClient, IConfiguration configuration)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
            _containerClient = containerClient;
            _configuration = configuration;
        }


        public async Task<ActionResult> GetBlob()
        {
            //Create a unique name for the container
          
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // Retreive blobs
            List<Uri> allBlobs = new List<Uri>();
            foreach (BlobItem blob in _containerClient.GetBlobs())
            {
                Console.WriteLine(blob.Name);
                if (blob.Properties.BlobType == BlobType.Block)
                    allBlobs.Add(_containerClient.GetBlobClient(blob.Name).Uri);
            }

            
            return View(allBlobs);

        }
        
    }
}

