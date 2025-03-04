using AutoMapper;
using Contracts.Repository;
using Entites.Models;
using Microsoft.Extensions.Logging;
using Service.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models;

internal sealed class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UserService(ILogger<UserService> logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<User> GetUserByEmail(string email, bool trackChanges)
    {
        var user = await _repository.User.GetUserByEmail(email, trackChanges);

        return user;
    }

    public async Task<User> GetUserById(string userId, bool trackChanges)
    {
        var user = await _repository.User.GetUserById(userId, trackChanges);

        return user;
    }
}
