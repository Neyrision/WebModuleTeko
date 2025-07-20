using Microsoft.AspNetCore.DataProtection;
using OtpNet;
using QRCoder;

namespace WebModuleTeko.Helpers;

public static class TotpHelper
{

    public static string NewSecret()
    {
        var secret = System.Security.Cryptography.RandomNumberGenerator.GetBytes(20);
        return Base32Encoding.ToString(secret);
    }

    public static string NewTotpUri(string issuer, string secret, string email)
    {
        issuer = Uri.EscapeDataString(issuer);
        return $"otpauth://totp/{issuer}:{Uri.EscapeDataString(email)}" +
            $"?secret={secret}&issuer={issuer}";
    }

    public static string Base64QrCodeFromTotpUri(string totpUri)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            var qrCodeImage = qrCode.GetGraphic(20);

            return Convert.ToBase64String(qrCodeImage);
        }
    }

}