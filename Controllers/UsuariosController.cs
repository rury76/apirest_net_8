using backendWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendWebApi.Controllers {
    internal class UsuariosController(WebApplication app)
    {
        public void Incializar() {
            app.MapGet("/api/usuario", () => Results.Ok("Ok Datos"));
            
            app.MapPost("/api/usuario", [Authorize] ([FromBody] usuarioModel usuario) => {
                if (string.IsNullOrEmpty(usuario.password)) 
                    return Results.BadRequest("Es necesario la contraseña");
                if (string.IsNullOrEmpty(usuario.passwordConfirm)) 
                    return Results.BadRequest("Es necesario confirmar la contraseña");
                return usuario.password.Trim() != usuario.passwordConfirm.Trim() ? Results.BadRequest("Las contraseñas no coindiden") : Results.Ok(usuario);
            });

            app.MapDelete("/api/usuario", (string id) => Results.Ok($"Ok delete {id}"));

            app.MapPut("/api/usuario", () => Results.Ok("Ok Actualizar"));
        }
    }
}
