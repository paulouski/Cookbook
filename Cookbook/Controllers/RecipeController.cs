using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Data;
using Cookbook.Models;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RecipeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Edit(int id)
        {

            var recipe = await _context.Recipes.SingleOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Recipe recipeModel)
        {

            var recipe = await _context.Recipes.SingleOrDefaultAsync(m => m.RecipeId == recipeModel.RecipeId);
            //if (post.TagString != null)
            //{
            //    char[] delimeterChars = { ' ', ',' };
            //    string[] words = post.TagString.Split(delimeterChars);
            //    foreach (var word in words)
            //    {
            //        if (await _context.Tags.FindAsync(word) == null)
            //            post.Tags.Add(new Tag() { Name = word });
            //    }
            //}
            recipe.RecipeName = recipeModel.RecipeName;
            recipe.Category = recipeModel.Category;
            recipe.Description = recipeModel.Description;
            recipe.IngredientsForRecipe = recipeModel.IngredientsForRecipe;
            recipe.LastEditTime = DateTime.UtcNow;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(int recipeId)
        {

            var recipe = _context.Recipes.Include(a => a.Author).Include(a => a.RatesUsers).SingleOrDefault(m => m.RecipeId == recipeId);
            if (recipe == null)
            {
                return NotFound();
            }

            foreach (var user in recipe.RatesUsers.Where(a => a.Id == _userManager.GetUserId(User)))
            {
                return RedirectToAction("Details/" + recipeId);
            }
            recipe.Rating += 1;
            recipe.RatesUsers.Add(await _userManager.GetUserAsync(User));
            _context.SaveChanges();
            return RedirectToAction("Details/" + recipeId);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, Comment comment)
        {
            var commentedRecipe = await _context.Recipes.SingleOrDefaultAsync(m => m.RecipeId == postId);
            if (commentedRecipe == null)
            {
                return NotFound();
            }

            //_context.Comments.Add(comment);       ????
            commentedRecipe.Comments.Add(new Comment()
            {
                Author = await _userManager.GetUserAsync(User),
                CreatedDate = DateTime.UtcNow,
                Text = comment.Text
            });
            _context.SaveChanges();
            return Redirect("Details/" + postId);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Recipes.Include(a => a.Author);
            return View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {

            var post = await _context.Recipes.Include(a => a.Author).SingleOrDefaultAsync(m => m.RecipeId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeName, Abstract,Description,Category")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                //if (recipe.Picture == null)
                //{
                //    var img = await _context.Source.FindAsync("DefaultUser");
                //    recipe.Picture = img.Picture;
                //}
                recipe.LastEditTime = DateTime.UtcNow;
                recipe.Author = await _userManager.GetUserAsync(User);
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public async Task<IActionResult> Delete(int id)
        {

            var recipe = await _context.Recipes.SingleOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var recipe = await _context.Recipes.Include(a => a.RatesUsers).Include(a => a.Comments)
                            .Include(a => a.Author).SingleOrDefaultAsync(m => m.RecipeId == id);

            foreach (var comment in recipe.Comments)
            {
                _context.Remove(comment);
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<string> UploadImage(IFormFile file)
        {
            var client = new ImgurClient(
                "6f723ed132879e0",
                "d2226573ab16224ab0901382ebc4e75fbca58357"
                );
            var endpoint = new ImageEndpoint(client);
            IImage image;
            using (var fileStream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    image = await endpoint.UploadImageBinaryAsync(fileBytes);
                }
            }

            return image.Link;
        }
    }
}