using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject;

public record RegistUserDto
{
    public string? Name { get; init; }
    public string? UserName { get; init; }
    public string? Password { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public int? PlatformCode { get; init; }
    public string? FcmToken { get; init; }
    public ICollection<string>? Roles { get; init; }
}
