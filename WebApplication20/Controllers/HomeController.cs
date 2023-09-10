using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using WebApplication20.Models;
using WebApplication20.Helpers;

namespace WebApplication20.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _azureConnectionString;



        public HomeController(ILogger<HomeController> logger)
        {
            _azureConnectionString = "DefaultEndpointsProtocol=https;AccountName=storage1blobyaroslavkop;AccountKey=xLSbC+rVNWKROoy00jC/CQ4OgnD4s6U2uenkX5J2/+ZJPi62bIlifbFjD0L4vNjdBoG7RLVaMawI+ASt7xSOMA==;EndpointSuffix=core.windows.net;";
            //configuration.GetConnectionString("AzureConnectionString");
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string email)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    
                 string extension = System.IO.Path.GetExtension(file.FileName);

                    if (extension == ".docx" && Email.IsValidEmail(email))
                    {

                        var container = new BlobContainerClient(_azureConnectionString, "upload-container");
                        var createResponse = await container.CreateIfNotExistsAsync();
                        if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                            await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                        
                        var blob = container.GetBlobClient(email+" "+file.FileName);
                        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                        using (var fileStream = file.OpenReadStream())
                        {
                            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                        }
                        return Ok(blob.Uri.ToString());
                    }
                }
                return StatusCode(500, "Email or file format validation error!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Please Select A file to upload(.docx)");
            }
        }
        public IActionResult Index()
        {
            return View();
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
}