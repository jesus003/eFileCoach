using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class CSesion
    {
        Usuarios usuario;
        Cuentas cuenta;
        String tipocuenta;
        Int64 id;
        public CSesion(Int64 id, String tipocuenta, Usuarios usuario, Cuentas cuenta)
        {
            this.id = id;
            this.tipocuenta = tipocuenta;
            this.usuario = usuario;
            this.cuenta = cuenta;
        }

        public Boolean EsCuenta()
        {
            if (this.tipocuenta == "cuenta")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean EsUsuario()
        {
            if (this.tipocuenta == "usuario")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Usuarios GetUsuario()
        {
            return this.usuario;
        }

        public Cuentas GetCuenta()
        {
            return this.cuenta;
        }

        public void ActualizarCuenta(Cuentas cuenta)
        {
            this.cuenta = cuenta;
        }

        public void ActualizarUsuario(Usuarios usuario)
        {
            this.usuario = usuario;
        }
    }
}
