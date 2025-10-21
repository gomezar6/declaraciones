using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;

public static class SincronizacionesExtensions
{
    public static SincronizacionDTO ToSincronizacionDTO(this Sincronizacion sincronizacion)
    {
        return new SincronizacionDTO
        {
            CantidadRegistros = sincronizacion.CantidadRegistros,
            EstatusSincronizacion = sincronizacion.EstatusSincronizacion,
            Fecha = sincronizacion.Fecha,
            IdSincronizacion = sincronizacion.IdSincronizacion,
            RegistrosConErrores = sincronizacion.RegistrosConErrores,
            RegistrosInsertados = sincronizacion.RegistrosInsertados,
            RegistrosModificados = sincronizacion.RegistrosModificados,
            TipoProceso = sincronizacion.TipoProceso,
            SincronizacionDetalles = sincronizacion.SincronizacionDetalles.ToSincronizacionDetalleDTOCollection()
        };
    }

    public static ICollection<SincronizacionDTO> ToSincronizacionDTOCollection(this ICollection<Sincronizacion> sincronizaciones)
    {
        return sincronizaciones.Select(sincronizacion => sincronizacion.ToSincronizacionDTO()).ToList();
    }

    public static SincronizacionDetalleDTO ToSincronizacionDetalleDTO(this SincronizacionDetalle sincronizacionDetalle)
    {
        return new SincronizacionDetalleDTO
        {
            IdSincronizacion = sincronizacionDetalle.IdSincronizacion,
            IdSincronizacionDetalle = sincronizacionDetalle.IdSincronizacionDetalle,
            Mensajes = sincronizacionDetalle.Mensajes
        };
    }

    public static ICollection<SincronizacionDetalleDTO> ToSincronizacionDetalleDTOCollection(this ICollection<SincronizacionDetalle> sincronizacionDetalles)
    {
        return sincronizacionDetalles.Select(sincronizacionDetalles => sincronizacionDetalles.ToSincronizacionDetalleDTO()).ToList();
    }
}

