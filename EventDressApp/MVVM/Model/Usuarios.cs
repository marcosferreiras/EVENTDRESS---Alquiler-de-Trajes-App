using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventDressApp.MVVM.Model
{
    public class Usuario : INotifyPropertyChanged
    {
        private int usuarioid;
        private int empresaid;
        private string rolid;
        private string nombreusuario;
        private string apellidousuario;
        private string usernameusuario;
        private string contraseñausuario;
        private string emailusuario;
        private string telefonousuario;
        private string documentousuario;
        private string descripcionusuario;
        private DateTime fechacontratacion;
        private string estadousuario;
        private DateTime ultimasesionusuario;
        private int intentosfallidos;

        public int UsuarioId { get => usuarioid; set { if (usuarioid != value) { usuarioid = value; OnPropertyChanged(); } } }
        public int EmpresaId { get => empresaid; set { if (empresaid != value) { empresaid = value; OnPropertyChanged(); } } }
        public string RolId { get => rolid; set { if (rolid != value) { rolid = value; OnPropertyChanged(); } } }
        public string NombreUsuario { get => nombreusuario; set { if (nombreusuario != value) { nombreusuario = value; OnPropertyChanged(); } } }
        public string ApellidoUsuario { get => apellidousuario; set { if (apellidousuario != value) { apellidousuario = value; OnPropertyChanged(); } } }
        public string UsernameUsuario { get => usernameusuario; set { if (usernameusuario != value) { usernameusuario = value; OnPropertyChanged(); } } }
        public string ContraseñaUsuario { get => contraseñausuario; set { if (contraseñausuario != value) { contraseñausuario = value; OnPropertyChanged(); } } }
        public string EmailUsuario { get => emailusuario; set { if (emailusuario != value) { emailusuario = value; OnPropertyChanged(); } } }
        public string TelefonoUsuario { get => telefonousuario; set { if (telefonousuario != value) { telefonousuario = value; OnPropertyChanged(); } } }
        public string DocumentoUsuario { get => documentousuario; set { if (documentousuario != value) { documentousuario = value; OnPropertyChanged(); } } }
        public string DescripcionUsuario { get => descripcionusuario; set { if (descripcionusuario != value) { descripcionusuario = value; OnPropertyChanged(); } } }
        public DateTime FechaContratacion { get => fechacontratacion; set { if (fechacontratacion != value) { fechacontratacion = value; OnPropertyChanged(); } } }
        public string EstadoUsuario { get => estadousuario; set { if (estadousuario != value) { estadousuario = value; OnPropertyChanged(); } } }
        public DateTime UltimaSesionUsuario { get => ultimasesionusuario; set { if (ultimasesionusuario != value) { ultimasesionusuario = value; OnPropertyChanged(); } } }
        public int IntentosFallidos { get => intentosfallidos; set { if (intentosfallidos != value) { intentosfallidos = value; OnPropertyChanged(); } } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}