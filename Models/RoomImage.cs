﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PrjFunNowWeb.Models;

public partial class RoomImage
{
    public int RoomImageId { get; set; }

    public string RoomImage1 { get; set; }

    public int RoomId { get; set; }

    public virtual ICollection<ImageCategoryReference> ImageCategoryReferences { get; set; } = new List<ImageCategoryReference>();

    public virtual Room Room { get; set; }
}