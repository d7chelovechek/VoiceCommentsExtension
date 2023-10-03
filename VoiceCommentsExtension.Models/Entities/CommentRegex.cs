using System.Text.RegularExpressions;

namespace VoiceCommentsExtension.Models
{
    public class CommentRegex
    {
        public Regex Indent { get; set; }
        public Regex FileName { get; set; }

        public CommentRegex(string commentPattern)
        {
            Indent = new Regex($"{commentPattern} ", RegexOptions.Compiled);
            FileName = new Regex(@"<voice-comment:(.*?)\/>", RegexOptions.Compiled);
        }
    }
}