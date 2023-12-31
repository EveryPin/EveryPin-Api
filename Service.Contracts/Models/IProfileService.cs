﻿using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models
{
    public interface IProfileService
    {
        IEnumerable<Profile> GetAllProfile(bool trackChanges);
    }
}