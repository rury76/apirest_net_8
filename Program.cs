using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backendWebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile(
    $"appsettings.{builder.Environment.EnvironmentName}.json", 
    optional: true, 
    reloadOnChange: true
);

var llave = builder.Configuration["settings:llave"];
var validIssuer = builder.Configuration["settings:validIssuer"];
var validAudience = builder.Configuration["settings:validAudience"];

if (string.IsNullOrEmpty(llave)) throw new ApplicationException("llave is null");
if (string.IsNullOrEmpty(validIssuer)) throw new ApplicationException("validIssuer is null");
if (string.IsNullOrEmpty(validAudience)) throw new ApplicationException("validAudience is null");

Console.WriteLine($"Entorno de ejecución de {builder.Environment.ApplicationName}: {builder.Environment.EnvironmentName}");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

new TokenController(app, llave, validIssuer, validAudience).Incializar();
new UsuariosController(app).Incializar();

app.Run();



