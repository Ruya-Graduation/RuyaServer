-- ═══════════════════════════════════════════════════════════════════════════════
-- RUYA Database Seeder SQL Script
-- ═══════════════════════════════════════════════════════════════════════════════
-- This script seeds the RUYA database with 6 sites and 84 Egyptian artifacts
-- Run this script in SQL Server Management Studio or Azure Data Studio
-- ═══════════════════════════════════════════════════════════════════════════════

USE RUYA;
GO

PRINT '═══════════════════════════════════════════════════════════';
PRINT '         RUYA Database Seeder';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════
-- STEP 1: Seed Sites
-- ═══════════════════════════════════════════════════════════════════════════════

IF NOT EXISTS (SELECT 1 FROM Sites)
BEGIN
    PRINT '🌍 Seeding sites...';
    
    INSERT INTO Sites (Name, City, Country, Latitude, Longitude, Hours, Ticket, Crowds, Description, CreatedAt, UpdatedAt)
    VALUES 
    ('Egyptian Museum (Cairo)', 'Cairo', 'Egypt', 30.0478, 31.2336, '9:00 AM - 5:00 PM', '200 EGP', 'High', 
     'The Museum of Egyptian Antiquities, known commonly as the Egyptian Museum, in Cairo, Egypt, is home to an extensive collection of ancient Egyptian antiquities.', 
     GETUTCDATE(), NULL),
    
    ('Grand Egyptian Museum (GEM)', 'Giza', 'Egypt', 29.9932, 31.1173, '9:00 AM - 5:00 PM', '400 EGP', 'Very High',
     'The Grand Egyptian Museum (GEM), also known as the Giza Museum, is an archaeological museum under construction in Giza, Egypt.',
     GETUTCDATE(), NULL),
    
    ('Luxor Temple', 'Luxor', 'Egypt', 25.6995, 32.6392, '6:00 AM - 9:00 PM', '160 EGP', 'Medium',
     'Luxor Temple is a large Ancient Egyptian temple complex located on the east bank of the Nile River.',
     GETUTCDATE(), NULL),
    
    ('Giza Necropolis', 'Giza', 'Egypt', 29.9792, 31.1342, '8:00 AM - 4:00 PM', '200 EGP', 'Very High',
     'The Giza pyramid complex is an archaeological site on the Giza Plateau, on the outskirts of Cairo, Egypt.',
     GETUTCDATE(), NULL),
    
    ('Saqqara', 'Saqqara', 'Egypt', 29.8714, 31.2169, '8:00 AM - 4:00 PM', '150 EGP', 'Low',
     'Saqqara, also spelled Sakkara or Saccara, is an Egyptian village in Giza Governorate, that contains ancient burial grounds of Egyptian royalty.',
     GETUTCDATE(), NULL),
    
    ('Dahshur', 'Dahshur', 'Egypt', 29.7908, 31.2292, '8:00 AM - 4:00 PM', '100 EGP', 'Low',
     'Dahshur is a royal necropolis located in the desert on the west bank of the Nile.',
     GETUTCDATE(), NULL);
    
    PRINT '✅ Sites seeded successfully: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' sites added';
END
ELSE
BEGIN
    PRINT '⏭️  Sites already exist. Skipping site seeding.';
END

PRINT '';

-- ═══════════════════════════════════════════════════════════════════════════════
-- STEP 2: Seed Artifacts (All 84 Egyptian Artifacts matching Python YOLO model)
-- ═══════════════════════════════════════════════════════════════════════════════

