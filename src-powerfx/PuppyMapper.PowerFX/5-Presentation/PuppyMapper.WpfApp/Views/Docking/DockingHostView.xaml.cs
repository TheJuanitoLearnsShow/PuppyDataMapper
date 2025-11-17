using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.WpfApp.ViewModels.Docking;

namespace PuppyMapper.WpfApp.Views.Docking
{
    public partial class DockingHostView : UserControl, IViewFor<DockingHostViewModel>
    {
        public DockingHostView()
        {
            InitializeComponent();

            // Theme assignment removed — Dirkster.AvalonDock theme types are resolved differently across package versions.
            // We'll apply a Dirkster theme via App.xaml resource dictionaries later.
        }

        public DockingHostViewModel ViewModel
        {
            get => (DockingHostViewModel)DataContext;
            set => DataContext = value;
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (DockingHostViewModel)value;
        }
    }
}
