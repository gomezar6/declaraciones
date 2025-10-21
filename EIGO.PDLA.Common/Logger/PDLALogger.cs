using EIGO.PDLA.Common.Models;
using Microsoft.AspNetCore.Http;

namespace EIGO.PDLA.Common.Logger;

public class PdlaLogger : IPdlaLogger
{
    private readonly DeclaracionesContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PdlaLogger(DeclaracionesContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public IEnumerable<string> SplitString( string str, int n)
    {
        if (String.IsNullOrEmpty(str) || n < 1)
        {
            throw new ArgumentException();
        }

        return Enumerable.Range(0, str.Length / n).Select(i => str.Substring(i * n, n));
    }

    public async Task LogAsync(int? IdProceso, string Evento, string Descripcion, string Resultado, TipoAuditoria TipoDeEvento)
    {
        string IdUsuario = string.Empty;
        string IP = string.Empty;
        string Usuario = string.Empty;
        if (_httpContextAccessor.HttpContext != null)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            Usuario = httpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "No determinado";
            IdUsuario = httpContext.User?.Identity?.Name ?? "No determinado";
            IP = httpContext.Connection?.RemoteIpAddress?.ToString() ?? "No determinado";
        }
        int size = 5950;

        IEnumerable<string> subDescripcion;
        if (Descripcion.Length > size)
        {
        
            subDescripcion = SplitString(Descripcion,size);
            for (int i = 0; i < subDescripcion.Count(); i ++)
                {
                var result = await _context.Auditoria.AddAsync(new Auditoria
                {
                    Descripcion = subDescripcion.ToList()[i],
                    Evento = Evento,
                    Fecha = DateTime.UtcNow,
                    IdUsuario = IdUsuario,
                    Ip = IP,
                    IdProceso = IdProceso,
                    Resultado = Resultado,
                    TipoEvento = (byte?)TipoDeEvento,
                    Usuario = Usuario
                });
                await _context.SaveChangesAsync();

            }

        }
        else
        {
            await _context.Auditoria.AddAsync(new Auditoria
            {
                Descripcion = Descripcion,
                Evento = Evento,
                Fecha = DateTime.UtcNow,
                IdUsuario = IdUsuario,
                Ip = IP,
                IdProceso = IdProceso,
                Resultado = Resultado,
                TipoEvento = (byte?)TipoDeEvento,
                Usuario = Usuario
            }); 
            await _context.SaveChangesAsync();


        }

    


    }
}


