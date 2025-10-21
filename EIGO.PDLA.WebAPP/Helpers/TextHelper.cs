namespace EIGO.PDLA.WebAPP.Helpers;

public static class TextHelper
{
    public static string ReplaceText(string value)
    {
        return value
            .Replace("details", "Detalles", StringComparison.InvariantCultureIgnoreCase)
            .Replace("edit", "Editar", StringComparison.InvariantCultureIgnoreCase)
            .Replace("create", "Crear", StringComparison.InvariantCultureIgnoreCase)
            .Replace("delete", "Eliminar", StringComparison.InvariantCultureIgnoreCase);
    }
}
