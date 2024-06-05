namespace NLayerArchTemplate.Core.Models;

public class HttpSaveRequestModel<T> : HttpRequestModel<T>
{
    public List<string> ModifiedProperties;
}
