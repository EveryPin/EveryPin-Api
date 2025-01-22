using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.Auth;

public record GoogleTokenDto(
    string? accessToken,
    int? expires_in,
    string? refreshToken,
    string? scope,
    string? id_token
);
