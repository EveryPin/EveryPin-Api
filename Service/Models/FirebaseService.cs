using AutoMapper;
using Contracts.Repository;
using ExternalLibraryService;
using Microsoft.Extensions.Logging;
using Service.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;

namespace Service.Models;
internal sealed class FirebaseService : IFirebaseService
{
    private readonly ILogger<PostService> _logger;
    private readonly IRepositoryManager _repository;

    public FirebaseService(ILogger<PostService> logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<string> SendAlert(string message)
    {
        FirebaseAdmin.FirebaseApp.Create(new AppOptions()
        {
            Credential = Google.Apis.Auth.OAuth2.GoogleCredential.GetApplicationDefault()
        });

        var messages = new FirebaseAdmin.Messaging.Message()
        {
            Notification = new FirebaseAdmin.Messaging.Notification()
            {
                Title = "Alert",
                Body = message
            },
            Topic = "alert"
        };

        var response = await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAsync(messages);

        return response;
    }
}
