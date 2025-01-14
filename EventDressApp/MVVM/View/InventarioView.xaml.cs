using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using EventDressApp.MVVM.Model;
using EventDressApp.MVVM.View.Dialogos;
using EventDressApp.MVVM.View.Dialogs;
using Microsoft.Data.SqlClient;

namespace EventDressApp.MVVM.View
{
    public partial class InventarioView : UserControl
    {
        public InventarioView()
        {
            InitializeComponent();
            LoadInventoryData();
            Eliminar_traje_btn.IsEnabled = false;
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

        private void InventarioDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Eliminar_traje_btn.IsEnabled = true;

        }

        private void Eliminar_traje_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si hay un cliente seleccionado
                DataRowView selectedRow = InventarioDG.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int clienteId = Convert.ToInt32(selectedRow["prenda_id"]);

                    // Mostrar el pop-up de confirmación
                    var dialogo = new DialogoConfirmacionEliminar();
                    dialogo.ShowDialog(); // Abrir el pop-up

                    // Si el usuario confirma, eliminar el cliente
                    if (dialogo.ConfirmacionExitosa)
                    {
                        // Crear el parámetro para el procedimiento almacenado
                        SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@prenda_id", SqlDbType.Int) { Value = clienteId }
                };

                        // Ejecutar el procedimiento almacenado para eliminar al cliente
                        int rowsAffected = DatabaseHelper.Instance.ExecuteStoredProcedure("EliminarPrenda", parameters);

                        // Verificar si el cliente fue eliminado exitosamente
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Prenda eliminada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadInventoryData(); // Recargar la lista de clientes después de eliminar
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar la prenda. Puede que no exista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un cliente para eliminar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void add_suit_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new DialogoInventario();
            dialogo.ShowDialog();
            LoadInventoryData();
        }
    }
}
