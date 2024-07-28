using AutoMapper;
using CarCompany.UI.DTO;
using CarCompany.UI.Exceptions;
using CarCompany.UI.Models.ViewModels;
using CarCompany.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace CarCompany.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IMapper _mapper;

        public AccountController(ApiService apiService,IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _apiService.LoginAsync(model);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                        return View(model);
                    }

                    // Create claims and sign in the user
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Token", user.Token)
                    };
                    
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    try
                    {
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(30)});
                    }
                    catch (Exception ex) 
                    {
                        throw new Exception(ex.Message);
                    
                    }


                    TempData["success"] = "You are successfully logged in.";
                    return RedirectToAction("Index", "Home");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                
                    ModelState.AddModelError("", $"Login failed: {ex.Message}");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Access denied: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _apiService.RegisterAsync(model);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User can not be instantiated.");
                        return View(model);
                    }

                    // Create claims and sign in the user
                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Token", user.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    try
                    {
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(30) });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);

                    }


                    TempData["success"] = "You are successfully registered and logged in.";
                    return RedirectToAction("Index", "Home");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
                {

                    ModelState.AddModelError("", $"Login failed: {ex.Message}");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Access denied: {ex.Message}");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Registeration failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                }
            }

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        
        {
            var model = new UserViewModel();
            try
            {
                model = await _apiService.ProfileAsync();
                if (model == null)
                {
                    ModelState.AddModelError("", "The User could not be found.");

                }

               
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ModelState.AddModelError("", $"Failed Result: {ex.Message}");
               
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UsersList()

        {
            var model = new List<UserViewModel>();
            try
            {
                
                model = await _apiService.UsersListAsync() as List<UserViewModel>;
                if (model == null)
                {
                    ModelState.AddModelError("", "The User could not be found.");

                }


            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ModelState.AddModelError("", $"Failed Result: {ex.Message}");

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");

            }

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddressDetail(string? email) 
        {
            
                try
                {
                    var user = await _apiService.ProfileByEmailAsync(email);
                    var model = user.Address;
                    return View(model);

                }
                
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {

                    ModelState.AddModelError("", $"Not Found: {ex.Message}");

                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
                {

                    ModelState.AddModelError("", $"Bad Request: {ex.Message}");

                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");

                }

            return View();            
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Response.Cookies.Delete("JwtToken", new CookieOptions 
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                });
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["success"] = "You are successfully signed out.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            
            {
                throw new Exception($"Failed to log out: {ex.Message}");

            }


           
        }


        // Other actions for user interactions
    }
}
