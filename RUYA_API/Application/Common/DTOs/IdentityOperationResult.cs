namespace RUYA_API.Application.Common.DTOs
{
    public class IdentityOperationResult
    {
        public bool Succeeded { get; private init; }
        public IEnumerable<string> Errors { get; private init; } = Array.Empty<string>();

        public static IdentityOperationResult Success() => new() { Succeeded = true };
        public static IdentityOperationResult Failure(IEnumerable<string> errors) =>
            new() { Succeeded = false, Errors = errors };
    }
}
