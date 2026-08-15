-- ═══════════════════════════════════════════════════════════════════════════════
-- Test Script: Verify Artifact Names Match Python API
-- ═══════════════════════════════════════════════════════════════════════════════

USE RUYA;
GO

PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Artifact Name Verification';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Check total count
DECLARE @TotalArtifacts INT = (SELECT COUNT(*) FROM Artifacts);
PRINT '📊 Total artifacts in database: ' + CAST(@TotalArtifacts AS VARCHAR(10));
PRINT '';

IF @TotalArtifacts = 0
BEGIN
    PRINT '❌ ERROR: No artifacts found in database!';
    PRINT '   Please run seed-database.sql first.';
    PRINT '';
END
ELSE IF @TotalArtifacts < 84
BEGIN
    PRINT '⚠️  WARNING: Expected 84 artifacts, found ' + CAST(@TotalArtifacts AS VARCHAR(10));
    PRINT '   Some artifacts may be missing.';
    PRINT '';
END
ELSE
BEGIN
    PRINT '✅ Artifact count looks good!';
    PRINT '';
END

-- Test specific artifact that Python commonly returns
PRINT '🔍 Testing common artifacts:';
PRINT '';

-- Mask of Tutankhamun (Python class_id: 36)
IF EXISTS (SELECT 1 FROM Artifacts WHERE Name = 'Mask of Tutankhamun')
BEGIN
    DECLARE @TutankhamunId INT = (SELECT Id FROM Artifacts WHERE Name = 'Mask of Tutankhamun');
    PRINT '✅ "Mask of Tutankhamun" found (ID: ' + CAST(@TutankhamunId AS VARCHAR(10)) + ')';
END
ELSE
BEGIN
    PRINT '❌ "Mask of Tutankhamun" NOT FOUND';
    
    -- Try to find similar
    IF EXISTS (SELECT 1 FROM Artifacts WHERE Name LIKE '%Tutankh%')
    BEGIN
        DECLARE @SimilarName NVARCHAR(255) = (SELECT TOP 1 Name FROM Artifacts WHERE Name LIKE '%Tutankh%');
        PRINT '   Similar artifact found: "' + @SimilarName + '"';
        PRINT '   ⚠️  Name mismatch! Update database to match Python API exactly.';
    END
END
PRINT '';

-- Sphinx (Python class_id: 53)
IF EXISTS (SELECT 1 FROM Artifacts WHERE Name = 'Sphinx')
BEGIN
    PRINT '✅ "Sphinx" found';
END
ELSE
BEGIN
    PRINT '❌ "Sphinx" NOT FOUND';
END
PRINT '';

-- Great Pyramids of Giza (Python class_id: 24)
IF EXISTS (SELECT 1 FROM Artifacts WHERE Name = 'Great Pyramids of Giza')
BEGIN
    PRINT '✅ "Great Pyramids of Giza" found';
END
ELSE
BEGIN
    PRINT '❌ "Great Pyramids of Giza" NOT FOUND';
END
PRINT '';

-- Nefertiti (Python class_id: 41)
IF EXISTS (SELECT 1 FROM Artifacts WHERE Name = 'Nefertiti')
BEGIN
    PRINT '✅ "Nefertiti" found';
END
ELSE
BEGIN
    PRINT '❌ "Nefertiti" NOT FOUND';
END
PRINT '';

PRINT '─────────────────────────────────────────────────────────────';
PRINT '';

-- Show sample of artifact names
PRINT '📋 Sample artifact names in database (first 20):';
PRINT '';

SELECT TOP 20 
    Id,
    Name,
    Category,
    Period
FROM Artifacts
ORDER BY Name;

PRINT '';
PRINT '─────────────────────────────────────────────────────────────';
PRINT '';

-- Check for potential issues
PRINT '🔍 Checking for potential issues:';
PRINT '';

-- Check for duplicates
DECLARE @Duplicates INT = (
    SELECT COUNT(*) 
    FROM (
        SELECT Name, COUNT(*) as cnt 
        FROM Artifacts 
        GROUP BY Name 
        HAVING COUNT(*) > 1
    ) as dup
);

IF @Duplicates > 0
BEGIN
    PRINT '⚠️  Found ' + CAST(@Duplicates AS VARCHAR(10)) + ' duplicate artifact names!';
    
    SELECT Name, COUNT(*) as Occurrences
    FROM Artifacts
    GROUP BY Name
    HAVING COUNT(*) > 1
    ORDER BY Name;
    
    PRINT '';
END
ELSE
BEGIN
    PRINT '✅ No duplicate names found';
END
PRINT '';

-- Check for empty names
DECLARE @EmptyNames INT = (SELECT COUNT(*) FROM Artifacts WHERE Name IS NULL OR Name = '');
IF @EmptyNames > 0
BEGIN
    PRINT '⚠️  Found ' + CAST(@EmptyNames AS VARCHAR(10)) + ' artifacts with empty names!';
END
ELSE
BEGIN
    PRINT '✅ All artifacts have names';
END
PRINT '';

-- Check for leading/trailing spaces
DECLARE @SpaceIssues INT = (
    SELECT COUNT(*) 
    FROM Artifacts 
    WHERE Name <> LTRIM(RTRIM(Name))
);

IF @SpaceIssues > 0
BEGIN
    PRINT '⚠️  Found ' + CAST(@SpaceIssues AS VARCHAR(10)) + ' artifacts with leading/trailing spaces!';
    
    SELECT Id, '>' + Name + '<' as NameWithBrackets
    FROM Artifacts
    WHERE Name <> LTRIM(RTRIM(Name));
    
    PRINT '';
    PRINT '   Fix with: UPDATE Artifacts SET Name = LTRIM(RTRIM(Name));';
END
ELSE
BEGIN
    PRINT '✅ No spacing issues found';
END
PRINT '';

PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Verification Complete';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

-- Final recommendation
IF @TotalArtifacts = 84 AND @Duplicates = 0 AND @EmptyNames = 0 AND @SpaceIssues = 0
BEGIN
    PRINT '🎉 Database looks perfect! Ready for testing.';
END
ELSE
BEGIN
    PRINT '⚠️  Some issues found. Please fix before testing.';
END

PRINT '';

GO
