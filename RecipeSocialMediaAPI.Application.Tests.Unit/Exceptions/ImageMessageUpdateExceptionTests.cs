﻿using FluentAssertions;
using RecipeSocialMediaAPI.Application.Exceptions;
using RecipeSocialMediaAPI.Application.Tests.Unit.TestHelpers;
using RecipeSocialMediaAPI.TestInfrastructure;
using System.Text.Json;

namespace RecipeSocialMediaAPI.Application.Tests.Unit.Exceptions;

public class ImageMessageUpdateExceptionTests
{
    [Fact]
    [Trait(Traits.DOMAIN, Traits.Domains.INFRASTRUCTURE)]
    [Trait(Traits.MODULE, Traits.Modules.APPLICATION)]
    public void ImageMessageUpdateException_IsCorrectlySerializedAndDeserialized()
    {
        // Given
        var messageId = "1";
        var reason = "Test reason";
        ImageMessageUpdateException exception = new(messageId, reason);

        ExceptionSerializationData serializedData = new(exception.Message, exception.HResult, exception.Source, exception.StackTrace);

        // When
        var json = JsonSerializer.Serialize(serializedData);

        // Then
        var deserializedData = JsonSerializer.Deserialize<ExceptionSerializationData>(json);

        deserializedData?.Message.Should().Be(exception.Message);
        deserializedData?.HResult.Should().Be(exception.HResult);
        deserializedData?.Source.Should().Be(exception.Source);
        deserializedData?.StackTrace.Should().Be(exception.StackTrace);
    }
}
