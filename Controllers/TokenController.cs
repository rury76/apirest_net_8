using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using backendWebApi.Models.LoginModel;
using Microsoft.AspNetCore.Mvc;

namespace backendWebApi.Controllers.TokenController
{
    class TokenController
    {
        private readonly WebApplication app;
        private readonly string llave;
        private readonly string validIssuer;
        private readonly string validAudience;

        public TokenController(WebApplication app, string llave, string validIssuer, string validAudience)
        {
            this.app = app;
            this.llave = llave;
            this.validIssuer = validIssuer;
            this.validAudience = validAudience;
        }

        public void Incializar()
        {
            app.MapPost("/api/token", ([FromBody] LoginModel usuario) =>
            {
                if (usuario.UserName == "admin" && usuario.Password == "1234")
                {
                    var token = GenerarJwtToken(usuario.UserName);
                    return Results.Ok(new { Token = token });
                }
                return Results.Unauthorized();
            });
        }

        private string GenerarJwtToken(string nombreUsuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nombreUsuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}