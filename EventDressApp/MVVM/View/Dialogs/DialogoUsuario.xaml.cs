using System.Windows;
using EventDressApp.MVVM.Model;
using EventDressApp.MVVM.ViewModel;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class DialogoUsuario : Window
    {
        public DialogoUsuario()
        {
            InitializeComponent();
            DataContext = new DialogoUsuarioViewModel(this);
        }

        public DialogoUsuario(Usuario usuario)
        {
            InitializeComponent();
            DataContext = new DialogoUsuarioViewModel(this)
            {
                Usuario = usuario
            };
        }
    }
}