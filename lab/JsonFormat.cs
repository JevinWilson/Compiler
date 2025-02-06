// used chat for this
// Prompt: (a long back and forth series of how to get "line" before "lexeme" in the json output)
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lab
{
    public class TokenConverter : JsonConverter<Token>
    {
        public override Token Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Not needed for serialization
        }

        public override void Write(Utf8JsonWriter writer, Token token, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("sym", token.sym);
            writer.WriteNumber("line", token.line);
            writer.WriteString("lexeme", token.lexeme.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n"));
            writer.WriteEndObject();
        }
    }
}