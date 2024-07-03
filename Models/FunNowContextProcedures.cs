﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrjFunNowWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace PrjFunNowWeb.Models
{
    public partial class FunNowContext
    {
        private IFunNowContextProcedures _procedures;

        public virtual IFunNowContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new FunNowContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IFunNowContextProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class FunNowContextProcedures : IFunNowContextProcedures
    {
        private readonly FunNowContext _context;

        public FunNowContextProcedures(FunNowContext context)
        {
            _context = context;
        }

        public virtual async Task<int> UpdateIsExpiredAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[UpdateIsExpired]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<int> UpdateOrderStatusIfCheckOutDatePassedAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[UpdateOrderStatusIfCheckOutDatePassed]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
