using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models;
using Portfolio.ViewModels;

namespace Portfolio.ConfigServices
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Models.Portfolio, CreatePortfolioViewModel>().ReverseMap()
                .ForMember(a => a.Image, b => b.ResolveUsing(a => a.Image.FileName));
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Models.Portfolio, PortfolioPathViewModel>().ReverseMap();
            
        }

        private void AfterMap(Func<object, object, object> p)
        {
            throw new NotImplementedException();
        }
    }

    public static class MapperConfigService
    {
        public static IServiceCollection RegisterMapper(this IServiceCollection services)
        {
            services.AddAutoMapper();

            return services;
        }
    }
}
