using Entites.Models;
using Shared.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface IUserService
{
    Task<User> RegistNewUser(SingleSignOnUserInfo userInfo, string fcmToken);
    Task<User> GetUserByEmail(string email, bool trackChanges);
    Task<User> GetUserById(string userId, bool trackChanges);
}
