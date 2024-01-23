using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record MissingTeamDto(string? Name,
                                    string? Url,
                                    string? TeamUrl,
                                    string? IncrowdUrl);
}
