using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using BlogApp.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {

        private readonly IUserRepository _userRepository;

        private IFollowRepository _followRepository;

        private readonly IPasswordHasher _passwordHasher;

        public UsersController(IUserRepository userRepository, IFollowRepository followRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _followRepository = followRepository;
            _passwordHasher = passwordHasher;

        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("index", "posts");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {


                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email);
                var hashPassword = _passwordHasher.Hash(model.Password);
                var check = _passwordHasher.Verify(isUser.Password, model.Password);


                if (isUser != null && check == true)
                {


                    var userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser!.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser!.UserName));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser!.Name));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser!.Image));



                    if (isUser.Email == "john@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Posts");


                }
            }
            else
            {
                ModelState.AddModelError("", "Username or Password is not valid.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "posts");
        }

        public IActionResult Register()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {


            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    var passwordHash = _passwordHasher.Hash(model.Password);
#pragma warning disable CS8601 
                    _userRepository.CreateUser(new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Password = passwordHash,
                        Name = model.Name,
                        Surname = model.Surname,

                    });
#pragma warning restore CS8601 
                    return RedirectToAction("login");
                }
                else
                {
                    ModelState.AddModelError("", "Mail or username is already in use");
                }

            }
            return View(model);
        }

        public IActionResult Profile(string? username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = _userRepository
                       .Users
                       .Include(x => x.Posts)
                       .Include(x => x.Comments)
                       .ThenInclude(x => x.Post)
                       .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            var currentUser = _userRepository.Users.FirstOrDefault(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            bool isFollowing = false;
            if (currentUser != null)
            {
                isFollowing = _followRepository.Follows
                                .Any(f => f.FollowerId == currentUser.UserId && f.FolloweeId == user.UserId);
            }
            int followersCount = _followRepository.Follows.Count(f => f.FolloweeId == user.UserId);
            int followingCount = _followRepository.Follows.Count(f => f.FollowerId == user.UserId);


            return View(new UserViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
                Image = user.Image,
                Posts = user.Posts,
                Comments = user.Comments,
                Followers = user.Followers,
                Following = user.Following,
                IsFollowing = isFollowing,
                FollowersCount = followersCount,
                FollowingCount = followingCount

            });
        }

        [HttpPost("users/follow/{username}")]

        public async Task<IActionResult> Follow(string? username)
        {
            var currentUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            var userToFollow = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (currentUser == null || userToFollow == null || currentUser.UserId == userToFollow.UserId)
            {
                return BadRequest();
            }

            var follow = new Follow
            {
                FollowerId = currentUser.UserId,
                FolloweeId = userToFollow.UserId
            };
            _followRepository.follow(follow);
            return Ok();



        }

        [HttpPost("users/unfollow/{username}")]
        public async Task<IActionResult> Unfollow(string username)
        {
            var currentUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            var userToUnFollow = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (currentUser == null || userToUnFollow == null || currentUser.UserId == userToUnFollow.UserId)
            {
                return BadRequest();
            }

            var follow = await _followRepository.Follows
            .SingleOrDefaultAsync(f => f.FollowerId == currentUser.UserId && f.FolloweeId == userToUnFollow.UserId);

            if (follow != null)
            {
                _followRepository.Unfollow(follow);
            }


            return Ok();



        }

        public IActionResult Edit(int? id)
        {


            var user = _userRepository
                       .Users
                       .FirstOrDefault(x => x.UserId == id);

            if (user == null || id.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }


            return View(new UserViewModel
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Image = user.Image,
                UserId = user.UserId

            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            // Handle file upload
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string fileExtension = Path.GetExtension(model.ImageFile.FileName);
                string randomFileName = Path.GetRandomFileName().Replace(".", string.Empty);
                string newFileName = $"{randomFileName}{fileExtension}";


                // Define the path to save the file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", newFileName);

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                // Update the model's image path
                model.Image = newFileName;
            }

            _userRepository.EditUser(model);


            return RedirectToAction(model.UserName, "Profile");
        }

    }
}