namespace WebModuleTeko.Models.Authentication;

public class TotpModel
{
    public string Image { get; set; } = null!;
    public string SetupCode { get; set; } = null!;
    public string Uri { get; set; } = null!;

}
