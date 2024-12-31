using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EventDressApp.MVVM.Model;
using EventDressApp.MVVM.ViewModel.DialogViewModels;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class DialogoCliente : Window
    {
        public DialogoCliente(Clientes cliente = null)
        {
            InitializeComponent();
            DataContext = new DialogoClienteViewModel(cliente);
            Owner = Application.Current.MainWindow;
        }
    }
}
