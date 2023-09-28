using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VoiceCommentsExtension.Extensions;
using VoiceCommentsExtension.Models;
using VoiceCommentsExtension.Models.Enums;

namespace VoiceCommentsExtension.Services
{
    public static class CommentsService
    {
        public static Dictionary<string, string> SupportedContentTypes { get; set; }

        private static readonly Dictionary<string, CommentRegex> _parsers;

        static CommentsService()
        {
            SupportedContentTypes = new Dictionary<string, string>();
            _parsers = new Dictionary<string, CommentRegex>();

            foreach (ContentType contentType in Enum.GetValues(typeof(ContentType)))
            {
                string name = contentType.GetContentTypeName();
                string pattern = contentType.GetContentTypeCommentPattern();

                SupportedContentTypes.Add(name, pattern);
                _parsers.Add(name, new CommentRegex(pattern));
            }
        }

        public static int TryMatch(
            string contentTypeName, 
            string voiceComment, 
            string fileNameComment, 
            out string filePath)
        {
            if (voiceComment.Contains("<voice-comment>") && 
                _parsers.TryGetValue(contentTypeName, out CommentRegex regex))
            {
                voiceComment = voiceComment.Split(
                    new[] { "\r\n", "\r" }, 
                    StringSplitOptions.None)[0];
                fileNameComment = fileNameComment.Split(
                    new[] { "\r\n", "\r" }, 
                    StringSplitOptions.None)[0];

                Match indentMatch = regex.Indent.Match(voiceComment);
                Match fileNameMatch = regex.FileName.Match(fileNameComment);

                if (indentMatch.Success && fileNameMatch.Success)
                {
                    filePath = $"{VisualStudioService.GetVoicesDirectory()}\\{fileNameMatch.Value}";

                    return indentMatch.Index + indentMatch.Length;
                }
            }

            filePath = string.Empty;

            return -1;
        }
    }
}