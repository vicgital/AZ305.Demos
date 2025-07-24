using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sqlapp.Models;
using sqlapp.Services;

namespace sqlapp.Pages
{
    public class IndexModel(ILogger<IndexModel> logger) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;

        public List<Course> Courses = []; 
        public void OnGet()
        {
            _logger.LogInformation("onGet({page})", "Index");
            CourseService productService = new ();
            Courses = productService.GetCourses();

        }
    }
}