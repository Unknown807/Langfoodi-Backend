﻿using RecipeSocialMediaAPI.Infrastructure.MongoDocuments;
using System.Runtime.Serialization;

namespace RecipeSocialMediaAPI.Infrastructure.Exceptions;

[Serializable]
public class DocumentAlreadyExistsException<TDocument> : Exception where TDocument : MongoDocument
{
    public TDocument Document { get; }

    public DocumentAlreadyExistsException(TDocument document) : base($"{document.GetType()} already exists with id {document.Id}")
    {
        Document = document;
    }

    protected DocumentAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Document = info.GetValue("Document", typeof(TDocument)) as TDocument ?? default!;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Document", Document);
    }
}
