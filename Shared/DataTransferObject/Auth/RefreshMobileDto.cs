using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.Auth;

public record RefreshMobileDto(
    string UserEmail,
    string RefreshToken
);
