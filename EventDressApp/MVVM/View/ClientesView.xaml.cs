using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using EventDressApp.MVVM.Model;

namespace EventDressApp.MVVM.View
{
    public partial class ClientesView : UserControl
    {
        public ClientesView()
        {
            InitializeComponent();
            LoadClientesData(); // Cargar datos al inicializar
        }

        // Método para cargar los clientes activos en el DataGrid
        private void LoadClientesData()
        {
            try
            {
                // Llamar al helper para ejecutar la stored procedure 'ObtenerClientesActivos'
                DataTable dtClientes = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerClientesActivos");

                // Asignar el DataTable al DataGrid
                ClientesDGW.ItemsSource = dtClientes.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos de los clientes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para el clic en un botón
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Lógica al cambiar selección en el DataGrid
        }
    }
}
