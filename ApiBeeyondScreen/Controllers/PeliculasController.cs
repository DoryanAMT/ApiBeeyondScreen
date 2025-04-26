using ApiBeeyondScreen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetBeeyondScreen.Models;

namespace ApiBeeyondScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IRepositoryCine repo;

        public PeliculasController(IRepositoryCine repo)
        {
            this.repo = repo;
        }

        // GET: api/Peliculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pelicula>>> GetPeliculas()
        {
            List<Pelicula> peliculas = await this.repo.GetPeliculasAsync();
            return Ok(peliculas);
        }

        // GET: api/Peliculas/5
        [HttpGet("{idPelicula}")]
        public async Task<ActionResult<ModelDetailsPelicula>> GetPelicula(int idPelicula)
        {
            ModelDetailsPelicula model = await this.repo.GetDetailsPeliculaAsync(idPelicula);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        // POST: api/Peliculas
        [HttpPost]
        public async Task<ActionResult<Pelicula>> CreatePelicula(Pelicula pelicula)
        {
            await this.repo.InsertPeliculaAsync(pelicula.IdPelicula, pelicula.Titulo,
                pelicula.FechaLanzamiento, pelicula.DuracionMinutos, pelicula.TituloEtiqueta,
                pelicula.Sinopsis, pelicula.ImgFondo, pelicula.ImgPoster);

            return CreatedAtAction("GetPelicula", new { idPelicula = pelicula.IdPelicula }, pelicula);
        }

        // PUT: api/Peliculas/5
        [HttpPut("{idPelicula}")]
        public async Task<IActionResult> UpdatePelicula(int idPelicula, Pelicula pelicula)
        {
            if (idPelicula != pelicula.IdPelicula)
            {
                return BadRequest();
            }

            await this.repo.UpdatePeliculaAsync(pelicula.IdPelicula, pelicula.Titulo,
                pelicula.FechaLanzamiento, pelicula.DuracionMinutos, pelicula.TituloEtiqueta,
                pelicula.Sinopsis, pelicula.ImgFondo, pelicula.ImgPoster);

            return NoContent();
        }

        // DELETE: api/Peliculas/5
        [HttpDelete("{idPelicula}")]
        public async Task<IActionResult> DeletePelicula(int idPelicula)
        {
            await this.repo.DeletePeliculaAsync(idPelicula);
            return NoContent();
        }
    }
}
