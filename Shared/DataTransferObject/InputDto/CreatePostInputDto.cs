﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject.InputDto;

public record CreatePostInputDto(
    string? PostContent,
    double X,
    double Y,
    List<IFormFile> PhotoFiles
);
