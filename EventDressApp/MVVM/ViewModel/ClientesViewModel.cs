using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EventDressApp.MVVM.Model;
using System.Windows.Input;
using EventDressApp.Helpers;
using EventDressApp.MVVM.View.Dialogs;
using System.Runtime.CompilerServices;
using EventDressApp.MVVM.ViewModel.DialogViewModels;

namespace EventDressApp.MVVM.ViewModel
{
    public class ClientesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Clientes> _clientes;
        private Clientes _clienteSeleccionado;
        private string _filtroEstado = "Todos";

        public ObservableCollection<Clientes> Clientes
        {
            get => _clientes;
            set
            {
                _clientes = value;
                OnPropertyChanged();
            }
        }

        public Clientes ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set
            {
                _clienteSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public string FiltroEstado
        {
            get => _filtroEstado;
            set
            {
                _filtroEstado = value;
                FiltrarClientes();
                OnPropertyChanged();
            }
        }

        public ICommand AgregarClienteCommand { get; }
        public ICommand EditarClienteCommand { get; }
        public ICommand VerDetallesCommand { get; }
        public ICommand CambiarEstadoCommand { get; }

        public ClientesViewModel()
        {
            CargarDatosIniciales();
            AgregarClienteCommand = new RelayCommand(AgregarCliente);
            EditarClienteCommand = new RelayCommand(EditarCliente, CanEditarCliente);
            VerDetallesCommand = new RelayCommand(VerDetalles, CanVerDetalles);
            CambiarEstadoCommand = new RelayCommand(CambiarEstado, CanCambiarEstado);
        }

        private void CargarDatosIniciales()
        {
            // Carga desde la base de datos
            Clientes = new ObservableCollection<Clientes>();
        }

        private void FiltrarClientes()
        {
            // Filtrado según FiltroEstado
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        private void AgregarCliente(object obj)
        {
            var dialog = new DialogoCliente();
            if (dialog.ShowDialog() == true)
            {
                // TODO: Agregar a la base de datos
                var nuevoCliente = ((DialogoClienteViewModel)dialog.DataContext).Cliente;
                Clientes.Add(nuevoCliente);
            }
        }

        private void EditarCliente(object obj)
        {
            var dialog = new DialogoCliente(ClienteSeleccionado);
            if (dialog.ShowDialog() == true)
            {
                // TODO: Actualizar en la base de datos
                var clienteActualizado = ((DialogoClienteViewModel)dialog.DataContext).Cliente;
                // Actualizar propiedades
            }
        }

        private bool CanEditarCliente(object obj)
        {
            return ClienteSeleccionado != null;
        }

        private void VerDetalles(object obj)
        {
            // Lógica para ver detalles del cliente
        }

        private bool CanVerDetalles(object obj)
        {
            return ClienteSeleccionado != null;
        }

        private void CambiarEstado(object obj)
        {
            // Lógica para cambiar estado del cliente
        }

        private bool CanCambiarEstado(object obj)
        {
            return ClienteSeleccionado != null;
        }
    }
}
