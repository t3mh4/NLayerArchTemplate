using NLayerArchTemplate.Core.Enums;

namespace NLayerArchTemplate.Core.Models;

public class ErrorModel
{
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public ErrorType ErrorType { get; set; } = ErrorType.Default;
	
    public static ErrorModel Create(string message, string stackTrace, ErrorType type)
	{
		return new ErrorModel
		{
			Message = message,
			StackTrace = stackTrace,
			ErrorType = type
		};
	}
}
