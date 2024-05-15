using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.EFCoreUtilities;

public class DbContextConfiguration<TDb>( ILoggerFactory? loggerFactory )
    where TDb : DbContext
{
    private readonly Dictionary<Type, Type> _configurators = [];
    private readonly ILogger? _logger = loggerFactory?.CreateLogger<DbContextConfiguration<TDb>>();

    public bool RegisterConfigurator<TEntity, TConfig>()
        where TEntity : class
        where TConfig : class, IEntityConfiguration<TEntity>, new()
    {
        if( _configurators.TryAdd( typeof( TEntity ), typeof(TConfig)) )
            return true;

        _logger?.DuplicateEntityConfigurator( typeof( TEntity ), typeof( TConfig ) );
        return false;
    }

    public bool ConfigureEntities( ModelBuilder builder )
    {
        var entityTypes = DbExtensions.GetEntityTypes<TDb>();
        var allOkay = true;

        foreach( var kvp in _configurators )
        {
            try
            {
                var configurator = Activator.CreateInstance( kvp.Value ) as IEntityConfiguration;
                configurator!.Configure( builder );

                entityTypes.Remove( kvp.Key );
            }
            catch( Exception ex )
            {
                _logger?.ConfigurationFailure( kvp.Key, kvp.Value, ex.Message );
                allOkay = false;
            }
        }

        if( entityTypes.Count == 0 )
            return allOkay;

        _logger?.UnconfiguredEntityTypes( typeof(TDb), string.Join( ", ", entityTypes.Select( et => et.Name ) ) );
        return false;
    }
}
