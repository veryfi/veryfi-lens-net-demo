using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

public class CustomJsonFormatter
{
    private readonly Context context;

    public CustomJsonFormatter(Context context)
    {
        this.context = context;
    }

    public SpannableString FormatJsonWithColors(string json)
    {
        var parsedJson = JToken.Parse(json);
        var spannableBuilder = new SpannableStringBuilder();

        void FormatToken(JToken token, string indent = "")
        {
            if (token == null)
            {
                spannableBuilder.Append("null");
                return;
            }

            switch (token.Type)
            {
                case JTokenType.Object:
                    spannableBuilder.Append(indent + "{");
                    bool firstProperty = true;
                    foreach (var property in ((JObject)token).Properties())
                    {
                        if (!firstProperty) spannableBuilder.Append(",");
                        spannableBuilder.Append("\n"); 

                        var keySpan = new SpannableString(indent + "  \"" + property.Name + "\": ");
                        keySpan.SetSpan(new ForegroundColorSpan(Color.Black), 0, keySpan.Length(), SpanTypes.ExclusiveExclusive);
                        spannableBuilder.Append(keySpan);

                        ApplyValueFormatting(property.Value, indent + "");

                        firstProperty = false;
                    }

                    spannableBuilder.Append("\n" + indent + "}");
                    break;

                case JTokenType.Array:
                    spannableBuilder.Append(indent + "[");
                    bool firstItem = true;
                    foreach (var arrayItem in ((JArray)token).Children())
                    {
                        if (!firstItem) spannableBuilder.Append(",");
                        spannableBuilder.Append("\n");

                        ApplyValueFormatting(arrayItem, indent + "");

                        firstItem = false;
                    }

                    spannableBuilder.Append("\n" + indent + "]");
                    break;

                default:
                    ApplyValueFormatting(token, indent);
                    break;
            }
        }

        void ApplyValueFormatting(JToken value, string indent)
        {
            if (value.Type == JTokenType.Object || value.Type == JTokenType.Array)
            {
                FormatToken(value, indent);
            }
            else
            {
                var valueColor = GetValueColor(value);
                var valueString = value.Type == JTokenType.String ? $"\"{value.ToString()}\"" : value.ToString();
                var valueSpan = new SpannableString(indent + valueString);
                valueSpan.SetSpan(new ForegroundColorSpan(valueColor), 0, valueSpan.Length(), SpanTypes.ExclusiveExclusive);
                spannableBuilder.Append(valueSpan);
            }
        }

        Color GetValueColor(JToken value)
        {
            return value.Type switch
            {
                JTokenType.String => Color.ParseColor("#00AA00"), 
                JTokenType.Integer => Color.Red, 
                JTokenType.Float => Color.Orange, 
                JTokenType.Boolean => Color.Blue, 
                _ => Color.Gray,
            };
        }

        FormatToken(parsedJson);

        return new SpannableString(spannableBuilder);
    }
}