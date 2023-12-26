using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.ProcessorType;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.ProcessorTypes.Queries
{
    public class GetProcessorTypeQuery : IQuery<Maybe<IEnumerable<ProcessorTypeDto>>>
    {
        public GetProcessorTypeQuery()
        {
            
        }
    }
}
