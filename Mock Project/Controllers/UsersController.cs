using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock_Project.DTOs;
using Mock_Project.Models;
using Mock_Project.Repositories;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mock_Project.Exceptions;

namespace Mock_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Retrieves a single user by the ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id); // Fetches the user record by ID
            if (user == null)
                throw new NotFoundException("User not Found.");

            var userDto = new UserDto  // Creates a DTO to return the user object
            {
                Id = user.Id,
                Name = user.Name,
                Bio = user.Bio,
                ProfilePictureBase64 = user.ProfilePictureBase64,
                Gallery = user.Gallery?.Select(g => new UserImageDto  // Maps the UserImage objects to UserImageDto objects
                {
                    Id = g.Id,
                    Base64Image = g.Base64Image,
                    UserId = g.UserId
                }).ToList(),
                Facts = user.Facts?.Select(f => new UserFactDto  // Maps the UserFact objects to UserFactDto objects
                {
                    Id = f.Id,
                    Fact = f.Fact,
                    UserId = f.UserId,
                    Type = f.Type
                }).ToList()
            };

            return Ok(userDto); // Returns the user DTO with status code 200 OK 
        }

        // Retrieves all users.
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync(); // Fetches all users from the repository
            var userDtos = users.Select(user => new UserDto // Maps the User objects to UserDto objects
            {
                Id = user.Id,
                Name = user.Name,
                Bio = user.Bio,
                ProfilePictureBase64 = user.ProfilePictureBase64,
                Gallery = user.Gallery?.Select(g => new UserImageDto 
                {
                    Id = g.Id,
                    Base64Image = g.Base64Image,
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
                throw new BadRequestException("No file uploaded.");

            string base64String;
            using (var memoryStream = new MemoryStream())
            {
                await model.File.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            var user = new User
            {
                Name = model.UserName,
                Bio = model.Bio,
                ProfilePictureBase64 = base64String,
                Gallery = new List<UserImage>()
            };

            await _userRepository.AddAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Bio = user.Bio,
                ProfilePictureBase64 = user.ProfilePictureBase64,
                Gallery = user.Gallery?.Select(g => new UserImageDto
                {
                    Id = g.Id,
                    Base64Image = g.Base64Image,
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
                throw new BadRequestException("User not Found.");

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not Found.");

            // Update profile picture if provided
            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    user.ProfilePictureBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            // Update gallery images if provided
            if (model.GalleryImages != null && model.GalleryImages.Count > 0)
            {
                if (user.Gallery == null)
                    user.Gallery = new List<UserImage>();

                foreach (var file in model.GalleryImages)
                {
                    if (file != null && file.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            var fileBytes = memoryStream.ToArray();
                            var base64String = Convert.ToBase64String(fileBytes);

                            var userImage = new UserImage
                            {
                                Base64Image = base64String,
                                UserId = user.Id
                            };

                            user.Gallery.Add(userImage);
                        }
                    }
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
                throw new NotFoundException("User not Found");

            await _userRepository.DeleteAsync(id);

            return NoContent();
        }

        // Adds an image to a user's gallery (base64).
        // Code Generated by Sidekick is for learning and experimentation purposes only.
        [HttpPost("gallery")]
        public async Task<IActionResult> AddImageToGallery([FromForm] GalleryImageUploadRequest model)
        {
            if (model.File == null || model.File.Length == 0)
                throw new BadRequestException("No file uploaded.");

            var user = await _userRepository.GetSingleUserAsync();
            if (user == null)
                throw new NotFoundException("User not Found.");

            string base64String;
            using (var memoryStream = new MemoryStream())
            {
                await model.File.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            var userImage = new UserImage
            {
                Base64Image = base64String,
                UserId = user.Id
            };

            if (user.Gallery == null)
                user.Gallery = new List<UserImage>();

            user.Gallery.Add(userImage);

            await _userRepository.UpdateAsync(user);

            var userImageDto = new UserImageDto
            {
                Id = userImage.Id,
                Base64Image = userImage.Base64Image,
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
                throw new NotFoundException("User not Found.");

            var image = user.Gallery.FirstOrDefault(g => g.Id == imageId);
            if (image == null)
                throw new NotFoundException("Image not Found.");

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
                throw new NotFoundException("User not Found.");

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
                throw new NotFoundException("Information not Found.");

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
                throw new NotFoundException("User not Found.");

            var fact = user.Facts.FirstOrDefault(f => f.Id == factId);
            if (fact == null)
                throw new NotFoundException("Information not Found.");

            await _userRepository.DeleteFactAsync(factId);

            return NoContent();
        }
    }
}




