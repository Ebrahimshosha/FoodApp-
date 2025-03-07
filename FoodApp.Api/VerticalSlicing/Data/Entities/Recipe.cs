﻿namespace FoodApp.Api.VerticalSlicing.Data.Entities;

public class Recipe : BaseEntity
{
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<RecipeDiscount> RecipeDiscounts { get; set; } = new List<RecipeDiscount>();
    public ICollection<FavouriteRecipe> FavouriteByUsers { get; set; } = new List<FavouriteRecipe>();
    public ICollection<RecipeRating> RecipeRatings { get; set; } = new List<RecipeRating>();

}
