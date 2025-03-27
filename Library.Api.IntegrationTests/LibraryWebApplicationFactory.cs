using Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.IntegrationTests;

internal class LibraryWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<LibraryContext>));

            var connString = GetConnectionString();
            services.AddSqlServer<LibraryContext>(connString);
        });
    }

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<LibraryWebApplicationFactory>()
            .Build();
        
        var connectionString = configuration.GetConnectionString("Library");
        return connectionString;
    }
}