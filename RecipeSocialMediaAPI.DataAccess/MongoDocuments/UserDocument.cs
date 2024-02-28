﻿using RecipeSocialMediaAPI.DataAccess.MongoConfiguration;

namespace RecipeSocialMediaAPI.DataAccess.MongoDocuments;

[MongoCollection("User")]
public record UserDocument(
    string Handler,
    string UserName,
    string Email,
    string Password,
    int Role,
    string? ProfileImageId = null,
    DateTimeOffset? AccountCreationDate = null,
    string? Id = null,
    List<string>? PinnedConversationIds = null,
    List<string>? BlockedConnectionIds = null
) : MongoDocument(Id);