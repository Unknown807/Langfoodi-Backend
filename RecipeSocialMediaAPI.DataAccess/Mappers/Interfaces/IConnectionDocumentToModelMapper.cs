﻿using RecipeSocialMediaAPI.DataAccess.MongoDocuments;
using RecipeSocialMediaAPI.Domain.Models.Messaging.Connections;

namespace RecipeSocialMediaAPI.DataAccess.Mappers.Interfaces;
public interface IConnectionDocumentToModelMapper
{
    IConnection MapConnectionFromDocument(ConnectionDocument connectionDocument);
}