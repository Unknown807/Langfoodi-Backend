﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RecipeSocialMediaAPI.TestInfrastructure;
using RecipeSocialMediaAPI.Application.Cryptography.Interfaces;
using RecipeSocialMediaAPI.Application.Cryptography;
using FluentAssertions;
using RecipeSocialMediaAPI.Domain.Services.Interfaces;
using RecipeSocialMediaAPI.Domain.Services;
using RecipeSocialMediaAPI.Application.Services.Interfaces;
using RecipeSocialMediaAPI.Application.Services;
using RecipeSocialMediaAPI.Application.WebClients.Interfaces;
using RecipeSocialMediaAPI.Application.WebClients;
using RecipeSocialMediaAPI.Application.Handlers.Messages.Notifications;
using RecipeSocialMediaAPI.Core.SignalR;
using System;

namespace RecipeSocialMediaAPI.Core.Tests.Configuration;

public class TransientServiceConfigurationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TransientServiceConfigurationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void HttpClientFactory_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var signatureService = scope.ServiceProvider.GetService(typeof(IHttpClientFactory));

        // Then
        signatureService.Should().NotBeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void CryptoService_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var cryptoService = scope.ServiceProvider.GetService(typeof(ICryptoService)) as CryptoService;

        // Then
        cryptoService.Should().NotBeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void MessageFactory_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var messageFactory = scope.ServiceProvider.GetService(typeof(IMessageFactory)) as MessageFactory;

        // Then
        messageFactory.Should().NotBeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void UserFactory_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var userFactory = scope.ServiceProvider.GetService(typeof(IUserFactory)) as UserFactory;

        // Then
        userFactory.Should().NotBeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void CloudinarySignatureService_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var signatureService = scope.ServiceProvider.GetService(typeof(ICloudinarySignatureService)) as CloudinarySignatureService;

        // Then
        signatureService.Should().NotBeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void CloudinaryWebClient_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var webClient = scope.ServiceProvider.GetService(typeof(ICloudinaryWebClient)) as CloudinaryWebClient;

        // Then
        webClient.Should().NotBeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void MessageNotificationService_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var messageNotificationService = scope.ServiceProvider.GetService(typeof(IMessageNotificationService)) as MessageNotificationService;

        // Then
        messageNotificationService.Should().NotBeNull();
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.CONFIGURATION)]
    [Trait(Traits.MODULE, Traits.Modules.CORE)]
    public void BearerTokenService_ShouldBeConfiguredCorrectly()
    {
        // Given
        using var scope = _factory.Services.CreateScope();
        var bearerTokenService = scope.ServiceProvider.GetService(typeof(IBearerTokenGeneratorService)) as BearerTokenGeneratorService;

        // Then
        bearerTokenService.Should().NotBeNull();
    }
}
