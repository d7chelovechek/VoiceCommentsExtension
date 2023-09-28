using System;

namespace VoiceCommentsExtension.Models.Attributes
{
    public class ContentTypeAttribute : Attribute
    {
        public string Name { get; protected set; }
        public string CommentPattern { get; protected set; }

        public ContentTypeAttribute(string name, string commentPattern)
        {
            Name = name;
            CommentPattern = commentPattern;
        }
    }
}