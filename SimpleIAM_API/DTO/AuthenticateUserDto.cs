﻿namespace SimpleIAM_API.DTO
{
    public class AuthenticateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
