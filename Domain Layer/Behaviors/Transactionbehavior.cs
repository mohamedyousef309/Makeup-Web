
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace Domain_Layer.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand
    {
        private readonly IunitofWork unitofWork;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> logger;

        public TransactionBehavior(IunitofWork UnitofWork, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            unitofWork = UnitofWork;
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                await unitofWork.BeginTransactionAsync();

                var response = await next();

                if (response is IRequestResponse result && !result.IsSuccess)
                {
                    await unitofWork.RollbackTransactionAsync();
                }
                else
                {
                    await unitofWork.CommitTransactionAsync();
                }
                return response;

            }
            catch (Exception ex)
            {

                await unitofWork.RollbackTransactionAsync();
                logger.LogError(ex, "Transaction failed for {Request}", typeof(TRequest).Name);

                throw; 

            }

        }
    }
}
