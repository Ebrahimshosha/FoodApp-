﻿namespace FoodApp.Api.VerticalSlicing.Data.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = null!;
    public bool IsDefault { get; set; } = false;
}
