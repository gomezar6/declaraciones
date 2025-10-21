namespace EIGO.PDLA.WebAPP.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? ErrorMsg { get; set; }

        public bool ShowErrorMsg => !string.IsNullOrEmpty(ErrorMsg);
    }
}