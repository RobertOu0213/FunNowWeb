﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PrjFunNowWeb.Models;

public partial class HotelSearchBox
{
    public int HotelId { get; set; }

    public string HotelName { get; set; }

    public string HotelAddress { get; set; }

    public string HotelPhone { get; set; }

    public string HotelDescription { get; set; }

    public int? LevelStar { get; set; }

    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public bool? IsActive { get; set; }

    public int MemberId { get; set; }

    public string CommentTitle { get; set; }

    public string CommentText { get; set; }

    public string CountryName { get; set; }

    public string HotelEquipmentName { get; set; }

    public string HotelImage { get; set; }

    public decimal? HotelPrice { get; set; }

    public int? HotelMaximumOccupancy { get; set; }
}