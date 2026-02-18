using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Notredame.Api.Settings;

internal partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound([DisallowNull] object? value)
    {
        return SlugifyRegex().Replace(value?.ToString() ?? string.Empty, "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex SlugifyRegex();
}