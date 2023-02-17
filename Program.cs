using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Storage.Blobs;

//Add the following lines under the var builder... line that already exists in Program.cs
var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.frosti.json");
}

builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KV_ENDPOINT"]),
    new DefaultAzureCredential());

// <cosmos_endpoint_key> 
// New instance of CosmosClient class using an endpoint and key string
var cosmosClient = new CosmosClient(builder.Configuration["CosmosConnection"]);
// </cosmos_endpoint_key>

// <create_database>
// New instance of Database class referencing the server-side database
Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync("CascadeBrewsDb");
builder.Services.AddSingleton(s =>
{
    return cosmosClient;
});
// </create_database>

// <create_container>
// New instance of Container class referencing the server-side container
Container container = await database.CreateContainerIfNotExistsAsync(
    id: "brews",
    partitionKeyPath: "/categoryId",
    throughput: 400
);
// </create_container>

// <storage_endpoint_key> 
// New instance of BlobServiceClient class using an endpoint and key string
var blobServiceClient = new BlobServiceClient(builder.Configuration["StorageConnection"]);
// </storage_endpoint_key>

// <create_container>
// New instance of Container class referencing the server-side container
builder.Services.AddSingleton(s =>
{
    return blobServiceClient;
});


// Create the container and return a container client object
string containerName = "brews98109";
BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
builder.Services.AddSingleton(s =>
{
    return containerClient;
});
// </create_container>






// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

