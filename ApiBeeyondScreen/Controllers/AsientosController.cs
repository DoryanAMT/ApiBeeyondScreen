using ApiBeeyondScreen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetBeeyondScreen.Models;
using System.Security.Claims;

namespace ApiBeeyondScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsientosController : ControllerBase
    {
        private readonly IRepositoryCine repo;

        public AsientosController(IRepositoryCine repo)
        {
            this.repo = repo;
        }

        // GET: api/Asientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asiento>>> GetAsientos()
        {
            List<Asiento> asientos = await this.repo.GetAsientosAsync();
            return Ok(asientos);
        }

        // GET: api/Asientos/5
        [HttpGet("{idAsiento}")]
        public async Task<ActionResult<Asiento>> GetAsiento(int idAsiento)
        {
            Asiento asiento = await this.repo.FindAsientoAsync(idAsiento);

            if (asiento == null)
            {
                return NotFound();
            }

            return Ok(asiento);
        }

        // POST: api/Asientos
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Asiento>> CreateAsiento(Asiento asiento)
        {
            await this.repo.InsertAsientoAsync(
                asiento.IdAsiento,
                asiento.IdSala,
                asiento.IdHorario,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
            );

            return CreatedAtAction(nameof(GetAsiento), new { idAsiento = asiento.IdAsiento }, asiento);
        }

        // PUT: api/Asientos/5
        [Authorize]
        [HttpPut("{idAsiento}")]
        public async Task<IActionResult> UpdateAsiento(int idAsiento, Asiento asiento)
        {
            if (idAsiento != asiento.IdAsiento)
            {
                return BadRequest();
            }

            await this.repo.UpdateAsientoAsync(
                asiento.IdAsiento,
                asiento.IdSala,
                asiento.IdHorario,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
            );

            return NoContent();
        }

        // DELETE: api/Asientos/5
        [Authorize]
        [HttpDelete("{idAsiento}")]
        public async Task<IActionResult> DeleteAsiento(int idAsiento)
        {
            var asiento = await this.repo.FindAsientoAsync(idAsiento);
            if (asiento == null)
            {
                return NotFound();
            }

            await this.repo.DeleteAsientoAsync(idAsiento);
            return NoContent();
        }

        // GET: api/Asientos/Reserva/5
        [Authorize]
        [HttpGet("Reserva/{idHorario}")]
        public async Task<ActionResult<ModelAsientosReserva>> GetAsientosReserva(int idHorario)
        {
            ModelAsientosReserva model = await this.repo.ReservaAsientoSalaHorarioId(idHorario);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        // POST: api/Asientos/Reserva
        [Authorize]
        [HttpPost("Reserva")]
        public async Task<ActionResult> CreateAsientoReserva(int usuarioId, Asiento asiento)
        {

            int idAsiento = await this.repo.GetLastIdAsientoAsync();
            await this.repo.InsertAsientoAsync(
                idAsiento,
                asiento.IdSala,
                asiento.IdHorario,
                asiento.Numero,
                asiento.Fila,
                asiento.Disponible
            );

            int idBoleto = await this.repo.GetLastIdBoletoAsync();
            await this.repo.InsertBoletoAsync(
                idBoleto,
                usuarioId,
                asiento.IdHorario,
                idAsiento,
                DateTime.Now,
                "Confirmado"
            );

            return CreatedAtAction(nameof(GetAsiento), new { idAsiento }, new { idAsiento, idBoleto });
        }
    }
}
