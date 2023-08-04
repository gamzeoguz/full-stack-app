using BackendProjem.Caching.Core;
using BackendProjem.Caching.Redis;
using BackendProjem.Infrastructure;
using BackendProjem.Infrastructure.Context;
using BackendProjem.Infrastructure.Services;
using BackendProjem.Serialization.Core;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer(options =>

        {

            options.AccessTokenJwtType = "JWT";

options.EmitStaticAudienceClaim = true;

        })

        .AddDeveloperSigningCredential()

        .AddInMemoryApiResources(new List<ApiResource>

                {

                    new ApiResource("Company_Scope_Api_1", "CompanyTokenApi"),

                })

        .AddInMemoryApiScopes(new List<ApiScope> {

            new ApiScope()

            {

                Name =  "Company_Scope_Api_1",

                DisplayName = "CompanyTokenApi"

            }

        })

        .AddInMemoryClients(new List<Client>

        {

            new Client

            {

                ClientId = "Company_Client_Id",

                AllowedGrantTypes = IdentityServer4.Models.GrantTypes.ClientCredentials,

                ClientSecrets =

                {

                    new Secret("Company_Secret_Token_543dfg765lkv_Key".Sha256())

                },

                AllowedScopes = {"Company_Scope_Api_1"},

                AccessTokenType = AccessTokenType.Jwt,

                RefreshTokenUsage=TokenUsage.OneTimeOnly,

            }

        });

string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";



var configurationBuilder = builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();



var Configuration = configurationBuilder.Build();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CompanyDbContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("CompanyDb"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIocContainer();
builder.Services.AddTransient<ITestUserRepository, TestUserRepository>();
builder.Services.AddTransient<ITestUserService, TestUserService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = "https://localhost:7282";
    options.Audience = "Company_Scope_Api_1";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddCors(options =>
     options.AddDefaultPolicy(builder =>
     builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.AddSingleton<IByteSerializer, ByteSerializer>();
builder.Services.AddSingleton<ICacheManager, CacheManager>();
builder.Services.AddSingleton<IDistributedCache, RedisCacheManager>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddTransient<ITestCompanyRepository, TestCompanyRepository>();
builder.Services.AddTransient<ITestCompanyService, TestCompanyService>();

var app = builder.Build();
app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}



app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
