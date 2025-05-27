using Microsoft.EntityFrameworkCore;
using Library.Models;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


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
        policy.WithOrigins("http://localhost:5174") // Explicitly allow frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Configure DbContext
builder.Services.AddDbContext<LibraryContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // Moved inside development block
}

var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Console.WriteLine($"Frontend directory path: {frontendPath}");

// Ensure the directory exists
if (!Directory.Exists(frontendPath))
{
    Console.WriteLine("ERROR: Frontend build directory not found!");
    Console.WriteLine("Please build your React/Vite app first and ensure it's in the correct location");
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("Allow");

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

app.Run();