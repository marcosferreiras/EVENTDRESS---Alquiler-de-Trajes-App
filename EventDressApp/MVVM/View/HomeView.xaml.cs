using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using EventDressApp.MVVM.Model;

namespace EventDressApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            LoadReservasData();
        }

        private void LoadReservasData()
        {
            try
            {
                // Llamar al stored procedure usando DatabaseHelper
                DataTable reservasData = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerReservas");

                // Enlazar los datos al DataGrid
                ReservasDataGrid.ItemsSource = reservasData.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las reservas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (ReservasDataGrid.SelectedItem != null)
        //    {
        //        // Obtener la fila seleccionada
        //        DataRowView selectedRow = ReservasDataGrid.SelectedItem as DataRowView;
        //        if (selectedRow != null)
        //        {
        //            // Ejemplo de acceso a datos de la fila seleccionada
        //            string reservaId = selectedRow["reserva_id"].ToString();
        //            MessageBox.Show($"Reserva seleccionada: {reservaId}", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }
        //}

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ReservasDataGrid.ItemsSource is DataView dataView)
                {
                    string filterText = SearchTextBox.Text.Trim();

                    if (string.IsNullOrEmpty(filterText))
                    {
                        // Limpiar el filtro si no hay texto en el cuadro de búsqueda
                        dataView.RowFilter = string.Empty;
                    }
                    else
                    {
                        // Escapar caracteres especiales en el filtro
                        string escapedFilterText = filterText.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");

                        // Aplicar filtro por ID o nombre
                        dataView.RowFilter = $"CONVERT([ID Reserva], System.String) LIKE '%{escapedFilterText}%' OR [Cliente] LIKE '%{escapedFilterText}%'";
                    }
                }
                else
                {
                    // Manejar casos donde ItemsSource no sea un DataView
                    MessageBox.Show("La fuente de datos no es válida para aplicar el filtro.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al aplicar el filtro: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}