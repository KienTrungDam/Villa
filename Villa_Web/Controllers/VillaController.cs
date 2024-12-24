using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_API.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> CreateVilla()
        {           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villaDTO)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(villaDTO);
                if (response.IsSuccess)
                {
                    TempData["success"] = "Created seccessfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Created failed";
            return View(villaDTO);
        }

        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);
            if (response.IsSuccess)
            {
                VillaDTO villaDTO = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                var temp = _mapper.Map<VillaUpdateDTO>(villaDTO);
                return View(temp);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaUpdateDTO)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(villaUpdateDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Updated seccessfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Update failed";
            return View(villaUpdateDTO);
        }
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);
            if (response.IsSuccess)
            {
                VillaDTO villaDTO = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                var temp = _mapper.Map<VillaUpdateDTO>(villaDTO);
                return View(temp);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO villaDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.DeleteAsync<APIResponse>(villaDTO.Id);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Deleted seccessfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Deleted failed";
            return View(villaDTO);
        }
    }
}
