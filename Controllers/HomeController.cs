using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserDashboard.Models;

namespace UserDashboard.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("signin")]
    public IActionResult SigninView()
    {
        return View("SigninView");
    }
    [HttpPost("signin")]
    public IActionResult ValidaLogin(LoginUser userLogin)
    {

        if (ModelState.IsValid)
        {

            User? UserExist = _context.users.FirstOrDefault(u => u.Email == userLogin.EmailLogin);
            if (UserExist == null)
            {
                ModelState.AddModelError("EmailLogin", "Correo o contraseña invalidos");
                return View("Index");
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(userLogin, UserExist.Password, userLogin.PasswordLogin);
            if (result == 0)
            {
                ModelState.AddModelError("EmailLogin", "Correo o contraseña invalidos");
                return View("Index");
            }
            HttpContext.Session.SetInt32("UserId", UserExist.UserId);
            int RoleIdUser = UserExist.RoleId;
            if (RoleIdUser == 9)
            {
                return RedirectToAction("ViewAdmin");
            }

            return RedirectToAction("Dashboard");

        }
        else
        {
            return View("Index");
        }
    }
    [HttpGet("register")]
    public IActionResult RegisterView()
    {
        return View("RegisterView");
    }
    [HttpPost("register")]
    public IActionResult RegisterUser(User newUser)
    {
        int countUser = _context.users.Count();
        if (ModelState.IsValid)
        {

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            if (countUser > 0)
            {
                newUser.RoleId = 1;
            }
            else
            {
                newUser.RoleId = 9;
            }
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            if (countUser > 0)
            {
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("ViewAdmin");
        }
        else
        {
            return View("RegisterView");
        }
    }
    [SessionCheck]
    [HttpGet("dashboard/admin")]
    public IActionResult ViewAdmin()
    {
        return View("DashboardAdmin");
    }
    [SessionCheck]
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        return View("Dashboard");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //Encontrar la sesion 
        int? UserId = context.HttpContext.Session.GetInt32("UserId");
        if (UserId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}

