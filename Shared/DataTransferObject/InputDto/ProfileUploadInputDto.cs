using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.InputDto;

public class ProfileUploadInputDto
{
    public string? TagId { get; set; }
    public string? Name { get; set; }
    public string? SelfIntroduction { get; set; }
    public IFormFile? PhotoFiles { get; set; }
}
