using System.Windows.Input;
using System.Windows;
using EventDressApp.MVVM.Model;
using EventDressApp.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Collections.ObjectModel;
using EventDressApp.MVVM.View.Dialogs;

namespace EventDressApp.MVVM.ViewModel
{
    public class DialogoUsuarioViewModel : BaseViewModel
    {
        private Usuario _usuario;
        private Window _window;
        private ObservableCollection<EmpresaItem> _empresas;
        private EmpresaItem _empresaSeleccionada;

        public Usuario Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public ObservableCollection<EmpresaItem> Empresas
        {
            get => _empresas;
            set
            {
                _empresas = value;
                OnPropertyChanged(nameof(Empresas));
            }
        }

        public EmpresaItem EmpresaSeleccionada
        {
            get => _empresaSeleccionada;
            set
            {
                _empresaSeleccionada = value;
                OnPropertyChanged(nameof(EmpresaSeleccionada));
            }
        }

        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }

        public DialogoUsuarioViewModel(Window window)
        {
            _window = window;
            Usuario = new Usuario();
            Empresas = new ObservableCollection<EmpresaItem>();
            GuardarCommand = new RelayCommand(GuardarUsuario, CanGuardarUsuario);
            CancelarCommand = new RelayCommand(Cancelar);
            CargarEmpresas();
        }

        private void CargarEmpresas()
        {
            try
            {
                var dt = DatabaseHelper.Instance.ExecuteQuery("SELECT empresa_id, nombre_empresa FROM Empresa");
                Empresas.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    Empresas.Add(new EmpresaItem
                    {
                        Id = Convert.ToInt32(row["empresa_id"]),
                        Nombre = row["nombre_empresa"].ToString()
                    });
                }

                if (Empresas.Count > 0)
                    EmpresaSeleccionada = Empresas[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las empresas: {ex.Message}");
            }
        }

        private bool CanGuardarUsuario(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Usuario.NombreUsuario) &&
                   !string.IsNullOrWhiteSpace(Usuario.UsernameUsuario) &&
                   EmpresaSeleccionada != null;
        }

        private void GuardarUsuario(object parameter)
        {
            try
            {
                var passwordBox = parameter as System.Windows.Controls.PasswordBox;
                if (passwordBox == null)
                {
                    MessageBox.Show("Error: No se pudo obtener la contraseña");
                    return;
                }

                if (EmpresaSeleccionada == null)
                {
                    MessageBox.Show("Por favor, seleccione una empresa");
                    return;
                }

                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    MessageBox.Show("Por favor, ingrese una contraseña");
                    return;
                }

                // Crear parámetros para el procedimiento almacenado
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@EmpresaId", SqlDbType.Int) { Value = EmpresaSeleccionada.Id },
                    new SqlParameter("@RolId", SqlDbType.Int) { Value = 1 }, // Valor por defecto
                    new SqlParameter("@NombreUsuario", SqlDbType.NVarChar) { Value = Usuario.NombreUsuario },
                    new SqlParameter("@ApellidoUsuario", SqlDbType.NVarChar) { Value = Usuario.ApellidoUsuario ?? (object)DBNull.Value },
                    new SqlParameter("@UsernameUsuario", SqlDbType.NVarChar) { Value = Usuario.UsernameUsuario },
                    new SqlParameter("@ContraseñaUsuario", SqlDbType.NVarChar) { Value = passwordBox.Password },
                    new SqlParameter("@EmailUsuario", SqlDbType.NVarChar) { Value = Usuario.EmailUsuario ?? (object)DBNull.Value },
                    new SqlParameter("@TelefonoUsuario", SqlDbType.NVarChar) { Value = Usuario.TelefonoUsuario ?? (object)DBNull.Value },
                    new SqlParameter("@DocumentoUsuario", SqlDbType.NVarChar) { Value = Usuario.DocumentoUsuario ?? (object)DBNull.Value },
                    new SqlParameter("@DireccionUsuario", SqlDbType.NVarChar) { Value = Usuario.DireccionUsuario ?? (object)DBNull.Value },
                    new SqlParameter("@FechaContratacionUsuario", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@EstadoUsuario", SqlDbType.Bit) { Value = true },
                    new SqlParameter("@UltimaSesionUsuario", SqlDbType.DateTime) { Value = DateTime.Now },
                    new SqlParameter("@IntentosFallidosLoginUsuario", SqlDbType.Int) { Value = 0 }
                };

                // Ejecutar el procedimiento almacenado
                DatabaseHelper.Instance.ExecuteStoredProcedure("InsertarUsuario", parameters);

                MessageBox.Show("Usuario creado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                // Cerrar solo el diálogo
                if (_window is DialogoUsuario dialogoUsuario)
                {
                    dialogoUsuario.DialogResult = true;
                    dialogoUsuario.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar(object parameter)
        { 
            if (_window is DialogoUsuario dialogoUsuario)
            {
                dialogoUsuario.DialogResult = false;
                dialogoUsuario.Close();
            }
        }
    }

    public class EmpresaItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}