# Idea 2.0-alpha

> Just Unit of work implementation with Repository and Domain Queries pattern.

## Unit of Work
Unit of work is a pretty known pattern, so just let me quickly recap what's going on. With a unit of work, you define a block of actions which all will be done after commit (typically in a database) or none of them. 
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

## Repository
The repository isn't, in fact, a Repository pattern, in fact, it is a Data access object pattern. But I've decided to use the term Repository because it's widely known and often interchanged with Data access object pattern.
Repository implemented in Idea can be used only from opened Unit of Work. In case of attempt to access data from not opened Unit of Work, the Repository will throw an exception.
Repository supports only basic CRUD operations over entities, but you're welcome to extend basic operations with your own.
### Working with Repository
The Idea comes with already implemented generic Repository. As it was said in the section with Unit of work; all methods must be called from opened Unit of work. 
Mutating operations (Create, Update, Delete) must be committed in order to be applied.
All repositories need to be registered in DI container to be able to use them as a dependency in services. One way how to register them is by using extension method `.AddRepository<TModelContext, TEntity, TKey>()`, and then you'll be able to resolve repository `IRepository<TEntity, TKey>` as a dependency.

## Domain Queries
Domain queries have one and only one purpose - get data. 
Maybe you're wondering why there is not `Idea.Query` package, why is there only `Idea.Query.EntityFrameworkCore` package? Well, that's simple; the way how to retrieve data is closely related with used technology, so instead of writing any generic approach I've decided to write queries which suit well to Entity Framework Core.
### Working with Domain Queries
Each query inherits from `Query<TEntity, TKey>` and implements abstract method `Task<IReadOnlyCollection<TEntity>> QueryAsync(IDataProvider provider)`. 
With that, you can use the constructor of the query to bootstrap or initialize values used by the query. 
Parameter `provider` of type `IDataProvider` allows you to retrieve data for a given generic type. 
This type is not tied with `TEntity` of the query, because a lot of times you need to retrieve data from several different tables and then do a final query.

## Entities
The Idea brings two types of entities. 
The simplest entity is `IEntity<TKey>` which only defines a primary key of type `TKey`. 
This is a must in all parts of Idea.
The more clever entity is `IRecord<TKey>` which inherits from `IEntity<TKey>` and extends it with properties used to track who and when creates an entity, who and when entity modified as last and who and when entity removes.
For correct tracking identity key you need to implement `IIdentityIdentifier<TKey>` interface and register your implementation to DI container; for example by using `AddIdentityIdentifier<TClass, TKey>()` extension method.

## Model Context
The Idea for Entity Framework Core cames with the idea of extended `DbContext` so-called `ModelContext`. The model context requires to define remove strategy. The Idea can mark entities as removed if an entity implements `IRecord<TKey>` interface or completely remove entity as it would be done in Entity Framework.
Next thing what Model context is handling is filtering out entities which were marked as deleted. With that, there is a limitation to define your model in protected `DbModel(ModelBuilder builder)` instead of `OnModelCreating()` method.

## Idea metapackage
The Idea brings Entity Framework Core implementation as one meta-package called `Idea.NetCore.EntityFrameworkCore`.
This is an implementation for Entity Framework Core for .NET Core with Dependency Injection.
The package also contains helper extension methods which make the code cleaner and readable.
For example, following snippet of code,

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

, can be simplified with 

```csharp
public Task<Entity> CreateEntityAsync(Entity entity) => _factory.With(_repository).CreateAndCommitAsync(entity);
```

## Sample project
A sample project is part of the repository. It's a simple Cookbook, but in that sample, you can find how to use the Idea and also sample Unit tests. 