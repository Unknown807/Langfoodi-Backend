﻿using FluentAssertions;
using Moq;
using RecipeSocialMediaAPI.Application.DTO.Message;
using RecipeSocialMediaAPI.Application.Exceptions;
using RecipeSocialMediaAPI.Application.Handlers.Messages.Queries;
using RecipeSocialMediaAPI.Application.Mappers.Messages.Interfaces;
using RecipeSocialMediaAPI.Application.Repositories.Messages;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Connections;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Conversations;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Messages;
using RecipeSocialMediaAPI.Domain.Tests.Shared;
using RecipeSocialMediaAPI.TestInfrastructure;

namespace RecipeSocialMediaAPI.Application.Tests.Unit.Handlers.Messages.Queries;

public class GetConversationByConnectionHandlerTests
{
    private readonly Mock<IConversationQueryRepository> _conversationQueryRepositoryMock;
    private readonly Mock<IMessageMapper> _messageMapperMock;

    private readonly GetConversationByConnectionHandler _getConversationByConnectionHandlerSUT;

    public GetConversationByConnectionHandlerTests()
    {
        _conversationQueryRepositoryMock = new Mock<IConversationQueryRepository>();
        _messageMapperMock = new Mock<IMessageMapper>();

        _getConversationByConnectionHandlerSUT = new(_conversationQueryRepositoryMock.Object, _messageMapperMock.Object);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.MESSAGING)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenThereAreNoMessages_ReturnConversationDtoWithNullLastMessage()
    {
        // Given
        TestUserAccount user1 = new()
        {
            Id = "u1",
            Handler = "user1",
            UserName = "User 1"
        };
        TestUserAccount user2 = new()
        {
            Id = "u2",
            Handler = "user2",
            UserName = "User 2"
        };

        Connection connection = new("conn1", user1, user2, ConnectionStatus.Pending);
        ConnectionConversation conversation = new(connection, "convo1", new List<Message>());

        _conversationQueryRepositoryMock
            .Setup(repo => repo.GetConversationByConnection(connection.ConnectionId))
            .Returns(conversation);

        GetConversationByConnectionQuery query = new(connection.ConnectionId);

        // When
        var result = await _getConversationByConnectionHandlerSUT.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.ConnectionId.Should().Be(connection.ConnectionId);
        result.ConversationId.Should().Be(conversation.ConversationId);
        result.LastMessage.Should().BeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.MESSAGING)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenThereAreMessages_ReturnConversationDtoWithTheLastMessage()
    {
        // Given
        TestUserAccount user1 = new()
        {
            Id = "u1",
            Handler = "user1",
            UserName = "User 1"
        };
        TestUserAccount user2 = new()
        {
            Id = "u2",
            Handler = "user2",
            UserName = "User 2"
        };

        MessageDTO lastMessageDto = new("3", user2.Id, new(2023, 1, 1, 14, 35, 0, TimeSpan.Zero));

        List<Message> messages = new()
        {
            new TestMessage("1", user1, new(2023, 1, 1, 12, 45, 0, TimeSpan.Zero), null),
            new TestMessage("2", user2, new(2023, 1, 1, 13, 30, 0, TimeSpan.Zero), null),
            new TestMessage(lastMessageDto.Id, user2, lastMessageDto.SentDate!.Value, null),
            new TestMessage("4", user1, new(2023, 1, 1, 14, 10, 0, TimeSpan.Zero), null),
            new TestMessage("5", user1, new(2023, 1, 1, 14, 15, 0, TimeSpan.Zero), null),
        };
        _messageMapperMock
            .Setup(mapper => mapper.MapMessageToMessageDTO(messages[2]))
            .Returns(lastMessageDto);

        Connection connection = new("conn1", user1, user2, ConnectionStatus.Pending);
        ConnectionConversation conversation = new(connection, "convo1", messages);

        _conversationQueryRepositoryMock
            .Setup(repo => repo.GetConversationByConnection(connection.ConnectionId))
            .Returns(conversation);

        GetConversationByConnectionQuery query = new(connection.ConnectionId);

        // When
        var result = await _getConversationByConnectionHandlerSUT.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.ConnectionId.Should().Be(connection.ConnectionId);
        result.ConversationId.Should().Be(conversation.ConversationId);
        result.LastMessage.Should().Be(lastMessageDto);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.MESSAGING)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenThereIsNoConversationFound_ThrowConversationNotFoundException()
    {
        // Given
        string connectionId = "conn1";
        _conversationQueryRepositoryMock
            .Setup(repo => repo.GetConversationByConnection(connectionId))
            .Returns((Conversation?)null);

        GetConversationByConnectionQuery query = new(connectionId);

        // When
        var testAction = async () => await _getConversationByConnectionHandlerSUT.Handle(query, CancellationToken.None);

        // Then
        await testAction.Should().ThrowAsync<ConversationNotFoundException>();
    }
}
