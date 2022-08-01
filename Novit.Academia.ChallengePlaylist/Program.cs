using Academia.JwtWrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Novit.Academia.ChallengePlaylist.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Playlist API",
        Description = "Novit Challenge Playlist",
    });
    
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Playlist API",
        Description = "Novit Challenge Playlist",
    });
});

var connectionString = builder.Configuration.GetConnectionString("playlist_db");
builder.Services.AddDbContext<PlaylistContexto>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<DbContext, PlaylistContexto>();
builder.Services.AddSingleton<JwtService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Sin Autenticacion");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Con Autenticacion");
});

app.MapControllers();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/v2/playlist"), appBuilder =>
{
    appBuilder.UseMiddleware<AutorizacionMiddleware>();
});

app.Run();