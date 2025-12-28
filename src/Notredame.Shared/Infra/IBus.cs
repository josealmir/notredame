using OperationResult;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;

namespace Notredame.Shared.Infra;

public interface IQueryBus<T> : IQuery<Result<T>>;

public interface ICommandBus<T> : ICommand<Result<T>>; 
