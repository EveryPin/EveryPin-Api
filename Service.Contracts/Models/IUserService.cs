using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface IUserService
{
    Task<User> GetUserByEmail(string email, bool trackChanges);
    Task<User> GetUserById(string userId, bool trackChanges);
}
