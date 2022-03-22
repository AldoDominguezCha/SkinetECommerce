using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(dto => dto.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dto => dto.ProductType, opt => opt.MapFrom(src => src.ProductType.Name));
        }
    }
}