using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using PetPPP.BLL.Interfaces;
using PetPPP.BLL.Interfaces.DTO;
using PetPPP.JWT;
using PetPPP.JWT.Services;
using PetPPP.Models;
using PetPPP.Responses;

namespace PetPPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper,
            IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
            _refreshTokenService = refreshTokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model, CancellationToken token)
        {
            if (model == null)
                return BadRequest("Model is empty");
            if (await _userService.IsUserWithUsernameExists(model.Username, token))
                return StatusCode(StatusCodes.Status422UnprocessableEntity, "Пользователь с таким ником уже существует");

            var user = _mapper.Map<AppUserDTO>(model);

            await _userService.AddUserAsync(user, token);
            return RedirectToAction("Login");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken token)
        {
            if (model == null)
                return BadRequest("Model is empty");
            var userId = await _userService.LoginUserAsync(_mapper.Map<AppUserDTO>(model), token);
            if (userId == Guid.Empty)
                return Unauthorized();
            var accessToken = _tokenService.GenerateAccessToken(userId);
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _refreshTokenService.SetRefreshTokenToUserAsync(userId, refreshToken, token);

            return Ok(new AuthenticatedResponse(accessToken, refreshToken));
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(TokenApiModel model)
        {
            if (model == null)
                return BadRequest("Model is empty");

            var accessToken = model.AccessToken;
            var refreshToken = model.RefreshToken;    


        }
    }
}
