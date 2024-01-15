﻿using RecipeSocialMediaAPI.Application.Repositories.Messages;
using RecipeSocialMediaAPI.DataAccess.Exceptions;
using RecipeSocialMediaAPI.DataAccess.Mappers.Interfaces;
using RecipeSocialMediaAPI.DataAccess.MongoConfiguration.Interfaces;
using RecipeSocialMediaAPI.DataAccess.MongoDocuments;
using RecipeSocialMediaAPI.Domain.Models.Messaging;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Connections;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Conversations;

namespace RecipeSocialMediaAPI.DataAccess.Repositories.Messages;

public class ConversationPersistenceRepository : IConversationPersistenceRepository
{
    private readonly IConversationDocumentToModelMapper _mapper;
    private readonly IMongoCollectionWrapper<ConversationDocument> _conversationCollection;
    private readonly IMongoCollectionWrapper<ConnectionDocument> _connectionCollection;

    public ConversationPersistenceRepository(IConversationDocumentToModelMapper mapper, IMongoCollectionFactory mongoCollectionFactory)
    {
        _mapper = mapper;
        _conversationCollection = mongoCollectionFactory.CreateCollection<ConversationDocument>();
        _connectionCollection = mongoCollectionFactory.CreateCollection<ConnectionDocument>();
    }

    public Conversation CreateConnectionConversation(IConnection connection)
    {
        ConnectionDocument connectionDocument = GetConnectionDocument(connection)
            ?? throw new ConnectionDocumentNotFoundException(connection.Account1, connection.Account2);

        ConversationDocument conversationDocument = _conversationCollection.Insert(new(
            ConnectionId: connectionDocument.Id,
            Messages: new()
        ));

        return _mapper.MapConversationFromDocument(conversationDocument, connection, null, new());
    }

    public Conversation CreateGroupConversation(Group group)
    {
        ConversationDocument conversationDocument = _conversationCollection.Insert(new(
            GroupId: group.GroupId,
            Messages: new()
        ));

        return _mapper.MapConversationFromDocument(conversationDocument, null, group, new());
    }

    public bool UpdateConversation(Conversation conversation, IConnection? connection = null, Group? group = null)
    {
        (string? connectionId, string? groupId) = conversation switch
        {
            ConnectionConversation connectionConversation =>
                (connection is not null 
                ? GetConnectionId(connection) 
                    ?? throw new InvalidConversationException($"No connection found for ConnectionConversation with id {conversation.ConversationId}")
                : throw new ArgumentException($"No connection provided when updating ConnectionConversation with id {conversation.ConversationId}"),
                (string?)null),
            GroupConversation groupConversation => 
            (null, 
            group?.GroupId ?? throw new ArgumentException($"No group provided when updating GroupConversation with id {conversation.ConversationId}")),

            _ => throw new InvalidConversationException($"Could not update conversation with id {conversation.ConversationId} of unknown type {conversation.GetType()}"),
        };

        return _conversationCollection.UpdateRecord(new ConversationDocument(
                Id: conversation.ConversationId,
                ConnectionId: connectionId,
                GroupId: groupId,
                Messages: conversation.Messages.Select(message => message.Id).ToList()
            ),
            conversationDoc => conversationDoc.Id == conversation.ConversationId);
    }

    private string? GetConnectionId(IConnection connection) => 
        connection is not null 
        ? (GetConnectionDocument(connection)?.Id) 
        : null;

    private ConnectionDocument? GetConnectionDocument(IConnection connection) =>
        _connectionCollection.Find(conn => (conn.AccountId1 == connection.Account1.Id && conn.AccountId2 == connection.Account2.Id) 
                                        || (conn.AccountId1 == connection.Account2.Id && conn.AccountId2 == connection.Account1.Id));

}
