# Configuring Entity Framework Classes

Database entities almost always require you configure the database traits of properties (e.g., is that string nullable or not?), relationships, etc.

EF Core lets you do this in at least two ways, one by decorating entity classes and their properties with attributes and one by calling various fluent design configuration methods (you can mix and match, too,
I think).

For various reasons I strongly prefer exclusively using the fluent design approach, rather than the attribute-based approach. But by default that would result in a very large
`OnModelCreating()` method override in my `DbContext`-derived class.

That's messy. So I wrote several classes to allow me to put the necessary fluent design method calls in a class I associate with an entity class. Each entity gets its own configuration class.

Here's an example entity class:

```csharp
public class Campaign
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Goal { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Appeal> Appeals { get; set; }
    public ICollection<Gift> Gifts { get; set; }
    public ICollection<Goal> Goals { get; set; }
    public ICollection<RelatedGift> RelatedGifts { get; set; }
}
```

Next, here's the configurator class for that entity. In it you use EF Core's fluent API to configure how the entity class fits into the database. Note that I tend to declare things explicitly that you can let be defined implicitly (e.g., the single field primary key).

```c#
internal class CampaignConfigurator : EntityConfigurator<Campaign>
{
    protected override void Configure( EntityTypeBuilder<Campaign> builder )
    {
        builder.ToTable( "campaigns" );
        builder.HasKey( e => e.Id ).HasName( "PK_campaigns" );

        builder.Property( e => e.Id ).HasColumnName( "id" ).IsRequired();
        builder.Property( e => e.Name ).HasColumnName( "name" ).HasMaxLength( 50 ).IsRequired();
        builder.Property( e => e.Code ).HasColumnName( "code" ).HasMaxLength( 20 );
        builder.Property( e => e.Description ).HasColumnName( "description" ).HasMaxLength( 100 );
        builder.Property( e => e.Goal ).HasColumnName( "goal" ).HasMaxLength( 50 );
        builder.Property( e => e.StartDate ).HasColumnName( "start_date" );
        builder.Property( e => e.EndDate ).HasColumnName( "end_date" );
        builder.Property( e => e.IsActive ).HasColumnName( "is_active" ).IsRequired();
    }
}
```

The `ICollection` navigation properties are configured in their respective entity classes,
and so are not shown here (but you could, of course, put them here instead).

Finally, you override the  OnModelCreating() method in your DbContext class, register each configurator class to its corresponding entity class, and then call `ConfigureEntities()`.

```c#
protected override void OnModelCreating( ModelBuilder modelBuilder )
{
    var helper = new DbContextConfiguration<ImportDb>( loggerFactory );

    helper.RegisterConfigurator<Campaign, CampaignConfigurator>();

    helper.ConfigureEntities( modelBuilder );
}
```

[return to readme](../readme.md)
