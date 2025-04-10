using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject;

public record ProfileDto
(
    int Id,
    Guid UserId,
    string? ProfileDisplayId,
    string? Name,
    string? SelfIntroduction,
    string? PhotoUrl,
    DateTime? UpdatedDate,
    DateTime? CreatedDate
);
