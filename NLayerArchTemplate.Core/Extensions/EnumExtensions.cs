using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NLayerArchTemplate.Core.Extensions;

public static class EnumExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
       where TAttribute : Attribute
    {
        var a = enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .FirstOrDefault();
        if (a == null) return null;
        return a.GetCustomAttribute<TAttribute>();
    }

    public static string GetDisplayName(this Enum constant)
    {
        var dispName = constant.ToString();
        var dn = constant.GetAttribute<DisplayAttribute>();
        if (dn != null)
            dispName = dn.Name;

        return dispName;
    }

    public static string GetShortName(this Enum constant)
    {
        var shortName = constant.ToString();
        var dn = constant.GetAttribute<DisplayAttribute>();
        if (dn != null)
            shortName = dn.ShortName;

        return shortName;
    }

    public static int ToInt32(this Enum constant)
    {
        return Convert.ToInt32(constant);
    }
}
