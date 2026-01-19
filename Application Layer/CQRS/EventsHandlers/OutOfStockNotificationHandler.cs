using Domain_Layer.DTOs;
using Domain_Layer.Events;
using Domain_Layer.Interfaces.ServiceInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.EventsHandlers
{
    public class OutOfStockNotificationHandler : INotificationHandler<OutOfStockEvent>
    {
        private readonly IEMailService EMailService;

        public OutOfStockNotificationHandler(IEMailService EMailService)
        {
            this.EMailService = EMailService;
        }
        public async Task Handle(OutOfStockEvent notification, CancellationToken cancellationToken)
        {
            var email = new EmailDTo
            {
                To = "kahled.pop.2003@gmail.com",
                Subject = "Product Out of Stock Alert",
                Body = $"The product with ID {notification.ProductId} and Name{notification.ProductName}-{notification.VariantName} is out of stock."

            };

            await EMailService.SendEmail(email);
        }
    }
}
