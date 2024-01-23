﻿using RecipeSocialMediaAPI.Domain.Models.Messaging.Conversations;
using RecipeSocialMediaAPI.Domain.Models.Users;

namespace RecipeSocialMediaAPI.Application.Repositories.Messages;

public interface IConversationQueryRepository
{
    Conversation? GetConversationById(string id);

    Conversation? GetConversationByConnection(string connectionId);

    List<Conversation> GetConversationsByUser(IUserAccount userAccount);
}
