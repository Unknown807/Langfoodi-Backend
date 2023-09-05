﻿using RecipeSocialMediaAPI.Domain.Models.Users;
using System.Collections.Immutable;

namespace RecipeSocialMediaAPI.Domain.Models.Recipes;

public class RecipeAggregate
{
    public string Id { get; }
    public Recipe Recipe { get; }
    public string Title { get; }
    public string Description { get; set; }
    public User Chef { get; }
    public int? NumberOfServings { get; set; }
    public int? CookingTimeInSeconds { get; set; }
    public int? KiloCalories { get; set; }
    public DateTimeOffset CreationDate { get; }
    public DateTimeOffset LastUpdatedDate { get; set; }

    private readonly ISet<string> _labels;
    public ISet<string> Labels => _labels.ToImmutableHashSet();

    public RecipeAggregate(
        string id,
        string title,
        Recipe recipe,
        string description,
        User chef,
        DateTimeOffset creationDate,
        DateTimeOffset lastUpdatedDate,
        ISet<string>? labels = null,
        int? numberOfServings = null,
        int? cookingTimeInSeconds = null,
        int? kiloCalories = null)
    {
        Id = id;
        Title = title;
        Recipe = recipe;
        Description = description;
        Chef = chef;
        CreationDate = creationDate;
        LastUpdatedDate = lastUpdatedDate;
        _labels = labels ?? new HashSet<string>();
        NumberOfServings = numberOfServings;
        CookingTimeInSeconds = cookingTimeInSeconds;
        KiloCalories = kiloCalories;
    }

    public bool AddLabel(string label)
    {
        return _labels.Add(label);
    }
}
