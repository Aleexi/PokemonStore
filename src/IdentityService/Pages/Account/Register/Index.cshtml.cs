using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IdentityService.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public bool RegisterSuccess { get; set; }

        public Index(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnGet([FromQuery] string returnUrl)
        {
            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // If user click cancel return user back to homepage
            if (Input.Button != "register") return Redirect("~/");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true
                };
                var result = _userManager.CreateAsync(user, Input.Password).Result;

                if (result.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, Input.FullName)
                    });

                    RegisterSuccess = true;
                }
            }

            return Page();
        }
    }
}
