using System.Windows;
using System.Windows.Input;
using EventDressApp.MVVM.Model;
using EventDressApp.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace EventDressApp.MVVM.ViewModel.DialogViewModels
{
    public class DialogoUsuarioViewModel : INotifyPropertyChanged
    {
        private Usuario _usuario;
        private string _titulo;
        private bool _esNuevo;

        public Usuario Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged();
            }
        }

        public string Titulo
        {
            get => _titulo;
            set
            {
                _titulo = value;
                OnPropertyChanged();
            }
        }

        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }

        public DialogoUsuarioViewModel(Usuario usuario = null)
        {
            _esNuevo = usuario == null;
            Usuario = usuario ?? new Usuario();
            Titulo = _esNuevo ? "Nuevo Usuario" : "Editar Usuario";
            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        private void Guardar(object obj)
        {
            if (ValidarDatos())
            {
                DialogResult = true;
                CloseDialog();
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(Usuario.NombreUsuario) ||
                string.IsNullOrWhiteSpace(Usuario.UsernameUsuario) ||
                string.IsNullOrWhiteSpace(Usuario.ContraseñaUsuario))
            {
                MessageBox.Show("Por favor complete los campos obligatorios");
                return false;
            }
            return true;
        }

        private void Cancelar(object obj)
        {
            DialogResult = false;
            CloseDialog();
        }

        private void CloseDialog()
        {
            if (System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) is Window window)
                window.Close();
        }

        public bool? DialogResult { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}