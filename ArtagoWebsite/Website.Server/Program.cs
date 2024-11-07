using Application;
using Data;
using Data.Context;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Shared;
using System.Text;
using Website.Server.Utils;
var builder = WebApplication.CreateBuilder(args);

// Add exception handlers and custom error messages here:


// Add services to the container.
builder.Services.addData(builder.Configuration);
builder.Services.addApplication();
builder.Services.AddControllers();
builder.Services.addShared();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// for Identity
builder.Services.AddIdentity<SystemUser, IdentityRole>(o =>
{
    o.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? string.Empty))
    };
});
string allowOrigins = "allowSpecificOrigins";
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: allowOrigins,
            policy =>
            {
                policy.WithOrigins(new string[] { "http://localhost:5173" })
                 .WithHeaders(new string[] { HeaderNames.ContentType, HeaderNames.Accept, HeaderNames.Authorization })
                 .WithMethods(new string[] { "GET", "POST", "DELETE", "PUT", "HEAD" });
            });
    });
}


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userContext = services.GetRequiredService<DatabaseContext>();
    var userManager = services.GetRequiredService<UserManager<SystemUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DatabaseInitializer.InitializeAsync(userContext, userManager, roleManager);
}

app.UseCors(allowOrigins);
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.GenerateClients("/swagger/v1/swagger.json", "../Website.client/src/utilities/api/client.ts", "../../../../../apiClient.cs");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
