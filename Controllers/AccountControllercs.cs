using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using Infrastructure.Models.ViewModels;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CarCompany.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        
        public AccountController(UserService UserService,IMapper mapper, Serilog.ILogger logger)
        {
            _userService = UserService;
            _mapper = mapper;
            _logger = logger;
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
                    var user = await _userService.LoginAsync(model);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                        _logger.Warning("Login failed for user {Email}: Invalid username or password", model.Email);
                        return View(model);
                    }

                    // Create claims and sign in the user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Token", user.Token)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(user.Token);

                    // Handle custom claim type for roles
                    claims.AddRange(jwtToken.Claims.Where(c => c.Type == "role").Select(c => new Claim(ClaimTypes.Role, c.Value)));

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(30) });

                    TempData["success"] = "You are successfully logged in.";
                    _logger.Information("User {Email} logged in successfully", user.Email);
                    return RedirectToAction("Index", "Home");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", $"Login failed: {ex.Message}");
                    _logger.Warning("Login failed for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Access denied: {ex.Message}");
                    _logger.Warning("Access denied for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                    _logger.Error("An unexpected error occurred during login for user {Email}: {Message}", model.Email, ex.Message);
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
                    var user = await _userService.RegisterAsync(model);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User can not be instantiated.");
                        _logger.Warning("Registration failed for user {Email}: User cannot be instantiated", model.Email);
                        return View(model);
                    }

                    // Create claims and sign in the user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Token", user.Token)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(user.Token);

                    // Handle custom claim type for roles
                    claims.AddRange(jwtToken.Claims.Where(c => c.Type == "role").Select(c => new Claim(ClaimTypes.Role, c.Value)));

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(30) });

                    TempData["success"] = "You are successfully registered and logged in.";
                    _logger.Information("User {Email} registered and logged in successfully", user.Email);
                    return RedirectToAction("Index", "Home");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", $"Login failed: {ex.Message}");
                    _logger.Warning("Register failed for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Access denied: {ex.Message}");
                    _logger.Warning("Access denied for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Registration failed: {ex.Message}");
                    _logger.Warning("Registration failed for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                    _logger.Error("An unexpected error occurred during registration for user {Email}: {Message}", model.Email, ex.Message);
                }
            }

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View(new RegisterViewModel());
        }


        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userService.RegisterAdminAsync(model);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User can not be instantiated.");
                        _logger.Warning("Registration failed for user {Email}: User cannot be instantiated", model.Email);
                        return View(model);
                    }

                    // Create claims and sign in the user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Token", user.Token)
                    };

                    //var handler = new JwtSecurityTokenHandler();
                    //var jwtToken = handler.ReadJwtToken(user.Token);

                    //// Handle custom claim type for roles
                    //claims.AddRange(jwtToken.Claims.Where(c => c.Type == "role").Select(c => new Claim(ClaimTypes.Role, c.Value)));

                    //var claimsIdentity = new ClaimsIdentity(
                    //    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //await HttpContext.SignInAsync(
                    //    CookieAuthenticationDefaults.AuthenticationScheme,
                    //    new ClaimsPrincipal(claimsIdentity),
                    //    new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(30) });

                    TempData["success"] = "You are successfully registered and logged in.";
                    _logger.Information("User {Email} registered and logged in successfully", user.Email);
                    return RedirectToAction("UsersList", "Account");
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", $"Login failed: {ex.Message}");
                    _logger.Warning("Register failed for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Access denied: {ex.Message}");
                    _logger.Warning("Access denied for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Handle lockout or forbidden scenarios
                    ModelState.AddModelError("", $"Registration failed: {ex.Message}");
                    _logger.Warning("Registration failed for user {Email}: {Message}", model.Email, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                    _logger.Error("An unexpected error occurred during registration for user {Email}: {Message}", model.Email, ex.Message);
                }
            }

            return View(model);
        }

        [Authorize/*(Roles ="Buyer,Seller,Admin")*/]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var model = new UserViewModel();
            try
            {
                model = await _userService.ProfileAsync();
                if (model == null)
                {
                    ModelState.AddModelError("", "The User could not be found.");
                    _logger.Warning("Profile retrieval failed: User not found");
                }
                _logger.Information("Profile retriaval successfull.");
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Failed Result: {ex.Message}");
                _logger.Warning("Profile retrieval failed: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during profile retrieval: {Message}", ex.Message);
            }

            return View(model);
        }

        [Authorize("Admin")]
        [HttpGet]
        public async Task<IActionResult> UsersList()
        {
            var model = new List<UserViewModel>();
            try
            {
                model = await _userService.UsersListAsync() as List<UserViewModel>;
                if (model == null)
                {
                    ModelState.AddModelError("", "The User could not be found.");
                    _logger.Warning("Users list retrieval failed: Users not found");
                }
                _logger.Information("List of users retrieved successfully.");
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Failed Result: {ex.Message}");
                _logger.Warning("Users list retrieval failed: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during users list retrieval: {Message}", ex.Message);
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddressDetail(string? email)
        {
            try
            {
                var user = await _userService.ProfileByEmailAsync(email);
                var model = user.Address;
                _logger.Information("Address detail retrieved for user {Email}", email);
                return View(model);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Not Found: {ex.Message}");
                _logger.Warning("Address detail retrieval failed for current user: {Message}", email, ex.Message);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", $"Bad Request: {ex.Message}");
                _logger.Warning("Address detail retrieval failed for user {Email}: {Message}", email, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during address detail retrieval for user {Email}: {Message}", email, ex.Message);
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateAddress(Guid? Id)
        {
            var address = new AddressViewModel();

            try
            {
                address = await _userService.GetAddressAsync(Id);
               
                _logger.Information("Address successfully retrieved.");
                TempData["success"] = "Address successfully retrieved.";
                return View(address);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Not Found: {ex.Message}");
                _logger.Warning("Address retriaval failed : {Message}", ex.Message);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", $"Bad Request: {ex.Message}");
                _logger.Warning("Address retriaval failed : {Message}",  ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during address retriaval : {Message}", ex.Message);
            }

            return View(address);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(AddressViewModel model)
        {
            try
            {
                var address = await _userService.UpdateAddressAsync(model);
          
                _logger.Information("Address detail updated");
                if (User.IsInRole("Admin")) { return RedirectToAction("UsersList", "Account"); }
                else { return RedirectToAction("Profile", "Account"); }
                
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Not Found: {ex.Message}");
                _logger.Warning("Address detail update failed: {Message}", ex.Message);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", $"Bad Request: {ex.Message}");
                _logger.Warning("Address detail update failed: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during address detail update: {Message}",  ex.Message);
            }

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateUser()
        {
            try
            {
                var user = await _userService.ProfileAsync();
                var model = _mapper.Map<UpdateUserViewModel>(user);
                _logger.Information("User detail retrieved for current user");
                return View(model);
            }

            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Not Found: {ex.Message}");
                _logger.Warning("User detail retrieval failed for current user: {Message}", ex.Message);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", $"Bad Request: {ex.Message}");
                _logger.Warning("User detail retrieval failed for current user: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during address detail retrieval for current user: {Message}", ex.Message);
            }

            return View();


        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(model);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Token", user.Token)
                    };

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(user.Token);

                

                // Handle custom claim type for roles
                claims.AddRange(jwtToken.Claims.Where(c => c.Type == "role").Select(c => new Claim(ClaimTypes.Role, c.Value)));

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(30) });



                _logger.Information("User detail updated");

                return RedirectToAction("Profile", "Account");

            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Not Found: {ex.Message}");
                _logger.Warning("User detail update failed: {Message}", ex.Message);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", $"Bad Request: {ex.Message}");
                _logger.Warning("User detail update failed: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during User detail update: {Message}", ex.Message);
            }

            return View(model);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string? email)
        {
            try
            {
                var resultstring = await _userService.DeleteUserAsync(email);

                _logger.Information("User deleted");

                return RedirectToAction("UsersList", "Account");

            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("", $"Not Found: {ex.Message}");
                _logger.Warning("User detail update failed: {Message}", ex.Message);
            }
            catch (UIException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", $"Bad Request: {ex.Message}");
                _logger.Warning("User detail update failed: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                _logger.Error("An unexpected error occurred during User detail update: {Message}", ex.Message);
            }

            return RedirectToAction("UsersList","Account");

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
                _logger.Information("User successfully signed out");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to _logger out: {Message}", ex.Message);
                throw new Exception($"Failed to _logger out: {ex.Message}");
            }
        }


        // Other actions for user interactions
    }
}
