using ApiBeeyondScreen.Helpers;
using ApiBeeyondScreen.Repositories;
using Azure.Security.KeyVault.Secrets;
using BeeyondScreen.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});

//  AZURE KEY VAULT
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secret = await secretClient.GetSecretAsync("SqlAzure");
string connectionString = secret.Value;


//  STORAGE
//KeyVaultSecret secretStorage = await secretClient.GetSecretAsync("StorageAccount");
//var azureKeys = secretStorage.Value;
//  SEGURIDAD
KeyVaultSecret secretIssuer = await secretClient.GetSecretAsync("Issuer");
HelperSeguridad.Issuer = secretIssuer.Value;
KeyVaultSecret secretAudience = await secretClient.GetSecretAsync("Audience");
HelperSeguridad.Audience = secretAudience.Value;
KeyVaultSecret secretSecretKey = await secretClient.GetSecretAsync("SecretKey");
HelperSeguridad.SecretKey = secretSecretKey.Value;
//  SALT
KeyVaultSecret secretSalt = await secretClient.GetSecretAsync("Salt");
HelperSeguridad.Salt = secretSalt.Value;
KeyVaultSecret secretKey = await secretClient.GetSecretAsync("Key");
HelperSeguridad.Key = secretKey.Value;
KeyVaultSecret secretIterate = await secretClient.GetSecretAsync("Iterate");
HelperSeguridad.Iterate = secretIterate.Value;


HelperCryptography.Initialize(builder.Configuration);
builder.Services.AddHttpContextAccessor();

HelperActionServicesOAuth helper =
    new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddTransient<IRepositoryCine, RepositoryCine>();
builder.Services.AddDbContext<CineContext>
    (options => options.UseSqlServer(connectionString));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.MapOpenApi();

app.UseHttpsRedirection();

app.MapScalarApiReference();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
