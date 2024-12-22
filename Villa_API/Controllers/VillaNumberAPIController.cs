using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Villa_API.Data;
using Villa_API.Models;
using Villa_API.Models.DTO;
using Villa_API.Repository.IRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Villa_API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VillaNumberAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> all = await _unitOfWork.VillaNumber.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(all);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {                  
                if (villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _unitOfWork.VillaNumber.GetAsync(u => u.VillaNo == villaNo);
                if(villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createVillaNumberDTO)
        {
            try { 
                
                if (createVillaNumberDTO == null)
                {
                    return BadRequest(createVillaNumberDTO);
                }
                /*if(villaDTO.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }*/
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createVillaNumberDTO);
            
                //lay so id lon nhat trong list roi +1
                await _unitOfWork.VillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new {id = villaNumber.VillaNo}, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [HttpDelete("{villaNo:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try { 
                if (villaNo == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _unitOfWork.VillaNumber.GetAsync(u => u.VillaNo == villaNo);
                if (villaNumber == null)
                {
                    return NotFound();
                }
                await _unitOfWork.VillaNumber.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }
        [HttpPut("{villaNo:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, [FromBody] VillaNumberUpdateDTO updateVillaNumberDTO)
        {
            try { 
                if (villaNo != updateVillaNumberDTO.VillaNo || updateVillaNumberDTO == null)
                {
                    return BadRequest();
                }
                //entityframecore tu dong update theo id
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(updateVillaNumberDTO);
            
                await _unitOfWork.VillaNumber.UpdateAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            return _response;
        }

        [HttpPatch("{villaNo:int}", Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int villaNo, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (villaNo == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villaNumber = await _unitOfWork.VillaNumber.GetAsync(u => u.VillaNo == villaNo, tracked: false);
            VillaNumberUpdateDTO updateVillaNumber = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

            if (updateVillaNumber == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(updateVillaNumber, ModelState);
            VillaNumber model = _mapper.Map<VillaNumber>(updateVillaNumber);

            await _unitOfWork.VillaNumber.UpdateAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
