
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public class JsonCamelCaseEnumConverter : JsonConverterAttribute
    {
        public override JsonConverter? CreateConverter(Type typeToConvert)
        {
            return new JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase);
        }
    }
}
