using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DistanceLearning.Areas.Identity.Pages.User;

[Authorize(Roles = Data.Authorization.Constants.Roles.ADMIN)]
public class StudentUsersModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<StudentUsersModel> _logger;

    public StudentUsersModel(
        UserManager<IdentityUser> userManager,
        ILogger<StudentUsersModel> logger)
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
        Input.Users = await _userManager.GetUsersInRoleAsync(Data.Authorization.Constants.Roles.STUDENT);
    }

    public async Task<IActionResult> OnPostAsync(string userId)
    {
        // �������� ������������
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRolesAsync(user, new string[] { Data.Authorization.Constants.Roles.STUDENT });
        }

        return Page();
    }
}
