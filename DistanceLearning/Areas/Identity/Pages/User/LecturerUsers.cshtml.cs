using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DistanceLearning.Areas.Identity.Pages.User;

[Authorize(Roles = Data.Authorization.Constants.Roles.ADMIN)]
public class LecturerUsersModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<LecturerUsersModel> _logger;

    public LecturerUsersModel(
        UserManager<IdentityUser> userManager,
        ILogger<LecturerUsersModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public IList<IdentityUser> Users { get; set; } = new List<IdentityUser>();
    }

    public async Task OnGet()
    {
        Input.Users = await _userManager.GetUsersInRoleAsync(Data.Authorization.Constants.Roles.LECTURER);
    }

    public async Task<IActionResult> OnPostAsync(string userId)
    {
        // получаем пользователя
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRolesAsync(user, new string[] { Data.Authorization.Constants.Roles.LECTURER });
        }

        return Page();
    }
}
