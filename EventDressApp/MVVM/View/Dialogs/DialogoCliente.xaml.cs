using System;
using Microsoft.Data.SqlClient;
using System.Windows;
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

        private void CrearCliente(object sender, RoutedEventArgs e)
        {
            // Obtener valores desde los TextBox
            string nombreClienteValue = NombreClienteTB.Text;
            string apellidoClienteValue = ApellidoClienteTB.Text;
            string documentoClienteValue = DocumentoClienteTB.Text;
            string direccionClienteValue = DireccionClienteTB.Text;
            string telefonoClienteValue = TelefonoClienteTB.Text;
            string emailClienteValue = EmailClienteTB.Text;

            // Otros valores fijos o generados
            DateTime fechaUltimoAlquiler = DateTime.Today; // Nuevo cliente, sin alquileres previos
            int totalAlquileres = 0;                       // Nuevo cliente
            string estadoCliente = "Activo";              // Estado inicial

            try
            {
                // Crear los parámetros para el procedimiento almacenado sin ternarios ni verificaciones
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@NombreCliente", nombreClienteValue),
                    new SqlParameter("@ApellidoCliente", apellidoClienteValue),
                    new SqlParameter("@FechaUltimoAlquiler", fechaUltimoAlquiler),
                    new SqlParameter("@TotalAlquileres", totalAlquileres),
                    new SqlParameter("@DocumentoCliente", documentoClienteValue),
                    new SqlParameter("@DireccionCliente", direccionClienteValue),
                    new SqlParameter("@TelefonoCliente", telefonoClienteValue),
                    new SqlParameter("@EmailCliente", emailClienteValue),
                    new SqlParameter("@EstadoCliente", estadoCliente)
                };

                // Usar el DatabaseHelper singleton para ejecutar el procedimiento almacenado
                DatabaseHelper.Instance.ExecuteStoredProcedure("InsertarCliente", parameters);

                MessageBox.Show("Cliente agregado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el cliente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
