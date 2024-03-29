﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GyMeApplication.IServices;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    HttpContext HttpContent { get; }
    string Email { get; }
    bool EmailConfirmed { get; }
    Guid UserId { get; }
}