﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PrjFunNowWeb.Models;

public partial class HotelEquipment
{
    public int HotelEquipmentId { get; set; }

    public string HotelEquipmentName { get; set; }

    public virtual ICollection<HotelEquipmentReference> HotelEquipmentReferences { get; set; } = new List<HotelEquipmentReference>();
}