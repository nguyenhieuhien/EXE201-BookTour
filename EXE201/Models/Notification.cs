﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EXE201.Models;

public partial class Notification
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }

    public virtual Account Account { get; set; }
}