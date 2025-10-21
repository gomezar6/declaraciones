namespace EIGO.PDLA.WebAPP.Helpers;

public class Breadcrumb
{
    public string? Icon { get; set; } = null;
    public string Text { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string Controller { get; set; } = null!;
    public bool IsActive { get; set; }
    public string Value { get; set; } = null!;
}
