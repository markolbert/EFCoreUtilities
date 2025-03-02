# J4JSoftware.EFCore.Utilities

|Version|Description|
|:-----:|-----------|
|2.0.0|updated to Net9, **breaking changes** (#200)|
|1.4.1|fixed nuget dependencies|
|1.4.0|**breaking changes**, [see details below](#140)|
|1.3.0|Updated to Net 7, updated packages, [see details below](#130)|
|1.2|revised design time factory|
|1.1|updated to Net 6|
|1.0.1|added nuget readme|
|1.0|initial release|

## 2.0.0

Updated to Net9.

Removed previously obsoleted entity configuration extension methods. The revised approach
revolves around creating an instance of `DbContextConfiguration`, registering `IEntityConfiguration` classes and then calling the `ConfigureEntities()` method.

Convenience methods were added:

- `GetEntityTypes()`, to get a list of all database entity types in a `DbContext`;
- `TryGetSet()`, to get a DbSet safely for a database entity;
- `TryGetEntitySingleFieldPrimaryKeyInfo()`, to get a `PropertyInfo` object for a single key
primary field, assuming the queried entity has one; and,
- `TryGetEntitySingleFieldPrimaryKey()`, to get the name of the single key primary field,
assuming the queried entity has one.

[return to readme](../readme.md)

## 1.4.0

To make the library more generally useful logging has been migrated from [Serilog](https://serilog.net/) to Microsoft's logging
system.

In general, this means instances of `ILoggerFactory` are used as construction parameters, rather than `ILogger`.
This is because, while Serilog lets you scope an `ILogger` instance to a new type, you can only define
the scope of a Microsoft `ILogger` by calling `ILoggerFactory.CreateLogger()`.

FWIW, in my projects I continue to use Serilog behind the scenes as my logging engine. It's great!

[return to readme](../readme.md)

## 1.3.0

- Added detailed formatting for `DbUpdateExceptions`

[return to readme](../readme.md)
