using NugetBeeyondScreen.DTOs;
using NugetBeeyondScreen.Models;

namespace ApiBeeyondScreen.Repositories
{
    public interface IRepositoryCine
    {
        #region Asientos
        Task<List<Asiento>> GetAsientosAsync();
        Task<Asiento> FindAsientoAsync(int idAsiento);
        Task InsertAsientoAsync(int idAsiento, int idSala, int idHorario, string numero, string fila, Boolean disponible);
        Task UpdateAsientoAsync(int idAsiento, int idSala, int idHorario, string numero, string fila, Boolean disponible);
        Task DeleteAsientoAsync(int idAsiento);
        Task<ModelAsientosReserva> ReservaAsientoSalaHorarioId(int idHorarioPelicula);
        Task<List<Asiento>> GetAsientosSalaHorarioPeliculaAsync(int idHorarioPelicula, int idSala);
        Task<int> GetLastIdAsientoAsync();
        #endregion

        #region Boletos
        Task<List<Boleto>> GetBoletosAsync();
        Task<Boleto> FindBoletoAsync(int idBoleto);
        Task InsertBoletoAsync(int idBoleto, int idUsuario, int idHorario, int idAsiento, DateTime fechaCompra, string estado);
        Task InsertBoletoAsync(int idBoleto, int idUsuario, int idAsiento, DateTime fechaCompra, string estado);
        Task UpdateBoletoAsync(int idBoleto, int idUsuario, int idAsiento, DateTime fechaCompra, string estado);
        Task DeleteBoletoAsync(int idBoleto);
        Task<int> GetLastIdBoletoAsync();
        Task<List<ComboPeliculas>> GetComboPeliculasAsync();
        Task<List<ComboSalas>> GetComboSalasAsync(int idCine);
        Task<List<ComboVersiones>> GetComboVersionesAsync();
        #endregion

        #region Peliculas
        Task<ModelDetailsPelicula> GetDetailsPeliculaAsync(int idPelicula);
        Task<List<Pelicula>> GetPeliculasAsync();
        Task<Pelicula> FindPeliculaAsync(int idPelicula);
        Task InsertPeliculaAsync(int idPelicula, string titulo, DateOnly fechaLanzamiento, int duracionMinutos, string tituloEtiqueta, string sinopsis, string imgFondo, string imgPoster);
        Task UpdatePeliculaAsync(int idPelicula, string titulo, DateOnly fechaLanzamiento, int duracionMinutos, string tituloEtiqueta, string sinopsis, string imgFondo, string imgPoster);
        Task DeletePeliculaAsync(int idPelicula);
        #endregion

        #region HorarioPeliculas
        Task<List<HorarioPelicula>> GetHorarioPeliculasAsync();
        Task<HorarioPelicula> FindHorarioPeliculaAsync(int idHorarioPelicula);
        Task<HorarioPelicula> GetHorarioPeliculaAsync(int idHorarioPelicula);
        Task InserHorarioPeliculaAsync(int idHorarioPelicula, int idPelicula, int idSala, int idVersion, DateTime horaFuncion, int asientosDisponibles, char estado);
        Task UpdateHorarioPeliculaAsync(int idHorarioPelicula, int idPelicula, int idSala, int idVersion, DateTime horaFuncion, int asientosDisponibles, char estado);
        Task DeleteHorarioPeliculaAsync(int idHorarioPelicula);
        Task<int> GetUltimoIdHorarioPeliculaAsync();
        Task<List<Evento>> GetCalendarioHorarioPeliculasAsync();
        #endregion

        #region Usuarios
        Task<List<ViewFacturaBoleto>> GetFacturasBoletoUserAsync(int idUsuario);
        Task<ViewFacturaBoleto> GetFacturaBoletoUserAsync(int idBoletoUser);
        Task<int> GetLastIdUserAsync();
        Task<List<Usuario>> GetUsuariosAsync();
        Task<Usuario> FindUsuarioAsync(int idUsuario);
        Task UpdateUsuarioProfileAsync(int idUsuario, string nombre, string email, string imagen);
        Task UpdateUsuarioAsync(int idUsuario, string nombre, string email, string imagen, string password);
        Task RegisterUserAsync(string nombre, string email, string password, string imagen);
        Task<Usuario> LoginUserAsync(string email, string password);
        #endregion
    }
}

