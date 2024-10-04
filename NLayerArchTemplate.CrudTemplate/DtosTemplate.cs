//namespace NLayerArchTemplate.CrudTemplate;

//public class DtosTemplate : BaseTemplate
//{
//    public DtosTemplate(DirectoryInfo directory, string projectName, string tableName) :
//        base(directory, projectName, tableName)
//    {
//    }

//    public async Task CreateAsync(CancellationToken ct)
//    {
//        await Task.Run(() =>
//        {
//            Message.Info("Dtos oluşturuluyor..!!");
//            var fullPath = Path.Combine(_directory.FullName + _tableName);
//            var sourceFiles = Directory.GetFiles(fullPath);
//            foreach (var sourceFile in sourceFiles)
//            {
//                string sourceFileContent = File.ReadAllText(sourceFile);
//                sourceFileContent = sourceFileContent.Replace("ProjectName", _projectName);
//                sourceFileContent = sourceFileContent.Replace("TableName", _tableName);
//                File.WriteAllText(sourceFile, sourceFileContent);
//                Message.Success($"{_tableNamePrefix + _tableName}.cs oluşturuldu.");
//            }
//            Message.Success("Entity oluşturuldu..!!");
//        }, ct);
//    }
//}
