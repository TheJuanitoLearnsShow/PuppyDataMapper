using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.WpfApp.ViewModels.Docking;

namespace PuppyMapper.WpfApp.Views.Docking
{
    public partial class DocumentEditorView : UserControl, IViewFor<PuppyMapper.WpfApp.ViewModels.Docking.DocumentEditorViewModel>
    {
        public DocumentEditorView()
        {
            InitializeComponent();
        }

        public PuppyMapper.WpfApp.ViewModels.Docking.DocumentEditorViewModel ViewModel
        {
            get => (PuppyMapper.WpfApp.ViewModels.Docking.DocumentEditorViewModel)DataContext!;
            set => DataContext = value;
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PuppyMapper.WpfApp.ViewModels.Docking.DocumentEditorViewModel)value!;
        }
    }
}
