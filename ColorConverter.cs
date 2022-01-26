using System;
using Newtonsoft.Json;
using SFML.Graphics;

namespace KeyOverlay
{
    class ColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            throw new NotImplementedException("don't care didn't ask");
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;

            return Util.ColorFromHex(s);
        }
    }
}