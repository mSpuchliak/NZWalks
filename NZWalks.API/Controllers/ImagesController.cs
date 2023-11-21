using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageDomainModel = new Image
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileDescription
            };
            
            await imageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsuported file extension");
            }

            // Cannot be bigger than 10MB
            if(request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is bigger than 10MB, upload a smaller file");
            }
        }
    }
}
