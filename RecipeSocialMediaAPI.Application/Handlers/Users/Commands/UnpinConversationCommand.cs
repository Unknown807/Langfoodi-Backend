﻿using MediatR;
using RecipeSocialMediaAPI.Application.Exceptions;
using RecipeSocialMediaAPI.Application.Repositories.Messages;
using RecipeSocialMediaAPI.Application.Repositories.Users;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Conversations;
using RecipeSocialMediaAPI.Domain.Models.Users;

namespace RecipeSocialMediaAPI.Application.Handlers.Users.Commands;
public record UnpinConversationCommand(string UserId, string ConversationId) : IRequest;

internal class UnpinConversationHandler : IRequestHandler<UnpinConversationCommand>
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserPersistenceRepository _userPersistenceRepository;
    private readonly IConversationQueryRepository _conversationQueryRepository;

    public UnpinConversationHandler(IUserQueryRepository userQueryRepository, IUserPersistenceRepository userPersistenceRepository, IConversationQueryRepository conversationQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
        _userPersistenceRepository = userPersistenceRepository;
        _conversationQueryRepository = conversationQueryRepository;
    }

    public Task Handle(UnpinConversationCommand request, CancellationToken cancellationToken)
    {
        IUserCredentials user = _userQueryRepository.GetUserById(request.UserId)
            ?? throw new UserNotFoundException($"User with id {request.UserId} does not exist");
        Conversation conversation = _conversationQueryRepository.GetConversationById(request.ConversationId)
            ?? throw new ConversationNotFoundException($"Conversation with id {request.ConversationId} does not exist");

        if (user.Account.RemovePin(conversation.ConversationId))
        {
            _userPersistenceRepository.UpdateUser(user);
        }

        return Task.CompletedTask;
    }
}
