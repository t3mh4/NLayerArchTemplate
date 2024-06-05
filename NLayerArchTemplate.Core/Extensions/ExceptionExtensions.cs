namespace NLayerArchTemplate.Core.Extensions;

public static class ExceptionExtensions
{
    public static string GetMessage(this Exception ex)
    {
        if (ex.InnerException == null) return ex.Message;
        return ex.InnerException.GetMessage();
    }

    public static string GetStackTrace(this Exception ex)
    {
        if (ex.InnerException == null) return ex.StackTrace;
        return ex.InnerException.GetStackTrace();
    }
}
