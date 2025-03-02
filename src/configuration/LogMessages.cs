using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.EFCoreUtilities;

internal static partial class LogMessages
{
    [ LoggerMessage( LogLevel.Warning,
                     "{caller}: Duplicate entity configurator for {entityType} ({configuratorType}" ) ]
    public static partial void DuplicateEntityConfigurator(
        this ILogger logger,
        Type entityType,
        Type configuratorType,
        [ CallerMemberName ] string caller = ""
    );

    [ LoggerMessage( LogLevel.Error,
                     "{caller}: Could not use configurator {configuratorType} for entity Type {entityType}, message was '{mesg}'" ) ]
    public static partial void ConfigurationFailure(
        this ILogger logger,
        Type entityType,
        Type configuratorType,
        string mesg,
        [ CallerMemberName ] string caller = ""
    );

    [ LoggerMessage( LogLevel.Warning, "{caller}: Entity types {types} were not configured for {dbType}" ) ]
    public static partial void UnconfiguredEntityTypes(
        this ILogger logger,
        Type dbType,
        string types,
        [ CallerMemberName ] string caller = ""
    );
}
