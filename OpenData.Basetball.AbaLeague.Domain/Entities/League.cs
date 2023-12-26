using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class League : AuditEntity
    {
        public string OfficalName { get; set; }
        public string ShortName { get; set; }
        public string Season { get; set; }
        public string StandingUrl { get; set; }
        public string CalendarUrl { get; set; }
        public string MatchUrl { get; set; }
        public string BoxScoreUrl { get; set; }
        public string? RosterUrl { get; set; }
        public string? BaseUrl { get; set; }
        public virtual Basketball.AbaLeague.Domain.Entities.ProcessorType? ProcessorType { get; set; }
        public short? ProcessorTypeId { get; set; }
        [NotMapped]
        public Basketball.AbaLeague.Domain.Enums.ProcessorType? ProcessorTypeEnum => (Basketball.AbaLeague.Domain.Enums.ProcessorType?) ProcessorTypeId;

        public ICollection<RoundMatch> RoundMatches { get; set; }

    }
}
