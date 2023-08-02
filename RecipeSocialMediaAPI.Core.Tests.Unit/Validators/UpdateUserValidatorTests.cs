﻿using FluentValidation.TestHelper;
using Moq;
using RecipeSocialMediaAPI.Core.Handlers.Users.Commands;
using RecipeSocialMediaAPI.Core.Services.Interfaces;
using RecipeSocialMediaAPI.TestInfrastructure.Traits;

namespace RecipeSocialMediaAPI.Core.Tests.Unit.Validators;

public class UpdateUserValidatorTests
{
    private readonly UpdateUserCommandValidator _updateUserValidatorSUT;
    private readonly Mock<IUserValidationService> _userValidationServiceMock;

    public UpdateUserValidatorTests()
    {
        _userValidationServiceMock = new Mock<IUserValidationService>();

        _updateUserValidatorSUT = new UpdateUserCommandValidator(_userValidationServiceMock.Object);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.USER)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void UpdateUserValidation_WhenValidUser_DontThrow()
    {
        // Given
        UpdateUserCommand testCommand = new(
            new()
            {
                Id = "testId",
                UserName = "TestUser",
                Email = "test@mail.com",
                Password = "Test@123"
            }
        );

        _userValidationServiceMock
            .Setup(service => service.ValidUserName(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();
        _userValidationServiceMock
            .Setup(service => service.ValidEmail(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();
        _userValidationServiceMock
            .Setup(service => service.ValidPassword(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

        // When
        var validationResult = _updateUserValidatorSUT.TestValidate(testCommand);

        // Then
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.USER)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void UpdateUserValidation_WhenInvalidUser_ThrowsValidationException()
    {
        // Given
        UpdateUserCommand testCommand = new (
            new()
            {
                Id = "TestId",
                UserName = string.Empty,
                Email = "test.com",
                Password = "test"
            }
        );

        _userValidationServiceMock
            .Setup(service => service.ValidUserName(It.IsAny<string>()))
            .Returns(false)
            .Verifiable();
        _userValidationServiceMock
            .Setup(service => service.ValidEmail(It.IsAny<string>()))
            .Returns(false)
            .Verifiable();
        _userValidationServiceMock
            .Setup(service => service.ValidPassword(It.IsAny<string>()))
            .Returns(false)
            .Verifiable();

        // When
        var validationResult = _updateUserValidatorSUT.TestValidate(testCommand);

        // Then
        validationResult.ShouldHaveValidationErrorFor(command => command.UpdateUserContract.UserName);
        validationResult.ShouldHaveValidationErrorFor(command => command.UpdateUserContract.Email);
        validationResult.ShouldHaveValidationErrorFor(command => command.UpdateUserContract.Password);
    }
}
