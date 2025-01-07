using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
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
            DeleteClientBtn.IsEnabled = false; // Deshabilitar el botón de eliminar por defecto

            try
            {
                // Llamar al helper para ejecutar la stored procedure 'ObtenerClientesActivos'
                DataTable dtClientes = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerClientesActivos");

                // Asignar el DataTable al DataGrid
                ClientesDGV.ItemsSource = dtClientes.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos de los clientes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento para manejar el cambio de selección en el DataGrid
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Habilitar el botón si hay elementos seleccionados
            DeleteClientBtn.IsEnabled = true;
        }

        // Evento para manejar el clic del botón "Eliminar Cliente"
        private void DeleteClientBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener el cliente_id desde la fila seleccionada en el DataGrid
                DataRowView selectedRow = ClientesDGV.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int clienteId = Convert.ToInt32(selectedRow["cliente_id"]); // Asumiendo que "cliente_id" es el nombre de la columna

                    // Mostrar el pop-up de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este cliente?",
                                                              "Confirmar eliminación",
                                                              MessageBoxButton.YesNo,
                                                              MessageBoxImage.Warning);

                    // Si el usuario confirma, eliminar el cliente
                    if (result == MessageBoxResult.Yes)
                    {
                        // Crear el parámetro para el stored procedure
                        SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@cliente_id", SqlDbType.Int) { Value = clienteId }
                };

                        // Ejecutar el procedimiento almacenado
                        int rowsAffected = DatabaseHelper.Instance.ExecuteStoredProcedure("BorrarCliente", parameters);

                        // Verificar si el cliente fue eliminado exitosamente
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente eliminado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadClientesData(); // Recargar la lista de clientes
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el cliente. Puede que no exista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para el clic en otros botones (si aplica)
        }
    }
}
