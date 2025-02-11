using AutoMapper;
using Entites.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts.Models;
using Shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObject.Auth;
using System.Security.Cryptography;

namespace Service.Models;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User? _user;

    public AuthenticationService(ILogger<AuthenticationService> logger, IMapper mapper,
                                UserManager<User> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(RegistUserDto registUserDto)
    {
        var user = _mapper.Map<User>(registUserDto);
        var result = await _userManager.CreateAsync(user, registUserDto.Password ?? "0");
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, registUserDto.Roles);
        return result;
    }
    
    public async Task<bool> ValidateUser(string userEmail)
    {
        _user = await _userManager.FindByEmailAsync(userEmail);

        var result = _user != null;

        if (!result)
            _logger.LogWarning($"{nameof(ValidateUser)}: 인증 실패. 잘못된 사용자 이름 또는 비밀번호.");
    
        return result;
    }
    
    //public async Task<string> CreateToken()
    //{
    //    var signingCredentials = GetSigningCredentials();
    //    var claims = await GetClaims();
    //    var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
    //    return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    //}

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        
        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<TokenDto> CreateTokenWithUpdateFcmToken(string fcmToken, bool populateExp)
    {
        _user.FcmToken = fcmToken;

        return await CreateToken(populateExp);
    }

    //public async Task<bool> ExpiringToken(string userId)
    //{
    //    bool result = false;
    //
    //    await _userManager.expired
    //
    //    return result;
    //}

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetConnectionString("JwtSettings-SECRET"));
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, _user.Id),
            new Claim(ClaimTypes.Email, _user.Email),
            new Claim(ClaimTypes.Name, _user.UserName)
        };
    
        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
    
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var validIssuer = _configuration.GetConnectionString("JwtSettings-validIssuer");
        var validAudience = _configuration.GetConnectionString("JwtSettings-validAudience");
        var expire = _configuration.GetConnectionString("JwtSettings-expire");


        var tokenOptions = new JwtSecurityToken
        (
            issuer: validIssuer,
            audience: validAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(expire)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var validIssuer = _configuration.GetConnectionString("JwtSettings-validIssuer");
        var validAudience = _configuration.GetConnectionString("JwtSettings-validAudience");
        var key = Encoding.UTF8.GetBytes(_configuration.GetConnectionString("JwtSettings-SECRET"));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = false,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var userEmail = principal.FindFirst(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(userEmail?.Value);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            _logger.LogError($"RefreshToken, userEmail: {userEmail}, user == null:{user == null}, user.RefreshToken != tokenDto.RefreshToken: {user.RefreshToken != tokenDto.RefreshToken}, user.RefreshTokenExpiryTime <= DateTime.Now: {user.RefreshTokenExpiryTime <= DateTime.Now}");
            throw new Exception($"RefreshToken, userEmail: {userEmail}, user == null:{user == null}, user.RefreshToken != tokenDto.RefreshToken: {user.RefreshToken != tokenDto.RefreshToken}, user.RefreshTokenExpiryTime <= DateTime.Now: {user.RefreshTokenExpiryTime <= DateTime.Now}");
        }

        _user = user;

        return await CreateToken(populateExp: false);
    }
}
