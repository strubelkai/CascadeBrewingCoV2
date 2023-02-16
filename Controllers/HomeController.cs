using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CascadeBrewingCo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using Microsoft.VisualBasic;
using System.Data;
using System.Net;

namespace CascadeBrewingCo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, CosmosClient cosmosClient, IConfiguration configuration)
    {
        _logger = logger;
        _cosmosClient = cosmosClient;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<HttpStatusCode> Cosmos()
    {
        var dbResp = await _cosmosClient.CreateDatabaseIfNotExistsAsync("cosmicworks");
        var containerResp = await dbResp.Database.CreateContainerIfNotExistsAsync("products", "/category");
        return containerResp.StatusCode;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

