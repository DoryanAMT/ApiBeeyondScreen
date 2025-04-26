using BeeyondScreen.Data;
using NugetBeeyondScreen.DTOs;
using NugetBeeyondScreen.Helpers;
using NugetBeeyondScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBeeyondScreen.Repositories
{
    public class RepositoryCine: IRepositoryCine
    {
        private CineContext context;
        public RepositoryCine(CineContext context)
        {
            this.context = context;
        }

        #region Asientos
        public async Task<List<Asiento>> GetAsientosAsync()
        {
            return await this.context.Asientos.ToListAsync();
        }
        public async Task<Asiento> FindAsientoAsync
            (int idAsiento)
        {
            return await this.context.Asientos
                .Where(x => x.IdAsiento == idAsiento)
                .FirstOrDefaultAsync();
        }

        public async Task InsertAsientoAsync
            (int idAsiento, int idSala, int idHorario,
            string numero, string fila, Boolean disponible)
        {
            Asiento asiento = new Asiento();
            asiento.IdAsiento = idAsiento;
            asiento.IdSala = idSala;
            asiento.IdHorario = idHorario;
            asiento.Numero = numero;
            asiento.Fila = fila;
            asiento.Disponible = disponible;
            await this.context.Asientos.AddAsync(asiento);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAsientoAsync
            (int idAsiento, int idSala, int idHorario,
            string numero, string fila, Boolean disponible)
        {
            Asiento asiento = await this.FindAsientoAsync(idAsiento);
            asiento.IdAsiento = idAsiento;
            asiento.IdSala = idSala;
            asiento.IdHorario = idHorario;
            asiento.Numero = numero;
            asiento.Fila = fila;
            asiento.Disponible = disponible;
            await this.context.SaveChangesAsync();
        }
        public async Task DeleteAsientoAsync
            (int idAsiento)
        {
            Asiento asiento = await this.FindAsientoAsync(idAsiento);
            this.context.Asientos.Remove(asiento);
            await this.context.SaveChangesAsync();
        }

        //  INSERT DE ASIENTOS SEGUN EL ID DEL HORARIO
        public async Task<ModelAsientosReserva> ReservaAsientoSalaHorarioId
            (int idHorarioPelicula)
        {
            ModelAsientosReserva model = new ModelAsientosReserva();
            HorarioPelicula horarioPelicula = await this.GetHorarioPeliculaAsync(idHorarioPelicula);
            // la vista me devuelve la informacion que necesito para recuperar los aientos que estan disponibles
            Pelicula pelicula = await this.FindPeliculaAsync(horarioPelicula.IdPelicula);
            List<Asiento> asientosOcupados = await this.GetAsientosSalaHorarioPeliculaAsync(idHorarioPelicula, horarioPelicula.IdSala);
            model.HorarioPelicula = horarioPelicula;
            model.Pelicula = pelicula;
            model.Asientos = asientosOcupados;
            //  POR OTRA PARTE NOS ENCARGAMOS DE GENERAR LOS BOLETOS POR CADA ASIENTO
            return model;

        }

        //  INSERTAR BOLETO
        public async Task InsertBoletoAsync
            (int idBoleto, int idUsuario, int idHorario,
            int idAsiento, DateTime fechaCompra, string estado)
        {

            Boleto boleto = new Boleto();
            boleto.IdBoleto = idBoleto;
            boleto.IdUsuario = idUsuario;
            boleto.IdHorario = idHorario;
            boleto.IdAsiento = idAsiento;
            boleto.FechaCompra = fechaCompra;
            boleto.Estado = estado;
            await this.context.AddAsync(boleto);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Asiento>> GetAsientosSalaHorarioPeliculaAsync
            (int idHorarioPelicula, int idSala)
        {
            return await this.context.Asientos
                .Where(x => x.IdHorario == idHorarioPelicula
                && x.IdSala == idSala)
                .ToListAsync();
        }

        public async Task<HorarioPelicula> GetHorarioPeliculaAsync
            (int idHorarioPelicula)
        {
            return await this.context.HorarioPeliculas
                .Where(x => x.IdHorario == idHorarioPelicula)
                .FirstOrDefaultAsync();
        }
        public async Task<Pelicula> FindPeliculaAsync
           (int idPelicula)
        {
            var consulta = from datos in this.context.Peliculas
                           .Where(x => x.IdPelicula == idPelicula)
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task<int> GetLastIdAsientoAsync()
        {
            {
                var consulta = this.context.Asientos.Any() ?
                    this.context.Asientos.Max(x => x.IdAsiento) + 1 :
                    1;
                int ultimoId = int.Parse(consulta.ToString());
                return ultimoId;
            }
        }
        public async Task<int> GetLastIdBoletoAsync()
        {
            {
                var consulta = this.context.Boletos.Any() ?
                    this.context.Boletos.Max(x => x.IdBoleto) + 1 :
                    1;
                int ultimoId = int.Parse(consulta.ToString());
                return ultimoId;
            }
        }
        #endregion

        #region Boletos
        //  CRUD DE BOLETOS
        public async Task<List<Boleto>> GetBoletosAsync()
        {
            return await this.context.Boletos.ToListAsync();
        }
        public async Task<Boleto> FindBoletoAsync
            (int idBoleto)
        {
            return await this.context.Boletos
                .Where(x => x.IdBoleto == idBoleto)
                .FirstOrDefaultAsync();
        }
        //  INSERTAR BOLETO
        public async Task InsertBoletoAsync
            (int idBoleto, int idUsuario, int idAsiento,
            DateTime fechaCompra, string estado)
        {
            fechaCompra = DateTime.Now;
            Boleto boleto = new Boleto();
            boleto.IdBoleto = idBoleto;
            boleto.IdUsuario = idUsuario;
            boleto.IdAsiento = idAsiento;
            boleto.FechaCompra = fechaCompra;
            boleto.Estado = estado;
            await this.context.AddAsync(boleto);
            await this.context.SaveChangesAsync();
        }
        //  ACTUALIZAR UN BOLETO
        public async Task UpdateBoletoAsync
            (int idBoleto, int idUsuario, int idAsiento,
            DateTime fechaCompra, string estado)
        {
            Boleto boleto = await this.FindBoletoAsync(idBoleto);
            boleto.IdBoleto = idBoleto;
            boleto.IdUsuario = idUsuario;
            boleto.IdAsiento = idAsiento;
            boleto.FechaCompra = fechaCompra;
            boleto.Estado = estado;
            await this.context.SaveChangesAsync();
        }
        //  ELIMINAR UN BOLETO
        public async Task DeleteBoletoAsync
            (int idBoleto)
        {
            Boleto boleto = await this.FindBoletoAsync(idBoleto);
            this.context.Remove(boleto);
            await this.context.SaveChangesAsync();
        }

        //  COMBO PELICULAS
        public async Task<List<ComboPeliculas>> GetComboPeliculasAsync()
        {
            var consulta = await this.context.Peliculas
                .Select(x => new ComboPeliculas
                {
                    Id = x.IdPelicula,
                    Nombre = x.Titulo
                })
                .ToListAsync();
            return consulta;
        }
        //  COMBO SALAS
        public async Task<List<ComboSalas>> GetComboSalasAsync
            (int idCine)
        {
            var consulta = await this.context.Salas
                .Where(x => x.IdCine == idCine)
                .Select(x => new ComboSalas
                {
                    Id = x.IdSala,
                    Nombre = x.Nombre
                })
                .ToListAsync();
            return consulta;
        }
        //  COMBO VERSIONES
        public async Task<List<ComboVersiones>> GetComboVersionesAsync()
        {
            var consulta = await this.context.Versions
                .Select(x => new ComboVersiones
                {
                    Id = x.IdVersion,
                    Nombre = x.Idioma
                })
                .ToListAsync();
            return consulta;
        }

        #endregion

        #region Peliculas
        public async Task<ModelDetailsPelicula> GetDetailsPeliculaAsync
            (int idPelicula)
        {
            ModelDetailsPelicula model = new ModelDetailsPelicula();
            var detailsPelicula = await this.context.Peliculas
                .Where(x => x.IdPelicula == idPelicula)
                .FirstOrDefaultAsync();
            var horarioPelicula = await this.context.HorarioPeliculas
                .Where(x => x.IdPelicula == idPelicula)
                .ToListAsync();
            model.Pelicula = detailsPelicula;
            model.HorarioPelicula = horarioPelicula;
            return model;

        }

        //  RECUPERAR TODAS LAS PELICULAS
        public async Task<List<Pelicula>> GetPeliculasAsync()
        {
            var consulta = from datos in this.context.Peliculas
                           orderby datos.FechaLanzamiento descending
                           select datos;
            return await consulta.ToListAsync();
        }
        
        //  INSERTAR UNA PELICULA
        public async Task InsertPeliculaAsync
            (int idPelicula, string titulo, DateOnly fechaLanzamiento,
            int duracionMinutos, string tituloEtiqueta, string sinopsis,
            string imgFondo, string imgPoster)
        {
            Pelicula pelicula = new Pelicula();
            pelicula.IdPelicula = idPelicula;
            pelicula.Titulo = titulo;
            pelicula.FechaLanzamiento = fechaLanzamiento;
            pelicula.DuracionMinutos = duracionMinutos;
            pelicula.TituloEtiqueta = tituloEtiqueta;
            pelicula.Sinopsis = sinopsis;
            pelicula.ImgFondo = imgFondo;
            pelicula.ImgPoster = imgPoster;
            await this.context.Peliculas.AddAsync(pelicula);
            await this.context.SaveChangesAsync();
        }
        //  ACTUALIZAR UNA PELICULA
        public async Task UpdatePeliculaAsync
            (int idPelicula, string titulo, DateOnly fechaLanzamiento,
            int duracionMinutos, string tituloEtiqueta, string sinopsis,
            string imgFondo, string imgPoster)
        {
            Pelicula pelicula = await this.FindPeliculaAsync(idPelicula);
            pelicula.IdPelicula = idPelicula;
            pelicula.Titulo = titulo;
            pelicula.FechaLanzamiento = fechaLanzamiento;
            pelicula.DuracionMinutos = duracionMinutos;
            pelicula.TituloEtiqueta = tituloEtiqueta;
            pelicula.Sinopsis = sinopsis;
            pelicula.ImgFondo = imgFondo;
            pelicula.ImgPoster = imgPoster;
            await this.context.SaveChangesAsync();
        }
        //  BORRA PELICULA
        public async Task DeletePeliculaAsync
            (int idPelicula)
        {
            Pelicula pelicula = await this.FindPeliculaAsync(idPelicula);
            this.context.Peliculas.Remove(pelicula);
            await this.context.SaveChangesAsync();
        }
        #endregion

        #region HorarioPeliculas
        public async Task<List<HorarioPelicula>> GetHorarioPeliculasAsync()
        {
            var consulta = from datos in this.context.HorarioPeliculas
                           orderby datos.HoraFuncion descending
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<HorarioPelicula> FindHorarioPeliculaAsync
            (int idHorarioPelicula)
        {
            var consulta = from datos in this.context.HorarioPeliculas
                           where datos.IdHorario == idHorarioPelicula
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task InserHorarioPeliculaAsync
            (int idHorarioPelicula, int idPelicula, int idSala,
            int idVersion, DateTime horaFuncion, int asientosDisponibles,
            char estado)
        {
            HorarioPelicula horarioPelicula = new HorarioPelicula();
            horarioPelicula.IdHorario = idHorarioPelicula;
            horarioPelicula.IdPelicula = idPelicula;
            horarioPelicula.IdSala = idSala;
            horarioPelicula.IdVersion = idVersion;
            horarioPelicula.HoraFuncion = horaFuncion;
            horarioPelicula.AsientosDisponibles = asientosDisponibles;
            horarioPelicula.Estado = estado;
            await this.context.HorarioPeliculas.AddAsync(horarioPelicula);
            await this.context.SaveChangesAsync();
        }
        public async Task UpdateHorarioPeliculaAsync
            (int idHorarioPelicula, int idPelicula, int idSala,
            int idVersion, DateTime horaFuncion, int asientosDisponibles,
            char estado)
        {
            HorarioPelicula horarioPelicula = await this.FindHorarioPeliculaAsync(idHorarioPelicula);
            horarioPelicula.IdHorario = idHorarioPelicula;
            horarioPelicula.IdPelicula = idPelicula;
            horarioPelicula.IdSala = idSala;
            horarioPelicula.IdVersion = idVersion;
            horarioPelicula.HoraFuncion = horaFuncion;
            horarioPelicula.AsientosDisponibles = asientosDisponibles;
            horarioPelicula.Estado = estado;
            await this.context.SaveChangesAsync();
        }
        public async Task DeleteHorarioPeliculaAsync
            (int idHorarioPelicula)
        {
            HorarioPelicula horarioPelicula = await this.FindHorarioPeliculaAsync(idHorarioPelicula);
            this.context.Remove(horarioPelicula);
            await this.context.SaveChangesAsync();
        }
        //  GET UTLIMO ID HORARIO PELICULA
        public async Task<int> GetUltimoIdHorarioPeliculaAsync()
        {
            var consulta = this.context.HorarioPeliculas.Any() ?
                this.context.HorarioPeliculas.Max(x => x.IdHorario) + 1 :
                1;
            int ultimoId = int.Parse(consulta.ToString());
            return ultimoId;
        }
        
        //  CALENDARIO HORARIO SIN TERMINAR *******
        public async Task<List<Evento>> GetCalendarioHorarioPeliculasAsync()
        {
            List<HorarioPelicula> horarioPeliculas = await this.GetHorarioPeliculasAsync();
            var eventos = new List<Evento>();
            foreach (HorarioPelicula horarioPelicula in horarioPeliculas)
            {
                Pelicula pelicula = await this.FindPeliculaAsync(horarioPelicula.IdPelicula);
                DateTime fechaFin = horarioPelicula.HoraFuncion.AddMinutes(pelicula.DuracionMinutos);
                eventos.Add(new Evento
                {
                    Id = horarioPelicula.IdHorario,
                    Titulo = pelicula.Titulo,
                    FechaInicio = horarioPelicula.HoraFuncion,
                    FechaFin = fechaFin,
                    Descripcion = pelicula.Sinopsis
                });
            }
            return eventos;
        }
        #endregion

        #region Usuarios
        public async Task<List<ViewFacturaBoleto>> GetFacturasBoletoUserAsync
            (int idUsuario)
        {
            return await this.context.ViewFacturaBoletos
                .Where(x => x.IdUsuario == idUsuario)
                .ToListAsync();
        }

        public async Task<ViewFacturaBoleto> GetFacturaBoletoUserAsync
            (int idBoletoUser)
        {
            return await this.context.ViewFacturaBoletos
                .Where(x => x.Id == idBoletoUser)
                .FirstOrDefaultAsync();
        }
        public async Task<int> GetLastIdUserAsync()
        {
            {
                var consulta = this.context.Usuarios.Any() ?
                    this.context.Usuarios.Max(x => x.IdUsuario) + 1 :
                    1;
                int ultimoId = int.Parse(consulta.ToString());
                return ultimoId;
            }
        }
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var consulta = from datos in this.context.Usuarios
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<Usuario> FindUsuarioAsync
            (int idUsuario)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.IdUsuario == idUsuario
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        //public async Task DeleteUsuario
        //    (int idUsuario)
        //{
        //    Usuario usuario = await this.FindUsuarioAsync(idUsuario);
        //    this.context.Remove(usuario);
        //    await this.context.SaveChangesAsync();
        //}
        // Actualizar perfil sin cambiar contraseña
        public async Task UpdateUsuarioProfileAsync
            (int idUsuario, string nombre, string email,
            string imagen)
        {
            Usuario usuario = await this.FindUsuarioAsync(idUsuario);
            if (usuario != null)
            {
                usuario.Nombre = nombre;
                usuario.Email = email;
                usuario.Imagen = imagen;
                await this.context.SaveChangesAsync();
            }
        }

        // Actualizar perfil con cambio de contraseña
        public async Task UpdateUsuarioAsync
            (int idUsuario, string nombre, string email,
            string imagen, string password)
        {
            Usuario usuario = await this.FindUsuarioAsync(idUsuario);
            if (usuario != null)
            {
                usuario.Nombre = nombre;
                usuario.Email = email;
                usuario.Imagen = imagen;
                // Actualizar la contraseña
                usuario.Pass = HelperCriptography.EncryptPassword(password, usuario.Salt);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task RegisterUserAsync
            (string nombre, string email, string password,
            string imagen)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = await this.GetLastIdUserAsync();
            usuario.Nombre = nombre;
            usuario.Email = email;
            usuario.Imagen = imagen;
            usuario.Salt = HelperCriptography.GenerateSalt();
            usuario.Pass = HelperCriptography.EncryptPassword(password, usuario.Salt);
            usuario.FechaCreacion = DateTime.Now;
            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }
        public async Task<Usuario> LoginUserAsync
            (string email, string password)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;
            Usuario usuario = await consulta.FirstOrDefaultAsync();
            if (usuario == null)
            {
                return null;
            }
            else
            {
                string salt = usuario.Salt;
                byte[] temp = HelperCriptography.EncryptPassword(password, salt);
                byte[] passBytes = usuario.Pass;
                bool response = HelperCriptography.CompararArrays(temp, passBytes);
                if (response == true)
                {

                    return usuario;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
