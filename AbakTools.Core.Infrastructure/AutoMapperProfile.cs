using AbakTools.Core.Domain.Enova.Customer;
using AutoMapper;
using EnovaApi.Models.Customer;

namespace AbakTools.Core.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, EnovaCustomer>();
        }
    }
}
