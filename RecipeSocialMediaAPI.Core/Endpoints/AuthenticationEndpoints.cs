﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecipeSocialMediaAPI.Application.Contracts.Authentication;
using RecipeSocialMediaAPI.Application.Handlers.Authentication.Queries;

namespace RecipeSocialMediaAPI.Core.Endpoints;

public static class AuthenticationEndpoints
{
    public static WebApplication MapAuthenticationEndpoints(this WebApplication app)
    {
        app.MapGroup("/auth")
            .AddAuthenticationEndpoints()
            .WithTags("Authentication");

        return app;
    }

    private static RouteGroupBuilder AddAuthenticationEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/authenticate", async (
            [FromBody] AuthenticationAttemptContract authenticationAttempt,
            [FromServices] ISender sender) =>
        {
            var successfulLogin = await sender.Send(new AuthenticateUserQuery(authenticationAttempt.HandlerOrEmail, authenticationAttempt.Password));
            return Results.Ok(successfulLogin);
        });

        return group;
    }
}
