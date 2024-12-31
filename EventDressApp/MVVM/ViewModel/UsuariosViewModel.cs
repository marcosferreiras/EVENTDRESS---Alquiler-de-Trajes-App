using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EventDressApp.MVVM.Model;
using EventDressApp.Helpers;
using EventDressApp.MVVM.View.Dialogs;
using EventDressApp.MVVM.ViewModel.DialogViewModels;

namespace EventDressApp.MVVM.ViewModel
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Usuario> _usuarios;
        private Usuario _usuarioSeleccionado;
        private string _filtroEstado = "Todos";
        private bool _puedeEditarUsuario;
        private bool _puedeDesactivarUsuario;

        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set
            {
                _usuarios = value;
                OnPropertyChanged();
            }
        }

        public bool PuedeEditarUsuario
        {
            get => _puedeEditarUsuario;
            set
            {
                _puedeEditarUsuario = value;
                OnPropertyChanged();
            }
        }

        public bool PuedeDesactivarUsuario
        {
            get => _puedeDesactivarUsuario;
            set
            {
                _puedeDesactivarUsuario = value;
                OnPropertyChanged();
            }
        }

        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;

                PuedeEditarUsuario = value != null;
                PuedeDesactivarUsuario = value != null && value.EstadoUsuario != "Inactivo";
                OnPropertyChanged();
            }
        }
        public UsuariosViewModel()
        {
            CargarDatosIniciales();
            AgregarUsuarioCommand = new RelayCommand(AgregarUsuario);
            EditarUsuarioCommand = new RelayCommand(EditarUsuario, CanEditarUsuario);
            DesactivarUsuarioCommand = new RelayCommand(DesactivarUsuario, CanDesactivarUsuario);
        }

        private void DesactivarUsuario(object obj)
        {
            if (UsuarioSeleccionado != null)
            {
                UsuarioSeleccionado.EstadoUsuario = "Inactivo";
                // Actualizar el estado del botón
                PuedeDesactivarUsuario = false;
                // Lógica para actualizar en la base de datos
            }
        }
        private bool CanEditarUsuario(object obj)
        {
            return UsuarioSeleccionado != null;
        }

        private bool CanDesactivarUsuario(object obj)
        {
            return UsuarioSeleccionado != null && UsuarioSeleccionado.EstadoUsuario != "Inactivo";
        }

        private void EditarUsuario(object obj)
        {
            if (UsuarioSeleccionado != null)
            {
                var dialog = new DialogoUsuario(UsuarioSeleccionado);
                if (dialog.ShowDialog() == true)
                {
                    var usuarioActualizado = ((DialogoUsuarioViewModel)dialog.DataContext).Usuario;
                    // Lógica para actualizar en la base de datos
                }
            }
        }

        public string FiltroEstado
        {
            get => _filtroEstado;
            set
            {
                _filtroEstado = value;
                FiltrarUsuarios();
                OnPropertyChanged();
            }
        }

        public ICommand AgregarUsuarioCommand { get; }
        public ICommand EditarUsuarioCommand { get; }
        public ICommand DesactivarUsuarioCommand { get; }
        

        private void CargarDatosIniciales()
        {
            // Carga desde la base de datos
            Usuarios = new ObservableCollection<Usuario>();
        }

        private void FiltrarUsuarios()
        {
            // Implementar filtrado según FiltroEstado
        }

        private void AgregarUsuario(object obj)
        {
            var dialog = new DialogoUsuario();
            if (dialog.ShowDialog() == true)
            {
                var nuevoUsuario = ((DialogoUsuarioViewModel)dialog.DataContext).Usuario;
                Usuarios.Add(nuevoUsuario);
                // La lógica para guardar en la base de datos
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}