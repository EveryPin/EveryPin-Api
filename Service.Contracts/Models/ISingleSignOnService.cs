using Shared.DataTransferObject.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface ISingleSignOnService
{
    Task<string> GetKakaoAccessToken(string code);
    Task<GoogleTokenDto> GetGoogleAccessToken(string code);
    Task<SingleSignOnUserInfo> GetKakaoUserInfo(string ssoAccessToken);
    Task<SingleSignOnUserInfo> GetGoogleUserInfo(string ssoAccessToken);
    Task<SingleSignOnUserInfo> GetGoogleUserInfoToIdToken(string googleIdToken);
}
