using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventDressApp.MVVM.Model
{
    public class UserSession
    {
        private static UserSession _instance;
        private static readonly object _lock = new object();

        public int UsuarioId { get; private set; }
        public int EmpresaId { get; private set; }
        public string RolId { get; private set; }
        public string NombreCompleto { get; private set; }
        public string Email { get; private set; }
        public DateTime UltimaSesion { get; private set; }

        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserSession();
                        }
                    }
                }
                return _instance;
            }
        }

        public void IniciarSesion(Usuario usuario)
        {
            UsuarioId = usuario.UsuarioId;
            EmpresaId = usuario.EmpresaId;
            RolId = usuario.RolId;
            NombreCompleto = $"{usuario.NombreUsuario} {usuario.ApellidoUsuario}";
            Email = usuario.EmailUsuario;
            UltimaSesion = DateTime.Now;
        }

        public void CerrarSesion()
        {
            _instance = null;
        }

        public bool TienePermiso(string[] rolesPermitidos)
        {
            return Array.Exists(rolesPermitidos, rol => rol.Equals(RolId, StringComparison.OrdinalIgnoreCase));
        }
    }
}