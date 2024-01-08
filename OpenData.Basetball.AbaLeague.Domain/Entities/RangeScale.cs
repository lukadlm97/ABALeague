using OpenData.Basetball.AbaLeague.Domain.Common;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Domain.Entities
{
    public class RangeScale : BaseEntity
    {
        public int? MinValue { get; set; } = null;
        public int? MaxValue { get; set; } = null;

        public virtual StatsProperty StatsProperty { get; set; }
        public short StatsPropertyId { get; set; }
        [NotMapped]
        public StatsPropertyEnum StatsPropertyEnum => (StatsPropertyEnum) StatsPropertyId;

        public virtual LevelOfScale LevelOfScale { get; set; }
        public short LevelOfScaleId { get; set; }
        [NotMapped]
        public LevelOfScaleEnum LevelOfScaleEnum => (LevelOfScaleEnum)LevelOfScaleId;

        public virtual GameLength GameLength { get; set; }
        public short GameLengthId { get; set; }
        [NotMapped]
        public GameLengthEnum GameLengthIdEnum => (GameLengthEnum) GameLengthId;
    }
}
