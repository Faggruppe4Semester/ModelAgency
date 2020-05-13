using System.Linq;
using AutoMapper;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;

namespace ModelsApi.Utilities
{
    // Read: Getting Started with AutoMapper in ASP.NET Core
    // at https://code-maze.com/automapper-net-core/
    //
    public class ModelsProfile : Profile
    {
        public ModelsProfile()
        {
            // CreateMap<FromType, ToType>();
            CreateMap<ModelDetails, EfModel>().PreserveReferences();
            CreateMap<EfModel, ModelDetails>().PreserveReferences();
            CreateMap<Job, EfJob>().PreserveReferences();
            CreateMap<EfJob, Job>()
                .ForMember(j => j.Models, opt => opt.MapFrom(ej => ej.JobModels.Select(jm => jm.Model)))
                .ForMember(j => j.JobId, opt => opt.MapFrom(ej => ej.EfJobId))
                .PreserveReferences();

            CreateMap<EfModel, Model>().PreserveReferences();
            CreateMap<NewJob, EfJob>().PreserveReferences();
            CreateMap<NewExpense, EfExpense>().PreserveReferences();
            CreateMap<Manager, EfManager>().PreserveReferences();
            CreateMap<EfManager, Manager>().PreserveReferences();
        }
    }
}
