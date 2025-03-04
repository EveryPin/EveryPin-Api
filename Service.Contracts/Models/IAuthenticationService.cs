using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObject;
using Shared.DataTransferObject.Auth;

namespace Service.Contracts.Models;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(RegistUserDto registUserDto);
    Task<bool> ValidateUser(string userEmail);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> CreateTokenWithUpdateFcmToken(string fcmToken, bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
    Task<IdentityResult> Logout(string userId);
}
