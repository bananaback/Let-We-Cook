using LetWeCook.Services.MediaUrlServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            throw new NotImplementedException();
        }

    }
}
