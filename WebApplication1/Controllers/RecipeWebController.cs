using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Areas.Identity.Data;
namespace WebApplication1.Controllers
    
{
    public class RecipeWebController : Controller
    {


        private readonly RecipeWebDbContext _context;
        private readonly WebApplication1.Areas.Identity.Data.WebApplication1Context _webApplication1Context;
        private readonly UserManager<WebApplication1User> _userManager;


        public RecipeWebController(RecipeWebDbContext context, WebApplication1.Areas.Identity.Data.WebApplication1Context webApplication1Context, UserManager<WebApplication1User> userManager)
        {
            _context = context;
            _webApplication1Context = webApplication1Context;
            _userManager = userManager;
        }

        // פעולה לאוטוקומפליט
        [HttpGet("/api/autocomplete")]
        public IActionResult Autocomplete(string query)
        {
            var results = _context.Recipes
                .Where(r => r.RecipeName.Contains(query))
                .Select(r => new { r.RecipeName, r.RecipeID })
                .ToList();

            return Ok(results);
        }


        public IActionResult HomePage()
        {
            return View();
        }


        //קונטרולר שמעביר את כל הרכיבים לצד שרת כדי שיכניס אותם לכפתורים
        [HttpGet]
        [Route("api/ingredients")]
        public IActionResult GetIngredients()
        {

            var ingredients = _context.Ingredients.Select(ingredient => new {
                Id = ingredient.IngredientID,
                Name = ingredient.IngredientName,
                Icon = ingredient.Icons
            }).ToList();

            return Json(ingredients);
        }

        //קונטרולר להצגת מתכונים בדף הראשי
        [HttpPost]
        [Route("api/findrecipes")]
        public IActionResult GetRecipesByIngredients([FromBody] List<int> ingredientIds)
        {
            var recipes = _context.Recipes
                .Where(r => ingredientIds.All(id => r.RecipesIngredients.Any(ri => ri.IngredientID == id)))
                .Select(r => new {
                    r.RecipeID,
                    r.RecipeName,
                    r.PreparationTime,
                    r.DifficultyLevel,
                    r.ImageRecipe
                }).ToList();

            return Json(recipes);
        }


        //קונטרולר למעבר לדף והצגת המתכון המלא
        [HttpGet]
        [Route("recipe/{recipeId}")]
        public IActionResult RecipeDetails(int recipeId)
        {
            var recipe = _context.Recipes
                .Where(r => r.RecipeID == recipeId)
                .Select(r => new
                {
                    r.RecipeID,
                    r.RecipeName,
                    r.Description,
                    r.Category,
                    r.Info,
                    r.PreparationTime,
                    r.DifficultyLevel,
                    r.Calories,
                    r.Entrances,
                    r.ImageRecipe,
                    Ingredients = r.RecipesIngredients.Select(ri => new
                    {
                        ri.Ingredient.IngredientName,
                        ri.Quantity
                    }).ToList()
                }).FirstOrDefault();

            if (recipe == null)
            {
                return NotFound("המתכון לא נמצא.");
            }

            return View(recipe);
        }

        [HttpGet]
        [Route("api/recipes")]
        public IActionResult GetRecipes()
        {
            var recipes = _context.Recipes.Select(r => new {
                r.RecipeID,
                r.ImageRecipe // נתיב התמונה
            }).ToList();

            return Ok(recipes);
        }

        //קונטרולר להצגת כול המתכונים נוצר כשלב ראשון לצורך בדיקה של חיבור מסד הנתונים!
        [HttpGet]
        [Route("RecipeWeb/ShowRecipes")]
        public IActionResult ShowRecipes()
        {
            var recipes = _context.Recipes.ToList();
            return Json(recipes);
        }

