using Microsoft.EntityFrameworkCore;
using Library.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
		.AddEnvironmentVariables();


// Add services to the container.
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "https://linuxlibrary-fyf7b2ctfbb2ebc3.westeurope-01.azurewebsites.net") // Explicitly allow frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Only if using cookies/auth
    });
});

builder.Services.AddControllers();
// Configure DbContext
builder.Services.AddDbContext<LibraryContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var port  = Environment.GetEnvironmentVariable("PORT") ?? "8080";

var app = builder.Build();


var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine($"frontend: {frontendPath}");
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // Moved inside development block
}


Console.WriteLine($"Frontend directory path: {frontendPath}");
Console.WriteLine(Directory.GetCurrentDirectory());
    
// Ensure the directory exists
if (!Directory.Exists(frontendPath))
{
    Console.WriteLine("ERROR: Frontend build directory not found!");
    Console.WriteLine("Please build your React/Vite app first and ensure it's in the correct location");
}

/* using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.Migrate();
    db.
} */

app.UseHttpsRedirection();

app.UseCors("Allow");

app.UseRouting();



// Serve static files from frontend dist directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
    RequestPath = ""
});

app.UseAuthorization();

app.MapControllers();

// Fallback to index.html for client-side routing
app.MapFallbackToFile("index.html", new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath)
});

//app.Urls.Add($"http://*:{port}");

app.Run();