IF NOT EXISTS (SELECT 1 FROM Artifacts)
BEGIN
    PRINT '🏺 Seeding artifacts...';
    
    DECLARE @EgyptianMuseumId INT = (SELECT Id FROM Sites WHERE Name LIKE '%Egyptian Museum (Cairo)%');
    DECLARE @GEMId INT = (SELECT Id FROM Sites WHERE Name LIKE '%Grand Egyptian Museum%');
    DECLARE @LuxorId INT = (SELECT Id FROM Sites WHERE Name LIKE '%Luxor%');
    DECLARE @GizaId INT = (SELECT Id FROM Sites WHERE Name LIKE '%Giza Necropolis%');
    DECLARE @SaqqaraId INT = (SELECT Id FROM Sites WHERE Name LIKE '%Saqqara%');
    DECLARE @DahshurId INT = (SELECT Id FROM Sites WHERE Name LIKE '%Dahshur%');
    
    -- Use Egyptian Museum as default if specific site not found
    IF @EgyptianMuseumId IS NULL SET @EgyptianMuseumId = (SELECT TOP 1 Id FROM Sites);
    IF @GEMId IS NULL SET @GEMId = @EgyptianMuseumId;
    IF @LuxorId IS NULL SET @LuxorId = @EgyptianMuseumId;
    IF @GizaId IS NULL SET @GizaId = @EgyptianMuseumId;
    IF @SaqqaraId IS NULL SET @SaqqaraId = @EgyptianMuseumId;
    IF @DahshurId IS NULL SET @DahshurId = @EgyptianMuseumId;
    
    INSERT INTO Artifacts (SiteId, Name, Category, Civilization, Period, ImageUrl, ImagePublicId, CreatedAt, UpdatedAt)
    VALUES 
    -- Class 0-9
    (@EgyptianMuseumId, 'Akhenaten', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Amenhotep III', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Amenhotep III and Tiye', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Amenhotep III with Plate', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Augustus', 'Statue', 'Greco-Roman Egypt', 'Ptolemaic Period', '', '', GETUTCDATE(), NULL),
    (@DahshurId, 'Bent Pyramid of King Sneferu', 'Pyramid', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Black Granite Bust of Mentuemhat', 'Bust', 'Ancient Egypt', 'Late Period', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Bust of Isis', 'Bust', 'Ancient Egypt', 'Ptolemaic Period', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Clossal Head of the god Serapis', 'Head', 'Greco-Roman Egypt', 'Ptolemaic Period', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Clossal head of Senwosret 1', 'Head', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 10-19
    (@EgyptianMuseumId, 'Coffin of Ahmose I', 'Coffin', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of Amenhotep III', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of God Ptah', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of Hormoheb', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of King Senwosret IlI', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of Middle Kingdom King', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of Queen Hatshepsut', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of Ramesses II', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Colossal Statue of Ramesses II beloved of Ptah', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@LuxorId, 'Colossoi of Memnon', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 20-29
    (@EgyptianMuseumId, 'Colossus of Senuseret I', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Column of Merenptah', 'Architectural Element', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Granite Statue of Osiris', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Granite Statue of Tutankhamun', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@GizaId, 'Great Pyramids of Giza', 'Pyramid', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Grey Granite of Ramesses II', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Hathor Capital', 'Architectural Element', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Hatshepsut face', 'Head', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Head Statue of Amenhotep III', 'Head', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Head Statue of Amenhotep iii', 'Head', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 30-39
    (@SaqqaraId, 'Head of Userkaf', 'Head', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Hor I', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Isis with her child', 'Statue', 'Ancient Egypt', 'Late Period', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'King Amenemhat 3', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'King Thutmose III', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Mask of Thuya', 'Funerary Mask', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@GEMId, 'Mask of Tutankhamun', 'Funerary Mask', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Mask of Yuya', 'Funerary Mask', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Menkaure Statue', 'Statue', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Mentuhotep Nebhetpre', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 40-49
    (@EgyptianMuseumId, 'Naos of Senwosert I', 'Shrine', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Nefertiti', 'Bust', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Obelsik Tip of Hatshepsut', 'Obelisk', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Offering table of Amenemhat 6', 'Relief', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@SaqqaraId, 'Pyramid of Djoser', 'Pyramid', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Rhetorical Stela of King Ramesses ll', 'Stela', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Seated Statue of Amenhotep III', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@SaqqaraId, 'Seated Statue of Djoser', 'Statue', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Seated Statue of God Sekhmet', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Seated Statue of Ramesses II', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 50-59
    (@EgyptianMuseumId, 'Seated Statue of Ramesses II and God Ptah', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Seated Statue of Thutmose III', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Senwosret III', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@GizaId, 'Sphinx', 'Statue', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Sphinx of Amenmhat III', 'Sphinx', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Sphinx of Kings Ramesses ll - Merenptah', 'Sphinx', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Standing Statue of King Ramses II', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Standing Statue of Thutmose III', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue Head of Akhenaten', 'Head', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Amenhotep III and God Re-Horakhty', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 60-69
    (@EgyptianMuseumId, 'Statue of Amenmhat I', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Amun and King', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Ankhesenamun', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Carcala', 'Statue', 'Greco-Roman Egypt', 'Roman Period', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of God Ptah Ramesses ll Goddess Sekhmet', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of God Ra-Horakhty', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Khafre', 'Statue', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Khufu', 'Statue', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of King Ramesses ll - Goddess Anath', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@GEMId, 'Statue of King Ramses II Grand Egyptian Museum', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 70-79
    (@LuxorId, 'Statue of King Ramses II Luxor Temple', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of King Sety Il Holding Standards', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@SaqqaraId, 'Statue of King Zoser', 'Statue', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Mentuhotep II', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Merenptah as standard Bearer', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Osiris', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Queen Metnoforet', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Ramesses III as standard Bearer', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@DahshurId, 'Statue of Snefru', 'Statue', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'Statue of Sobekhotep V', 'Statue', 'Ancient Egypt', 'Middle Kingdom', '', '', GETUTCDATE(), NULL),
    
    -- Class 80-83
    (@GEMId, 'Statue of Tutankhamun', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@DahshurId, 'Stela of king Snefero', 'Stela', 'Ancient Egypt', 'Old Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'bust of Ramesses II', 'Bust', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL),
    (@EgyptianMuseumId, 'kneeling statue of queen hatshibsut', 'Statue', 'Ancient Egypt', 'New Kingdom', '', '', GETUTCDATE(), NULL);
    
    PRINT '✅ Artifacts seeded successfully: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' artifacts added';
END
ELSE
BEGIN
    PRINT '⏭️  Artifacts already exist. Skipping artifact seeding.';
END

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '✅ Database seeding completed successfully!';
PRINT '   Total Sites: ' + CAST((SELECT COUNT(*) FROM Sites) AS VARCHAR(10));
PRINT '   Total Artifacts: ' + CAST((SELECT COUNT(*) FROM Artifacts) AS VARCHAR(10));
PRINT '═══════════════════════════════════════════════════════════';

GO
