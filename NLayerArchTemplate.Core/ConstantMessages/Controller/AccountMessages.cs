namespace NLayerArchTemplate.Core.ConstantMessages;

public class AccountMessages
{
    public const string CheckAuthorizationFail = "Kullanıcı adı veya şifre hatalı";
    public const string Logout = "\"Giriş Sayfasına Yönlendiriliyorsunuz..!!\"";
    public static string LoginSuccess(string userFullName)
    {
        return $"Hoş geldiniz {userFullName}.<br/>Anasayfaya yönlendiriliyorsunuz.";
    }
}
