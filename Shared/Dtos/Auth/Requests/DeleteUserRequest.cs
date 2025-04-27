using System;

namespace Shared.Dtos.Auth.Requests;

public class DeleteUserRequest
{
    public string? ConfirmationText { get; set; }
}