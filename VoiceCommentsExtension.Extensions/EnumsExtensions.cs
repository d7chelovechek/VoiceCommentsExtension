using System;
using System.Linq;
using System.Reflection;
using VoiceCommentsExtension.Models.Attributes;

namespace VoiceCommentsExtension.Extensions
{
    public static class EnumsExtensions
    {
        public static string GetContentTypeName(this Enum value)
        {
            ContentTypeAttribute[] attributes = GetAttributes(value);

            return attributes?.Any() is true ?
                attributes[0].Name : string.Empty;
        }

        public static string GetContentTypeCommentPattern(this Enum value)
        {
            ContentTypeAttribute[] attributes = GetAttributes(value);

            return attributes?.Any() is true ?
                attributes[0].CommentPattern : string.Empty;
        }

        private static ContentTypeAttribute[] GetAttributes(Enum value) 
        {
            FieldInfo fieldInfo = value
                .GetType()
                .GetField(value.ToString());

            return fieldInfo?.GetCustomAttributes(
                typeof(ContentTypeAttribute), false) as ContentTypeAttribute[];
        }
    }
}