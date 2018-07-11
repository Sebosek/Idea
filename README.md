# Idea 2.0-alpha

> Just Unit of work implementation with Repository and Domain Queries pattern.

## Overview
The idea is an implementation of the following design patterns: Unit of Work, Repository and Domain queries with few helper functions which should be handy in daily programming live. The documentation starts with requirements defined by the library, then we're continuing with separated aspects of the Idea and at the end, there is a section which describes helper functions and a brief overview of sample project which is part of Library itself.

## Entities
The Idea brings two built-in types of entities. 
The simplest entity is `IEntity<TKey>` which only defines a primary key of type `TKey`. Your entity must implement `IEntity<TKey>` interface, it's a must. Currently, the Idea supports on simple key, composed keys are not supported as a primary key, but you can create a composed index for the entity.

The more clever entity is `IRecord<TKey>` which inherits from `IEntity<TKey>` and extends it with properties used to track who and when creates an entity, who and when entity modified at last and who and when entity removes.

For correct tracking identity key you need to implement `IIdentityIdentifier<TKey>` interface and register your implementation to DI container by using `AddIdentityIdentifier<TClass, TKey>()` extension method.

### Working with Entities
Those are only build-in entities and you're welcome to create and extend those with your own. With that Idea comes with an idea of entity expands which allows us to manipulate with entities before any mutation operation (mutation operations are created, update and delete).

Entity expand is an implementation of `IEntityExpand<TKey>` which is registered into dependency injection via `AddEntityExpand<TEntityExpand, TKey>` extension method.

For example, if you want to create a tenant entity, you would simply create a base entity class.
```csharp
public class TenantEntity : Entity<Guid>
{
    public Guid TenandId { get; set; }
}
```

Then you need to create an entity expand which will fill-in `TenantId` before the entity is created. Also, do not forget register TenantEntityExpand to DI container via `.AddEntityExpand<TenantEntityExpand, Guid>()`.
```csharp
public class TenantEntityExpand : IEntityExpand<Guid>
{
    private readonly ITenantProvider _provider;

    public TenantEntityExpand(ITenantProvider provider)
    {
        _provider = provider;
    }

    public void BeforeCreate(IEntity<Guid> entity)
    {
        if (!(entity is TenantEntity tenantEntity))
        {
            return;
        }
            
        tenantEntity.TenantId = _provider.TenantIdentity();
    }

    public void BeforeUpdate(IEntity<Guid> entity) // Skip
    {
    }

    public void BeforeRemove(IEntity<Guid> entity) // Skip
    {
    }
}
```

In fact, tracking all information about `IRecord<TKey>` is done via built-in entity expands.

## Model Context
The Idea for Entity Framework Core cames with the idea of extended `DbContext` so-called `ModelContext`. The model context requires to define remove strategy. The Idea can mark entities as removed if an entity implements `IRecord<TKey>` interface or completely remove entity as it would be done in Entity Framework.
Next thing what Model context is handling is filtering out entities which were marked as deleted. With that, there is a limitation to define your model in protected `DbModel(ModelBuilder builder)` instead of `OnModelCreating()` method.

### Working with ModelContext
The Idea brings helper extension methods for configure basic `IEntity<TKey>` classes. Those extension methods are `.Entity<TEntity, TKey>()` and `.Entity<TEntity, TKey>(Action<EntityTypeBuilder<TEntity>> action)` and they simple define the `Id` property as a key.
As well as for configuring `IRecord<TKey>` classes there are `Record<TRecord, TKey>()` and `Record<TRecord, TKey>()` and those extension methods define the `Id` property as key and `Removed` property as an index.
Any other additional configuration can be done via anonymous function defined in the argument.

## Unit of Work
Unit of work is a pretty known pattern, so just let me quickly recap what's going on.

With a unit of work, you define a block of actions which all will be done after commit (typically in a database) or none of them. 

Unit of Work implemented by Idea is based on "cumulative" approach - one unit of work inside different unit of work inside unit of work, all Unit of Works must be committed and data are stored after the last commit.
This is the main difference for example with DevExpress's implementation of Unit of Work.

### Working with Unit of Work
Working with unit of work is really easy; in your `AwesomeService.cs` just require a dependency on `IUnitOfWorkFactory` from DI container and create a unit of work.

```csharp
public async Task<Entity> CreateEntityAsync(Entity entity)
{
    using (var uow = _factory.Create()) // Creates new Unit of Work
    {
        await _repository.CreateAsync(entity).ConfigureAwait(false); // Create an entity thru Repostory
        await uow.CommitAsync().ConfigureAwait(false); // Apply changes

        return entity;
    }
}
```

If you wouldn't call a `.CommitAsync()` an entity wouldn't be created in the store. Also, all unit of works in the chain needs to be committed. All unit of work blocks are rollbacked by calling `Dispose()` or by calling `.RollbackAsync()`.

## Repository
The repository isn't, in fact, a Repository pattern, in fact, it is a Data access object pattern. 

But I've decided to use the term Repository because it's widely known and often interchanged with Data access object pattern.

Repository implemented in the Idea can be used only from an opened unit of work. In case of attempt to access data from not opened Unit of Work, the repository will throw an exception.

Repository supports only basic CRUD operations over entities, but you're welcome to extend base repository class with your own operations, but for a data query I would suggest using domain queries instead of the repository, but it's completely up to you.

