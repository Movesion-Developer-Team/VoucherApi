using System.Text;
using DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MobilityManagerApi;
using Persistence;
using Swashbuckle.AspNetCore.Filters;
using UserStoreLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AuthSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization(opt =>
{
    opt.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddUserManager<UserManager<IdentityUser>>();

builder.Services.AddScoped<IUserStore<IdentityUser>, UserStore<IdentityUser, IdentityRole, UserDbContext>>();
builder.Services.AddAutoMapper(typeof(CompanyDto).Assembly);


var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:PostgreSqlConnectionString");

builder.Services.AddDbContext<VoucherContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddDbContext<UserDbContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbInitializer = services.GetService<IDbInitializer>();
    await dbInitializer.Initialize();
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

app.MapControllers();

app.Run();