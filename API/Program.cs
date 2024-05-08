using System.Text.Json.Serialization;
using API.Extensions;
using Core.Entities.AppUser;
using Core.Interfaces;
using Core.Interfaces.auth;
using Core.Interfaces.licenses;
using FileUpload.Services;
using Infrastructure.Data;
using Infrastructure.Data.Auth;
using Infrastructure.Data.Identity;
using Infrastructure.Data.Licenses;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEntityFrameworkMySQL()
                .AddDbContext<StoreContext>(options =>
                {
                    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

builder.Services.AddEntityFrameworkMySQL()
                           .AddDbContext<AppIdentityDbContext>(options =>
                           {
                               options.UseMySQL(builder.Configuration.GetConnectionString("IdentityConnection"));
                           });

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
builder.Services.AddScoped<ISubAdminService, SubAdminService>();

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IKnowledgeBaseRepository, KnowledgeBaseRepository>();
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<ISharedResourceRepository, SharedResourceRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<ISoftwareProductRepository, SoftwareProductRepository>();
builder.Services.AddScoped<ILicenseManagerRepository, LicenseManagerRepository>();
builder.Services.AddScoped<ILicenseExpirationService, LicenseExpirationService>();
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
builder.Services.AddScoped<ILicenseDashboardService, LicenseDashboardService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
    app.UseStaticFiles();

    app.UseSwaggerDocumentation();
}


app.UseSwaggerDocumentation();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);

    var userManager = services.GetRequiredService<UserManager<SubAdmin>>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityContext.Database.MigrateAsync();
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager, logger);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
