using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Converters
{
    public class TimeOnlyJsonConverter : System.Text.Json.Serialization.JsonConverter<TimeOnly>
    {
        private const string TimeFormat = "HH:mm";

        public override TimeOnly Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, TimeOnly value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(TimeFormat));
        }
    }
}
