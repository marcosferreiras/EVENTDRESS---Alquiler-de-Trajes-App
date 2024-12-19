using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventDressApp.MVVM.Model
{
    public class Clientes : INotifyPropertyChanged
    {
        private int clienteid;
        private string nombrecliente;
        private string apellidocliente;
        private DateTime? fechaultimoalquilercliente;
        private int totalalquilerescliente;
        private DetalleCliente detallecliente;

        public int ClienteId { get => clienteid; set { if (clienteid != value) { clienteid = value; OnPropertyChanged(); } } }
        public string NombreCliente { get => nombrecliente; set { if (nombrecliente != value) { nombrecliente = value; OnPropertyChanged(); } } }
        public string ApellidoCliente { get => apellidocliente; set { if (apellidocliente != value) { apellidocliente = value; OnPropertyChanged(); } } }
        public DateTime? FechaUltimoAlquilerCliente { get => fechaultimoalquilercliente; set { if (fechaultimoalquilercliente != value) { fechaultimoalquilercliente = value; OnPropertyChanged(); } } }
        public int TotalAlquileresCliente { get => totalalquilerescliente; set { if (totalalquilerescliente != value) { totalalquilerescliente = value; OnPropertyChanged(); } } }
        public DetalleCliente DetalleCliente { get => detallecliente; set { if (detallecliente != value) { detallecliente = value; OnPropertyChanged(); } } }

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor
        public Clientes()
        {
            detallecliente = new DetalleCliente();
        }
    }

    public class DetalleCliente : INotifyPropertyChanged
    {
        private int detalleclienteid;
        private int clienteid;
        private string documentocliente;
        private string direccioncliente;
        private string telefonocliente;
        private string emailcliente;
        private string estadocliente;

        public int DetalleClienteId { get => detalleclienteid; set { if (detalleclienteid != value) { detalleclienteid = value; OnPropertyChanged(); } } }
        public int ClienteId { get => clienteid; set { if (clienteid != value) { clienteid = value; OnPropertyChanged(); } } }
        public string DocumentoCliente { get => documentocliente; set { if (documentocliente != value) { documentocliente = value; OnPropertyChanged(); } } }
        public string DireccionCliente { get => direccioncliente; set { if (direccioncliente != value) { direccioncliente = value; OnPropertyChanged(); } } }
        public string TelefonoCliente { get => telefonocliente; set { if (telefonocliente != value) { telefonocliente = value; OnPropertyChanged(); } } }
        public string EmailCliente { get => emailcliente; set { if (emailcliente != value) { emailcliente = value; OnPropertyChanged(); } } }
        public string EstadoCliente { get => estadocliente; set { if (estadocliente != value) { estadocliente = value; OnPropertyChanged(); } } }

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}