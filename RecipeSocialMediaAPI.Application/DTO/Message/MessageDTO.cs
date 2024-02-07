﻿using RecipeSocialMediaAPI.Application.DTO.Recipes;

namespace RecipeSocialMediaAPI.Application.DTO.Message;

public record MessageDTO(
    string Id,
    string SenderId,
    string SenderName,
    List<string> SeenByUserIds,
    DateTimeOffset? SentDate = null,
    DateTimeOffset? UpdatedDate = null,
    string? RepliedToMessageId = null,
    string? TextContent = null,
    List<string>? ImageURLs = null,
    List<RecipePreviewDTO>? Recipes = null
);