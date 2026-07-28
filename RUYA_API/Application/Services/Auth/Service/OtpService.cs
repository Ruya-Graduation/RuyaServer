using Microsoft.Extensions.Caching.Memory;
using RUYA_API.Application.Services.Auth.DTOs;
using RUYA_API.Application.Services.Auth.Enums;
using RUYA_API.Application.Services.Auth.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace RUYA_API.Application.Services.Auth.Service
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;

        private const int OtpLength = 6;
        private static readonly TimeSpan OtpTtl = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan ResetTokenTtl = TimeSpan.FromMinutes(5);
        private const int MaxAttempts = 5;

        private static string OtpKey(string email) => $"otp:{Normalize(email)}";
        private static string ResetKey(string token) => $"reset:{token}";
        private static string Normalize(string email) => email.Trim().ToLowerInvariant();

        public OtpService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string? ConsumeResetToken(string email, string resetToken)
        {
            var key = ResetKey(resetToken);

            if (!_cache.TryGetValue<string>(key, out var storedEmail) || storedEmail is null)
                return null;

            _cache.Remove(key); // one-time use regardless of outcome below

            return string.Equals(storedEmail, Normalize(email), StringComparison.Ordinal)
                ? storedEmail
                : null;
        }

        public string GenerateAndStoreOtp(string email)
        {
            var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString($"D{OtpLength}");

            var entry = new OtpEntry
            {
                CodeHash = Hash(code),
                Attempts = 0,
                ExpiresAtUtc = DateTimeOffset.UtcNow.Add(OtpTtl)
            };

            _cache.Set(OtpKey(email), entry, OtpTtl);
            return code;
        }

        public string IssueResetToken(string email)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-").Replace("/", "_").Replace("=", "");

            _cache.Set(ResetKey(token), Normalize(email), ResetTokenTtl);
            return token;
        }

        public OtpVerificationResult VerifyOtp(string email, string code)
        {
            var key = OtpKey(email);

            if (!_cache.TryGetValue<OtpEntry>(key, out var entry) || entry is null)
                return OtpVerificationResult.InvalidOrExpired;

            if (DateTimeOffset.UtcNow > entry.ExpiresAtUtc)
            {
                _cache.Remove(key);
                return OtpVerificationResult.InvalidOrExpired;
            }

            if (entry.Attempts >= MaxAttempts)
            {
                _cache.Remove(key);
                return OtpVerificationResult.TooManyAttempts;
            }

            if (!FixedTimeEquals(entry.CodeHash, Hash(code)))
            {
                entry.Attempts++;

                // Re-set with the *remaining* time to the original expiry, not a fresh OtpTtl —
                // otherwise repeated wrong guesses would keep extending the code's lifetime.
                var remaining = entry.ExpiresAtUtc - DateTimeOffset.UtcNow;
                _cache.Set(key, entry, remaining > TimeSpan.Zero ? remaining : TimeSpan.FromSeconds(1));

                return OtpVerificationResult.InvalidOrExpired;
            }

            _cache.Remove(key); // one-time use
            return OtpVerificationResult.Success;
        }

        private static string Hash(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(bytes);
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);
            return aBytes.Length == bBytes.Length && CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
        }
    }
}
