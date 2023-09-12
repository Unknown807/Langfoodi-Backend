﻿using FluentAssertions;
using RecipeSocialMediaAPI.Domain.Models.Recipes;
using RecipeSocialMediaAPI.Domain.Models.Users;
using RecipeSocialMediaAPI.TestInfrastructure;

namespace RecipeSocialMediaAPI.Domain.Tests.Unit.Models.Recipes;

public class RecipeAggregateTests
{
    public readonly RecipeAggregate _recipeAggregateSUT;

    public RecipeAggregateTests()
    {
        string testId = "AggId";
        string testTitle = "My Recipe";
        Recipe testRecipe = new(new() { new("Test Ingredient", 2, "g") }, new(new[] { new RecipeStep("Test Step") }));
        string testDescription = "";
        User testChef = new("TestId", "TestUsername", "TestEmail", "TestPassword");
        DateTimeOffset testCreationDate = new(2023, 1, 1, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset testLastUpdatedDate = new(2023, 8, 30, 0, 0, 0, TimeSpan.Zero);
        int testKiloCalories = 2300;
        int testCookingTimeInSeconds = 500;
        int testNumberOfServings = 10;

        _recipeAggregateSUT = new
            (
                testId,
                testTitle,
                testRecipe,
                testDescription,
                testChef,
                testCreationDate,
                testLastUpdatedDate,
                null,
                testNumberOfServings,
                testCookingTimeInSeconds,
                testKiloCalories
            );
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.RECIPE)]
    [Trait(Traits.MODULE, Traits.Modules.DOMAIN)]
    public void Recipe_CanBeModifiedThroughInstanceMethods()
    {
        // Given
        Ingredient testIngredient = new("New Ingredient", 2, "g");

        // When
        _recipeAggregateSUT.Recipe.AddIngredient(testIngredient);

        // Then
        _recipeAggregateSUT.Recipe.Ingredients.Should().Contain(testIngredient);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.RECIPE)]
    [Trait(Traits.MODULE, Traits.Modules.DOMAIN)]
    public void Description_CanBeModified()
    {
        // Given
        string newLongDescription = "New, long, windy description";

        // When
        _recipeAggregateSUT.Description = newLongDescription;

        // Then
        _recipeAggregateSUT.Description.Should().Be(newLongDescription);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.RECIPE)]
    [Trait(Traits.MODULE, Traits.Modules.DOMAIN)]
    public void Chef_CanBeModifiedThroughInstanceMethods()
    {
        // Given
        var user = _recipeAggregateSUT.Chef;
        string newEmail = "mynewemail@mail.com";

        // When
        user.Email = newEmail;

        // Then
        _recipeAggregateSUT.Chef.Email.Should().Be(newEmail);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.RECIPE)]
    [Trait(Traits.MODULE, Traits.Modules.DOMAIN)]
    public void LastUpdatedDate_CanBeModified()
    {
        // Given
        DateTimeOffset newLastUpdatedDate = _recipeAggregateSUT.LastUpdatedDate.AddDays(5);

        // When
        _recipeAggregateSUT.LastUpdatedDate = newLastUpdatedDate;

        // Then
        _recipeAggregateSUT.LastUpdatedDate.Should().Be(newLastUpdatedDate);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.RECIPE)]
    [Trait(Traits.MODULE, Traits.Modules.DOMAIN)]
    public void AddLabel_WhenLabelIsNotYetAdded_AddsLabelAndReturnsTrueAndDoesNotChangeReturnedSet()
    {
        // Given
        string testLabel = "new_label";

        // When
        var wasAdded = _recipeAggregateSUT.AddLabel(testLabel);

        // Then
        wasAdded.Should().BeTrue();
        _recipeAggregateSUT.Labels.Should().HaveCount(1).And.Contain(testLabel);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.RECIPE)]
    [Trait(Traits.MODULE, Traits.Modules.DOMAIN)]
    public void AddLabel_WhenLabelIsAlreadyAdded_ReturnsFalse()
    {
        // Given
        string existingLabel = "existing";
        _recipeAggregateSUT.AddLabel(existingLabel);

        // When
        var wasAdded = _recipeAggregateSUT.AddLabel(existingLabel);

        // Then
        wasAdded.Should().BeFalse();
        _recipeAggregateSUT.Labels.Should().HaveCount(1).And.Contain(existingLabel);
    }
}
