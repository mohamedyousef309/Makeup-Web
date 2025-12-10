
using Domain_Layer.Interfaces.ServiceInterfaces;
using Infastructure_Layer;
using MediatR;
using System.Windows.Input;

namespace Makeup_Web.Middlewares
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand
    {
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
