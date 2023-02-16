using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CascadeBrewingCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using Azure;


namespace CascadeBrewingCo.Controllers
{
	public class InventoryController : Controller
	{
        private readonly ILogger<InventoryController> _logger;
        private readonly CosmosClient _cosmosClient;

        public InventoryController(ILogger<InventoryController> logger, CosmosClient cosmosClient)
		{
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        public async Task AddInventory()
        {
            var container = _cosmosClient.GetContainer("CascadeBrewsDb", "brews");
            // Create new object and upsert (create or replace) to container
            Product newItem = new(
                id: "70b63682-b93a-4c77-aad2-65501347284f",
                categoryId: "61dba35b-4f02-45c5-b648-c6badc0cbd79",
                categoryName: "Beer",
                name: "Blue Bird Pale Ale",
                quantity: 12,
                sale: false
            );

            Product createdItem = await container.CreateItemAsync<Product>(newItem);

            Console.WriteLine($"Created item:\t{createdItem.id}\t[{createdItem.categoryName}]");
        }

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            var container = _cosmosClient.GetContainer("CascadeBrewsDb", "brews");
            try
            {
                // Create query using a SQL string and parameters
                var query = new QueryDefinition(
                    query: "SELECT * FROM c"
                );

                using FeedIterator<Product> feed = container.GetItemQueryIterator<Product>(
                    queryDefinition: query
                );
                List<Product> products = new List<Product>();
                while (feed.HasMoreResults)
                {
                    FeedResponse<Product> response = await feed.ReadNextAsync();
                    foreach (Product item in response)
                    {
                        products.Add(item);
                        Console.WriteLine($"Found item:\t{item.name}");
                    }
                }
                
                return View(products);
            }
            catch
            {
                throw new Exception($"No Brews Today");
            }

        }
    }
}

