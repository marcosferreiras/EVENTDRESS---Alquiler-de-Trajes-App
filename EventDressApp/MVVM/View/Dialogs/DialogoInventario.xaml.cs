using System.Windows;
using EventDressApp.MVVM.Model;
using EventDressApp.MVVM.ViewModel.DialogViewModels;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class DialogoInventario : Window
    {
        public DialogoInventario(DETALLE_MODELO traje = null)
        {
            InitializeComponent();
            DataContext = new DialogoInventarioViewModel(traje);
            Owner = Application.Current.MainWindow;
        }
    }
}