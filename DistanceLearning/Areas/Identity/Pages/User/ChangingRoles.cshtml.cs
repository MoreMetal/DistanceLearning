using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DistanceLearning.Areas.Identity.Pages;

[Authorize(Roles = Data.Authorization.Constants.Roles.ADMIN)]
public class ChangingRolesModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<ChangingRolesModel> _logger;

    public ChangingRolesModel(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<ChangingRolesModel> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public IList<string> UsersRoles { get; set; } = new List<string>();
        public List<IdentityRole> Roles { get; set; } = new();
    }

    public async Task OnGet(string userId)
    {
        Input.Roles = _roleManager.Roles.ToList();

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            Input.UsersRoles = await _userManager.GetRolesAsync(user);

            Input.Roles = _roleManager.Roles.ToList();
        }

    }

    public async Task<IActionResult> OnPostAsync(string userId, List<string> roles)
    {
        // �������� ������������
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // ������� ������ ����� ������������
            var userRoles = await _userManager.GetRolesAsync(user);
            // �������� ��� ����
            var allRoles = _roleManager.Roles.ToList();
            // �������� ������ �����, ������� ���� ���������
            var addedRoles = roles.Except(userRoles);
            // �������� ����, ������� ���� �������
            var removedRoles = userRoles.Except(roles);

            await _userManager.AddToRolesAsync(user, addedRoles);

            await _userManager.RemoveFromRolesAsync(user, removedRoles);
        }

        return RedirectToPage("AllUsers");
    }
}
