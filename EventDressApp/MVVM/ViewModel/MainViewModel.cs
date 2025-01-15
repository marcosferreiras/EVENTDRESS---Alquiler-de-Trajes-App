using System.Windows;
using System.Windows.Input;
using EventDressApp.Helpers;
using EventDressApp.Views;

namespace EventDressApp.MVVM.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand HomeViewCommand { get; set; }
        public ICommand InventarioViewCommand { get; set; }
        public ICommand ClientesViewCommand { get; set; }
        public ICommand UsuariosViewCommand { get; set; }
        public ICommand CerrarSesionCommand { get; set; }

        public MainViewModel()
        {
            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = new HomeViewModel();
            });

            InventarioViewCommand = new RelayCommand(o =>
            {
                CurrentView = new InventarioViewModel();
            });

            ClientesViewCommand = new RelayCommand(o =>
            {
                CurrentView = new ClientesViewModel();
            });

            UsuariosViewCommand = new RelayCommand(o =>
            {
                CurrentView = new UsuariosViewModel();
            });

            // Comando para cerrar sesión
            CerrarSesionCommand = new RelayCommand(o =>
            {
                CerrarSesion();
            });

            // Vista por defecto
            CurrentView = new HomeViewModel();
        }

        private void CerrarSesion()
        {
            if (MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                              "Cerrar Sesión",
                              MessageBoxButton.YesNo,
                              MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Abrir la ventana de login
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();

                // Cerrar la ventana principal
                if (Application.Current.MainWindow != null)
                    Application.Current.MainWindow.Close();
            }
        }
    }
}