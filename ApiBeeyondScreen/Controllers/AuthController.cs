using ApiBeeyondScreen.Helpers;
using ApiBeeyondScreen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NugetBeeyondScreen.Models;
using System.Security.Claims;
using Newtonsoft.Json;

namespace ApiBeeyondScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IRepositoryCine repo;
        private HelperActionServicesOAuth helperAction;
        public AuthController(IRepositoryCine repo, 
            HelperActionServicesOAuth helperAction)
        {
            this.repo = repo;
            this.helperAction = helperAction;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult>
        Login(LoginModel model)
        {
            Usuario usuario = await
                this.repo.LoginUserAsync(model.UserName
                , model.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                //DEBEMOS CREAR UNAS CREDENCIALES PARA 
                //INCLUIRLAS DENTRO DEL TOKEN Y QUE ESTARAN 
                //COMPUESTAS POR EL SECRET KEY CIFRADO Y EL 
                //TIPO DE CIFRADO QUE INCLUIREMOS EN EL TOKEN
                SigningCredentials credentials =
                    new SigningCredentials
                    (this.helperAction.GetKeyToken(),
                    SecurityAlgorithms.HmacSha256);
                string jsonUsuario = JsonConvert.SerializeObject(usuario);
                string jsonCifrado = HelperCryptography.EncryptString(jsonUsuario);
                //CREAMOS UN ARRAY DE CLAIMS
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonCifrado)
                };

                //EL TOKEN SE GENERA CON UNA CLASE
                //Y DEBEMOS INDICAR LOS DATOS QUE ALMACENARA EN SU 
                //INTERIOR
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helperAction.Issuer,
                        audience: this.helperAction.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        notBefore: DateTime.UtcNow
                        );
                //POR ULTIMO, DEVOLVEMOS LA RESPUESTA AFIRMATIVA
                //CON UN OBJETO QUE CONTENGA EL TOKEN (anonimo)
                return Ok(new
                {
                    response =
                    new JwtSecurityTokenHandler()
                    .WriteToken(token)
                });
            }
        }

    }
}
