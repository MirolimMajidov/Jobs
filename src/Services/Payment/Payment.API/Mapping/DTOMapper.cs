using AutoMapper;
using PaymentService.Models;

namespace PaymentService.Mapping
{
    public class DTOMapper : Profile
    {
        public DTOMapper()
        {
            CreateMap<Payment, PaymentDTO>().ReverseMap();
        }
    }
}
