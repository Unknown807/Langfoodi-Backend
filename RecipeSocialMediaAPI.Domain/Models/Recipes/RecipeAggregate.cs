﻿using RecipeSocialMediaAPI.Domain.Models.Users;
using System.Collections.Immutable;

namespace RecipeSocialMediaAPI.Domain.Models.Recipes;

public class RecipeAggregate
{
    public string Id { get; }
    public Recipe Recipe { get; }
    public string Title { get; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public User Chef { get; }
    public DateTimeOffset CreationDate { get; }
    public DateTimeOffset LastUpdatedDate { get; set; }

    private readonly ISet<string> _labels;
    public ISet<string> Labels => _labels.ToImmutableHashSet();

    public RecipeAggregate(
        string id,
        string title,
        Recipe recipe,
        string shortDescription,
        string longDescription,
        User chef,
        DateTimeOffset creationDate,
        DateTimeOffset lastUpdatedDate,
        ISet<string>? labels = null)
    {
        Id = id;
        Title = title;
        Recipe = recipe;
        ShortDescription = shortDescription;
        LongDescription = longDescription;
        Chef = chef;
        CreationDate = creationDate;
        LastUpdatedDate = lastUpdatedDate;
        _labels = labels ?? new HashSet<string>();
    }

    public bool AddLabel(string label)
    {
        return _labels.Add(label);
    }
}
