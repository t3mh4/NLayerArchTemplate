namespace NLayerArchTemplate.Core.Exceptions;

public class CustomException : Exception
{
    public CustomException()
  : base()
    {
    }

    public CustomException(string message)
        : base(message)
    {
    }
}
