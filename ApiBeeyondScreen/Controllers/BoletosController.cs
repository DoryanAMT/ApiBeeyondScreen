using ApiBeeyondScreen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetBeeyondScreen.Models;

namespace ApiBeeyondScreen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoletosController : ControllerBase
    {
        private readonly IRepositoryCine repo;

        public BoletosController(IRepositoryCine repo)
        {
            this.repo = repo;
        }

        // GET: api/Boletos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletos()
        {
            List<Boleto> boletos = await this.repo.GetBoletosAsync();
            return Ok(boletos);
        }

        // GET: api/Boletos/5
        [HttpGet("{idBoleto}")]
        public async Task<ActionResult<Boleto>> GetBoleto(int idBoleto)
        {
            Boleto boleto = await this.repo.FindBoletoAsync(idBoleto);
            if (boleto == null)
            {
                return NotFound();
            }
            return Ok(boleto);
        }

        // POST: api/Boletos
        [HttpPost]
        public async Task<ActionResult<Boleto>> CreateBoleto(Boleto boleto)
        {
            int ultimoId = await this.repo.GetLastIdBoletoAsync();
            await this.repo.InsertBoletoAsync(
                ultimoId,
                boleto.IdUsuario,
                boleto.IdAsiento,
                boleto.FechaCompra,
                boleto.Estado);

            return CreatedAtAction(nameof(GetBoleto), new { idBoleto = ultimoId }, boleto);
        }

        // PUT: api/Boletos/5
        [HttpPut("{idBoleto}")]
        public async Task<IActionResult> UpdateBoleto(int idBoleto, Boleto boleto)
        {
            if (idBoleto != boleto.IdBoleto)
            {
                return BadRequest();
            }

            await this.repo.UpdateBoletoAsync(
                boleto.IdBoleto,
                boleto.IdUsuario,
                boleto.IdAsiento,
                boleto.FechaCompra,
                boleto.Estado);

            return NoContent();
        }

        // DELETE: api/Boletos/5
        [HttpDelete("{idBoleto}")]
        public async Task<IActionResult> DeleteBoleto(int idBoleto)
        {
            Boleto boleto = await this.repo.FindBoletoAsync(idBoleto);
            if (boleto == null)
            {
                return NotFound();
            }

            await this.repo.DeleteBoletoAsync(idBoleto);
            return NoContent();
        }
    }
}
