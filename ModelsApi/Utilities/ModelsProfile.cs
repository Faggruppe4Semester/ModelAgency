using AutoMapper;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;

namespace ModelsApi.Utilities
{
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            // CreateMap<FromType, ToType>();
            CreateMap<ModelDetails, EfModel>();
            CreateMap<EfModel, ModelDetails>();
            CreateMap<Job, EfJob>();
            CreateMap<EfJob, Job>();
            CreateMap<EfModel, Model>();
            CreateMap<NewJob, EfJob>();
            CreateMap<NewExpense, EfExpense>();
        }
    }
}
