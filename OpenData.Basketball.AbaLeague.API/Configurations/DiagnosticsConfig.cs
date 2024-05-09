using System.Diagnostics;

namespace OpenData.Basketball.AbaLeague.API.Configurations
{

    public static class DiagnosticsConfig
    {
        public const string ServiceName = "BasketballAnalyseService";
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
    }



}
