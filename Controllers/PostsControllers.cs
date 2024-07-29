using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ITagRepository _tagRepository;
        private ICommentRepository _commentRepository;

        private IUserRepository _userRepository;

        private IFollowRepository _followRepository;

        public PostsController(IPostRepository postRepository, ITagRepository tagRepository, ICommentRepository commentRepository, IUserRepository userRepository, IFollowRepository followRepository)
        {
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _followRepository = followRepository;
        }
        public async Task<IActionResult> Index(string tag_url)
        {
            var posts = _postRepository.Posts.Where(x => x.IsActive == true);
            if (!string.IsNullOrEmpty(tag_url))
            {
                posts = posts.Where(x => x.Tags.Any(y => y.Url == tag_url));
            }
            return View(
                new PostViewModel
                {
                    Posts = await posts.ToListAsync()
                }
            );
        }

        [Authorize]
        public async Task<IActionResult> FollowingPosts()
        {

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            int[] followList = _followRepository.Follows
                .Where(x => x.FollowerId == currentUserId)
                .Select(x => x.FolloweeId)
                .ToArray();
            var posts = _postRepository.Posts.Where(x => x.IsActive == true && followList.Contains(x.UserId) && followList.Contains(x.UserId));


            return View(
                new PostViewModel
                {
                    Posts = await posts.ToListAsync()
                }
            );
        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postRepository
                            .Posts
                            .Include(x => x.Tags)
                            .Include(x => x.User)
                            .Include(x => x.Comments)
                            .ThenInclude(x => x.User)
                            .FirstOrDefaultAsync(p => p.Url == url));

        }

        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var profilphoto = User.FindFirstValue(ClaimTypes.UserData);


            var entity = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = int.Parse(userid ?? "")
            };


            _commentRepository.CreateComment(entity);

            var fullname = _commentRepository.Comments
                    .Where(c => c.PostId == PostId && c.CommentId == entity.CommentId)
                    .Include(c => c.User)
                    .Select(c => c.User.FullName)
                    .ToList();


            return Json(
                new
                {
                    username,
                    entity.Text,
                    entity.PublishedOn,
                    profilphoto,
                    fullname


                }
            );

        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]

        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
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
                Guid g = Guid.NewGuid();


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _postRepository.CreatePost(
                    new Post
                    {

                        Title = model.Title,
                        Content = model.Content,
                        Url = g.ToString(),
                        Description = model.Description,
                        UserId = int.Parse(userId ?? ""),
                        PublishedOn = DateTime.Now,
                        Image = model.Image,
                        IsActive = false,



                    }
                );
                return RedirectToAction("index");
            }

            return View(model);

        }

        [Authorize]

        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;

            if (role != "admin")
            {
                posts = posts.Where(i => i.UserId == userId);
            }
            return View(await posts.ToListAsync());
        }


        [Authorize]
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";

            var post = _postRepository.Posts.Include(i => i.Tags).FirstOrDefault(i => i.PostId == id);
            if (post == null || post.UserId != currentUserId)
            {
                if (role != "admin" || post == null)
                {
                    return NotFound();
                }

            }

            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(new PostEditViewModel
            {

                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags

            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(PostEditViewModel model, int[] TagId)
        {

            if (ModelState.IsValid)
            {
                _postRepository.EditPost(model, TagId);
                return RedirectToAction("list");
            }
            ViewBag.Tags = _tagRepository.Tags.ToList();


            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            var post = _postRepository.Posts.FirstOrDefault(i => i.PostId == id);
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            if ((post != null && post.UserId == currentUserId) || (post != null && role == "admin"))
            {
                _postRepository.DeletePost(post);
            }

            return RedirectToAction("list");
        }
    }
}