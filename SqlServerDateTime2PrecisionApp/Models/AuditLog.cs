﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SqlServerDateTime2PrecisionApp.Models;

public partial class AuditLog
{
    public int Id { get; set; }

    public string User { get; set; }

    public DateTime? Created { get; set; }
}