using NLayerArchTemplate.Core.Enums;

namespace NLayerArchTemplate.Core.Models;

public class ErrorModel
{
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public ErrorType ErrorType { get; set; } = ErrorType.Default;
}
