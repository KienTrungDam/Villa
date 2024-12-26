using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Villa.Utility;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO model = new ();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            APIResponse result = await _authService.LoginAsync<APIResponse>(model);
            if(result.IsSuccess && result != null)
            {
                LoginResponseDTO login = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(result.Result));
                //important 
                //lay role trong token  
                var hander = new JwtSecurityTokenHandler();
                var token = hander.ReadJwtToken(login.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                //name trong identity phai trung voi cai trong claim va name indentity la id cua user
                identity.AddClaim(new Claim(ClaimTypes.Name, token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, token.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(SD.SessionToken, login.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", result.ErrorMessages.FirstOrDefault());
                return View(model);
            }



        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO model)
        {
            APIResponse result = await _authService.RegisterAsync<APIResponse>(model);
            if (result.IsSuccess)
            {
                TempData["success"] = "Registration Successful";
                return RedirectToAction("Login");
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");   
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
