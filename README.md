# EFCoreUtilities

This library contains classes which can systematize configuring and creating Entity Framework Core databases.

## DesignTimeFactory<TDbContext>

If you define your EF Core database in its own assembly (which is a necessity if you're including it in multiple projects) you need to 
create a design time factory so that the EF Core tools will be able to work (at least as of January, 2020; personally, I hope that
requirement gets removed at some point).

The abstract class `DesignTimeFactory<TDbContext>` takes care of that. All you have to do is:

* define a method to return an IDbContextFactoryConfiguration object. I generally do this by annotating my core configuration class
with that interface, ensuring it has a DatabasePath property.

* ensure your DbContext class sports a constructor taking an IDbContextFactoryConfiguration object as an argument.

Example (using Sqlite3):

```csharp
public class AVDesignTimeFactory : DesignTimeFactory<AlphaVantageContext>
{
    protected override IDbContextFactoryConfiguration GetDatabaseConfiguration()
    {
        return new AppConfiguration()
        {
            DatabasePath = Path.Combine( Environment.CurrentDirectory, AppConfiguration.DbName )
        };
    }
}

public class AlphaVantageContext : DbContext
{
    private readonly IDbContextFactoryConfiguration _config;

    public AlphaVantageContext( IDbContextFactoryConfiguration config )
    {
        _config = config ?? throw new NullReferenceException( nameof(config) );
    }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
        base.OnConfiguring( optionsBuilder );

        // we open the connection, and use the opened connection to initialize the entity
        // framework via optionsBuilder, to preserve the UDF configuration
        var connection = new SqliteConnection( $"DataSource={_config.DatabasePath}" );
        connection.Open();

        optionsBuilder.UseSqlite( connection );
    }
}
```

## Configuring Entity Classes

I use the "functional" approach to defining EF entities and contexts rather than the attribute-based style. By default that 
results in a large and difficult to read `OnModelCreating()` override. What's worse, the entity details (e.g., properties, methods)
are generally in a separate file requiring you to bounce back and forth between two files as you're defining an entity's 
configuration.

`EntityConfigurationAttribute` and `EntityConfigurator` and the extension method `ConfigureEntities` lets
you configure entities in an internal class you can embed in the entity's class file and make just one call in `OnModelCreating()`.

You start by decorating an entity class with `EntityConfiguration[<name of configuration class>]`:

```csharp
[EntityConfiguration( typeof( HistoricalDataConfigurator ) )]
public class HistoricalData
{
    public int SecurityInfoID { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }

    public SecurityInfo SecurityInfo { get; set; }
}
```

and then define a configuration class:

```csharp
internal class HistoricalDataConfigurator : EntityConfigurator<HistoricalData>
{
    protected override void Configure( EntityTypeBuilder<HistoricalData> builder )
    {
        builder.HasKey( x => new { SymbolID = x.SecurityInfoID, x.Timestamp } );

        builder.HasOne<SecurityInfo>( x => x.SecurityInfo )
            .WithMany( x => x.HistoricalData );
    }
}
```

After you've done this for all your entity classes you create a simple `OnModelCreating()` override in your DbContext class 
(this context uses Sqlite3):

```csharp
public class AlphaVantageContext : DbContext
{
    private readonly IDbContextFactoryConfiguration _config;

    public AlphaVantageContext( IDbContextFactoryConfiguration config )
    {
        _config = config ?? throw new NullReferenceException( nameof(config) );
    }

    public DbSet<SecurityInfo> Securities { get; set; }
    public DbSet<HistoricalData> HistoricalData { get; set; }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
        base.OnConfiguring( optionsBuilder );

        // we open the connection, and use the opened connection to initialize the entity
        // framework via optionsBuilder, to preserve the UDF configuration
        var connection = new SqliteConnection( $"DataSource={_config.DatabasePath}" );
        connection.Open();

        optionsBuilder.UseSqlite( connection );
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ConfigureEntities( this.GetType().Assembly );
    }
}
```
