﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EXE201.Models;

public partial class Payment
{
    public int Id { get; set; }

    public long BookingId { get; set; }

    public long? OrderCode { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public string Items { get; set; }

    public string CancelUrl { get; set; }

    public string ReturnUrl { get; set; }

    public string PaymentLink { get; set; }

    public string TransactionId { get; set; }

    public string PaymentMethod { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Booking Booking { get; set; }
}