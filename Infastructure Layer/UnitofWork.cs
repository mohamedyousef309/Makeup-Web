using Domain_Layer.Interfaces.ServiceInterfaces;
using Infastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer
{
    public class UnitofWork : IunitofWork
    {
        private readonly AppDbContext _dbContext;

        public IDbContextTransaction? _Transaction { get; set; }


        public UnitofWork(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;

        }
        public async Task BeginTransactionAsync()
        {
            if (_Transaction != null)
            {
                return;
            }
            _Transaction = await _dbContext.Database.BeginTransactionAsync();

        }

        public async Task CommitTransactionAsync()
        {
            if (_Transaction != null)
            {
                await _Transaction.CommitAsync();
                await _Transaction.DisposeAsync();
                _Transaction = null;
            }
        }

        public void Dispose()
        {
            _Transaction?.Dispose();
            _dbContext.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_Transaction!=null)
            {
                await _Transaction.RollbackAsync();
                await _Transaction.DisposeAsync();
                _Transaction = null;

            }
        }
    }
}
