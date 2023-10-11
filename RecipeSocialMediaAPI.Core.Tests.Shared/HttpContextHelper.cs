﻿using Microsoft.AspNetCore.Http;

namespace RecipeSocialMediaAPI.Core.Tests.Shared;

public class HttpContextHelper
{
    public static async Task<string> GetResponseBodyAsync(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        return await reader.ReadToEndAsync();
    }
}
