using System;
using System.Linq;

using AutoMapper;

using Idea.Cookbook.Models;

namespace Idea.Cookbook.Mappings
{
    public class CookbookProfile : Profile
    {
        public CookbookProfile()
        {
            // DTO -> Entity
            CreateMap<UnitCreate, Entities.Unit>()
                .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(d => d.Ingredients, opt => opt.Ignore());

            CreateMap<UnitUpdate, Entities.Unit>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Ingredients, opt => opt.Ignore());

            CreateMap<IngredientCreate, Entities.Ingredient>()
                .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(d => d.UnitId, opt => opt.MapFrom(s => s.Unit.Id))
                .ForMember(d => d.Unit, opt => opt.Ignore());

            CreateMap<IngredientUpdate, Entities.Ingredient>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.UnitId, opt => opt.MapFrom(s => s.Unit.Id))
                .ForMember(d => d.Unit, opt => opt.Ignore());

            CreateMap<RecipeCreate, Entities.Recipe>()
                .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(d => d.Ingredients, opt => opt.MapFrom(s => s.Ingredients.Select(e => new Ingredient {Id = e.Id})));

            CreateMap<RecipeUpdate, Entities.Recipe>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Ingredients, opt => opt.MapFrom(s => s.Ingredients.Select(e => new Ingredient { Id = e.Id })));

            // Entity -> DTO
            CreateMap<Entities.Ingredient, Ingredient>()
                .ForMember(d => d.Unit, ops => ops.MapFrom(s => new IdModel { Id = s.UnitId }))
                .ReverseMap();

            CreateMap<Entities.Recipe, Recipe>().ReverseMap();

            CreateMap<Entities.Unit, Unit>().ReverseMap();
        }
    }
}