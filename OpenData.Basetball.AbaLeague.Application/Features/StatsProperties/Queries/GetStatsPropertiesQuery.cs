﻿using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.StatsProperties;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.StatsProperties.Queries
{
    public class GetStatsPropertiesQuery : IQuery<Maybe<StatsPropertiesDto>>
    {
    }
}
