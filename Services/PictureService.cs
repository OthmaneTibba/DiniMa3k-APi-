using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace DiniM3ak.Services
{
    public class PictureService
    {
        private readonly Cloudinary _cloudinary;

        public PictureService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }



        public string UaploadProdilePicture(IFormFile formFile)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(formFile.FileName, formFile.OpenReadStream()),
                PublicId = $"profile_pictures/{Path.GetRandomFileName()}",
            };

            var uploadResult = _cloudinary.Upload(uploadParams);  
            string imageUrl = uploadResult.SecureUri.ToString();

            return imageUrl;


        }
    }
}
