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
        public int? RoundsToPlay { get; set; }
        public ICollection<RoundMatch> RoundMatches { get; set; }
        public ICollection<ResourceSelector> ResourceSelectors { get; set; }
        public virtual Season Season { get; set; }
        public int SeasonId { get; set; }
        public virtual Basketball.AbaLeague.Domain.Entities.CompetitionOrganization? CompetitionOrganization { get; set; }
        public short? CompetitionOrganizationId { get; set; }
        [NotMapped]
        public Basketball.AbaLeague.Domain.Enums.CompetitionOrganizationEnum? CompetitionOrganizationEnumEnum => (Basketball.AbaLeague.Domain.Enums.CompetitionOrganizationEnum?) CompetitionOrganizationId;
    }
}
