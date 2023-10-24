
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Validation
{
    internal static partial class ValidationErrors
    {
        internal static class League
        {
            internal static Error EmptyOrTooShortOfficialName => new Error("League.EmptyOrTooShortOfficialName",
                "Try to write more descriptive name");
            internal static Error EmptyOrTooBigShortName => new Error("League.EmptyOrTooBigShortName",
                "Try to write more shorter league short name");
            internal static Error IdShouldBePositive => new Error("League.NegativeId", "The league id must be positive.");
            internal static Error NotFound => new Error("League.NotFound", "The league with the specified identifier was not found.");
        }
    }
}
