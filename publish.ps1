# Script Çalıştırma
# 1 - PowerShell scripti yönetici olarak çalıştır
# 2 - Uygulama Dizinine git ( cd D:\repos\NLayerArchTemplate )
# 3 - Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass çalıştır
# 4 - .\publish.ps1 çalıştır


function Write-Green {
    param (
        [string]$message
    )
    Write-Host $message -ForegroundColor Green
}

function Stop-AppPool {
    param (
        [string]$appPoolName
    )

    Write-Green "Stopping application pool: $appPoolName..."
    Stop-WebAppPool -Name $appPoolName
}

function Start-AppPool {
    param (
        [string]$appPoolName
    )

    Write-Green "Starting application pool: $appPoolName..."
    Start-WebAppPool -Name $appPoolName
}

function Remove-Files{
	param (
        [string]$projectPath
    )
	
	# wwwroot klasör yolu
	$wwwrootPath = "$projectPath\Publish\wwwroot" # Buraya wwwroot klasör yolunu belirtin
	$excludeFolder = "$wwwrootPath\Files"    # Korunacak klasör

	# Eski dosyaları sil (Files klasörünü hariç tutarak)
	Write-Green "Cleaning up old files in the wwwroot directory, excluding the 'Files' folder..."
	Get-ChildItem -Path $wwwrootPath | ForEach-Object {
		if ($_.FullName -ne $excludeFolder) {
			Remove-Item -Path $_.FullName -Recurse -Force
		}
	}
}

function Publish{
	param (
        [string]$projectPath
    )
	
	Write-Green "Publishing .NET Core project..."
	$outputPath = "$projectPath\publish"  # Publish klasörünü belirtin
	dotnet publish $projectPath -c Release -o $outputPath
}

# Uygulama havuzunun adı
$appPoolName = "NLayerArchTemplate" # Buraya uygulama havuzunun adını girin
# Uygulama dizini
$projectPath = "D:\repos\NLayerArchTemplate" # Projenin yolunu belirtin

# Uygulama havuzunu durdur
Stop-AppPool $appPoolName

# Eski dosyaları sil
Remove-Files $projectPath

# Publish işlemi
publish $projectPath

# Uygulama havuzunu başlat
Start-AppPool $appPoolName

Write-Green "The publish operation is completed and the application pool is started."
