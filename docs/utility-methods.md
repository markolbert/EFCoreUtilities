# Utility Extension Methods

- [Formatting exceptions](#formatting-dbupdateexceptions)
- [Getting entity types](#getting-entity-types)
- [Getting entity type from name](#getting-entity-name)
- [Getting entity name from type](#getting-entity-type)
- Single field primary keys
  - [Getting PropertyInfo for the key](#getting-propertyinfo-for-single-field-key)
  - [Getting name of the key field](#getting-name-of-single-field-key)

## Formatting DbUpdateExceptions

Figuring out what caused an Entity Framework exception to be thrown can be messy, because many of the details are buried within inner exceptions, the entities involved aren't included in the exception messages, etc.

The `DbUpdateException.FormatDbException()` extension method extracts more detailed information from a `DbUpdateException` and formats it into a string you can log or otherwise display.

If you find additional Entity Framework exception information useful, check out [Entity Framework Exceptions](https://github.com/Giorgi/EntityFramework.Exceptions), which provides even more detailed information.

[return to readme](../readme.md)

## Getting entity types

There are two static methods operating on a `DbContext` type that let you retrieve a list
of the entity types managed by the `DbContext`. Both methods return `List<Type>`.

The generic version, `GetEntityTypes<TContext>()`, requires that `TContext` be descended from
`DbContext`. This version is not an extension method.

The non-generic version, `GetEntityTypes()`, is an extension method that takes a single argument, which must be of type `DbContext`.

[return to readme](../readme.md)

## Getting entity type

Sometimes you want to get the entity type from its name. The `TryGetEntityType()` extension method, which operates on instances of `DbContext`, does this.

|Argument|Type|Comments|
|--------|----|--------|
|dbContext|`DbContext`|extension method parameter|
|entityName|`string`|the name of the entity|
|entityType|`out Type?`|will contain the entity Type if it was found|

The method returns `true` if the supplied entity name matches an entity type, `false` otherwise.

[return to readme](../readme.md)

## Getting entity name

Sometimes you want to get the entity name from its type. The `TryGetEntityName()` extension method, which operates on instances of `DbContext`, does this.

|Argument|Type|Comments|
|--------|----|--------|
|dbContext|`DbContext`|extension method parameter|
|entityType|`Type`|the entity type|
|entityName|`out string?`|will contain the entity name if it was found|

The method returns `true` if the supplied entity type matches an entity type in the `DbContext`, `false` otherwise.

[return to readme](../readme.md)

## Getting PropertyInfo for Single Field Key

The `TryGetEntitySingleFieldPrimaryKeyInfo()` extension method, operating on an instance of `DbContext`, returns a `PropertyInfo` object for an entity's key field, provided the key field
involves only a single property (i.e., is not a complex or compound key).

|Argument|Type|Comments|
|--------|----|--------|
|dbContext|`DbContext`|extension method parameter|
|entityType|`Type`|the entity type|
|keyInfo|`out PropertyInfo?`|will contain a `PropertyInfo` object for the primary key field if the entity has a single field primary key|

The method returns `true` if the entity type has a single field primary key, `false` otherwise.

[return to readme](../readme.md)

## Getting Name of Single Field Key

The `TryGetEntitySingleFieldPrimaryKey()` extension method, operating on an instance of `DbContext`, returns a `string` containing the name of the entity's key field, provided the key field involves only a single property (i.e., is not a complex or compound key).

|Argument|Type|Comments|
|--------|----|--------|
|dbContext|`DbContext`|extension method parameter|
|entityType|`Type`|the entity type|
|keyField|`out string?`|will contain the name of the primary key field if the entity has a single field primary key|

The method returns `true` if the entity type has a single field primary key, `false` otherwise.

[return to readme](../readme.md)
