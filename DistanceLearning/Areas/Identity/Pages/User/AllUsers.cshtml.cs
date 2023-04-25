using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DistanceLearning.Areas.Identity.Pages;

public class AllUsersModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AllUsersModel> _logger;

    public AllUsersModel(
        UserManager<IdentityUser> userManager,
        ILogger<AllUsersModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public List<IdentityUser> Users { get; set; } = new();
    }

    public void OnGet()
    {
        Input.Users = _userManager.Users.ToList();
    }
}
