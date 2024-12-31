using System.Windows;
using System.Windows.Input;
using EventDressApp.MVVM.Model;
using EventDressApp.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace EventDressApp.MVVM.ViewModel.DialogViewModels
{
    public class DialogoClienteViewModel : INotifyPropertyChanged
    {
        private Clientes _cliente;
        private string _titulo;
        private bool _esNuevo;

        public Clientes Cliente
        {
            get => _cliente;
            set
            {
                _cliente = value;
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

        public DialogoClienteViewModel(Clientes cliente = null)
        {
            _esNuevo = cliente == null;
            Cliente = cliente ?? new Clientes();
            Titulo = _esNuevo ? "Nuevo Cliente" : "Editar Cliente";

            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        private void Guardar(object obj)
        {
            // Validar datos
            if (string.IsNullOrWhiteSpace(Cliente.NombreCliente) ||
                string.IsNullOrWhiteSpace(Cliente.DetalleCliente.DocumentoCliente))
            {
                MessageBox.Show("Por favor complete los campos obligatorios");
                return;
            }

            // TODO: Guardar en base de datos
            DialogResult = true;
            CloseDialog();
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