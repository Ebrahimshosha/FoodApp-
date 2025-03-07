﻿namespace FoodApp.Api.VerticalSlicing.Features.Categories.ViewCategory;

public class MapperProfile :Profile
{
    public MapperProfile()
    {
        CreateMap<ViewCategoryResponse, Category>().ReverseMap();
        CreateMap<Recipe, RecipesNamesToReturnDto>();
    }
}