using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using EventDressApp.MVVM.Model;
using EventDressApp.MVVM.ViewModel.DialogViewModels;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class DialogoInventario : Window
    {
        public DialogoInventario(DETALLE_MODELO traje = null)
        {
            InitializeComponent();
            DataContext = new DialogoInventarioViewModel(traje);
            Owner = Application.Current.MainWindow;
        }

        private void Save_clothing_item(object sender, RoutedEventArgs e)
        {
            // Obtener valores desde los controles del formulario
            string nombreTraje = NombreTrajeTB.Text;
            string descripcionTraje = DescripcionTrajeTB.Text;
            int categoriaId = Convert.ToInt32(CategoriasCB.SelectedValue);
            int marcaId = Convert.ToInt32(MarcasCB.SelectedValue);
            string generoTraje = ((ComboBoxItem)GeneroCB.SelectedItem).Content.ToString();
            string tallaTraje = ((ComboBoxItem)TallaCB.SelectedItem).Content.ToString();
            string colorTraje = ColorTrajeTB.Text;
            decimal precioDiarioTraje = Convert.ToDecimal(PrecioDiarioTB.Text);
            string estadoTraje = ((ComboBoxItem)EstadoCB.SelectedItem).Content.ToString();
            string rutaImagenTraje = RutaImagenTB.Text;

            // Valores adicionales
            int cantidadTotal = 10; // Por defecto, puedes ajustarlo
            int cantidadReservada = 0;
            int cantidadAlquilada = 0;
            DateTime fechaCompra = DateTime.Today; // Fecha actual

            try
            {
                // Crear los parámetros para el procedimiento almacenado
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CategoriaID", categoriaId),
                    new SqlParameter("@MarcaID", marcaId),
                    new SqlParameter("@NombrePrenda", nombreTraje),
                    new SqlParameter("@DescripcionPrenda", descripcionTraje),
                    new SqlParameter("@GeneroPrenda", generoTraje),
                    new SqlParameter("@TallaPrenda", tallaTraje),
                    new SqlParameter("@ColorPrenda", colorTraje),
                    new SqlParameter("@PrecioDiarioPrenda", precioDiarioTraje),
                    new SqlParameter("@EstadoPrenda", estadoTraje),
                    new SqlParameter("@RutaImagenPrenda", rutaImagenTraje),
                    new SqlParameter("@CantidadTotal", cantidadTotal),
                    new SqlParameter("@CantidadReservada", cantidadReservada),
                    new SqlParameter("@CantidadAlquilada", cantidadAlquilada),
                    new SqlParameter("@FechaCompra", fechaCompra)
                };

                // Usar el DatabaseHelper para ejecutar el procedimiento almacenado
                DatabaseHelper.Instance.ExecuteStoredProcedure("InsertarPrenda", parameters);

                MessageBox.Show("Prenda agregada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar la prenda: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}