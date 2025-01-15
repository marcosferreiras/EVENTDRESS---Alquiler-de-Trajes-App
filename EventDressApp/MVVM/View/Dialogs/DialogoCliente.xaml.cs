using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using EventDressApp.MVVM.Model;

namespace EventDressApp.MVVM.View.Dialogs
{
    public partial class DialogoCliente : Window
    {
        private bool _esEdicion;
        private int _clienteID;

        public DialogoCliente(int? clienteID = null)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;

            if (clienteID.HasValue)
            {
                _esEdicion = true;
                _clienteID = clienteID.Value;

                // Cargar datos del cliente desde la base de datos
                CargarCliente(_clienteID);
            }
            else
            {
                _esEdicion = false; // Nuevo cliente
            }
        }

        private void CargarCliente(int clienteID)
        {
            try
            {
                // Llamar al procedimiento almacenado
                SqlParameter[] parameters = {
                    new SqlParameter("@ClienteID", clienteID)
                };
                DataTable clienteData = DatabaseHelper.Instance.ExecuteStoredProcedureWithResults("ObtenerClientePorID", parameters);

                if (clienteData.Rows.Count > 0)
                {
                    // Mapear datos al formulario
                    DataRow cliente = clienteData.Rows[0];
                    NombreClienteTB.Text = cliente["Nombre"].ToString();
                    ApellidoClienteTB.Text = cliente["Apellido"].ToString();
                    DocumentoClienteTB.Text = cliente["Documento"].ToString();
                    DireccionClienteTB.Text = cliente["Direccion"].ToString();
                    TelefonoClienteTB.Text = cliente["Telefono"].ToString();
                    EmailClienteTB.Text = cliente["Email"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontró el cliente con el ID especificado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos del cliente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CrearCliente(object sender, RoutedEventArgs e)
        {
            string nombreCliente = NombreClienteTB.Text;
            string apellidoCliente = ApellidoClienteTB.Text;
            string documentoCliente = DocumentoClienteTB.Text;
            string direccionCliente = DireccionClienteTB.Text;
            string telefonoCliente = TelefonoClienteTB.Text;
            string emailCliente = EmailClienteTB.Text;

            if (string.IsNullOrWhiteSpace(nombreCliente) ||
                string.IsNullOrWhiteSpace(apellidoCliente) ||
                string.IsNullOrWhiteSpace(documentoCliente))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_esEdicion)
                {
                    // Editar cliente existente
                    EditarCliente(_clienteID, nombreCliente, apellidoCliente, documentoCliente, direccionCliente, telefonoCliente, emailCliente);
                }
                else
                {
                    // Crear nuevo cliente
                    InsertarCliente(nombreCliente, apellidoCliente, documentoCliente, direccionCliente, telefonoCliente, emailCliente);
                }

                MessageBox.Show("Operación completada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos del cliente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditarCliente(int clienteID, string nombre, string apellido, string documento, string direccion, string telefono, string email)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@ClienteID", clienteID),
                    new SqlParameter("@NombreCliente", nombre),
                    new SqlParameter("@ApellidoCliente", apellido),
                    new SqlParameter("@DocumentoCliente", documento),
                    new SqlParameter("@DireccionCliente", direccion),
                    new SqlParameter("@TelefonoCliente", telefono),
                    new SqlParameter("@EmailCliente", email)
                };

                DatabaseHelper.Instance.ExecuteStoredProcedure("ActualizarCliente", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al editar el cliente: {ex.Message}");
            }
        }

        private void InsertarCliente(string nombre, string apellido, string documento, string direccion, string telefono, string email)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@NombreCliente", nombre),
                    new SqlParameter("@ApellidoCliente", apellido),
                    new SqlParameter("@DocumentoCliente", documento),
                    new SqlParameter("@DireccionCliente", direccion),
                    new SqlParameter("@TelefonoCliente", telefono),
                    new SqlParameter("@EmailCliente", email)
                };

                DatabaseHelper.Instance.ExecuteStoredProcedure("InsertarCliente", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar el cliente: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
