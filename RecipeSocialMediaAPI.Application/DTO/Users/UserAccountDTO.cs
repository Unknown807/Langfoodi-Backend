﻿namespace RecipeSocialMediaAPI.Application.DTO.Users;

public class UserAccountDTO
{
    required public string Id { get; set; }
    required public string Handler { get; set; }
    required public string UserName { get; set; }
    public DateTimeOffset AccountCreationDate { get; set; }
}
