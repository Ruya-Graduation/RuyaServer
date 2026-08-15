-- ═══════════════════════════════════════════════════════════════════════════════
-- Update Artifact Material and Place of Discovery
-- ═══════════════════════════════════════════════════════════════════════════════
-- Run this AFTER the migration to populate Material and PlaceOfDiscovery
-- ═══════════════════════════════════════════════════════════════════════════════

USE RUYA;
GO

PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Updating Artifact Material and Place of Discovery';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Update specific well-known artifacts with accurate data
BEGIN TRANSACTION;

-- Tutankhamun artifacts
UPDATE Artifacts SET 
    Material = 'Gold, Lapis Lazuli, Carnelian, Obsidian, Turquoise',
    PlaceOfDiscovery = 'Valley of the Kings (KV62), Luxor'
WHERE Name = 'Mask of Tutankhamun';

UPDATE Artifacts SET 
    Material = 'Granite, Quartzite',
    PlaceOfDiscovery = 'Valley of the Kings (KV62), Luxor'
WHERE Name = 'Granite Statue of Tutankhamun';

UPDATE Artifacts SET 
    Material = 'Wood, Gold leaf, Stone',
    PlaceOfDiscovery = 'Valley of the Kings (KV62), Luxor'
WHERE Name = 'Statue of Tutankhamun';

-- Pyramids
UPDATE Artifacts SET 
    Material = 'Limestone, Granite',
    PlaceOfDiscovery = 'Giza Plateau'
WHERE Name = 'Great Pyramids of Giza';

UPDATE Artifacts SET 
    Material = 'Limestone',
    PlaceOfDiscovery = 'Dahshur'
WHERE Name = 'Bent Pyramid of King Sneferu';

UPDATE Artifacts SET 
    Material = 'Limestone',
    PlaceOfDiscovery = 'Saqqara'
WHERE Name = 'Pyramid of Djoser';

-- Sphinx
UPDATE Artifacts SET 
    Material = 'Limestone',
    PlaceOfDiscovery = 'Giza Plateau'
WHERE Name = 'Sphinx';

-- Nefertiti
UPDATE Artifacts SET 
    Material = 'Limestone, Stucco',
    PlaceOfDiscovery = 'Amarna (Tell el-Amarna)'
WHERE Name = 'Nefertiti';

-- Masks
UPDATE Artifacts SET 
    Material = 'Gilded Cartonnage, Linen',
    PlaceOfDiscovery = 'Valley of the Kings (KV46), Luxor'
WHERE Name = 'Mask of Yuya';

UPDATE Artifacts SET 
    Material = 'Gilded Cartonnage, Linen',
    PlaceOfDiscovery = 'Valley of the Kings (KV46), Luxor'
WHERE Name = 'Mask of Thuya';

-- Granite statues
UPDATE Artifacts SET 
    Material = 'Black Granite',
    PlaceOfDiscovery = 'Karnak Temple, Thebes'
WHERE Name = 'Black Granite Bust of Mentuemhat';

UPDATE Artifacts SET 
    Material = 'Grey Granite',
    PlaceOfDiscovery = 'Tanis'
WHERE Name = 'Grey Granite of Ramesses II';

UPDATE Artifacts SET 
    Material = 'Granite',
    PlaceOfDiscovery = 'Abydos'
WHERE Name = 'Granite Statue of Osiris';

-- Colossal statues
UPDATE Artifacts SET 
    Material = 'Quartzite, Granite',
    PlaceOfDiscovery = 'Thebes (Luxor)'
WHERE Name LIKE '%Colossal Statue of Amenhotep III%';

UPDATE Artifacts SET 
    Material = 'Granite',
    PlaceOfDiscovery = 'Memphis, Tanis'
WHERE Name LIKE '%Colossal Statue of Ramesses II%';

UPDATE Artifacts SET 
    Material = 'Limestone',
    PlaceOfDiscovery = 'Deir el-Bahari, Thebes'
WHERE Name = 'Colossal Statue of Queen Hatshepsut';

