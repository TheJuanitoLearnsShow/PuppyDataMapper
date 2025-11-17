using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.ViewModels.Inputs;

namespace PuppyMapper.WpfApp.Views
{
    public partial class InputEditorView : UserControl, IViewFor<InputEditorViewModel>
    {
        public InputEditorView()
        {
            InitializeComponent();
        }

        public InputEditorViewModel ViewModel
        {
            get => (InputEditorViewModel)DataContext;
            set => DataContext = value;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (InputEditorViewModel)value;
        }
    }
}

