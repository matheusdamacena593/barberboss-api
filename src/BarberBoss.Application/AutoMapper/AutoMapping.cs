using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Entities.Billing;

namespace BarberBoss.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, config => config.Ignore());

            CreateMap<RequestBillingJson, Billing>();
        }

        private void EntityToResponse()
        {
            CreateMap<Billing, ResponseBillingJson>();
            CreateMap<User, ResponseRegisteredUserJson>();
            CreateMap<User, ResponseUserProfileJson>();
        }
    }
}
