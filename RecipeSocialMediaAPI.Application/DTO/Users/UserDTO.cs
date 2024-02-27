﻿namespace RecipeSocialMediaAPI.Application.DTO.Users;

public record UserDTO(
    string Id, 
    string Handler, 
    string UserName, 
    string Email, 
    string Password, 
    List<string> PinnedConversationIds,
    List<string> BlockedConnectionIds,
    string? ProfileImageId = null,
    DateTimeOffset? AccountCreationDate = null
);