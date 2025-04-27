using ApiBeeyondScreen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetBeeyondScreen.Models;

namespace ApiBeeyondScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioPeliculasController : ControllerBase
    {
        private IRepositoryCine repo;

        public HorarioPeliculasController(IRepositoryCine repo)
        {
            this.repo = repo;
        }

        // GET: api/HorarioPeliculas
        [HttpGet]
        public async Task<ActionResult<List<HorarioPelicula>>> GetHorarioPeliculas()
        {
            List<HorarioPelicula> horarioPeliculas = await repo.GetHorarioPeliculasAsync();
            return Ok(horarioPeliculas);
        }

        // GET: api/HorarioPeliculas/5
        [HttpGet("{idHorarioPelicula}")]
        public async Task<ActionResult<HorarioPelicula>> GetHorarioPelicula(int idHorarioPelicula)
        {
            HorarioPelicula horarioPelicula = await repo.FindHorarioPeliculaAsync(idHorarioPelicula);

            if (horarioPelicula == null)
            {
                return NotFound();
            }

            return Ok(horarioPelicula);
        }

        // GET: api/HorarioPeliculas/Opciones
        [HttpGet("Opciones")]
        public async Task<ActionResult> GetOpciones()
        {
            int idCine = 762509; // Este valor deberá obtenerse de otra forma en producción

            var result = new
            {
                Peliculas = await repo.GetComboPeliculasAsync(),
                Salas = await repo.GetComboSalasAsync(idCine),
                Versiones = await repo.GetComboVersionesAsync(),
                CalendarioHorarios = await repo.GetCalendarioHorarioPeliculasAsync()
            };

            return Ok(result);
        }

        // POST: api/HorarioPeliculas
        [HttpPost]
        public async Task<ActionResult<HorarioPelicula>> PostHorarioPelicula(HorarioPelicula horarioPelicula)
        {
            int ultimoId = await repo.GetUltimoIdHorarioPeliculaAsync();

            await repo.InserHorarioPeliculaAsync(
                ultimoId,
                horarioPelicula.IdPelicula,
                horarioPelicula.IdSala,
                horarioPelicula.IdVersion,
                horarioPelicula.HoraFuncion,
                horarioPelicula.AsientosDisponibles,
                horarioPelicula.Estado
            );

            // Configurar la respuesta para devolver la ruta al recurso creado
            return CreatedAtAction(
                nameof(GetHorarioPelicula),
                new { idHorarioPelicula = ultimoId },
                horarioPelicula
            );
        }

        // PUT: api/HorarioPeliculas/5
        [HttpPut("{idHorarioPelicula}")]
        public async Task<IActionResult> PutHorarioPelicula(int idHorarioPelicula, HorarioPelicula horarioPelicula)
        {
            if (idHorarioPelicula != horarioPelicula.IdHorario)
            {
                return BadRequest();
            }

            await repo.UpdateHorarioPeliculaAsync(
                horarioPelicula.IdHorario,
                horarioPelicula.IdPelicula,
                horarioPelicula.IdSala,
                horarioPelicula.IdVersion,
                horarioPelicula.HoraFuncion,
                horarioPelicula.AsientosDisponibles,
                horarioPelicula.Estado
            );

            return NoContent();
        }

        // DELETE: api/HorarioPeliculas/5
        [HttpDelete("{idHorarioPelicula}")]
        public async Task<IActionResult> DeleteHorarioPelicula(int idHorarioPelicula)
        {
            var horarioPelicula = await repo.FindHorarioPeliculaAsync(idHorarioPelicula);
            if (horarioPelicula == null)
            {
                return NotFound();
            }

            await repo.DeleteHorarioPeliculaAsync(idHorarioPelicula);

            return NoContent();
        }
    }
}
