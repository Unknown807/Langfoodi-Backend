﻿using Microsoft.Extensions.Logging;
using RecipeSocialMediaAPI.Application.Repositories.Messages;
using RecipeSocialMediaAPI.DataAccess.Mappers.Interfaces;
using RecipeSocialMediaAPI.DataAccess.MongoConfiguration.Interfaces;
using RecipeSocialMediaAPI.DataAccess.MongoDocuments;
using RecipeSocialMediaAPI.Domain.Models.Messaging;
using RecipeSocialMediaAPI.Domain.Models.Users;

namespace RecipeSocialMediaAPI.DataAccess.Repositories.Messages;

public class GroupQueryRepository : IGroupQueryRepository
{
    private readonly ILogger<GroupQueryRepository> _logger;
    private readonly IGroupDocumentToModelMapper _mapper;
    private readonly IMongoCollectionWrapper<GroupDocument> _groupCollection;

    public GroupQueryRepository(ILogger<GroupQueryRepository> logger, IGroupDocumentToModelMapper groupDocumentToModelMapper, IMongoCollectionFactory mongoCollectionFactory)
    {
        _logger = logger;
        _mapper = groupDocumentToModelMapper;
        _groupCollection = mongoCollectionFactory.CreateCollection<GroupDocument>();
    }

    public Group? GetGroupById(string groupId)
    {
        GroupDocument? groupDocument;
        try
        {
            groupDocument = _groupCollection.Find(
                groupDoc => groupDoc.GroupId == groupId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was an error trying to get the group with the id {GroupID}: {ErrorMessage}", groupId, ex.Message);
            return null;
        }
        
        return groupDocument is not null
            ? _mapper.MapGroupFromDocument(groupDocument)
            : null;
    }

    public IEnumerable<Group> GetGroupsByUser(IUserAccount userAccount)
    {
        try
        {
            return _groupCollection
                .GetAll(groupDoc => groupDoc.UserIds.Contains(userAccount.Id))
                .Select(_mapper.MapGroupFromDocument);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was an error trying to get the groups for user with id {UserId}: {ErrorMessage}", userAccount.Id, ex.Message);
            return Enumerable.Empty<Group>();
        }
    }
}