        //קונטרולר לשליחת איימיל מתיבעת כתבו לי
        public IActionResult SubmitForm(string name, string email, string message)
        {
            string siteEmail = "itayedriitay@gmail.com"; // האימייל שאליו אתה רוצה לשלוח את ההודעה

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(siteEmail); // האימייל שלך ב-Elastic Email
            mail.To.Add(siteEmail); // האימייל שבו תרצה לקבל את ההודעה
            mail.Subject = "New Contact Form Submission";
            mail.Body = $"Name: {name}\nEmail: {email}\nMessage: {message}";

            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.elasticemail.com", 2525))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("itayedriitay@gmail.com", "1AFDADFAC8A5597A8217C7620AD35D39DAA4");
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(mail);

                }
                return Json(new { success = true, message = "Thank you for your message!" });
            }
            catch (Exception)
            {
                // Handle exception
                return Json(new { success = false, message = "There was a problem sending your message. Please try again later." });
            }

        }
        

        public IActionResult MeatGuide()
        {
            return View();
        }

        public IActionResult FoodPreservationGuide()
        {
            return View();
        }

        public IActionResult FreezingFoodGuide()
        {
            return View();
        }
        public IActionResult SpicesGuide()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Policy = "connectionOnly")]
        public IActionResult AddComment(AddComment model)
        {
            if (ModelState.IsValid)
            {
                var comment = new AddComment
                {
                    RecipeID = model.RecipeID,
                    UserName = model.UserName,
                    Email = model.Email,
                    Rating = model.Rating,
                    CommentText = model.CommentText,
                    CreatedAt = DateTime.Now
                };

                _webApplication1Context.Add(comment);
                _webApplication1Context.SaveChanges();

                // Redirect back to the same page
                return Redirect($"http://localhost:5030/recipe/{model.RecipeID}");

            }

            // If model is not valid, return to the form with errors
            return View("CommentForm", model);
        }


        [Route("api/GetCommentstoRecipes")]
        [HttpGet]
        public JsonResult GetComments(int recipeId)
        {
            var comments = _webApplication1Context.AddComment
                .Where(c => c.RecipeID == recipeId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    CommentId = c.CommentID,
                    UserName = c.UserName,
                    CommentText = c.CommentText,
                    Rating = c.Rating,
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToList();

            return Json(comments);
        }


        //קונטרולר לספירת כניסות לדפי מתכונים--להמשיך לעבוד להבין איך קוראים לזה כדי שיספור
        [HttpPost]
        public IActionResult CountEntrances(int id)
        {
            var recipe = _context.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            // הגדלת מספר הכניסות
            recipe.Entrances += 1;

            // שמירת השינוי בבסיס הנתונים
            _context.SaveChanges();

            return Ok();
        }
        [HttpGet]
        [Route("api/popularRecipes")]
        public IActionResult GetPopularRecipes()
        {
            var popularRecipes = _context.Recipes
                .OrderByDescending(r => r.Entrances)
                .Take(4)
                .Select(r => new
                {
                    r.RecipeID,
                    r.RecipeName
                })
                .ToList();

            return Json(popularRecipes);
        }

        [Authorize]
        public IActionResult Conversion()
        {
            return View();
        }
        [Authorize]
        public IActionResult BMIcalculator()
        {


            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult BMIcalculator(double weight, double height, int age, string gender)
        {
            // המרת הגובה למטרים
            height = height / 100;
            double bmi = weight / (height * height);
            ViewBag.BMI = bmi;
            ViewBag.Age = age;
            ViewBag.Gender = gender;
            return View();
        }

        [Authorize]
        public IActionResult Calories()
        {
            return View();
        }


        // פעולה להוספת מתכון למועדפים
        /* [HttpPost]
         [Authorize]
         [Route("RecipeWeb/AddToFavorites/{recipeId}")]
         public async Task<IActionResult> AddToFavorites(int recipeId)
         {
             var userId = _userManager.GetUserId(User);
             var userName = User.Identity.Name;

             if (!_webApplication1Context.FavoriteModel.Any(f => f.UserId.ToString() == userId && f.RecipeId == recipeId))
             {
                 var favorite = new FavoriteModel
                 {
                     UserId = Guid.Parse(userId),
                     UserName = userName,
                     RecipeId = recipeId,
                     Date = DateTime.Now,
                     FavoriteIcon = true,
                     LikeIcon = false
                 };

                 _webApplication1Context.FavoriteModel.Add(favorite);
                 await _webApplication1Context.SaveChangesAsync();
             }

             return Ok();
         }*/

        [HttpGet]
        [Authorize]
        [Route("RecipeWeb/IsRecipeInFavorites/{recipeId}")]
        public IActionResult IsRecipeInFavorites(int recipeId)
        {
            var userId = _userManager.GetUserId(User);
            var isFavorite = _webApplication1Context.FavoriteModel
                             .Any(f => f.UserId.ToString() == userId && f.RecipeId == recipeId);

            return Ok(new { isFavorite = isFavorite });
        }

        [HttpPost]
        [Authorize]
        [Route("RecipeWeb/AddToFavorites/{recipeId}")]
        public async Task<IActionResult> AddToFavorites(int recipeId)
        {
            var userId = _userManager.GetUserId(User);

            var existingFavorite = _webApplication1Context.FavoriteModel
                                   .FirstOrDefault(f => f.UserId.ToString() == userId && f.RecipeId == recipeId);

            if (existingFavorite != null)
            {
                return Ok(new { success = false, alreadyInFavorites = true });
            }

            var favorite = new FavoriteModel
            {
                UserId = Guid.Parse(userId),
                UserName = User.Identity.Name,
                RecipeId = recipeId,
                Date = DateTime.Now,
                FavoriteIcon = true,
                LikeIcon = false
            };

            _webApplication1Context.FavoriteModel.Add(favorite);
            await _webApplication1Context.SaveChangesAsync();

            return Ok(new { success = true });
        }


        //הצגת המתכונים בעמוד המעודפים
        [Authorize]
        public async Task<IActionResult> FavoritePage()
        {
            var userId = _userManager.GetUserId(User);

            // קבלת רשימת המועדפים
            var favoriteIds = await _webApplication1Context.FavoriteModel
                                .Where(f => f.UserId.ToString() == userId)
                                .Select(f => f.RecipeId)
                                .ToListAsync();

            // קבלת פרטי המתכונים מהמועדפים
            var favoriteRecipes = await _context.Recipes
                                  .Where(r => favoriteIds.Contains(r.RecipeID))
                                  .ToListAsync();

            return View(favoriteRecipes);
        }

        //פעולה להסרה מתכון מהמעודפים
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int recipeId)
        {
            var userId = _userManager.GetUserId(User);
            var favorite = await _webApplication1Context.FavoriteModel
                                 .FirstOrDefaultAsync(f => f.UserId.ToString() == userId && f.RecipeId == recipeId);

            if (favorite != null)
            {
                _webApplication1Context.FavoriteModel.Remove(favorite);
                await _webApplication1Context.SaveChangesAsync();
            }

            return RedirectToAction("FavoritePage");
        }

        //קונטרולר הרשמה לניוזליטור
        [HttpPost("RecipeWeb/subscribe")]
        public IActionResult Subscribe([FromBody] SubscriberModel subscriber)
        {
            
            if (string.IsNullOrEmpty(subscriber.Email))
            {
                return BadRequest("Email is required");
            }

            // בדיקה אם האימייל כבר קיים
            if (_webApplication1Context.SubscriberModel.Any(s => s.Email == subscriber.Email))
            {
                return Conflict("Email is already subscribed");
            }

            subscriber.SubscriptionDate = DateTime.Now;
            _webApplication1Context.SubscriberModel.Add(subscriber);
            _webApplication1Context.SaveChanges();

            return Ok("Subscription successful");
        }






    }


}


