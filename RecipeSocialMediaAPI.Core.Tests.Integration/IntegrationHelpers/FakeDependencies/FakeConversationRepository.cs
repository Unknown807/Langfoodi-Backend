﻿using RecipeSocialMediaAPI.Application.Repositories.Messages;
using RecipeSocialMediaAPI.Domain.Models.Messaging;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Connections;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Conversations;
using RecipeSocialMediaAPI.Domain.Models.Users;

namespace RecipeSocialMediaAPI.Core.Tests.Integration.IntegrationHelpers.FakeDependencies;

internal class FakeConversationRepository : IConversationQueryRepository, IConversationPersistenceRepository
{
    private readonly List<Conversation> _collection;

    public FakeConversationRepository()
    {
        _collection = new();
    }

    public Conversation? GetConversationByConnection(string connectionId) => _collection
        .FirstOrDefault(conversation => conversation is ConnectionConversation connConvo 
                                     && connConvo.Connection.ConnectionId == connectionId);

    public Conversation? GetConversationByGroup(string groupId) => _collection
        .FirstOrDefault(conversation => conversation is GroupConversation groupConvo
                                     && groupConvo.Group.GroupId == groupId);

    public Conversation? GetConversationById(string id) => _collection
        .FirstOrDefault(conversation => conversation.ConversationId == id);

    public List<Conversation> GetConversationsByUser(IUserAccount userAccount) => _collection
        .Where(conversation => (conversation is ConnectionConversation connConvo 
                                && (connConvo.Connection.Account1 == userAccount || connConvo.Connection.Account2 == userAccount))
                            || (conversation is GroupConversation groupConvo
                                && groupConvo.Group.Users.Contains(userAccount)))
        .ToList();

    public Conversation CreateConnectionConversation(IConnection connection)
    {
        var id = NextId();
        ConnectionConversation conversation = new(connection, id);
        _collection.Add(conversation);

        return conversation;
    }

    public Conversation CreateGroupConversation(Group group)
    {
        var id = NextId();
        GroupConversation conversation = new(group, id);
        _collection.Add(conversation);

        return conversation;
    }

    public bool UpdateConversation(Conversation conversation, IConnection? connection = null, Group? group = null)
    {
        Conversation? existingConversation = _collection.FirstOrDefault(convo => convo.ConversationId == conversation.ConversationId);
        if (existingConversation is null)
        {
            return false;
        }

        Conversation updatedConversation = connection is not null
            ? new ConnectionConversation(connection, conversation.ConversationId, conversation.Messages)
            : new GroupConversation(group!, conversation.ConversationId, conversation.Messages);

        _collection.Remove(existingConversation);
        _collection.Add(updatedConversation);

        return true;
    }

    private string NextId()
    {
        return _collection.Count.ToString();
    }
}