UPDATE Artifacts SET 
    Material = 'Quartzite',
    PlaceOfDiscovery = 'West Bank, Thebes'
WHERE Name = 'Colossoi of Memnon';

-- Seated statues
UPDATE Artifacts SET 
    Material = 'Diorite',
    PlaceOfDiscovery = 'Saqqara'
WHERE Name = 'Seated Statue of Djoser';

UPDATE Artifacts SET 
    Material = 'Granite',
    PlaceOfDiscovery = 'Karnak Temple, Thebes'
WHERE Name LIKE '%Seated Statue of%' AND Name LIKE '%Sekhmet%';

-- Standing statues
UPDATE Artifacts SET 
    Material = 'Granite',
    PlaceOfDiscovery = 'Luxor Temple'
WHERE Name = 'Statue of King Ramses II Luxor Temple';

UPDATE Artifacts SET 
    Material = 'Granite',
    PlaceOfDiscovery = 'Grand Egyptian Museum (from Ramesses II Temple, Mit Rahina)'
WHERE Name = 'Statue of King Ramses II Grand Egyptian Museum';

-- Akhenaten
UPDATE Artifacts SET 
    Material = 'Sandstone, Limestone',
    PlaceOfDiscovery = 'Karnak Temple, Amarna'
WHERE Name = 'Akhenaten' OR Name = 'Statue Head of Akhenaten';

-- Old Kingdom rulers
UPDATE Artifacts SET 
    Material = 'Diorite',
    PlaceOfDiscovery = 'Giza (Valley Temple)'
WHERE Name = 'Statue of Khafre';

UPDATE Artifacts SET 
    Material = 'Ivory',
    PlaceOfDiscovery = 'Abydos'
WHERE Name = 'Statue of Khufu';

UPDATE Artifacts SET 
    Material = 'Schist (Greywacke)',
    PlaceOfDiscovery = 'Giza (Valley Temple)'
WHERE Name = 'Menkaure Statue';

-- Middle Kingdom
UPDATE Artifacts SET 
    Material = 'Granite',
    PlaceOfDiscovery = 'Fayum'
WHERE Name = 'Sphinx of Amenmhat III';

UPDATE Artifacts SET 
    Material = 'White Limestone',
    PlaceOfDiscovery = 'Karnak Temple'
WHERE Name = 'Naos of Senwosert I';

-- Others with general data
UPDATE Artifacts SET 
    Material = CASE
        WHEN Material = '' OR Material IS NULL THEN 'Stone (Limestone/Granite)'
        ELSE Material
    END,
    PlaceOfDiscovery = CASE
        WHEN PlaceOfDiscovery = '' OR PlaceOfDiscovery IS NULL THEN 'Ancient Egypt'
        ELSE PlaceOfDiscovery
    END
WHERE Material = '' OR PlaceOfDiscovery = '' OR Material IS NULL OR PlaceOfDiscovery IS NULL;

COMMIT TRANSACTION;

PRINT '✅ Artifact data updated successfully!';
PRINT '';

-- Show summary
PRINT '📊 Summary of updates:';
SELECT 
    COUNT(*) as TotalArtifacts,
    COUNT(CASE WHEN Material <> '' THEN 1 END) as WithMaterial,
    COUNT(CASE WHEN PlaceOfDiscovery <> '' THEN 1 END) as WithPlaceOfDiscovery
FROM Artifacts;

PRINT '';
PRINT '📋 Sample updated artifacts:';
SELECT TOP 15
    Name,
    Period,
    Material,
    PlaceOfDiscovery
FROM Artifacts
WHERE Name IN (
    'Mask of Tutankhamun',
    'Great Pyramids of Giza',
    'Sphinx',
    'Nefertiti',
    'Pyramid of Djoser',
    'Colossoi of Memnon',
    'Seated Statue of Djoser',
    'Statue of Khafre',
    'Bent Pyramid of King Sneferu',
    'Akhenaten'
)
ORDER BY Name;

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '✅ Update Complete!';
PRINT '═══════════════════════════════════════════════════════════';

GO
