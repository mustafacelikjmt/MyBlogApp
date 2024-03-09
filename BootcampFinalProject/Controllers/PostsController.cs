using BootcampFinalProject.Data.Abstract;
using BootcampFinalProject.Entity;
using BootcampFinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BootcampFinalProject.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;
        private readonly UserManager<AppUser> _userManager;
        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, ITagRepository tagRepository, UserManager<AppUser> userManager)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var claims = User.Claims;

            var posts = _postRepository.Posts.Where(x => x.IsActive);
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
            }

            return View(new PostViewModel { Posts = await posts.Include(x => x.User).Include(x => x.Tags).ToListAsync() });
        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postRepository.Posts
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpPost]
        public async Task<JsonResult> AddComment(int PostId, string Text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            //var avatar = User.FindFirstValue(ClaimTypes.UserData); //bu ve bununla ilgili accountcontroller kodları doğru çalışmıyor. onun yerine aşağıdaki gibi db den veri direk alındı. (ekstra db iletişimi sunucuda ekstra iletişim yükü olabilir, geçici bi çözüm uyguladık ama UserData dan çekebilmeyi çözmeyi unutma)

            var user = await _userManager.FindByIdAsync(userId);
            var avatar = user.Image;

            var entity = new Comment
            {
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = userId ?? ""
            };
            _commentRepository.CreateComments(entity);
            return Json(new
            {
                userName,
                Text,
                entity.PublishedOn,
                avatar
            });
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _postRepository.CreatePost(
                    new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Url = model.Url,
                        Description = model.Description,
                        UserId = userId,
                        PublishedOn = DateTime.Now,
                        Image = "1.jpg",
                        IsActive = false
                    });
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var role = User.FindFirstValue(ClaimTypes.Role);
            var posts = _postRepository.Posts;

            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(x => x.UserId == userId);
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
            var post = _postRepository.Posts.Include(x => x.Tags).FirstOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();

            return View(new PostCreateViewModel
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
        public IActionResult Edit(PostCreateViewModel model, int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };
                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }
                _postRepository.EditPost(entityToUpdate, tagIds);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = _postRepository.Posts.FirstOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
            });
        }
        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    _postRepository.DeletePost(id);
                }
            }
            return RedirectToAction("List");
        }
    }
}