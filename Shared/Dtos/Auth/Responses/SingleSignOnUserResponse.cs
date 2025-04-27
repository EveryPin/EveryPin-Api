using Shared.Dtos.Common;
using System;

namespace Shared.Dtos.Auth.Responses;

public class SingleSignOnUserResponse
{
    public string? UserNickName { get; set; }
    public string? UserEmail { get; set; }
    public string? PlatformCode { get; set; }

    public SingleSignOnUserResponse? FromSingleSignOnUserInfo(SingleSignOnUserInfo info)
    {
        if (info == null) return null;

        return new SingleSignOnUserResponse
        {
            UserNickName = info.UserNickName,
            UserEmail = info.UserEmail,
            PlatformCode = info.PlatformCode != null ? info.PlatformCode.ToString() : null
        };
    }
}
