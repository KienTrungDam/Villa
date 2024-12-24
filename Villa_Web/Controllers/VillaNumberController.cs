using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Villa_API.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Models.VM;
using Villa_Web.Services;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaVM = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {

                villaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }); 
            }
            return View(villaVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(villaNumberVM.VillaNumber);
                if (response.IsSuccess && response != null)
                {
                    TempData["success"] = "Created seccessfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            //TempData["error"] = "Created failed";
            //khi loi chay lai view va lay lai list villa
            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {

                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result))
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            return View(villaNumberVM);

        }

        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaVM = new();
            
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            // gan gia tri cho villaVM.VillaNumber
            if (response.IsSuccess)
            {               
                VillaNumberDTO villaNumberDTO = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(villaNumberDTO);
            }
            //gan gia tri cho villaVM.VillaList
            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {

                villaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(villaVM);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaUpdateVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(villaUpdateVM.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Updated seccessfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            //list villa
            var res = await _villaService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {

                villaUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result))
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(villaUpdateVM);
            }
            //error
            TempData["error"] = "Update failed";
            return View(villaUpdateVM);
        }
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaVM = new();

            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            // gan gia tri cho villaVM.VillaNumber
            if (response.IsSuccess)
            {
                VillaNumberDTO villaNumberDTO = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaVM.VillaNumber = villaNumberDTO;
            }
            //gan gia tri cho villaVM.VillaList
            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {

                villaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(villaVM);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM villaVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.DeleteAsync<APIResponse>(villaVM.VillaNumber.VillaNo);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Deleted seccessfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }
            TempData["error"] = "Deleted failed";
            return View(villaVM);
        }

    }
}
