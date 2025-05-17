using NugetBeeyondScreen.DTOs;
using NugetBeeyondScreen.Models;

namespace ApiBeeyondScreen.Models
{
    public class OpcionesDTO
    {
        public List<ComboPeliculas> Peliculas { get; set; }
        public List<ComboSalas> Salas{ get; set; }
        public List<ComboVersiones> Versiones { get; set; }
        public List<Evento> Eventos { get; set; }
    }
}
