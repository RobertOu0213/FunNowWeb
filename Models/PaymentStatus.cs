﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PrjFunNowWeb.Models;

public partial class PaymentStatus
{
    public int PaymentStatusId { get; set; }

    public string PaymentMethod { get; set; }

    public int? CouponId { get; set; }

    public virtual Coupon Coupon { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}