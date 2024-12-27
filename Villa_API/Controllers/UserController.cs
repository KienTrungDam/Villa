using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villa_API.Models;
using Villa_API.Models.DTO;
using Villa_API.Repository.IRepository;

namespace Villa_API.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserController : Controller
    {
        //private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        protected APIResponse _response;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _response = new APIResponse();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            //send the model to the repository
            var loginResponse = await _unitOfWork.User.Login(model);
            //check if the user is null
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect.");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            //return the response
            _response.Result = loginResponse;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool ifUserNameUnique = _unitOfWork.User.IsUniqueUser(model.UserName);
            // check if username is unique
            if (!ifUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }
            //send the model to the repository
            var user = await _unitOfWork.User.Register(model);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Something went wrong");
                return BadRequest(_response);
            }
            // thong bao thanh cong
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        

    }
}
