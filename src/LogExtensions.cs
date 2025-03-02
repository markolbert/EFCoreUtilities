using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.EFCoreUtilities;

internal static partial class LogExtensions
{
    [LoggerMessage(LogLevel.Information, "{caller}: {text}")]
    public static partial void PendingChanges(
        this ILogger logger,
        string text,
        [ CallerMemberName ] string caller = ""
    );

    [LoggerMessage(LogLevel.Critical, "{caller}: {text}")]
    public static partial void Exception(
        this ILogger logger,
        string text,
        [CallerMemberName] string caller = ""
    );
}