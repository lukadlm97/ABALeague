using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Common;

namespace OpenData.Basetball.AbaLeague.Domain.Entities
{
    public class League : AuditEntity
    {
        public string OfficalName { get; set; }
        public string ShortName { get; set; }
        public string Season { get; set; }
        public string TeamParticipansUrl { get; set; }
        public string StandingUrl { get; set; }


    }
}
