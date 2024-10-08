using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PiggyScaleApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using Project.Hubs;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    string dbName = builder.Configuration["DB_NAME"];
    var dbPath = Path.Combine(Directory.GetCurrentDirectory(), dbName);
    var connectionString = $"Data Source={dbPath}";

    options.UseSqlite(connectionString);
    
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer((x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT_SETTINGS_ISSUER"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SETTINGS_KEY"])),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
}));
builder.Services.AddAuthorizationCore();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "PiggyScaleApi",
        Description = "Persistent Data Storage for PiggyScale",
        TermsOfService = new Uri("https://github.com/Altishofer/piggyScale-Server"),
        Contact = new OpenApiContact
        {
            Name = "Sandrin Hunkeler",
            Email = "sandrin@hunkeler.ws",
            Url = new Uri("https://github.com/Altishofer"),
        },
        License = new OpenApiLicense
        {
            Name = "PragimTech Open License",
            Url = new Uri("https://pragimtech.com"),
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
        c.RoutePrefix = "swagger";
    });
}



using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
dbContext.Database.Migrate();

app.MapControllers();
app.MapHub<QuizHub>("PigHub");

app.UseCors(x => x
    .AllowAnyOrigin() //.WithOrigins("http://localhost:4200", "http://172.23.49.21:8017")
    .AllowAnyHeader()
    .AllowAnyMethod());

//app.UseHttpsRedirection();
app.UseWebSockets();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
