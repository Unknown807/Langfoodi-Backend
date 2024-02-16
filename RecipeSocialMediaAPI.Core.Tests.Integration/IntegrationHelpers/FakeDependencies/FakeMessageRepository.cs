﻿using RecipeSocialMediaAPI.Application.Repositories.Messages;
using RecipeSocialMediaAPI.Application.Repositories.Recipes;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Messages;
using RecipeSocialMediaAPI.Domain.Models.Users;
using RecipeSocialMediaAPI.Domain.Services.Interfaces;

namespace RecipeSocialMediaAPI.Core.Tests.Integration.IntegrationHelpers.FakeDependencies;

internal class FakeMessageRepository : IMessageQueryRepository, IMessagePersistenceRepository
{
    private readonly IMessageFactory _messageFactory;
    private readonly IRecipeQueryRepository _recipeQueryRepository;

    private readonly List<Message> _collection;

    public FakeMessageRepository(IMessageFactory messageFactory, IRecipeQueryRepository recipeQueryRepository)
    {
        _messageFactory = messageFactory;
        _recipeQueryRepository = recipeQueryRepository;

        _collection = new();
    }

    public Message? GetMessage(string id) => _collection.FirstOrDefault(m => m.Id == id);

    public Message CreateMessage(IUserAccount sender, string? text, List<string>? recipeIds, List<string>? imageURLs, DateTimeOffset sentDate, Message? messageRepliedTo, List<string> seenByUserIds)
    {
        var id = _collection.Count.ToString();
        Message message;

        if (recipeIds?.Count > 0)
        {
            var recipes = recipeIds
                .Select(id => _recipeQueryRepository.GetRecipeById(id)!)
                .ToList();

            message = _messageFactory
                .CreateRecipeMessage(id, sender, recipes, text, new(), sentDate, repliedToMessage: messageRepliedTo);
        }
        else if (imageURLs?.Count > 0)
        {
            message = _messageFactory.CreateImageMessage(id, sender, imageURLs, text, new(), sentDate, repliedToMessage: messageRepliedTo);
        }
        else
        {
            message = _messageFactory.CreateTextMessage(id, sender, text!, new(), sentDate, repliedToMessage: messageRepliedTo);
        }

        _collection.Add(message);
        return message;
    }

    public bool UpdateMessage(Message message)
    {
        Message? existingMessage = _collection.FirstOrDefault(x => x.Id == message.Id);
        if (existingMessage is null)
        {
            return false;
        }

        _collection.Remove(existingMessage);
        _collection.Add(message);

        return true;
    }

    public bool DeleteMessage(Message message) => _collection.Remove(message);

    public bool DeleteMessage(string messageId)
    {
        var message = GetMessage(messageId);

        return message is not null && _collection.Remove(message);
    }
}
