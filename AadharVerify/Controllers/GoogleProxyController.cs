using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json; // Add this using directive
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class GoogleDriveProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GoogleDriveProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    [HttpGet]
    public async Task<IActionResult> GetImage([FromQuery] string fileName)
    {
        try
        {
            var apiKey = "AIzaSyCnlu8YkrbZ1YJxa2LPouQnMNCdheLGJUU"; // Replace with your actual API key
            var query = $"name='{fileName}' and '1vdCXCXmd6ImFC0TH1jj2w3_IcW5iOQ3b' in parents"; // Replace with your folder ID
            var apiUrl = $"https://www.googleapis.com/drive/v3/files?key={apiKey}&q={query}";

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var fileResponse = await response.Content.ReadFromJsonAsync<GoogleDriveFileResponse>();

                if (fileResponse?.Files != null && fileResponse.Files.Count > 0)
                {
                    var fileId = fileResponse.Files[0].Id;
                    var imageUrl = $"https://drive.google.com/uc?id={fileId}&export=download";

                    var imageResponse = await httpClient.GetAsync(imageUrl);
                    imageResponse.EnsureSuccessStatusCode();

                    var stream = await imageResponse.Content.ReadAsStreamAsync();
                    return new FileStreamResult(stream, imageResponse.Content.Headers.ContentType?.ToString() ?? "application/octet-stream");
                }
                else
                {
                    return NotFound("Image not found.");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle exception
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }
}

    public class GoogleDriveFileResponse
{
    public List<GoogleDriveFile> Files { get; set; }
}

public class GoogleDriveFile
{
    public string Id { get; set; }
    // Add other necessary properties here
}
