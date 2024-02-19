using RecipeSocialMediaAPI.Core.Utilities;
using RecipeSocialMediaAPI.Core.Configuration;
using RecipeSocialMediaAPI.Core.Middleware;
using RecipeSocialMediaAPI.Core.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureSwagger();

builder.ConfigureOptions();
builder.ConfigureServices();
builder.ConfigureAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMappingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MessagingHub>("messaging-hub");
app.UseCors("AllowAll");
app.MapEndpoints();
app.Run();


public partial class Program { }
