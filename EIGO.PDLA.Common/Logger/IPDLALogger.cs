namespace EIGO.PDLA.Common.Logger;

public interface IPdlaLogger
{
    Task LogAsync(int? IdProceso, string Evento, string Descripcion, string Resultado, TipoAuditoria TipoDeEvento);
}

public enum TipoAuditoria { Negocio, Sistema }