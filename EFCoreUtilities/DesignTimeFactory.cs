using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace J4JSoftware.EFCoreUtilities
{
    public class DesignTimeFactory<TDbContext, TDbConfig> : IDesignTimeDbContextFactory<TDbContext>
        where TDbContext : DbContext
        where TDbConfig : IDbContextFactoryConfiguration, new()

    {
        private readonly IDbContextFactoryConfiguration _factoryConfig;
        private readonly ConstructorInfo _ctor;

        public DesignTimeFactory( IDbContextFactoryConfiguration factoryConfig )
        {
            _factoryConfig = factoryConfig ?? throw new NullReferenceException( nameof(factoryConfig) );

            // ensure TDbContext can be created from a IDbContextFactoryConfiguration
            _ctor = typeof(TDbContext).GetConstructor( new Type[] { typeof(IDbContextFactoryConfiguration) } );

            if( _ctor == null )
                throw new ArgumentException(
                    $"{typeof(TDbContext).Name} cannot be constructed from a {nameof(IDbContextFactoryConfiguration)}" );
        }

        public TDbContext CreateDbContext( string[] args )
        {
            return (TDbContext) _ctor.Invoke( new object[] { _factoryConfig } );
        }
    }
}
