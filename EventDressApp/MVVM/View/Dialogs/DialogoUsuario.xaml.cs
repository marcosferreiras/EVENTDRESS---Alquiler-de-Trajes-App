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

namespace EventDressApp.MVVM.View.Dialogs
{
    /// <summary>
    /// Lógica de interacción para DialogoUsuario.xaml
    /// </summary>
    public partial class DialogoUsuario : Window
    {
        public DialogoUsuario()
        {
            InitializeComponent();
        }

        public DialogoUsuario(Model.Usuario usuarioSeleccionado)
        {
            InitializeComponent();
        }
    }
}
