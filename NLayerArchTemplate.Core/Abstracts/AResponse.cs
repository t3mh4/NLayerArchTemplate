namespace NLayerArchTemplate.Core.Abstracts;

public abstract class AResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string ReturnUrl { get; set; }
}
