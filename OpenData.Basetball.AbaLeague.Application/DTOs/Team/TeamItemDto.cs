using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record TeamItemDto(int Id, 
                                string Name, 
                                string ShortName, 
                                int? CountryId, 
                                string CountryName);
}
