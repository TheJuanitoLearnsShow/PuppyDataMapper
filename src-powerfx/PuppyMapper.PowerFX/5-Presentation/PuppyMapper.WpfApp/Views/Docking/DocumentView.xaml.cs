using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.WpfApp.ViewModels.Docking;

namespace PuppyMapper.WpfApp.Views.Docking
{
    public partial class DocumentView : UserControl, IViewFor<PuppyMapper.WpfApp.ViewModels.Docking.DocumentViewModel>
    {
        public DocumentView()
        {
            InitializeComponent();
        }

        public PuppyMapper.WpfApp.ViewModels.Docking.DocumentViewModel ViewModel
        {
            get => (PuppyMapper.WpfApp.ViewModels.Docking.DocumentViewModel)DataContext!;
            set => DataContext = value;
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PuppyMapper.WpfApp.ViewModels.Docking.DocumentViewModel)value!;
        }
    }
}
