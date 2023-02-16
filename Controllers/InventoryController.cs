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
using Newtonsoft.Json;


namespace CascadeBrewingCo.Controllers
{
	public class InventoryController
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
                id: "70b63682-b93a-4c77-aad2-65501347265f",
                categoryId: "61dba35b-4f02-45c5-b648-c6badc0cbd79",
                categoryName: "Beer",
                name: "Steep and Deep IPA",
                quantity: 12,
                sale: false
            );

            Product createdItem = await container.CreateItemAsync<Product>(
                item: newItem,
                partitionKey: new PartitionKey("61dba35b-4f02-45c5-b648-c6badc0cbd79")
            );

            Console.WriteLine($"Created item:\t{createdItem.id}\t[{createdItem.categoryName}]");
        }
	}
}

