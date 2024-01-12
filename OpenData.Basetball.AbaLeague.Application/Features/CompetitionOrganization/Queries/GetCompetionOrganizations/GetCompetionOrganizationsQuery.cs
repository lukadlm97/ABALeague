using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.CompetionOrganization;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.CompetitionOrganization.Queries.GetCompetionOrganizations
{
    public class GetCompetionOrganizationsQuery : IQuery<Maybe<CompetionOrganizationDto>>
    {
        public GetCompetionOrganizationsQuery()
        {
            
        }
    }
}
