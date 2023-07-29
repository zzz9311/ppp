using AutoMapper;
using Core.Exceptions;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetPPP.BLL.Interfaces;
using PetPPP.BLL.Interfaces.DTO;
using PetPPP.BLL.Interfaces.Users;
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

        public AuthController(IUserService userService,
            ITokenService tokenService,
            IMapper mapper,
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

            var user = _mapper.Map<UserChangeableDTO>(model);

            await _userService.AddAsync(user, token);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model, CancellationToken token)
        {
            if (model == null)
                return BadRequest("Model is empty");
            var userId = await _userService.LoginAsync(_mapper.Map<LoginDTO>(model), token);
            if (userId == Guid.Empty)
                return Unauthorized();
            var accessToken = _tokenService.GenerateAccessToken(userId);
            var refreshToken = _tokenService.GenerateRefreshToken();
            Request.Headers.TryGetValue("user-agent", out var deviceInfo);
            await _refreshTokenService.SetRefreshTokenToUserAsync(userId, refreshToken, deviceInfo, token);

            return Ok(new AuthenticatedResponse(accessToken, refreshToken));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenApiModel model, CancellationToken token)
        {
            if (model == null)
                return BadRequest("Model is empty");

            var accessToken = model.AccessToken;
            var refreshToken = model.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            Request.Headers.TryGetValue("user-agent", out var deviceInfo);
            var userId = new Guid(principal.FindFirst("Id").Value);
            var userRefreshToken = await _refreshTokenService.GetUserRefreshTokenAsync(userId, deviceInfo, token);
            
            if (userRefreshToken == null || refreshToken != userRefreshToken.RefreshToken ||
                userRefreshToken.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var newAccessToken = _tokenService.GenerateAccessToken(userId);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _refreshTokenService.SetRefreshTokenToUserAsync(userId, newRefreshToken, deviceInfo, token);

            return Ok(new AuthenticatedResponse(newAccessToken, newRefreshToken));
        }

        [HttpPost("revoke/{userId}")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromRoute] Guid userId, CancellationToken token)
        {
            var user = await _userService.GetAsync(userId, token) ??
                       throw new EntityNotFoundException("User was not found");
            
            Request.Headers.TryGetValue("user-agent", out var deviceInfo);
            if (await _refreshTokenService.RevokeUserRefreshTokenAsync(user.Id, deviceInfo, token))
            {
                return Ok();
            }
            
            return BadRequest("Revoke token failed");
        }
    }
}
