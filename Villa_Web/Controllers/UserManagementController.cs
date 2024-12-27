using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa.Utility;
using Villa_API.Migrations;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserManagementController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexUser()
        {
            List<UserDTO> list = new();

            var response = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> DeleteUser(string Id)
        {
            UserDTO userDTO = new UserDTO();
            var response = await _userService.GetAsync<APIResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response.IsSuccess)
            {
                userDTO = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(response.Result));
                return View(userDTO);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.DeleteAsync<APIResponse>(userDTO.Id, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Deleted seccessfully";
                    return RedirectToAction(nameof(IndexUser));
                }
            }
            TempData["error"] = "Deleted failed";
            return View(userDTO);
        }
    }
}
