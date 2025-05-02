using Arkano.Common.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
