using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace YourApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to inject IHttpContextAccessor for Cookie handling
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Display the email input form
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Handle form submission and set both Cookie and Session
        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            string email = form["email"];

            // Set a Cookie
            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(10),  // Set cookie expiration time
                Path = "/"
            };
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("email", email, cookieOptions);

            // Set a Session
            HttpContext.Session.SetString("email", email);

            return RedirectToAction("ReadCookie", "Home");
        }

        // GET: Read both Cookie and Session values
        public IActionResult ReadCookie()
        {
            // Read Cookie value
            string? cookieEmail = _httpContextAccessor.HttpContext?.Request.Cookies["email"];

            // Read Session value
            string? sessionEmail = HttpContext.Session.GetString("email");

            // Pass both Cookie and Session values to View
            ViewBag.CookieEmail = cookieEmail;
            ViewBag.SessionEmail = sessionEmail;

            return View();
        }

        // GET: Working with Session (display form)
        public IActionResult WorkingWithSession()
        {
            return View();
        }

        // POST: Save email into Session and Redirect
        [HttpPost]
        public IActionResult WorkingWithSession(IFormCollection form)
        {
            string email = form["email"];

            // Save email to Session
            HttpContext.Session.SetString("email", email);

            // Redirect to ReadSession action to display session value
            return RedirectToAction("ReadSession");
        }

        // GET: Read Session value
        public IActionResult ReadSession()
        {
            // Retrieve email from Session
            ViewBag.SessionEmail = HttpContext.Session.GetString("email");

            return View();
        }
    }
}
