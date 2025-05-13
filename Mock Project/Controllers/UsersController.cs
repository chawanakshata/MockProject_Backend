using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Models;
using Mock_Project.Repositories;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mock_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _environment;

        public UsersController(IUserRepository userRepository, IWebHostEnvironment environment)
        {
            _userRepository = userRepository;
            _environment = environment;
        }

        // Retrieves a single user by the ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Gallery = user.Gallery?.Select(g => new UserImageDto
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    UserId = g.UserId
                }).ToList(),
                Facts = user.Facts?.Select(f => new UserFactDto
                {
                    Id = f.Id,
                    Fact = f.Fact,
                    UserId = f.UserId,
                    Type = f.Type
                }).ToList()
            };

            return Ok(userDto);
        }

        // Retrieves all users.
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Gallery = user.Gallery?.Select(g => new UserImageDto
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    UserId = g.UserId
                }).ToList(),
                Facts = user.Facts?.Select(f => new UserFactDto
                {
                    Id = f.Id,
                    Fact = f.Fact,
                    UserId = f.UserId,
                    Type = f.Type
                }).ToList()
            }).ToList();

            return Ok(userDtos);
        }

        // Uploads a user's profile picture and creates a new user record.
        [HttpPost("upload")]
        public async Task<IActionResult> UploadUserImage([FromForm] UserFileUploadRequest model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, model.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var user = new User
            {
                Name = model.UserName,
                Bio = model.Bio,
                ProfilePictureUrl = $"/uploads/{model.File.FileName}",
                Gallery = new List<UserImage>()
            };

            await _userRepository.AddAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Gallery = user.Gallery?.Select(g => new UserImageDto
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    UserId = g.UserId
                }).ToList(),
                Facts = user.Facts?.Select(f => new UserFactDto
                {
                    Id = f.Id,
                    Fact = f.Fact,
                    UserId = f.UserId,
                    Type = f.Type
                }).ToList()
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // Updates an existing user's information, including profile picture and gallery images.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserUpdateRequest model)
        {
            if (model.UserName == null || model.Bio == null)
                return BadRequest();

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, model.ProfilePicture.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = $"/uploads/{model.ProfilePicture.FileName}";
            }

            if (model.GalleryImages != null && model.GalleryImages.Count > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                foreach (var file in model.GalleryImages)
                {
                    var filePath = Path.Combine(uploadsFolder, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var userImage = new UserImage
                    {
                        ImageUrl = $"/uploads/{file.FileName}",
                        UserId = user.Id
                    };

                    user.Gallery.Add(userImage);
                }
            }

            user.Name = model.UserName;
            user.Bio = model.Bio;

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        // Deletes a user by ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userRepository.DeleteAsync(id);

            return NoContent();
        }

        // Adds an image to a user's gallery.
        [HttpPost("gallery")]
        public async Task<IActionResult> AddImageToGallery([FromForm] GalleryImageUploadRequest model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("No file uploaded.");

            var user = await _userRepository.GetSingleUserAsync();
            if (user == null)
                return NotFound();

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, model.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var userImage = new UserImage
            {
                ImageUrl = $"/uploads/{model.File.FileName}",
                UserId = user.Id
            };

            user.Gallery.Add(userImage);

            await _userRepository.UpdateAsync(user);

            var userImageDto = new UserImageDto
            {
                Id = userImage.Id,
                ImageUrl = userImage.ImageUrl,
                UserId = userImage.UserId
            };

            return Ok(userImageDto);
        }

        // Deletes an image from a user's gallery by its ID.
        [HttpDelete("gallery/{imageId}")]
        public async Task<IActionResult> DeleteImageFromGallery(int imageId)
        {
            var user = await _userRepository.GetSingleUserAsync();
            if (user == null)
                return NotFound();

            var image = user.Gallery.FirstOrDefault(g => g.Id == imageId);
            if (image == null)
                return NotFound();

            var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            user.Gallery.Remove(image);

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }


        // Adds a fact to a user's profile.
        [HttpPost("facts")]
        public async Task<IActionResult> AddFact([FromBody] UserFactRequestDto model)
        {
            var user = await _userRepository.GetSingleUserAsync();
            if (user == null)
                return NotFound();

            var userFact = new UserFact
            {
                Fact = model.Fact,
                UserId = user.Id,
                Type = model.Type
            };

            await _userRepository.AddFactAsync(userFact);

            return Ok(new UserFactDto
            {
                Id = userFact.Id,
                Fact = userFact.Fact,
                UserId = userFact.UserId,
                Type = userFact.Type
            });
        }

        // Updates an existing fact by its ID.
        [HttpPut("facts/{factId}")]
        public async Task<IActionResult> UpdateFact(int factId, [FromBody] UserFactRequestDto model)
        {
            var userFact = await _userRepository.GetFactByIdAsync(factId);
            if (userFact == null)
                return NotFound();

            userFact.Fact = model.Fact;
            userFact.Type = model.Type;

            await _userRepository.UpdateFactAsync(userFact);

            return NoContent();
        }


        // Deletes a fact from a user's profile by its ID.
        [HttpDelete("facts/{factId}")]
        public async Task<IActionResult> DeleteFact(int factId)
        {
            var user = await _userRepository.GetSingleUserAsync();
            if (user == null)
                return NotFound();

            var fact = user.Facts.FirstOrDefault(f => f.Id == factId);
            if (fact == null)
                return NotFound();

            await _userRepository.DeleteFactAsync(factId);

            return NoContent();
        }
    }
