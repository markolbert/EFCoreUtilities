# Generalized Design-time Factory

Debugging a `DbContext` can be complicated if things like the location of the database is different at design time. I often find that's the case in my projects because a configuration class may be defined in its own assembly so it can be shared across multiple projects.

`DesignTimeFactory<TDbContext>` abstracts this process. All you need to do is implement a simple class deriving from
`DesignTimeFactory<TDbContext>`. Here's an example of doing this when you're using Sqlite3:

```csharp
public class QbDbDesignTimeFactory : DesignTimeFactory<QbDbContext>
{
    public QbDbDesignTimeFactory()
        : base( GetSourceCodeDirectoryOfClass() )
    {
    }

    protected override void ConfigureOptionsBuilder( DbContextOptionsBuilder<QbDbContext> builder, string dbDirectory )
    {
        dbPath = Path.EndsInDirectorySeparator( dbPath )
            ? Path.Combine( dbPath, "QbDatabase.db" )
            : dbPath;

        if( File.Exists( dbPath ) )
            File.Delete( dbPath );

        var connBuilder = new SqliteConnectionStringBuilder() { DataSource = dbPath };

        builder.UseSqlite(connBuilder.ConnectionString);
    }
}
```

All the derived class does is call the base constructor with the directory the derived design time factory is defined in, and then define the path to the database via a connection string. You could make other changes to the connection string at this point, too.

**The easiest way to get the source code directory of the derived design time
factory is by calling the protected method `GetSourceCodeDirectoryOfClass()`
method, as shown in the example.** Otherwise you'd end up hardcoding the path,
which is not a good idea.

You use this with the **ef tools package** like this:

- within the *Package Manager Console*, move into the directory where your database
assembly is defined
- if needed, create a new migration
- run the update command specifying the path to where the Sqlite3 db file should
be created

If the database is defined in a folder called **QbDatabase** and the consuming
assembly is called **ImportNames** this would look like this (the example starts
off by printing out the current working directory, which I generally find to be a
good cautionary step to take):

```cmd
PM> pwd

Path                                    
----                                    
C:\Programming\Quickbooks2LGL\QbDatabase


PM> dotnet ef migrations add Initial
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
PM> dotnet ef database update -- ../ImportNames
Build started...
Build succeeded.
Applying migration '20220802204340_Initial'.
Done.
PM> 
```

If you don't include the path to where you want the database file created it will
be created in the directory defining the database.

[return to readme](../readme.md)
