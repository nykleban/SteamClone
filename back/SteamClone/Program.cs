using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SteamClone.BLL.Services;
using SteamClone.BLL.Settings;
using SteamClone.DAL;
using SteamClone.DAL.Initializer;
using SteamClone.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add repositories
builder.Services.AddScoped<GenreRepository>();
builder.Services.AddScoped<DeveloperRepository>();
builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<GameImageRepository>();

// Add services
builder.Services.AddScoped<DeveloperService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<FileService>();


// Add automapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODA2MDE5MjAwIiwiaWF0IjoiMTc3NDU1NDcyMSIsImFjY291bnRfaWQiOiIwMTlkMmJiMzYwMTc3MzJiOGEzNDhjYzk1MzMzMjI1MiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa21udjhhdms3cndtbWdrNW1ya2s5OTZlIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.S-wcF9VgAHe6sjDgoklxsaU6OUiIOoGGVDYkMavLudxzlTT12b8j29esFln1p8NTgxraIUy7yDZ-El3OBPtGq53cj44udiegv3Xdttd7CdM6HyDeraVeG4GzLK3m1FB7O8rAGGMWKHd2yu_abTtNyW0y-6mo6ZJgX06ZEdbYcILkV4D_P_fdaXeKJZU6sIomu7_7FRNVZvvbYup-t3qLvK_cv-8a6UQuAEXAcwZWTmlFaCRbU3qjC7eAct_iITDGIJP2xNt_IF_czGKTzgLHBjtcuO1NGSsgCpLE_igYFzqBgDRNNxrnjead4XeAISEN-MFkiY_YowdvbCKR3AsRRg";
}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

// Add swagger
builder.Services.AddSwaggerGen();

// Add cors
string corsPolicy = "allowAll";
builder.Services.AddCors(cfg =>
{
    cfg.AddPolicy(corsPolicy, cfg =>
    {
        cfg.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Cors
app.UseCors(corsPolicy);

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Static files
var root = builder.Environment.ContentRootPath;
var storagePath = Path.Combine(root, StaticFilesSettings.Storage);
var sharePath = Path.Combine(storagePath, StaticFilesSettings.Share);
var developersPath = Path.Combine(storagePath, StaticFilesSettings.Developers);
var gamesPath = Path.Combine(storagePath, StaticFilesSettings.Games);

if (!Directory.Exists(storagePath))
{
    Directory.CreateDirectory(storagePath);
}

if (!Directory.Exists(sharePath))
{
    Directory.CreateDirectory(sharePath);
}

if (!Directory.Exists(developersPath))
{
    Directory.CreateDirectory(developersPath);
}

if (!Directory.Exists(gamesPath))
{
    Directory.CreateDirectory(gamesPath);
}

// Share
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(sharePath),
    RequestPath = "/share"
});

// Developers
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(developersPath),
    RequestPath = "/developer"
});

// Games
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(gamesPath),
    RequestPath = "/game"
});

app.UseAuthorization();

app.MapControllers();

app.UseCors(corsPolicy);

await app.SeedAsync();

app.Run();
