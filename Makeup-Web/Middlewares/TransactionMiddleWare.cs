
using Domain_Layer.Interfaces.ServiceInterfaces;
using Infastructure_Layer;

namespace Makeup_Web.Middlewares
{
    public class TransactionMiddleWare : IMiddleware
    {
        private readonly IunitofWork unitofwork;

        public TransactionMiddleWare(IunitofWork unitofwork)
        {
            this.unitofwork = unitofwork;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (context.Request.Method == HttpMethods.Get)
                {

                    await next(context);

                }
                else 
                {
                    await unitofwork.BeginTransactionAsync();
                    await next(context);
                    await unitofwork.CommitTransactionAsync();
                }



            }
            catch (Exception)
            {

              await  unitofwork.RollbackTransactionAsync();

                throw;

            }

        }
    }
}
