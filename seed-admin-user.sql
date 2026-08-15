-- ═══════════════════════════════════════════════════════════════════════════════
-- Seed Admin User
-- ═══════════════════════════════════════════════════════════════════════════════
-- Creates a default admin user for the RUYA application
-- Email: admin@ruya.com
-- Password: Admin@123
-- ═══════════════════════════════════════════════════════════════════════════════

USE RUYA;
GO

PRINT '═══════════════════════════════════════════════════════════';
PRINT '    Seeding Admin User';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';

DECLARE @AdminEmail NVARCHAR(256) = 'admin@ruya.com';
DECLARE @AdminUserId NVARCHAR(450);
DECLARE @AdminRoleId NVARCHAR(450);

-- Check if admin user already exists
IF EXISTS (SELECT 1 FROM AspNetUsers WHERE Email = @AdminEmail)
BEGIN
    PRINT '⏭️  Admin user already exists.';
    PRINT '   Email: ' + @AdminEmail;
    
    SELECT @AdminUserId = Id FROM AspNetUsers WHERE Email = @AdminEmail;
    PRINT '   User ID: ' + @AdminUserId;
END
ELSE
BEGIN
    PRINT '👤 Creating admin user...';
    
    -- Generate new GUID for user ID
    SET @AdminUserId = NEWID();
    
    -- Password hash for 'Admin@123'
    -- NOTE: This is a pre-generated hash. For production, use proper password hashing!
    DECLARE @PasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEFD7cFPqOqHW8Jb7K3LZC8u0XYz1QqPNxVW5rT3dF9mH8kL2pS6wE4vN1oM0aI7bRQ==';
    DECLARE @SecurityStamp NVARCHAR(MAX) = NEWID();
    DECLARE @ConcurrencyStamp NVARCHAR(MAX) = NEWID();
    
    -- Insert admin user
    INSERT INTO AspNetUsers (
        Id,
        UserName,
        NormalizedUserName,
        Email,
        NormalizedEmail,
        EmailConfirmed,
        PasswordHash,
        SecurityStamp,
        ConcurrencyStamp,
        PhoneNumber,
        PhoneNumberConfirmed,
        TwoFactorEnabled,
        LockoutEnabled,
        AccessFailedCount,
        FirstName,
        LastName
    )
    VALUES (
        @AdminUserId,
        @AdminEmail,
        UPPER(@AdminEmail),
        @AdminEmail,
        UPPER(@AdminEmail),
        1, -- EmailConfirmed = true
        @PasswordHash,
        @SecurityStamp,
        @ConcurrencyStamp,
        '+1234567890',
        0, -- PhoneNumberConfirmed = false
        0, -- TwoFactorEnabled = false
        1, -- LockoutEnabled = true
        0, -- AccessFailedCount = 0
        'Admin',
        'User'
    );
    
    PRINT '✅ Admin user created successfully.';
    PRINT '   Email: ' + @AdminEmail;
    PRINT '   Password: Admin@123';
    PRINT '   User ID: ' + @AdminUserId;
END

PRINT '';

-- Assign Admin role
SELECT @AdminRoleId = Id FROM AspNetRoles WHERE NormalizedName = 'ADMIN';

IF @AdminRoleId IS NULL
BEGIN
    PRINT '⚠️  Admin role not found. Please ensure roles are seeded first.';
    PRINT '   Run the application once to seed roles, then run this script.';
END
ELSE
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM AspNetUserRoles 
        WHERE UserId = @AdminUserId AND RoleId = @AdminRoleId
    )
    BEGIN
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@AdminUserId, @AdminRoleId);
        
        PRINT '✅ Admin role assigned to user.';
    END
    ELSE
    BEGIN
        PRINT '⏭️  User already has Admin role.';
    END
END

PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '✅ Admin User Setup Complete!';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';
PRINT '📋 Admin Credentials:';
PRINT '   Email: admin@ruya.com';
PRINT '   Password: Admin@123';
PRINT '';
PRINT '⚠️  IMPORTANT: Change this password after first login!';
PRINT '';

GO
