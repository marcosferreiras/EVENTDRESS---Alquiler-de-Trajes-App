using System;
using Microsoft.Data.SqlClient;
using System.Windows;
using Microsoft.Win32;
using System.Data;
using EventDressApp.MVVM.Model;
using EventDressApp.Views;
using EventDressApp.MVVM.View.Dialogs;

namespace EventDressApp
{
    public partial class SetupWindow : Window
    {
            public string NombreEmpresa { get; set; }
            public string LogoEmpresa { get; set; }
            public string DireccionEmpresa { get; set; }
            public string TelefonoEmpresa { get; set; }
            public string EmailEmpresa { get; set; }
            public string SitioWebEmpresa { get; set; }

            public SetupWindow()
            {
            InitializeComponent();
            }

            private void SeleccionarLogo_Click(object sender, RoutedEventArgs e)
            {
                // Abrir diálogo para seleccionar un archivo de imagen
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                if (openFileDialog.ShowDialog() == true)
                {
                    // Guardar la ruta del archivo seleccionado (podrías mostrarlo en una caja de texto o similar)
                    string rutaLogo = openFileDialog.FileName;
                    LogoEmpresaTextBox.Text = rutaLogo; // Asumiendo que tienes un TextBox para mostrar la ruta del logo
                }
            }
            private void Guardar_Click(object sender, RoutedEventArgs e)
            {
                // Capturar los datos ingresados
                string nombreEmpresa = NombreEmpresaTextBox.Text;
                string logoEmpresa = LogoEmpresaTextBox.Text;
                string direccionEmpresa = DireccionEmpresaTextBox.Text;
                string telefonoEmpresa = TelefonoEmpresaTextBox.Text;
                string emailEmpresa = EmailEmpresaTextBox.Text;
                string sitioWebEmpresa = SitioWebEmpresaTextBox.Text;
                DateTime fechaRegistroEmpresa = DateTime.Now;

                // Validar y guardar en la base de datos
                GuardarDatosEmpresa(nombreEmpresa, logoEmpresa, direccionEmpresa, telefonoEmpresa, emailEmpresa, sitioWebEmpresa, fechaRegistroEmpresa);

                DialogoUsuario dialogoUsuario = new DialogoUsuario();
                bool? userResult = dialogoUsuario.ShowDialog();

                // Si la creación de usuario fue exitosa, se va al login
                if (userResult == true)
                {
                    // Abrir la ventana de login
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                }

                // Cerrar la ventana de configuración
                //this.Close();
            }

        private void GuardarDatosEmpresa(string nombre, string logo, string direccion, string telefono, string email, string sitioWeb, DateTime fechaRegistro)
            {
                // Parámetros para el procedimiento almacenado
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@Nombre", SqlDbType.NVarChar) { Value = nombre },
                new SqlParameter("@Logo", SqlDbType.NVarChar) { Value = logo },
                new SqlParameter("@Direccion", SqlDbType.NVarChar) { Value = direccion },
                new SqlParameter("@Telefono", SqlDbType.NVarChar) { Value = telefono },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email },
                new SqlParameter("@SitioWeb", SqlDbType.NVarChar) { Value = sitioWeb },
                new SqlParameter("@FechaRegistro", SqlDbType.DateTime) { Value = fechaRegistro }
                };
            try
            {
                // Llamar al procedimiento almacenado a través de DatabaseHelper
                DatabaseHelper.Instance.ExecuteStoredProcedure("InsertarEmpresa", parameters);
                MessageBox.Show("Datos de empresa guardados correctamente.");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar los datos de la empresa: {ex.Message}");
            }
        }
    }
}