using Newtonsoft.Json;
using NugetBeeyondScreen.Models;
using System.Security.Claims;

namespace ApiBeeyondScreen.Helpers
{
    public class HelperUsuarioToken
    {
        private IHttpContextAccessor contextAccessor;

        public HelperUsuarioToken(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public Usuario GetUsuario()
        {
            Claim claim =
                this.contextAccessor.HttpContext
                .User.FindFirst(x => x.Type == "UserData");
            string json = claim.Value;
            string jsonUsuarioRaw =
                HelperCryptography.DecryptString(json);
            Usuario model = JsonConvert
                .DeserializeObject<Usuario>(jsonUsuarioRaw);
            return model;
        }
    }
}
