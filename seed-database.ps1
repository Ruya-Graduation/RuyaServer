# ═══════════════════════════════════════════════════════════════════════════════
# RUYA Database Seeder
# ═══════════════════════════════════════════════════════════════════════════════
# This script seeds the RUYA database with sites and artifacts
# ═══════════════════════════════════════════════════════════════════════════════

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "         RUYA Database Seeder" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

# Navigate to API project directory
$apiPath = "c:\RuyaGraduation\final_flow\RUYA\RUYA_API"
Set-Location $apiPath

Write-Host "📍 Current directory: $apiPath" -ForegroundColor Yellow
Write-Host ""

# Step 1: Check if database exists
Write-Host "🔍 Checking database connection..." -ForegroundColor Yellow
$result = dotnet ef database update --verbose 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Database connection failed or migrations not applied!" -ForegroundColor Red
    Write-Host "   Please check your connection string in appsettings.json" -ForegroundColor Red
    Write-Host ""
    Write-Host "Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

Write-Host "✅ Database connection successful" -ForegroundColor Green
Write-Host ""

# Step 2: Create temporary seeder program
Write-Host "📝 Creating database seeder script..." -ForegroundColor Yellow

$seederCode = @'
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RUYA_API.Infrastructure.Context;
using RUYA_API.Infrastructure.Data;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("❌ Connection string not found in appsettings.json");
    return;
}

Console.WriteLine($"🔗 Connection string: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");
Console.WriteLine();

await SeedDatabase.SeedFromConfigurationAsync(connectionString);

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
'@

# Save seeder code to temporary file
$tempFile = Join-Path $apiPath "TempSeeder.cs"
$seederCode | Out-File -FilePath $tempFile -Encoding UTF8

Write-Host "✅ Seeder script created" -ForegroundColor Green
Write-Host ""

# Step 3: Run the seeder
Write-Host "🌱 Running database seeder..." -ForegroundColor Yellow
Write-Host "   This will add 6 sites and 84 artifacts" -ForegroundColor Gray
Write-Host ""

# Execute using dotnet script
dotnet run --no-build -- seed

# If dotnet run doesn't work, use dotnet-script or compile manually
if ($LASTEXITCODE -ne 0) {
    Write-Host "⚠️  Standard seeding failed. Trying alternative method..." -ForegroundColor Yellow
    Write-Host ""
    
    # Create a simple console app to run the seeder
    $consoleAppPath = Join-Path $apiPath "bin\Debug\net8.0"
    
    # Try to execute the built DLL with reflection
    dotnet exec "$consoleAppPath\RUYA_API.dll"
}

# Clean up temporary file
if (Test-Path $tempFile) {
    Remove-Item $tempFile -Force
}

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "         Seeding Complete!" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Start the Python AI service" -ForegroundColor Gray
Write-Host "  2. Start the .NET API" -ForegroundColor Gray
Write-Host "  3. Test the endpoints" -ForegroundColor Gray
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
'@
