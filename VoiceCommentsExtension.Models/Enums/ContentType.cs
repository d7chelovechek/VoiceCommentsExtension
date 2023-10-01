using VoiceCommentsExtension.Models.Attributes;

namespace VoiceCommentsExtension.Models.Enums
{
    public enum ContentType
    {
        [ContentType("C/C++", "//")]
        C_CPP,
        [ContentType("CSharp", "//")]
        CSharp,
        [ContentType("TypeScript", "//")]
        TypeScript,
        [ContentType("code++.F#", "//")]
        Code_FSharp,
        [ContentType("F#", "//")]
        FSharp,
        [ContentType("SQL Server Tools", "--")]
        SQL,
        [ContentType("Basic", "'")]
        VB
    }
}