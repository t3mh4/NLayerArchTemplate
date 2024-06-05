namespace NLayerArchTemplate.Core.Exceptions;

public class DataNotFoundException : Exception
{
    public DataNotFoundException()
  : base("İlgili kayıt bulunamadı..!!")
    {
    }

    public DataNotFoundException(string message)
        : base(message)
    {
    }
}
