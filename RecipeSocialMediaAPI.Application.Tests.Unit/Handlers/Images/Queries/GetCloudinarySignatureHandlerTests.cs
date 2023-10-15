﻿using FluentAssertions;
using Moq;
using RecipeSocialMediaAPI.Application.DTO.ImageHosting;
using RecipeSocialMediaAPI.Application.Handlers.Images.Queries;
using RecipeSocialMediaAPI.Application.Repositories.ImageHosting;
using RecipeSocialMediaAPI.TestInfrastructure;

namespace RecipeSocialMediaAPI.Application.Tests.Unit.Handlers.Images.Queries;
public class GetCloudinarySignatureHandlerTests
{
    private readonly Mock<IImageHostingQueryRepository> _imageHostingQueryRepositoryMock;

    private readonly GetCloudinarySignatureHandler _getCloudinarySignatureHandlerSUT;

    private const string TEST_PUBLIC_ID = "354234535sgf45";

    public GetCloudinarySignatureHandlerTests()
    {
        _imageHostingQueryRepositoryMock = new Mock<IImageHostingQueryRepository>();
        _getCloudinarySignatureHandlerSUT = new GetCloudinarySignatureHandler(_imageHostingQueryRepositoryMock.Object);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.IMAGE)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenPublicIdIsNullAndGenerateClientSignatureWorks_ReturnSignatureDTO()
    {
        // Given
        _imageHostingQueryRepositoryMock
            .Setup(x => x.GenerateClientSignature(null))
            .Returns(new CloudinarySignatureDTO() { Signature = "sig", TimeStamp = 1000 });

        // When
        var result = await _getCloudinarySignatureHandlerSUT.Handle(new GetCloudinarySignatureQuery(), CancellationToken.None);

        // Then
        result.Signature.Should().Be("sig");
        result.TimeStamp.Should().Be(1000);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.IMAGE)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenPublicIdIsNotNullAndGenerateClientSignatureWorks_ReturnSignatureDTO()
    {
        // Given
        _imageHostingQueryRepositoryMock
            .Setup(x => x.GenerateClientSignature(TEST_PUBLIC_ID))
            .Returns(new CloudinarySignatureDTO() { Signature = "sig", TimeStamp = 1000 });

        // When
        var result = await _getCloudinarySignatureHandlerSUT.Handle(new GetCloudinarySignatureQuery(TEST_PUBLIC_ID), CancellationToken.None);

        // Then
        result.Signature.Should().Be("sig");
        result.TimeStamp.Should().Be(1000);
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.IMAGE)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenPublicIdIsNotNullAndNoSignatureGenerated_ThrowInvalidOperationException()
    {
        // Given
        _imageHostingQueryRepositoryMock
            .Setup(x => x.GenerateClientSignature(TEST_PUBLIC_ID))
            .Returns((CloudinarySignatureDTO)null);

        // When
        var action = async () => await _getCloudinarySignatureHandlerSUT.Handle(new GetCloudinarySignatureQuery(TEST_PUBLIC_ID), CancellationToken.None);

        // Then
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to generate signature");
    }

    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.IMAGE)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public async Task Handle_WhenPublicIdIsNullAndNoSignatureGenerated_ThrowInvalidOperationException()
    {
        // Given
        _imageHostingQueryRepositoryMock
            .Setup(x => x.GenerateClientSignature(null))
            .Returns((CloudinarySignatureDTO)null);

        // When
        var action = async () => await _getCloudinarySignatureHandlerSUT.Handle(new GetCloudinarySignatureQuery(), CancellationToken.None);

        // Then
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Failed to generate signature");
    }
}
