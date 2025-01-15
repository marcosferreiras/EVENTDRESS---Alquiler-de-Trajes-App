using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using EventDressApp.MVVM.Model;
using EventDressApp.MVVM.View.Dialogos;
using EventDressApp.MVVM.View.Dialogs;
using EventDressApp.MVVM.ViewModel.DialogViewModels;

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
            AlquilarBtn.IsEnabled = false; // Deshabilitar el botón de alquilar por defecto
            EditarClienteBtn.IsEnabled = false; // Deshabilitar el botón de editar por defecto

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
            AlquilarBtn.IsEnabled = true;
            EditarClienteBtn.IsEnabled = true;
        }

        // Evento para manejar el clic del botón "Eliminar Cliente"
        private void DeleteClientBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si hay un cliente seleccionado
                DataRowView selectedRow = ClientesDGV.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    int clienteId = Convert.ToInt32(selectedRow["cliente_id"]);

                    // Mostrar el pop-up de confirmación
                    var dialogo = new DialogoConfirmacionEliminar();
                    dialogo.ShowDialog(); // Abrir el pop-up

                    // Si el usuario confirma, eliminar el cliente
                    if (dialogo.ConfirmacionExitosa)
                    {
                        // Crear el parámetro para el procedimiento almacenado
                        SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@cliente_id", SqlDbType.Int) { Value = clienteId }
                };

                        // Ejecutar el procedimiento almacenado para eliminar al cliente
                        int rowsAffected = DatabaseHelper.Instance.ExecuteStoredProcedure("BorrarCliente", parameters);

                        // Verificar si el cliente fue eliminado exitosamente
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente eliminado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadClientesData(); // Recargar la lista de clientes después de eliminar
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

        private void Add_Client_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new DialogoCliente();
            dialogo.ShowDialog(); // Abrir el pop-up
            LoadClientesData(); // Cargar datos al 

            //var dialog = new DialogoCliente();
            //var nuevoCliente = new DialogoClienteViewModel();
            //if (dialog.ShowDialog() == true)
            //{
            //    // TODO: Agregar a la base de datos
            //}
        }

        private void AlquilarBtn_click(object sender, RoutedEventArgs e)
        {
            // Verificar si hay un cliente seleccionado
            DataRowView selectedRow = ClientesDGV.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                // Obtener información del cliente seleccionado
                int clienteId = Convert.ToInt32(selectedRow["cliente_id"]);
                string nombre = selectedRow["nombre_cliente"].ToString();
                string apellido = selectedRow["apellido_cliente"].ToString();

                // Pasar datos del cliente al constructor de DialogoAlquiler
                var dialogo = new DialogoAlquiler(clienteId, nombre, apellido);
                dialogo.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un cliente antes de alquilar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditarClienteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si se seleccionó un cliente en el DataGrid
                if (ClientesDGV.SelectedItem is DataRowView selectedRow)
                {
                    // Obtener el ID del cliente de la fila seleccionada
                    int clienteID = Convert.ToInt32(selectedRow["cliente_id"]);

                    // Abrir el diálogo de edición con el ID del cliente seleccionado
                    DialogoCliente dialogo = new DialogoCliente(clienteID);
                    dialogo.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Seleccione un cliente para editar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al intentar editar el cliente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
