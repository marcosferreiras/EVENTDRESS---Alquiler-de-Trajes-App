using System;
using System.Windows;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using EventDressApp.MVVM.Model;

namespace EventDressApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly DatabaseHelper _db;

        public LoginWindow()
        {
            InitializeComponent();
            _db = DatabaseHelper.Instance;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            

            try
            {
                if (ValidateUser(username, password))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    txtError.Text = "Usuario o contraseña incorrectos";
                    txtPassword.Password = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar iniciar sesión: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", hashedPassword)
                };

                DataTable result = _db.ExecuteStoredProcedureWithResults("SP_ValidarUsuario", parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    // Crear objeto Usuario con los datos retornados
                    Usuario usuario = new Usuario
                    {
                        UsuarioId = Convert.ToInt32(result.Rows[0]["usuario_id"]),
                        EmpresaId = Convert.ToInt32(result.Rows[0]["empresa_id"]),
                        RolId = result.Rows[0]["rol_id"].ToString(),
                        NombreUsuario = result.Rows[0]["nombre_usuario"].ToString(),
                        ApellidoUsuario = result.Rows[0]["apellido_usuario"].ToString(),
                        EmailUsuario = result.Rows[0]["email_usuario"].ToString()
                    };

                    // Iniciar sesión
                    UserSession.Instance.IniciarSesion(usuario);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar usuario: " + ex.Message, ex);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}