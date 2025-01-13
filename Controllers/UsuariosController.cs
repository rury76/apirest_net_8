
using backendWebApi.Models.UsuarioModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendWebApi.Controllers.UsuariosController {
    class UsuariosController {

        WebApplication app;
        public UsuariosController(WebApplication app){
            this.app = app;

        }

        public void Incializar() {
            app.MapGet("/api/usuario", () => {
                return Results.Ok("Ok Datos");
            });
            
            app.MapPost("/api/usuario", [Authorize] ([FromBody] usuarioModel usuario) => {
                if (string.IsNullOrEmpty(usuario.password)) {
                    return Results.BadRequest("Es necesario la contraseña");
                } 
                if (string.IsNullOrEmpty(usuario.passwordConfirm)) {
                    return Results.BadRequest("Es necesario confirmar la contraseña");
                }
                if (usuario.password.Trim() != usuario.passwordConfirm.Trim()) {
                    return Results.BadRequest("Las contraseñas no coindiden");
                }
                return Results.Ok(usuario);
            });

            app.MapDelete("/api/usuario", () => {
                return Results.Ok("Ok delete");
            });

            app.MapPut("/api/usuario", () => {
                return Results.Ok("Ok Actualizar");
            });
        }
    }
}
