using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.ViewComponents
{
    public class RecipeReviewViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Temporary placeholder data. Bind this dynamically later.
            var reviews = new List<dynamic>
            {
                new { Username = "John Doe", Rating = 4, Text = "This recipe was amazing! Highly recommended.", Date = "2 days ago", IsPositive = true },
                new { Username = "Jane Smith", Rating = 2, Text = "Not very good. The taste was off.", Date = "3 days ago", IsPositive = false },
            };

            return View(reviews);
        }
    }
}
