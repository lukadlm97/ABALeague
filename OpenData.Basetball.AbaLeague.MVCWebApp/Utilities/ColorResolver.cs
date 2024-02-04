using OpenData.Basetball.AbaLeague.Domain.Enums;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Utilities
{
    public static class ColorResolver
    {
        public static string ConvertPositionEnumToColor(this PositionEnum position)
        {
            return position switch
            {
                Domain.Enums.PositionEnum.Guard => "#F4F269",
                Domain.Enums.PositionEnum.ShootingGuard => "#CEE26B",
                Domain.Enums.PositionEnum.Forward => "#A8D26D",
                Domain.Enums.PositionEnum.PowerForward => "#82C26E",
                Domain.Enums.PositionEnum.Center => "#5CB270",
                _ => "#5E3719"
            };
        }
    }
}
