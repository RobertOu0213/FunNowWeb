﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PrjFunNowWeb.Models;

public partial class HotelLike
{
    public int HotelLikeId { get; set; }

    public int HotelId { get; set; }

    public int MemberId { get; set; }

    public bool LikeStatus { get; set; }

    public virtual Hotel Hotel { get; set; }

    public virtual Member Member { get; set; }
}