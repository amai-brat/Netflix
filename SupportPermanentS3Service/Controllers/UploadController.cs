using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportPermanentS3Service.Services;

namespace SupportPermanentS3Service.Controllers;

[ApiController]
public class UploadController(
    IFileCopyService fileCopyService): ControllerBase
{
    [HttpPost("copy-to-perm")]
    [Authorize]
    public async Task<IActionResult> Upload(
        List<Uri> guids,
        CancellationToken cancellationToken)
    {
        var uriList = await fileCopyService.CopyFilesAsync(guids,cancellationToken);

        return Ok(uriList);
    }
}