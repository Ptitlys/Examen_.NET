using System.Security.Claims;
using System.Text;
using Examen.Datas;
using Examen.Datas.Seed;
using Examen.Middlewares;
using Examen.Models;
using Examen.Models.Entities;
using Examen.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

// data access
builder.Services.AddScoped<IInterventionDataAccess, InterventionDataAccess>();
builder.Services.AddScoped<IRefreshTokenDataAccess, RefreshTokenDataAccess>();
builder.Services.AddScoped<ICustomerDataAccess, CustomerDataAccess>();
builder.Services.AddScoped<ITechnicianDataAccess, TechnicianDataAccess>();
builder.Services.AddScoped<IServiceTypeDataAccess, ServiceTypeDataAccess>();
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();

// services
builder.Services.AddScoped<IInterventionService, InterventionService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//seeders
builder.Services.AddScoped<ISeederAsync, UserSeederAsync>();
builder.Services.AddScoped<DbSeeder>();

builder.Services.AddIdentity<User, Role>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredUniqueChars = 1;
        options.Lockout.MaxFailedAccessAttempts = 5;
    
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    var seeder = services.GetRequiredService<DbSeeder>();
    // seeder.Seed(context);
    await seeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization(); 
app.UseRequestLocalization(options =>
{
    var supportedCultures = new[] { "en-US", "fr-FR" };
    options.SetDefaultCulture("fr-FR")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});
app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<ExceptionToJsonResponseMiddleware>();


app.MapControllers();

app.Run();