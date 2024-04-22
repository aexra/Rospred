using Backend.Web.Dtos.Roles;
using Backend.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Web.Controllers;

[Route("api/roles")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("{username}")]
    public async Task<IActionResult> GetRoles([FromRoute] string username)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

        //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == HttpContext.User.Identity.Name);

        //return Ok(user.UserName);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(_userManager.GetRolesAsync(user).Result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetRoles([FromBody] RolesInfoDto dto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.Username);

        if (user == null)
        {
            return NotFound($"User \"{dto.Username}\" not found");
        }

        List<IdentityRole> roles = new();

        foreach (var rolename in dto.Roles)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            if (role != null) roles.Add(role);
        }

        if (roles.Count == 0)
        {
            return NotFound($"No available roles found");
        }

        foreach (var role in roles)
        {
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Errors);
            }
        }

        return Ok(new RolesInfoDto() { Username = dto.Username, Roles = roles.Select(x => x.Name).ToList() });
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRoles([FromBody] RolesInfoDto dto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.Username);

        if (user == null)
        {
            return NotFound($"User \"{dto.Username}\" not found");
        }

        List<IdentityRole> roles = new();

        foreach (var rolename in dto.Roles)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            if (role != null) roles.Add(role);
        }

        if (roles.Count == 0)
        {
            return NotFound($"No available roles found");
        }

        foreach (var role in roles)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Errors);
            }
        }

        return Ok(new RolesInfoDto() { Username = dto.Username, Roles = roles.Select(x => x.Name).ToList() });
    }
}
