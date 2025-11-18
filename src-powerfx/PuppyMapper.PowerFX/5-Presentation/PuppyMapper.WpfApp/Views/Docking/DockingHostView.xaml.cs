using System.Reactive.Disposables;
using System.Windows.Controls;
using ReactiveUI;
using PuppyMapper.WpfApp.ViewModels.Docking;

namespace PuppyMapper.WpfApp.Views.Docking
{
    public partial class DockingHostView : DockingHostViewBase
    {
        public DockingHostView()
        {
            InitializeComponent();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this.Bind(ViewModel, vm => vm.Layout, v => v.DockManager.Layout)
                    .DisposeWith(disposables);
            });
        }
    }
}
