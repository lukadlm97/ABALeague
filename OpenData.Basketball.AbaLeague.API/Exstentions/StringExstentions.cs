using OpenTelemetry.Exporter;

namespace OpenData.Basketball.AbaLeague.API.Exstentions
{
    public static class StringExtensions
    {
        public static OtlpExportProtocol MapProtocolFromConfigs(this string protocolName)
        {
            if (protocolName.ToLower().Equals("grpc", StringComparison.Ordinal))
            {
                return OtlpExportProtocol.Grpc;
            }

            return OtlpExportProtocol.HttpProtobuf;
        }

    }
}
