
namespace NLayerArchTemplate.Core.ConstantKeys;

public sealed class KeyValues
{
    private KeyValues() { }

    public const int CookieTimeOut = 30;
    public const string JsonContentType = "application/json";
    public const string ClaimTypeUserFullName = "UserFullName";
    public const string ClaimTypeId = "UserId";
    public const string ClaimTypeEmail = "ClaimTypeEmail";
    public const string XRequestedWith="X-Requested-With";
}
