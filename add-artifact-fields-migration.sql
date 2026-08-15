-- ═══════════════════════════════════════════════════════════════════════════════
-- Migration: Add Material and PlaceOfDiscovery to Artifacts Table
-- ═══════════════════════════════════════════════════════════════════════════════
-- This migration adds the required fields for the Python conversation endpoint
-- ═══════════════════════════════════════════════════════════════════════════════

USE RUYA;
GO

PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Adding Material and PlaceOfDiscovery to Artifacts';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Check if columns already exist
IF NOT EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID('Artifacts') 
               AND name = 'Material')
BEGIN
    PRINT '📝 Adding Material column...';
    ALTER TABLE Artifacts
    ADD Material NVARCHAR(255) NOT NULL DEFAULT '';
    PRINT '✅ Material column added.';
END
ELSE
BEGIN
    PRINT '⏭️  Material column already exists.';
END

PRINT '';

IF NOT EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID('Artifacts') 
               AND name = 'PlaceOfDiscovery')
BEGIN
    PRINT '📝 Adding PlaceOfDiscovery column...';
    ALTER TABLE Artifacts
    ADD PlaceOfDiscovery NVARCHAR(500) NOT NULL DEFAULT '';
    PRINT '✅ PlaceOfDiscovery column added.';
END
ELSE
BEGIN
    PRINT '⏭️  PlaceOfDiscovery column already exists.';
END

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Migration Complete';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Show updated schema
PRINT '📊 Current Artifacts table columns:';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Artifacts'
ORDER BY ORDINAL_POSITION;

GO

-- ═══════════════════════════════════════════════════════════════════════════════
-- Update Existing Artifacts with Sample Data (Optional)
-- ═══════════════════════════════════════════════════════════════════════════════

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Updating Artifacts with Sample Material/Discovery Data';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Update common materials based on artifact type
UPDATE Artifacts
SET Material = CASE
    WHEN Name LIKE '%Granite%' THEN 'Granite'
    WHEN Name LIKE '%Mask%' THEN 'Gold, Lapis Lazuli, Carnelian'
    WHEN Name LIKE '%Pyramid%' THEN 'Limestone, Granite'
    WHEN Name LIKE '%Sphinx%' THEN 'Limestone'
    WHEN Name LIKE '%Statue%' AND Name NOT LIKE '%Granite%' THEN 'Limestone, Sandstone'
    WHEN Name LIKE '%Bust%' AND Name LIKE '%Granite%' THEN 'Black Granite'
    WHEN Name LIKE '%Bust%' THEN 'Limestone'
    WHEN Name LIKE '%Column%' THEN 'Granite'
    WHEN Name LIKE '%Capital%' THEN 'Sandstone'
    WHEN Name LIKE '%Stela%' THEN 'Limestone, Granite'
    WHEN Name LIKE '%Obelisk%' THEN 'Red Granite'
    WHEN Name LIKE '%Coffin%' THEN 'Wood, Gold leaf'
    WHEN Name LIKE '%Naos%' THEN 'Granite'
    WHEN Category = 'Statue' THEN 'Stone'
    ELSE 'Stone, Mixed Materials'
END
WHERE Material = '' OR Material IS NULL;

PRINT '✅ Material data updated for ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' artifacts.';
PRINT '';

-- Update place of discovery based on artifact name and location
UPDATE Artifacts
SET PlaceOfDiscovery = CASE
    -- Valley of the Kings artifacts
    WHEN Name LIKE '%Tutankhamun%' THEN 'Valley of the Kings, Luxor'
    WHEN Name LIKE '%Yuya%' OR Name LIKE '%Thuya%' THEN 'Valley of the Kings, Luxor'
    
    -- Giza artifacts
    WHEN Name LIKE '%Pyramid%' AND Name LIKE '%Giza%' THEN 'Giza Plateau'
    WHEN Name LIKE '%Khafre%' OR Name LIKE '%Khufu%' THEN 'Giza Plateau'
    WHEN Name = 'Sphinx' THEN 'Giza Plateau'
    WHEN Name LIKE '%Menkaure%' THEN 'Giza Plateau'
    
    -- Saqqara artifacts
    WHEN Name LIKE '%Djoser%' THEN 'Saqqara'
    WHEN Name LIKE '%Pyramid of Djoser%' THEN 'Saqqara'
    WHEN Name LIKE '%Userkaf%' THEN 'Saqqara'
    WHEN Name LIKE '%Zoser%' THEN 'Saqqara'
    
    -- Dahshur artifacts
    WHEN Name LIKE '%Sneferu%' OR Name LIKE '%Snefru%' OR Name LIKE '%Snefero%' THEN 'Dahshur'
    WHEN Name LIKE '%Bent Pyramid%' THEN 'Dahshur'
    
    -- Luxor/Thebes artifacts
    WHEN Name LIKE '%Amenhotep%' THEN 'Thebes (Luxor)'
    WHEN Name LIKE '%Ramses%' OR Name LIKE '%Ramesses%' THEN 'Thebes, Tanis, Abu Simbel'
    WHEN Name LIKE '%Hatshepsut%' THEN 'Deir el-Bahari, Thebes'
    WHEN Name LIKE '%Thutmose%' THEN 'Karnak, Thebes'
    WHEN Name LIKE '%Colossoi of Memnon%' THEN 'West Bank, Thebes'
    
    -- Amarna artifacts
    WHEN Name LIKE '%Akhenaten%' THEN 'Amarna (Akhetaten)'
    WHEN Name LIKE '%Nefertiti%' THEN 'Amarna (Akhetaten)'
    
    -- Other Old Kingdom
    WHEN Name LIKE '%Senwosret%' OR Name LIKE '%Senuseret%' THEN 'Karnak, Lisht'
    WHEN Name LIKE '%Amenemhat%' OR Name LIKE '%Amenmhat%' THEN 'Dahshur, Lisht, Hawara'
    WHEN Name LIKE '%Mentuhotep%' THEN 'Deir el-Bahari, Thebes'
    
    -- Ptolemaic/Roman
    WHEN Name LIKE '%Augustus%' THEN 'Alexandria'
    WHEN Name LIKE '%Carcala%' THEN 'Alexandria'
    WHEN Name LIKE '%Serapis%' THEN 'Alexandria'
    
    -- Gods/Goddesses
    WHEN Name LIKE '%Isis%' THEN 'Various temples'
    WHEN Name LIKE '%Osiris%' THEN 'Abydos, Various temples'
    WHEN Name LIKE '%Ptah%' THEN 'Memphis, Karnak'
    WHEN Name LIKE '%Sekhmet%' THEN 'Karnak, Memphis'
    WHEN Name LIKE '%Ra-Horakhty%' OR Name LIKE '%Re-Horakhty%' THEN 'Heliopolis, Various temples'
    WHEN Name LIKE '%Amun%' THEN 'Karnak, Thebes'
    
    ELSE 'Egypt'
END
WHERE PlaceOfDiscovery = '' OR PlaceOfDiscovery IS NULL;

PRINT '✅ PlaceOfDiscovery data updated for ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' artifacts.';
PRINT '';

PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Data Update Complete';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Show sample of updated data
PRINT '📋 Sample artifacts with new fields:';
SELECT TOP 10
    Name,
    Period,
    Material,
    PlaceOfDiscovery
FROM Artifacts
WHERE Name LIKE '%Tutankhamun%' 
   OR Name LIKE '%Sphinx%'
   OR Name LIKE '%Nefertiti%'
   OR Name LIKE '%Pyramid%'
ORDER BY Name;

PRINT '';
PRINT '✅ Migration and data update completed successfully!';
PRINT '';

GO
