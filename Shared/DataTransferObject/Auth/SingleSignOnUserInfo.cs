using Entites.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.Auth;

public class SingleSignOnUserInfo
{
    public string? UserNickName { get; set; }
    public string? UserEmail { get; set;}
    public CodePlatform PlatformCode { get; set; }
}
