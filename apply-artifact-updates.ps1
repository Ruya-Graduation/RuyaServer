# ═══════════════════════════════════════════════════════════════════════════════
# Apply Artifact Fields Update
# ═══════════════════════════════════════════════════════════════════════════════
# This script applies all necessary changes to add Material and PlaceOfDiscovery
# ═══════════════════════════════════════════════════════════════════════════════

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "    Artifact Fields Update Script" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

$apiPath = "c:\RuyaGraduation\final_flow\RUYA\RUYA_API"
Set-Location $apiPath

Write-Host "📍 Working directory: $apiPath" -ForegroundColor Yellow
Write-Host ""

# ═══════════════════════════════════════════════════════════════════════════════
# Step 1: Create and Apply EF Migration
# ═══════════════════════════════════════════════════════════════════════════════

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "Step 1: Creating Entity Framework Migration" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

Write-Host "🔨 Creating migration..." -ForegroundColor Yellow
$migrationOutput = dotnet ef migrations add AddMaterialAndPlaceOfDiscovery 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migration created successfully" -ForegroundColor Green
} elseif ($migrationOutput -like "*already exists*") {
    Write-Host "⏭️  Migration already exists, skipping..." -ForegroundColor Yellow
} else {
    Write-Host "⚠️  Migration creation failed or already exists" -ForegroundColor Yellow
    Write-Host "   Continuing with database update..." -ForegroundColor Gray
}

Write-Host ""
Write-Host "📊 Applying migration to database..." -ForegroundColor Yellow

$updateOutput = dotnet ef database update 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Database updated successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Database update failed!" -ForegroundColor Red
    Write-Host "   Error: $updateOutput" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please run the SQL scripts manually:" -ForegroundColor Yellow
    Write-Host "  1. add-artifact-fields-migration.sql" -ForegroundColor Gray
    Write-Host "  2. update-artifact-data.sql" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

Write-Host ""

# ═══════════════════════════════════════════════════════════════════════════════
# Step 2: Prompt for Data Update
# ═══════════════════════════════════════════════════════════════════════════════

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "Step 2: Update Artifact Data" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

Write-Host "The database schema has been updated." -ForegroundColor Green
Write-Host "Now you need to populate the Material and PlaceOfDiscovery fields." -ForegroundColor Yellow
Write-Host ""
Write-Host "Options:" -ForegroundColor Cyan
Write-Host "  1. Run update-artifact-data.sql in SSMS/Azure Data Studio" -ForegroundColor Gray
Write-Host "  2. Manual SQL update" -ForegroundColor Gray
Write-Host ""

$sqlScriptPath = Join-Path $PSScriptRoot "update-artifact-data.sql"

if (Test-Path $sqlScriptPath) {
    Write-Host "📄 SQL script location: $sqlScriptPath" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Please open this file in SQL Server Management Studio and execute it." -ForegroundColor Cyan
    Write-Host ""
    
    # Ask if user wants to open the script
    $openScript = Read-Host "Do you want to open the SQL script now? (Y/N)"
    if ($openScript -eq 'Y' -or $openScript -eq 'y') {
        try {
            Start-Process $sqlScriptPath
            Write-Host "✅ SQL script opened" -ForegroundColor Green
        } catch {
            Write-Host "⚠️  Could not open script automatically" -ForegroundColor Yellow
            Write-Host "   Please open it manually: $sqlScriptPath" -ForegroundColor Gray
        }
    }
} else {
    Write-Host "⚠️  SQL script not found at: $sqlScriptPath" -ForegroundColor Yellow
    Write-Host "   You can run this SQL manually:" -ForegroundColor Gray
    Write-Host ""
    Write-Host @"
    UPDATE Artifacts
    SET 
        Material = 'Stone (Limestone/Granite)',
        PlaceOfDiscovery = 'Ancient Egypt'
    WHERE Material = '' OR PlaceOfDiscovery = '';
"@ -ForegroundColor DarkGray
}

Write-Host ""
Write-Host "Press any key after you've updated the data..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# ═══════════════════════════════════════════════════════════════════════════════
# Step 3: Rebuild Application
# ═══════════════════════════════════════════════════════════════════════════════

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "Step 3: Rebuilding Application" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

Write-Host "🧹 Cleaning..." -ForegroundColor Yellow
dotnet clean | Out-Null

Write-Host "📦 Restoring packages..." -ForegroundColor Yellow
dotnet restore | Out-Null

Write-Host "🔨 Building..." -ForegroundColor Yellow
$buildOutput = dotnet build 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful" -ForegroundColor Green
} else {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    Write-Host "$buildOutput" -ForegroundColor Red
    Write-Host ""
    Write-Host "Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

# ═══════════════════════════════════════════════════════════════════════════════
# Step 4: Verification
# ═══════════════════════════════════════════════════════════════════════════════

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "Step 4: Verification" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

Write-Host "✅ Migration applied" -ForegroundColor Green
Write-Host "✅ Application built successfully" -ForegroundColor Green
Write-Host ""

Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Ensure artifact data is updated (Material, PlaceOfDiscovery)" -ForegroundColor Gray
Write-Host "  2. Start the .NET API: dotnet run" -ForegroundColor Gray
Write-Host "  3. Ensure Python API is running" -ForegroundColor Gray
Write-Host "  4. Test the chat endpoint with an image" -ForegroundColor Gray
Write-Host ""

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "✅ Update Complete!" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

Write-Host "Would you like to start the API now? (Y/N)" -ForegroundColor Cyan
$startApi = Read-Host

if ($startApi -eq 'Y' -or $startApi -eq 'y') {
    Write-Host ""
    Write-Host "🚀 Starting API..." -ForegroundColor Green
    Write-Host ""
    dotnet run
} else {
    Write-Host ""
    Write-Host "To start the API later, run:" -ForegroundColor Yellow
    Write-Host "  cd $apiPath" -ForegroundColor Gray
    Write-Host "  dotnet run" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Press any key to exit..." -ForegroundColor Gray
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
