using System.Text;

namespace NLayerArchTemplate.CrudTemplate;

public class BaseTemplate
{
    protected readonly string _tableName;
    protected readonly string _projectName;
    private readonly List<string> _allFiles;
    private string _entityPath;
    private string _corePath;

    public BaseTemplate(List<string> allFiles, string projectName, string tableName)
    {
        _allFiles = allFiles;
        _tableName = tableName;
        _projectName = projectName;
    }

    public async Task Create(CancellationToken ct)
    {
        await Task.Run(() =>
        {
            foreach (var file in _allFiles)
            {
                string sourceFileContent = File.ReadAllText(file);
                if (sourceFileContent.Contains($"{_projectName}.Entities"))
                {
                    _entityPath = file;
                }
                else if (file.EndsWith("CorE.cshhmtl"))
                {
                    _corePath = file;
                }
                sourceFileContent = sourceFileContent.Replace("ProjectName", _projectName); 
                sourceFileContent = sourceFileContent.Replace("LowerCaseTableName", char.ToLower(_tableName[0]) + _tableName.Substring(1));
                sourceFileContent = sourceFileContent.Replace("TableName", _tableName);
                File.WriteAllText(file, sourceFileContent, new UTF8Encoding(false));
                Message.Success($"{file} oluşturuldu.");
            }
        }, ct);
    }

    public void AddFormInputs(string sourceFileContent)
    {
        var input = "<div class=\"form-group row\">\r\n        <label asp-for=\"Name\" class=\"col-sm-3 col-form-label\">Ad (*)</label>\r\n        <div class=\"col-sm-9\">\r\n            <input asp-for=\"Name\" class=\"form-control\"/>\r\n        </div>\r\n    </div>";
    }

    public List<string> GetEntities(string file)
    {
        return null;
    }
}