### Working with Repositories
The Idea comes with already implemented generic Repository and as was said in the section with a unit of work; all methods must be called from opened Unit of work. 

Mutating operations must be committed in order to be applied.

I would suggest registering all repositories in DI container to be able to use them as a dependency on services. In your code, you can simply use built-in generic repository by registering those repositories in DI container by using extension method `.AddRepository<ModalContext, TEntity, TKey>` where you just specify used `ModelContext`, an entity and the key.


## Domain Queries
Domain queries have one and only one purpose - get data. 

### Working with Domain Queries
Each query must inherits from `Query<TEntity, TKey>` and implements abstract method `Task<IReadOnlyCollection<TEntity>> QueryAsync(IDataProvider provider)`. 

With that, you can use the constructor of the query to bootstrap or initialize values used by the query. 
Parameter `provider` of type `IDataProvider` allows you to retrieve data for a given generic type. 

This type is not tied with `TEntity` of the query, because a lot of times you need to retrieve data from several different tables and then do a final query.

A quick example from the sample project shows how to define a query.
```csharp
public class FetchIngredient : Query<Ingredient, Guid>
{
    private readonly Guid _id;

    public FetchIngredient(Guid id)
    {
        _id = id;
    }

    protected override Task<IReadOnlyCollection<Ingredient>> QueryAsync(IDataProvider provider) =>
        Task.FromResult<IReadOnlyCollection<Ingredient>>(
            provider.Data<Ingredient, Guid>().Where(w => w.Id == _id).Include(i => i.Unit).ToList());
}
```

As you can see, in the constructor we're expecting an id which is used for filtering and also applied an `Include()` method from Entity Framework to load depended `Unit`.

To retrieve data from a query in your service you'll simply create a new instance of a query and call `.ExecuteAsync(_factory)` where `_factory` is an instance of `IUnitOfWorkFactory` class.
```cshaprp
public async Task<Ingredient> GetIngredientAsync(Guid id)
{
    var data = await new FetchIngredient(id).ExecuteAsync(_factory);

    return data.FirstOrDefault();
}
```

Because all queries return a collection of data and this case we need only single entity the Idea comes with built-in extension method `.FetchAsync(_factory)` over queries which allows us to retrieve the single entity.

So previes example would be rewriten to `public Task<Ingredient> GetIngredientAsync(Guid id) => new FetchIngredient(id).FetchAsync(_factory);`.

## Idea metapackage
The Idea brings Entity Framework Core implementation as one meta-package called `Idea.NetCore.EntityFrameworkCore`.

This is an implementation for Entity Framework Core for .NET Core with Dependency Injection.
The package also contains helper extension methods which make the code cleaner and readable. 
Some of them were already presented in the text and the rest of them will be introduced in the next section.

### Use in application
In your application you need to register all classes needed by the Idea into DI, fortunelly the Idea declares extension method `.AddIdea<TDbContext, TKey>(Action<DbContextOptionsBuilder> options)`.

With that is the use of the Idea very simple like it's shown in the upcoming snippet, where you can see the more explicit use of the Idea.
```csharp
public void ConfigureServices(IServiceCollection services) =>
    services
        .AddAutoMapper()
        .AddSwaggerGen(SetSwagger)
        .AddIdea<CookbookModelContext, Guid>(
            a => a.UseSqlServer(Configuration.GetConnectionString("Cookbook")))
        .AddRepository<CookbookModelContext, Unit, Guid>()
        .AddRepository<CookbookModelContext, Ingredient, Guid>()
        .AddRepository<CookbookModelContext, Recipe, Guid>()
        .AddIdentityIdentifier<IdentityIdentifier, Guid>()
        .AddScoped<IUnitService, UnitService>()
        .AddScoped<UnitOrchestration>()
        .AddScoped<IIngredientService, IngredientService>()
        .AddScoped<IngredientOrchestration>()
        .AddScoped<RecipeService>()
        .AddScoped<RecipeOrchestration>()
        .AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
```

### Working with repositories

For repositories, the Idea brings few extension methods, for example creating an entity can be simplified.
```csharp
public async Task<Entity> CreateEntityAsync(Entity entity)
{
    using (var uow = _factory.Create()) // Creates new Unit of Work
    {
        await _repository.CreateAsync(entity).ConfigureAwait(false); // Create an entity thru Repostory
        await uow.CommitAsync().ConfigureAwait(false); // Apply changes

        return entity;
    }
}
```

With more readable form.
```csharp
public Task<Entity> CreateEntityAsync(Entity entity) => _factory.With(_repository).CreateAndCommitAsync(entity);
```

In fact, all mutating operations are supported, so the chain of `_factory.With(_repository)` can continue with

 - `.CreateAndCommitAsync(TEntity entity)` as was already introduced it will create an entity and commits unit of work.
 - `.DeleteAndCommitAsync(TKey key)`, will delete an entity given by key and commit the unit of work
 - `.FindAsync(TKey key)`, will find an entity by a key
 - `.ComulativeUpdateAndCommitAsync(TKey key, params Action<TEntity>[] updates)`, will update an entity with applied array of actions

## Sample project
A sample project is part of the repository. 
It's a simple Cookbook and in that sample, you can find how to use the Idea and also sample Unit tests. 
The sample is just a web API with OpenAPI (Swagger) UI and data are stored in a Microsoft SQL server.
As a data source, you're able to use any provider based on your needs.
