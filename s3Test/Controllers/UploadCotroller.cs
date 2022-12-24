using Microsoft.AspNetCore.Mvc;
using s3Test.Helper;

namespace s3Test.Controllers;

[ApiController]
[Route("[controller]")]

public class UploadController : Controller
{
    private readonly IFileHandler _fileHandler;

    public UploadController(IFileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        var result = await _fileHandler.UploadFileAsync(file, cancellationToken);

        return Ok(result);
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download(string fileName, CancellationToken cancellationToken)
    {
        var result = await _fileHandler.DownloadAsync(fileName, cancellationToken);

        return File(result, "application/octet-stream", fileName);

    }

}