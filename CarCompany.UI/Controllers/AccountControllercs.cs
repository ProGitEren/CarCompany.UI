using AutoMapper;
using Infrastructure.DTO.Dto_Users;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Models.ViewModels.Addresses;
using Infrastructure.Models.ViewModels.Engines;
using Infrastructure.Models.ViewModels.Users;
using Infrastructure.Services;
using Infrastucture.Helpers;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CarCompany.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly EncryptionService _encryptionservice;

        public AccountController(IUserService userService, IMapper mapper, Serilog.ILogger logger, EncryptionService encryptionService)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _encryptionservice = encryptionService;
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
                    var encryptedmessage = _encryptionservice.Encrypt(model.Password);
                    var logindto = _mapper.Map<LoginDto>(model);
                    logindto.EncryptedPassword = encryptedmessage;

                    var user = await _userService.LoginAsync(logindto);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                        _logger.Warning("Login failed for user {Email}: Invalid username or password", model.Email);
                        return View(model);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Token", user.Token)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(user.Token);

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
                catch (Exception ex)
                {
                    ExceptionHelper.HandleException(ex, model.Email, _logger, ModelState, "Login");
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
                    var encryptedpassword = _encryptionservice.Encrypt(model.Password);
                    var registerdto = _mapper.Map<RegisterDto>(model);
                    registerdto.EncryptedPassword = encryptedpassword;

                    var user = await _userService.RegisterAsync(registerdto);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User cannot be instantiated.");
                        _logger.Warning("Registration failed for user {Email}: User cannot be instantiated", model.Email);
                        return View(model);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Token", user.Token)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(user.Token);

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
                

                catch (Exception ex)
                {
                    ExceptionHelper.HandleException(ex, model.Email, _logger, ModelState, "Register");
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var encryptedpassword = _encryptionservice.Encrypt(model.Password);
                    var registerdto = _mapper.Map<RegisterDto>(model);
                    registerdto.EncryptedPassword = encryptedpassword;

                    var user = await _userService.RegisterAdminAsync(registerdto);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User cannot be instantiated.");
                        _logger.Warning("Registration failed for user {Email}: User cannot be instantiated", model.Email);
                        return View(model);
                    }

                    TempData["success"] = "You are successfully registered and logged in.";
                    _logger.Information("User {Email} registered successfully", user.Email);
                    return RedirectToAction("UsersList", "Account");
                }
                catch (Exception ex)
                {
                    ExceptionHelper.HandleException(ex, model.Email, _logger, ModelState, "RegisterAdmin");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PaginatedUsers(string? sortoptions, string? email,string? phone ,string? filterVehicleId,string? role,
                   Guid? filterAddressId, string? searchInput, int PageNumber = 1, int PageSize = 10)
        {

            var param = new UserParams
            {
                Search = searchInput,
                Sorting = sortoptions,
                Phone = phone,
                Email = email,
                VehicleId = filterVehicleId,
                PageNumber = PageNumber,
                Pagesize = PageSize,
                AddressId = filterAddressId,
                Role = role,
            };

            // Preserve filter, sort, and pagination parameters in ViewData
            ViewData["sortoptions"] = sortoptions;
            ViewData["filterVehicleId"] = filterVehicleId;
            ViewData["filterAddressId"] = filterAddressId;
            ViewData["searchInput"] = searchInput;
            ViewData["phone"] = phone;
            ViewData["email"] = email;
            ViewData["role"] = role;


            try
            {
                var result = await _userService.GetUsersAsync(param);
                if (result == null)
                {
                    ModelState.AddModelError("", "The Users could not be found.");
                    _logger.Warning("User retrieval failed: Model could not found");
                }
                _logger.Information("User retrieval successful.");
                var model = _mapper.Map<Pagination<UserViewModel>>(result);

                IndexPageErrorsHelper.ShowTempDataErrors(ModelState, TempData);
                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetUsers");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("Index", "Home");
        }



        [Authorize]
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
                _logger.Information("Profile retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Profile");
            }

            if(ModelState.IsValid)
                return View(model);
            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UsersList()
        {
            var model = new List<UserViewModel>();
            try
            {
                model = await _userService.UsersListAsync() as List<UserViewModel>;
                if (model == null)
                {
                    ModelState.AddModelError("", "The Users could not be found.");
                    _logger.Warning("Users list retrieval failed: Users not found");
                }
                _logger.Information("List of users retrieved successfully.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UsersList");
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
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, email, _logger, ModelState, "AddressDetail");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return User.IsInRole("Admin") ? RedirectToAction("PaginatedUsers","Account") : RedirectToAction("Profile","Account");
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
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateAddress");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return User.IsInRole("Admin") ? RedirectToAction("PaginatedUsers", "Account") : RedirectToAction("Profile", "Account");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(AddressViewModel model)
        {
            try
            {
                var address = await _userService.UpdateAddressAsync(model);
                _logger.Information("Address detail updated");
                return User.IsInRole("Admin") ? RedirectToAction("PaginatedUsers", "Account") : RedirectToAction("Profile", "Account");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateAddress");
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
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateUser");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return User.IsInRole("Admin") ? RedirectToAction("PaginatedUsers", "Account") : RedirectToAction("Profile", "Account");

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
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateUser");
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string? email)
        {
            try
            {
                var user = await _userService.ProfileByEmailAsync(email);
                _logger.Information("User detail retrieved for user {Email}", email);
                return View(user);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, email, _logger, ModelState, "DeleteUser");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedUsers", "Account");

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUserPost(string? email)
        {
            try
            {
                var resultstring = await _userService.DeleteUserAsync(email);
                _logger.Information("User deleted");
                return RedirectToAction("PaginatedUsers", "Account");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, email, _logger, ModelState, "DeleteUserPost");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedUsers", "Account");
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
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Logout");
                IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}