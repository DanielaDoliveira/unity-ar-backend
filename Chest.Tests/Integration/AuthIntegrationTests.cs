using System.Net;
using System.Net.Http.Json;
using Chest.Application.DTOs;
using Chest.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Chest.Tests.Integration;


public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
     
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
             
                var sp = services.BuildServiceProvider();

            
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ChestDbContext>();

         
                db.Database.EnsureCreated();
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Register_ShouldCreateUserInDatabase_WhenDataIsValid()
    {
        // Arrange
        var request = new UserRegisterRequest { Name = "PirataReal", Password = "senhaSegura123" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        // SE DEU ERRO 500, vamos ler o que o servidor respondeu antes de falhar o teste
        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            var errorDetail = await response.Content.ReadAsStringAsync();
            throw new System.Exception($"O servidor explodiu com: {errorDetail}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}