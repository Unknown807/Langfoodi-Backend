﻿namespace RecipeSocialMediaAPI.Application.DTO.Users;

public record UserDTO
{
    required public string Id { get; set; }
    required public string Handler { get; set; }
    required public string UserName { get; set; }
    required public string Email { get; set; }
    required public string Password { get; set; }
    public DateTimeOffset AccountCreationDate { get; set; }
}
