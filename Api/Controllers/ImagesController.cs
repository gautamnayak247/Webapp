namespace Api.Controllers
{
    using Api.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/v1/images")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageHandler _imageHandler;

        public ImagesController(IImageHandler imageHandler)
            => _imageHandler = imageHandler ?? throw new ArgumentNullException(nameof(imageHandler));

        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
            if (files == null)
                return BadRequest();

            var message = await _imageHandler.UploadImage(files);
            return Created("", message);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Images");
        }
    }
}
