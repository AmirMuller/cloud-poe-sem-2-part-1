using cloud_poe_sem_2_part_1.Models;
using cloud_poe_sem_2_part_1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace cloud_poe_sem_2_part_1.Controllers;

public class AccountController : Controller
{
    private readonly UserTableService _userService;

    public AccountController(IConfiguration config)
    {
        var connStr = config.GetConnectionString("AzureTableStorage");
        _userService = new UserTableService(connStr);
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userService.GetUserAsync(model.Username);
        if (user != null && user.PasswordHash == HashPassword(model.Password))
        {
            // Example only — replace with proper cookie/auth setup
            HttpContext.Session.SetString("User", user.RowKey);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login.");
        return View(model);
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var existing = await _userService.GetUserAsync(model.Username);
        if (existing != null)
        {
            ModelState.AddModelError("", "User already exists.");
            return View(model);
        }

        var newUser = new UserEntity
        {
            RowKey = model.Username,
            FullName = model.FullName,
            Email = model.Email,
            PasswordHash = HashPassword(model.Password)
        };

        await _userService.AddUserAsync(newUser);
        return RedirectToAction("Login");
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
