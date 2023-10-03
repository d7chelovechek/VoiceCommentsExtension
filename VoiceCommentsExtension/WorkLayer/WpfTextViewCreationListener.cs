using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace VoiceCommentsExtension.WorkLayer
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ 
        ContentType("code"),
        ContentType("CSharp"),
        ContentType("C/C++"),
        ContentType("Basic"),
        ContentType("code++.F#"),
        ContentType("F#"),
        ContentType("JScript"),
        ContentType("Python"),
        ContentType("XML"),
        ContentType("SQL Server Tools")
    ]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class WpfTextViewCreationListener : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("VoiceCommentsLayer")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition EditorAdornmentLayer { get; set; }

        public IClassificationFormatMapService ClassificationFormatMap { get; private set; }
        public IClassificationTypeRegistryService ClassificationTypeRegistry { get; private set; }

        [ImportingConstructor]
        public WpfTextViewCreationListener(
            IClassificationFormatMapService classificationFormatMapService, 
            IClassificationTypeRegistryService classificationRegistry)
        {
            ClassificationFormatMap = classificationFormatMapService;
            ClassificationTypeRegistry = classificationRegistry;
        }

        public void TextViewCreated(IWpfTextView textView)
        {
            VoiceCommentsLayer layer = textView.Properties.GetOrCreateSingletonProperty(
                "VoiceCommentsLayer", 
                () => new VoiceCommentsLayer(textView));

            layer.ClassificationFormatMap = ClassificationFormatMap;
            layer.ClassificationTypeRegistry = ClassificationTypeRegistry;
        }
    }
}