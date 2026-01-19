using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Events
{
    public record OutOfStockEvent(int ProductId, string ProductName, string VariantName) : INotification;
}
