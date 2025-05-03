using Arkano.Common.Models;
using AutoMapper;

namespace Arkano.Transaction.Application.Transaction.Mappings
{
    class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
                CreateMap<Domain.Entities.Transaction, Application.Transaction.Dto.TransactionDto>()
                .ReverseMap();

                CreateMap<Domain.Entities.Transaction, TransactionEvent>().ReverseMap();
        }
    }
}
