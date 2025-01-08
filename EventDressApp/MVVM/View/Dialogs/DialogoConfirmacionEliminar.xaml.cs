using System.Windows;

namespace EventDressApp.MVVM.View.Dialogos
{
    public partial class DialogoConfirmacionEliminar : Window
    {
        // Definición de la propiedad ConfirmacionExitosa
        public bool ConfirmacionExitosa { get; private set; } = false;

        public DialogoConfirmacionEliminar()
        {
            InitializeComponent();
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si el usuario ha escrito "Eliminar"
            if (ConfirmarEliminarTB.Text == "Eliminar")
            {
                ConfirmacionExitosa = true;  // Se confirma la eliminación
                this.Close();  // Cerrar la ventana emergente
            }
            else
            {
                // Si no escribe "Eliminar", mostrar advertencia
                MessageBox.Show("Por favor, escribe 'Eliminar' para confirmar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();  // Cerrar la ventana sin realizar ninguna acción
        }
    }
}
