//namespace NLayerArchTemplate.CrudTemplate;

//public class EntityTemplate : BaseTemplate
//{
//    public EntityTemplate(DirectoryInfo directory, string projectName, string tableName) :
//        base(directory, projectName, tableName)
//    {
//    }

//    public async Task CreateAsync(CancellationToken ct)
//    {
//        await Task.Run(() =>
//        {
//            Message.Info("Entity oluşturuluyor..!!");
//            string sourceFileContent = File.ReadAllText(_fileFullPath);
//            sourceFileContent = sourceFileContent.Replace("ProjectName", _projectName);
//            sourceFileContent = sourceFileContent.Replace("TableName", _tableName);
//            File.WriteAllText(_fileFullPath, sourceFileContent);
//            Message.Success("Entity oluşturuldu..!!");
//        }, ct);
//    }
//}
