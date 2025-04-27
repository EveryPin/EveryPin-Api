using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Common.Blob;

public class BlobResultDto
{
    public BlobDto Blob { get; set; }
    public string? Message { get; set; }
    public bool Error { get; set; }

    public BlobResultDto()
    {
        Blob = new BlobDto();
    }
}
