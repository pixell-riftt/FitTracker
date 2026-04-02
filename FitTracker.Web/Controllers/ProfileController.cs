using FitTracker.Services.Core.Contracts;
using FitTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitTracker.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(IProfileService profileService, IWebHostEnvironment environment)
        {
            _profileService = profileService;
            _environment = environment;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<IActionResult> View(string id)
        {
            string userId = id ?? GetUserId();
            await _profileService.EnsureProfileExistsAsync(userId, User.Identity?.Name ?? "user");
            var model = await _profileService.GetProfileAsync(userId);
            ViewBag.IsOwnProfile = userId == GetUserId();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            string userId = GetUserId();
            await _profileService.EnsureProfileExistsAsync(userId, User.Identity?.Name ?? "user");

            var model = await _profileService.GetProfileForEditAsync(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserProfileEditViewModel model, IFormFile? ProfileImage)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                string imagesFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                string filePath = Path.Combine(imagesFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                model.ProfileImageUrl = "/images/" + uniqueFileName;
            }

            await _profileService.UpdateProfileAsync(GetUserId(), model);
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("View", new { id = GetUserId() });
        }
    }
}