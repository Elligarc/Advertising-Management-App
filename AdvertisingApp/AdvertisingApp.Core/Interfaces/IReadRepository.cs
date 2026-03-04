using Ardalis.Specification;

namespace AdvertisingApp.Core.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}