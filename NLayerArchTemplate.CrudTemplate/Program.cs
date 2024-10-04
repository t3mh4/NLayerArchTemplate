using NLayerArchTemplate.CrudTemplate;

Message.Info("Crud işlemleri için hazırlanacak template ayarlarına başlıyoruz..");
Console.WriteLine();
Message.Info("Lütfen proje adını giriniz: ", false);
var projectName = Console.ReadLine();

Message.Info("Lütfen tablo adı giriniz: ", false);
var tableName = Console.ReadLine();

Message.Info("Lütfen tablo property'lerini giriniz: ", false);
var properties = Console.ReadLine();

if (string.IsNullOrWhiteSpace(projectName) || string.IsNullOrWhiteSpace(tableName))
{
    Message.Error("Proje adi ve tablo adı boş bırakılamaz. İşlem sonlandırıldı...");
    return;
}
//exe'nin dosya yolu
var rootDirectoryFullPath = Directory.GetCurrentDirectory();
//template'in dosya yolu
var sourceRootDirectoryFullPath = @"D:\CrudTemplate";
var sourceRootDirectory = new DirectoryInfo(sourceRootDirectoryFullPath);
//kopyalanacak klasör oluşturuluyor
var targetRootDirectory = new DirectoryInfo(Path.Combine(@"D:\Test", projectName));
var targetRootDirectoryFullPath = targetRootDirectory.FullName;
//template klasörü kopyalanıyor
CopyAllFiles(sourceRootDirectory, targetRootDirectory, projectName, tableName);
List<string> allFiles = new List<string>();
GetAllFiles(targetRootDirectoryFullPath, allFiles);
if(!allFiles.Any())
{
    Message.Error($"{targetRootDirectory} içinde dosya bulunamadığı için işlem sonlandırıldı.");
    return;
}

//yeni template klasöründe ki alt klasörler alınıyor
var sourceRootSubDirectoryNames = targetRootDirectory.GetDirectories();
try
{
    using var cts = new CancellationTokenSource();
    // Kullanıcıdan iptal sinyali beklemek için
    Console.CancelKeyPress += (sender, eventArgs) =>
    {
        eventArgs.Cancel = true;
        cts.Cancel();
    };
    Message.Info("İşlem başlatılıyor.");
    var template = new BaseTemplate(allFiles, projectName, tableName);
    await template.Create(cts.Token);
}
catch (Exception ex)
{
    Message.Error(ex.ToString());
    Message.Error("İşlem iptal edildi..");
    Console.ReadLine();
    return;
}
Message.Info("İşlem başarıyla tamamlandı..");
Console.ReadLine();


void CopyAllFiles(DirectoryInfo source, DirectoryInfo target,string projectName,string tableName)
{
    Directory.CreateDirectory(target.FullName);
    foreach (FileInfo fi in source.GetFiles())
    {
        var fileName = fi.Name;
        if (fileName.Contains("LowerCaseTableName"))
            fileName = fileName.Replace("LowerCaseTableName", tableName.ToLower());
        else if (fileName.Contains("TableName"))
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