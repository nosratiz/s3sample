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

}