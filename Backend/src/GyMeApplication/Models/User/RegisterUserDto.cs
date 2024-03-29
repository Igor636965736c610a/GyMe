﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GyMeApplication.Models.User;

public class RegisterUserDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool PrivateAccount { get; set; }
    public GenderDto GenderDto { get; set; }
    public string? Description { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}