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
    public partial interface IFunNowContextProcedures
    {
        Task<int> UpdateIsExpiredAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> UpdateOrderStatusIfCheckOutDatePassedAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
