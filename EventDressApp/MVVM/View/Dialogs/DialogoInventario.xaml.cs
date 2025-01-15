using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using EventDressApp.MVVM.Model;
using EventDressApp.MVVM.ViewModel.DialogViewModels;
using System.Data;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class DialogoInventario : Window
    {
        public DialogoInventario(DETALLE_MODELO traje = null)
        {
            InitializeComponent();
            DataContext = new DialogoInventarioViewModel(traje);
            Owner = Application.Current.MainWindow;
            LoadCategorias();
            LoadMarcas();
        }

        private void LoadCategorias()
        {
            try
            {
                // Ejecutar el procedimiento almacenado para obtener las categorías
                DataTable dataTable = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerCategorias");

                // Asignar los datos al ComboBox
                CategoriasCB.ItemsSource = dataTable.DefaultView;
                CategoriasCB.DisplayMemberPath = "nombre_categoria"; // Nombre de la columna que se muestra
                CategoriasCB.SelectedValuePath = "categoria_id"; // Valor asociado a cada ítem
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las categorías: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMarcas()
        {
            try
            {
                // Ejecutar el procedimiento almacenado para obtener las marcas
                DataTable dataTable = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerMarcas");

                // Asignar los datos al ComboBox de Marcas
                MarcasCB.ItemsSource = dataTable.DefaultView;
                MarcasCB.DisplayMemberPath = "nombre_marca"; // Nombre de la columna que se muestra
                MarcasCB.SelectedValuePath = "marca_id"; // Valor asociado a cada ítem
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las marcas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    new SqlParameter("@EstadoPrenda", "Disponible"),
                    new SqlParameter("@RutaImagenPrenda", rutaImagenTraje),
                    new SqlParameter("@CantidadTotal", cantidadTotal),
                    new SqlParameter("@CantidadReservada", cantidadReservada),
                    new SqlParameter("@CantidadAlquilada", cantidadAlquilada),
                    new SqlParameter("@FechaCompra", fechaCompra)
                };

                // Usar el DatabaseHelper para ejecutar el procedimiento almacenado
                DatabaseHelper.Instance.ExecuteStoredProcedure("InsertarPrenda", parameters);

                MessageBox.Show("Prenda agregada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpiar los campos del formulario
                ClearFormFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar la prenda: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFormFields()
        {
            // Limpiar los valores de los campos
            NombreTrajeTB.Text = string.Empty;
            DescripcionTrajeTB.Text = string.Empty;
            CategoriasCB.SelectedIndex = -1;
            MarcasCB.SelectedIndex = -1;
            GeneroCB.SelectedIndex = -1;
            TallaCB.SelectedIndex = -1;
            ColorTrajeTB.Text = string.Empty;
            PrecioDiarioTB.Text = string.Empty;
            RutaImagenTB.Text = string.Empty;

            // Opcional: restablecer cualquier validación o estado adicional
        }

        private void CategoriasCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
