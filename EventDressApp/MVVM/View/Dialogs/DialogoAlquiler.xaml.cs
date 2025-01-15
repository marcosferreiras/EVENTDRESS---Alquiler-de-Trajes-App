using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EventDressApp.MVVM.Model;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class DialogoAlquiler : Window
    {
        private DataTable originalData; // Datos originales
        private HashSet<int> selectedItems; // IDs de los elementos seleccionados

        private int _clienteId;
        private string _nombreCliente;
        private string _apellidoCliente;

        public DialogoAlquiler(int clienteId, string nombreCliente, string apellidoCliente)
        {
            InitializeComponent();

            _clienteId = clienteId;
            _nombreCliente = nombreCliente;
            _apellidoCliente = apellidoCliente;

            LoadInventoryData();
        }

        private void LoadInventoryData()
        {
            try
            {
                // Carga los datos originales desde la base de datos
                originalData = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerTrajes");


                // Agregar la columna 'IsSelected' al DataTable si no existe
                if (!originalData.Columns.Contains("IsSelected"))
                {
                    originalData.Columns.Add("IsSelected", typeof(bool)); // Columna para el checkbox
                }

                // Inicializar la columna 'IsSelected' con valores 'false'
                foreach (DataRow row in originalData.Rows)
                {
                    row["IsSelected"] = false;
                }

                // Establecer los datos en el DataGrid
                InventarioDG.ItemsSource = originalData.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos del inventario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (originalData == null) return;

            string filter = filterTB.Text.Trim().ToLower();

            // Guardar IDs de las filas seleccionadas
            var selectedIds = selectedItems.ToList();

            // Filtrar datos
            var filteredData = originalData.AsEnumerable()
                .Where(row => row["nombre_prenda"].ToString().ToLower().Contains(filter) ||
                              row["nombre_marca"].ToString().ToLower().Contains(filter));

            // Crear nuevo DataTable con datos filtrados
            var filteredTable = filteredData.Any() ? filteredData.CopyToDataTable() : originalData.Clone();

            // Actualizar el DataGrid
            InventarioDG.ItemsSource = filteredTable.DefaultView;

            // Restaurar las selecciones después del filtrado
            RestoreSelection(selectedIds);
        }

        private void InventarioDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Guardar los IDs seleccionados
            foreach (DataRowView item in e.AddedItems)
            {
                int id = Convert.ToInt32(item["prenda_id"]);
                selectedItems.Add(id);

                // Mostrar mensaje cuando se añade un elemento a la selección
                MessageBox.Show($"Elemento añadido con ID: {id}", "Selección Añadida", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Eliminar los IDs deseleccionados
            foreach (DataRowView item in e.RemovedItems)
            {
                int id = Convert.ToInt32(item["prenda_id"]);
                selectedItems.Remove(id);

                // Mostrar mensaje cuando se elimina un elemento de la selección
                MessageBox.Show($"Elemento eliminado con ID: {id}", "Selección Eliminada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RestoreSelection(IEnumerable<int> selectedIds)
        {
            // Limpiar las selecciones previas
            InventarioDG.SelectedItems.Clear();

            // Restaurar las selecciones
            foreach (DataRowView row in InventarioDG.ItemsSource)
            {
                int id = Convert.ToInt32(row["prenda_id"]);
                if (selectedIds.Contains(id))
                {
                    InventarioDG.SelectedItems.Add(row);
                    MessageBox.Show($"Elemento restaurado con ID: {id}", "Selección Restaurada", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void GenerateInvoice(object sender, RoutedEventArgs e)
        {
            // Verificar si se ha seleccionado algún elemento
            var selectedRows = new List<DataRowView>();

            foreach (DataRowView row in InventarioDG.ItemsSource)
            {
                if (row.Row["IsSelected"] != DBNull.Value && (bool)row.Row["IsSelected"])
                {
                    selectedRows.Add(row);
                }
            }

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione al menos un elemento para facturar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Pasar información del cliente y las filas seleccionadas a la ventana de factura
            var invoiceWindow = new InvoiceWindow(selectedRows, _clienteId, _nombreCliente, _apellidoCliente);
            invoiceWindow.ShowDialog();
        }

        // Método para cambiar IsSelected cuando se hace clic en la casilla de verificación
        private void InventarioDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Verificar si la columna editada es la columna de selección
            if (e.Column.Header.ToString() == "Seleccionar")
            {
                var row = (DataRowView)e.Row.Item;
                var isSelected = (row["IsSelected"] != DBNull.Value && (bool)row["IsSelected"]);

                // Si la casilla fue marcada
                if (isSelected)
                {
                    MessageBox.Show("Se ha seleccionado un elemento.", "Selección", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
