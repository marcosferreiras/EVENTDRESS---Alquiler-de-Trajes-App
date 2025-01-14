using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using EventDressApp.MVVM.Model;

namespace EventDressApp.MVVM.View
{
    /// <summary>
    /// Lógica de interacción para InventarioView.xaml
    /// </summary>
    public partial class InventarioView : UserControl
    {
        public InventarioView()
        {
            InitializeComponent();
            LoadInventoryData();
        }

        private void LoadInventoryData()
        {
            try
            {
                // Llamamos al stored procedure 'ObtenerTrajes'
                DataTable dataTable = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerTrajes");

                // Establecemos los datos en el DataGrid
                InventarioDG.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos del inventario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Aquí puedes agregar lógica para manejar la selección de filas si es necesario
        }
    }
}
