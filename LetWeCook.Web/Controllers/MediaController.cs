using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.MediaUrlServices;
using LetWeCook.Web.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LetWeCook.Web.Controllers
{
    public class MediaController : Controller
    {
        private IMediaUrlService _mediaUrlService;
        public MediaController(IMediaUrlService mediaUrlService)
        {
            _mediaUrlService = mediaUrlService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveMediaUrl(string url, CancellationToken cancellationToken = default)
        {
            if (url.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse(
                    "URL cannot be null or empty."
                ));
            }

            Result<MediaUrlDTO> result = await _mediaUrlService.CreateMediaUrlAsync(url, cancellationToken);

            if (!result.IsSuccess)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { Message = "Failed to save media URL.", Errors = result.Exception });
            }

            return Ok(new { Message = "Media URL saved successfully.", Data = result.Data });
        }

    }
}
