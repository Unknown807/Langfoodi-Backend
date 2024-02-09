﻿using Moq;
using RecipeSocialMediaAPI.Application.Handlers.Messages.Commands;
using RecipeSocialMediaAPI.Application.Repositories.Messages;
using RecipeSocialMediaAPI.Application.Repositories.Users;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Connections;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Conversations;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Messages;
using RecipeSocialMediaAPI.Domain.Tests.Shared;
using RecipeSocialMediaAPI.TestInfrastructure;

namespace RecipeSocialMediaAPI.Application.Tests.Unit.Handlers.Messages.Commands;

public class MarkConversationAsReadHandlerTests
{
    private readonly Mock<IUserQueryRepository> _userQueryRepositoryMock;
    private readonly Mock<IConversationQueryRepository> _conversationQueryRepositoryMock;
    private readonly Mock<IMessagePersistenceRepository> _messagePersistenceRepositoryMock;

    private readonly MarkConversationAsReadHandler _markConversationAsReadHandlerSUT;

    public MarkConversationAsReadHandlerTests()
    {
        _userQueryRepositoryMock = new Mock<IUserQueryRepository>();
        _conversationQueryRepositoryMock = new Mock<IConversationQueryRepository>();
        _messagePersistenceRepositoryMock = new Mock<IMessagePersistenceRepository>();

        _markConversationAsReadHandlerSUT = new(_userQueryRepositoryMock.Object, _conversationQueryRepositoryMock.Object, _messagePersistenceRepositoryMock.Object);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.MESSAGING)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenConversationExistsWithNoMessages_DoNothing()
    {
        // Given
        TestUserCredentials user1 = new()
        {
            Account = new TestUserAccount()
            {
                Id = "u1",
                Handler = "user_1",
                UserName = "User 1"
            },
            Email = "user1@mail.com",
            Password = "Test@123"
        };
        TestUserCredentials user2 = new()
        {
            Account = new TestUserAccount()
            {
                Id = "u2",
                Handler = "user_2",
                UserName = "User 2"
            },
            Email = "user2@mail.com",
            Password = "Test@123"
        };

        _userQueryRepositoryMock
            .Setup(repo => repo.GetUserById(user1.Account.Id))
            .Returns(user1);

        Connection connection = new("conn1", user1.Account, user2.Account, ConnectionStatus.Pending);
        ConnectionConversation conversation = new(connection, "convo1", new List<Message>());

        _conversationQueryRepositoryMock
            .Setup(repo => repo.GetConversationById(conversation.ConversationId))
            .Returns(conversation);

        MarkConversationAsReadCommand command = new(user1.Account.Id, conversation.ConversationId);

        // When
        await _markConversationAsReadHandlerSUT.Handle(command, CancellationToken.None);

        // Then
        _messagePersistenceRepositoryMock
            .Verify(repo => repo.UpdateMessage(It.IsAny<Message>()), Times.Never);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.MESSAGING)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenConversationExistsWithMessages_UpdateMessagesToBeSeenByUser()
    {
        // Given
        TestUserCredentials user1 = new()
        {
            Account = new TestUserAccount()
            {
                Id = "u1",
                Handler = "user_1",
                UserName = "User 1"
            },
            Email = "user1@mail.com",
            Password = "Test@123"
        };
        TestUserCredentials user2 = new()
        {
            Account = new TestUserAccount()
            {
                Id = "u2",
                Handler = "user_2",
                UserName = "User 2"
            },
            Email = "user2@mail.com",
            Password = "Test@123"
        };

        _userQueryRepositoryMock
            .Setup(repo => repo.GetUserById(user1.Account.Id))
            .Returns(user1);

        List<Message> messages = new()
        {
            new TestMessage("m1", user1.Account, new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), null, seenBy: new() { user1.Account, user2.Account } ),
            new TestMessage("m2", user2.Account, new(2024, 1, 1, 0, 15, 0, TimeSpan.Zero), null, seenBy: new() { user1.Account, user2.Account } ),
            new TestMessage("m3", user1.Account, new(2024, 1, 1, 0, 30, 0, TimeSpan.Zero), null, seenBy: new() { user1.Account } ),
            new TestMessage("m4", user2.Account, new(2024, 1, 1, 0, 45, 0, TimeSpan.Zero), null, seenBy: new() { user2.Account } ),
            new TestMessage("m5", user2.Account, new(2024, 1, 1, 1, 0, 0, TimeSpan.Zero), null, seenBy: new() { user2.Account } ),
        };

        Connection connection = new("conn1", user1.Account, user2.Account, ConnectionStatus.Pending);
        ConnectionConversation conversation = new(connection, "convo1", messages);

        _conversationQueryRepositoryMock
            .Setup(repo => repo.GetConversationById(conversation.ConversationId))
            .Returns(conversation);

        MarkConversationAsReadCommand command = new(user1.Account.Id, conversation.ConversationId);

        // When
        await _markConversationAsReadHandlerSUT.Handle(command, CancellationToken.None);

        // Then
        _messagePersistenceRepositoryMock
            .Verify(repo => repo.UpdateMessage(It.IsAny<Message>()), Times.Exactly(2));
        _messagePersistenceRepositoryMock
            .Verify(repo => repo.UpdateMessage(It.Is<Message>(message => 
                    message.Id == messages[3].Id &&
                    message.SeenBy.Contains(user1.Account))), 
                Times.Once);
        _messagePersistenceRepositoryMock
            .Verify(repo => repo.UpdateMessage(It.Is<Message>(message =>
                    message.Id == messages[4].Id &&
                    message.SeenBy.Contains(user1.Account))),
                Times.Once);
    }
}
