using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repository.Models;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUser(bool trackChanges);
    Task<User> GetUserByEmail(string email, bool tackChanges);
    Task<User> GetUserById(string userId, bool tackChanges);
}
