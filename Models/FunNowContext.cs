﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PrjFunNowWeb.Models;

public partial class FunNowContext : DbContext
{
    public FunNowContext()
    {
    }

    public FunNowContext(DbContextOptions<FunNowContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentTravelerType> CommentTravelerTypes { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Dialog> Dialogs { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<HotelEquipment> HotelEquipments { get; set; }

    public virtual DbSet<HotelEquipmentReference> HotelEquipmentReferences { get; set; }

    public virtual DbSet<HotelImage> HotelImages { get; set; }

    public virtual DbSet<HotelLike> HotelLikes { get; set; }

    public virtual DbSet<HotelSearchBox> HotelSearchBoxes { get; set; }

    public virtual DbSet<HotelType> HotelTypes { get; set; }

    public virtual DbSet<ImageCategory> ImageCategories { get; set; }

    public virtual DbSet<ImageCategoryReference> ImageCategoryReferences { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<RatingScore> RatingScores { get; set; }

    public virtual DbSet<ReportReview> ReportReviews { get; set; }

    public virtual DbSet<ReportSubtitle> ReportSubtitles { get; set; }

    public virtual DbSet<ReportTitle> ReportTitles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomEquipment> RoomEquipments { get; set; }

    public virtual DbSet<RoomEquipmentReference> RoomEquipmentReferences { get; set; }

    public virtual DbSet<RoomImage> RoomImages { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<TravelerType> TravelerTypes { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=FunNow5;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName).IsRequired();
            entity.Property(e => e.CountryId).HasColumnName("CountryID");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK_Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentStatus).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Comments)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Hotel");

            entity.HasOne(d => d.Member).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Member");
        });

        modelBuilder.Entity<CommentTravelerType>(entity =>
        {
            entity.ToTable("CommentTravelerType");

            entity.Property(e => e.CommentTravelerTypeId).HasColumnName("CommentTravelerTypeID");
            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.TravelerTypeId).HasColumnName("TravelerTypeID");

            entity.HasOne(d => d.Comment).WithMany(p => p.CommentTravelerTypes)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentTravelerType_Comments");

            entity.HasOne(d => d.TravelerType).WithMany(p => p.CommentTravelerTypes)
                .HasForeignKey(d => d.TravelerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommentTravelerType_TravelerTypes");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CountryName).IsRequired();
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK_HotelCoupon");

            entity.ToTable("Coupon");

            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.CouponDescription).HasMaxLength(150);
            entity.Property(e => e.CouponName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Dialog>(entity =>
        {
            entity.ToTable("Dialog");

            entity.Property(e => e.DialogId).HasColumnName("DialogID");
            entity.Property(e => e.CalltoMemberId).HasColumnName("CalltoMemberID");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Detail)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Member).WithMany(p => p.Dialogs)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dialog_Member");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.ToTable("Hotel");

            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.HotelAddress).IsRequired();
            entity.Property(e => e.HotelDescription).IsRequired();
            entity.Property(e => e.HotelName).IsRequired();
            entity.Property(e => e.HotelPhone).IsRequired();
            entity.Property(e => e.HotelTypeId).HasColumnName("HotelTypeID");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .HasColumnName("isActive");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.City).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_City");

            entity.HasOne(d => d.HotelType).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.HotelTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_HotelType");

            entity.HasOne(d => d.Member).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_Member");
        });

        modelBuilder.Entity<HotelEquipment>(entity =>
        {
            entity.HasKey(e => e.HotelEquipmentId).HasName("PK_Equipment");

            entity.ToTable("HotelEquipment");

            entity.Property(e => e.HotelEquipmentId).HasColumnName("HotelEquipmentID");
            entity.Property(e => e.HotelEquipmentName).IsRequired();
        });

        modelBuilder.Entity<HotelEquipmentReference>(entity =>
        {
            entity.HasKey(e => e.HotelEquipmentReferenceId).HasName("PK_Hotel_Equipment");

            entity.ToTable("Hotel_Equipment_Reference");

            entity.Property(e => e.HotelEquipmentReferenceId).HasColumnName("HotelEquipmentReferenceID");
            entity.Property(e => e.HotelEquipmentId).HasColumnName("HotelEquipmentID");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");

            entity.HasOne(d => d.HotelEquipment).WithMany(p => p.HotelEquipmentReferences)
                .HasForeignKey(d => d.HotelEquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_Equipment_Reference_HotelEquipment");

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelEquipmentReferences)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_Equipment_Reference_Hotel");
        });

        modelBuilder.Entity<HotelImage>(entity =>
        {
            entity.HasKey(e => e.HotelImageId).HasName("PK_HotelImages");

            entity.ToTable("HotelImage");

            entity.Property(e => e.HotelImageId).HasColumnName("HotelImageID");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.HotelImage1)
                .IsRequired()
                .HasColumnName("HotelImage");

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelImages)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HotelImage_Hotel");
        });

        modelBuilder.Entity<HotelLike>(entity =>
        {
            entity.Property(e => e.HotelLikeId).HasColumnName("HotelLikeID");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelLikes)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HotelLikes_Hotel");

            entity.HasOne(d => d.Member).WithMany(p => p.HotelLikes)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HotelLikes_Member");
        });

        modelBuilder.Entity<HotelSearchBox>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HotelSearchBox");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName).IsRequired();
            entity.Property(e => e.CommentText).IsRequired();
            entity.Property(e => e.CommentTitle).IsRequired();
            entity.Property(e => e.CountryName).IsRequired();
            entity.Property(e => e.HotelAddress).IsRequired();
            entity.Property(e => e.HotelDescription).IsRequired();
            entity.Property(e => e.HotelEquipmentId).HasColumnName("HotelEquipmentID");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.HotelName).IsRequired();
            entity.Property(e => e.HotelPhone).IsRequired();
            entity.Property(e => e.HotelPrice).HasColumnType("decimal(38, 6)");
            entity.Property(e => e.HotelTypeId).HasColumnName("HotelTypeID");
            entity.Property(e => e.HotelTypeName).IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.RoomEquipmentId).HasColumnName("RoomEquipmentID");
        });

        modelBuilder.Entity<HotelType>(entity =>
        {
            entity.ToTable("HotelType");

            entity.Property(e => e.HotelTypeId).HasColumnName("HotelTypeID");
            entity.Property(e => e.HotelTypeName).IsRequired();
        });

        modelBuilder.Entity<ImageCategory>(entity =>
        {
            entity.ToTable("ImageCategory");

            entity.Property(e => e.ImageCategoryId).HasColumnName("ImageCategoryID");
            entity.Property(e => e.ImageCategoryName).IsRequired();
        });

        modelBuilder.Entity<ImageCategoryReference>(entity =>
        {
            entity.ToTable("ImageCategory_Reference");

            entity.Property(e => e.ImageCategoryReferenceId).HasColumnName("ImageCategoryReferenceID");
            entity.Property(e => e.HotelImageId).HasColumnName("HotelImageID");
            entity.Property(e => e.ImageCategoryId).HasColumnName("ImageCategoryID");
            entity.Property(e => e.RoomImageId).HasColumnName("RoomImageID");

            entity.HasOne(d => d.HotelImage).WithMany(p => p.ImageCategoryReferences)
                .HasForeignKey(d => d.HotelImageId)
                .HasConstraintName("FK_ImageCategory_Reference_HotelImage");

            entity.HasOne(d => d.ImageCategory).WithMany(p => p.ImageCategoryReferences)
                .HasForeignKey(d => d.ImageCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImageCategory_Reference_ImageCategory");

            entity.HasOne(d => d.RoomImage).WithMany(p => p.ImageCategoryReferences)
                .HasForeignKey(d => d.RoomImageId)
                .HasConstraintName("FK_ImageCategory_Reference_RoomImage");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.RoleId)
                .HasDefaultValue(1)
                .HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Members)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Role");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CouponId).HasColumnName("CouponID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");
            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK_Order_Coupon");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderStatus");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_PaymentStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK_HotelOrderDetails");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.CheckInDate).HasColumnType("date");
            entity.Property(e => e.CheckOutDate).HasColumnType("date");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsOrdered).HasColumnName("isOrdered");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Member).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Member");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetails_Order");

            entity.HasOne(d => d.Room).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Room");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus");

            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.ToTable("PaymentStatus");

            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.CouponId).HasColumnName("CouponID");

            entity.HasOne(d => d.Coupon).WithMany(p => p.PaymentStatuses)
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK_PaymentStatus_Coupon");
        });

        modelBuilder.Entity<RatingScore>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.CleanlinessScore).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.ComfortScore).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.FacilitiesScore).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.FreeWifiScore).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.LocationScore).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.StaffScore).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.TravelerType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.ValueScore).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.Comment).WithMany(p => p.RatingScores)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingScores_Comments");

            entity.HasOne(d => d.Room).WithMany(p => p.RatingScores)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RatingScores_Room");
        });

        modelBuilder.Entity<ReportReview>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ReportReason).IsRequired();
            entity.Property(e => e.ReportSubtitleId).HasColumnName("ReportSubtitleID");
            entity.Property(e => e.ReportTitleId).HasColumnName("ReportTitleID");
            entity.Property(e => e.ReportedAt).HasColumnType("datetime");
            entity.Property(e => e.ReviewStatus)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.ReviewedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Comment).WithMany(p => p.ReportReviews)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportReviews_Comments");

            entity.HasOne(d => d.Member).WithMany(p => p.ReportReviews)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportReviews_Member");

            entity.HasOne(d => d.ReportSubtitle).WithMany(p => p.ReportReviews)
                .HasForeignKey(d => d.ReportSubtitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportReviews_ReportSubtitles");

            entity.HasOne(d => d.ReportTitle).WithMany(p => p.ReportReviews)
                .HasForeignKey(d => d.ReportTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportReviews_ReportTitles");
        });

        modelBuilder.Entity<ReportSubtitle>(entity =>
        {
            entity.Property(e => e.ReportSubtitleId).HasColumnName("ReportSubtitleID");
            entity.Property(e => e.ReportSubtitle1)
                .IsRequired()
                .HasColumnName("ReportSubtitle");
            entity.Property(e => e.ReportTitleId).HasColumnName("ReportTitleID");

            entity.HasOne(d => d.ReportTitle).WithMany(p => p.ReportSubtitles)
                .HasForeignKey(d => d.ReportTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReportSubtitles_ReportTitles");
        });

        modelBuilder.Entity<ReportTitle>(entity =>
        {
            entity.Property(e => e.ReportTitleId).HasColumnName("ReportTitleID");
            entity.Property(e => e.ReportTitle1)
                .IsRequired()
                .HasColumnName("ReportTitle");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK_RoomType");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.HotelId).HasColumnName("HotelID");
            entity.Property(e => e.MaximumOccupancy).HasDefaultValue(2);
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.RoomName).IsRequired();
            entity.Property(e => e.RoomPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomSize).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoomStatus).HasDefaultValue(true);
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Hotel");

            entity.HasOne(d => d.Member).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Member");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_RoomType");
        });

        modelBuilder.Entity<RoomEquipment>(entity =>
        {
            entity.ToTable("RoomEquipment");

            entity.Property(e => e.RoomEquipmentId).HasColumnName("RoomEquipmentID");
            entity.Property(e => e.RoomEquipmentName).IsRequired();
        });

        modelBuilder.Entity<RoomEquipmentReference>(entity =>
        {
            entity.ToTable("Room_Equipment_Reference");

            entity.Property(e => e.RoomEquipmentReferenceId).HasColumnName("RoomEquipmentReferenceID");
            entity.Property(e => e.RoomEquipmentId).HasColumnName("RoomEquipmentID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.RoomEquipment).WithMany(p => p.RoomEquipmentReferences)
                .HasForeignKey(d => d.RoomEquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Equipment_Reference_RoomEquipment");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomEquipmentReferences)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Equipment_Reference_Room");
        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.ToTable("RoomImage");

            entity.Property(e => e.RoomImageId).HasColumnName("RoomImageID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.RoomImage1)
                .IsRequired()
                .HasColumnName("RoomImage");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomImages)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomImage_Room");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.RoomTypeId).HasName("PK_RoomType2");

            entity.ToTable("RoomType");

            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.RoomTypeName).IsRequired();
        });

        modelBuilder.Entity<TravelerType>(entity =>
        {
            entity.Property(e => e.TravelerTypeId).HasColumnName("TravelerTypeID");
            entity.Property(e => e.TravelerType1)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("TravelerType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}