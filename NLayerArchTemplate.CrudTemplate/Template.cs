namespace NLayerArchTemplate.CrudTemplate;

public class Template
{
    protected readonly DirectoryInfo _directory;
    protected readonly string _tableName;
    protected readonly string _tableNamePrefix = "Tbl";
    protected readonly string _projectName;
    protected readonly string _fileFullPath;
    private readonly List<string> _allFiles;

    public Template(List<string> allFiles, string projectName, string tableName)
    {
        _allFiles = allFiles;
    }

    public void CopyAllFiles(DirectoryInfo source, DirectoryInfo target, string projectName, string tableName)
    {
        Directory.CreateDirectory(target.FullName);
        foreach (FileInfo fi in source.GetFiles())
        {
            var fileName = fi.Name;
            if (fileName.Contains("TableName"))
                fileName = fileName.Replace("TableName", tableName);
            Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
            fi.CopyTo(Path.Combine(target.FullName, fileName), true);
        }
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            var newDicName = diSourceSubDir.Name;
            if (newDicName.StartsWith("ProjectName"))
                newDicName = diSourceSubDir.Name.Replace("ProjectName", projectName);
            else if (newDicName.StartsWith("TableName"))
                newDicName = diSourceSubDir.Name.Replace("TableName", tableName);
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(newDicName);
            CopyAllFiles(diSourceSubDir, nextTargetSubDir, projectName, tableName);
        }
    }

    void GetAllFiles(string directoryPath, List<string> fileList)
    {
        string[] files = Directory.GetFiles(directoryPath);
        fileList.AddRange(files);

        // Alt klasörleri al
        string[] directories = Directory.GetDirectories(directoryPath);
        foreach (string directory in directories)
        {
            // Alt klasördeki dosyaları listele (rekürsif çağrı)
            GetAllFiles(directory, fileList);
        }
    }

    public async Task Create(CancellationToken ct)
    {
        await Task.Run(() =>
        {
            foreach (var file in _allFiles)
            {
                if(file.Contains("Index.cshtml"))
                {

                }
                if(file.Contains("UserController"))
                {

                }
                string sourceFileContent = File.ReadAllText(file);
                sourceFileContent = sourceFileContent.Replace("ProjectName", _projectName);
                sourceFileContent = sourceFileContent.Replace("TableName", _tableName);
                sourceFileContent = sourceFileContent.Replace("LowerCaseTableName", _tableName.ToLower());
                sourceFileContent = sourceFileContent.Replace("PrivateTableName", "_" + _tableName.ToLower());
                File.WriteAllText(file, sourceFileContent);
                Message.Info($"{file} oluşturuldu.");
            }
        }, ct);
    }
}