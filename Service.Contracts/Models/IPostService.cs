﻿using Shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models
{
    public interface IPostService
    {
        IEnumerable<PostDto> GetAllPost(bool trackChanges);
    }
}
