using ApiBeeyondScreen.Helpers;
using ApiBeeyondScreen.Models;
using ApiBeeyondScreen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NugetBeeyondScreen.Helpers;
using NugetBeeyondScreen.Models;
using System.Security.Claims;

namespace ApiBeeyondScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IRepositoryCine repo;
        private HelperUsuarioToken helperUsuario; 

        public UsuariosController(
            IRepositoryCine repo, 
            HelperUsuarioToken helperUsuario)
        {
            this.repo = repo;
            this.helperUsuario = helperUsuario;
        }

        // GET: api/Usuarios
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            List<Usuario> usuarios = await this.repo.GetUsuariosAsync();
            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        [Authorize]
        [HttpGet("{idUsuario}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int idUsuario)
        {
            Usuario usuario = await this.repo.FindUsuarioAsync(idUsuario);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        // GET: api/Usuarios/Perfil
        [Authorize]
        [HttpGet("Perfil")]
        public async Task<ActionResult<Usuario>> GetPerfil()
        {
            Usuario usuario = this.helperUsuario.GetUsuario();
            int idUsuario = usuario.IdUsuario;
            return await
                this.repo.FindUsuarioAsync(usuario.IdUsuario);

        }

        // PUT: api/Usuarios/Perfil
        [Authorize]
        [HttpPut("Perfil")]
        public async Task<ActionResult> UpdatePerfil(UsuarioModel usuario, string? currentPassword, string? newPassword, string? confirmPassword, bool? cambiarPassword)
        {
            try
            {
                Usuario usuarioRegistrado = this.helperUsuario.GetUsuario();
                int idUsuario = usuarioRegistrado.IdUsuario;
                Usuario usuarioActual = await this.repo.FindUsuarioAsync(idUsuario);

                if (usuarioActual == null)
                {
                    return NotFound(new { error = "Usuario no encontrado" });
                }

                // Si se solicitó cambiar la contraseña
                if (cambiarPassword.Value != null)
                {
                    // Verificar la contraseña actual
                    byte[] passActual = NugetBeeyondScreen.Helpers.HelperCryptography.EncryptPassword(currentPassword, usuarioActual.Salt);
                    bool passCorrecta = NugetBeeyondScreen.Helpers.HelperCryptography.CompararArrays(passActual, usuarioActual.Pass);
                    if (!passCorrecta)
                    {
                        return BadRequest(new { error = "La contraseña actual es incorrecta" });
                    }

                    // Verificar que las nuevas contraseñas coincidan
                    if (newPassword != confirmPassword)
                    {
                        return BadRequest(new { error = "Las nuevas contraseñas no coinciden" });
                    }

                    // Actualizar el usuario con la nueva contraseña
                    await this.repo.UpdateUsuarioAsync(
                        idUsuario,
                        usuario.Nombre,
                        usuario.Email,
                        usuario.Imagen,
                        newPassword
                    );
                }
                else
                {
                    // Actualizar el usuario sin cambiar la contraseña
                    await this.repo.UpdateUsuarioProfileAsync(
                        idUsuario,
                        usuario.Nombre,
                        usuario.Email,
                        usuario.Imagen
                    );
                }

                return Ok(new { mensaje = "Perfil actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar el perfil: " + ex.Message });
            }
        }

        // POST: api/Usuarios/Register
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDTO usuario)
        {
            try
            {
                await this.repo.RegisterUserAsync(usuario.Nombre, usuario.Email, usuario.Password, usuario.Imagen);
                return CreatedAtAction(nameof(GetUsuarios), new { mensaje = "Usuario registrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al registrar usuario: " + ex.Message });
            }
        }

        // GET: api/Usuarios/Boletos
        [Authorize]
        [HttpGet("Boletos")]
        public async Task<ActionResult<List<ViewFacturaBoleto>>> GetBoletosUser()
        {
            try
            {
                Usuario usuario = this.helperUsuario.GetUsuario();
                int idUsuario = usuario.IdUsuario;
                List<ViewFacturaBoleto> viewFacturaBoletos = await this.repo.GetFacturasBoletoUserAsync(idUsuario);
                return Ok(viewFacturaBoletos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Usuarios/Boletos/5
        [Authorize]
        [HttpGet("Boletos/{idBoletoUser}")]
        public async Task<ActionResult<ViewFacturaBoleto>> GetDetailsBoletoUser(int idBoletoUser)
        {
            try
            {
                ViewFacturaBoleto viewFacturaBoleto = await this.repo.GetFacturaBoletoUserAsync(idBoletoUser);
                if (viewFacturaBoleto == null)
                {
                    return NotFound();
                }
                return Ok(viewFacturaBoleto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
