using System.Windows.Controls;
using VoiceCommentsExtension.ViewModels;

namespace VoiceCommentsExtension.Views
{
    public partial class VoiceCommentView : UserControl
    {
        public VoiceCommentViewModel ViewModel { get; private set; }

        public VoiceCommentView()
        {
            InitializeComponent();

            ViewModel = DataContext as VoiceCommentViewModel;
        }
    }
}