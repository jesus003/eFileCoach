using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Usuarios
    {
        Int64 id;
        dsUsuariosTableAdapters.usuariosTableAdapter q;
        dsUsuarios.usuariosRow datos;
        Boolean tiene_datos;
        IUUsuariosTipos tipo_usuario;

        public Usuarios()
        {
            id = -1;
            this.q = new dsUsuariosTableAdapters.usuariosTableAdapter();
            this.tiene_datos = false;
        }

        public Usuarios(Int64 id)
        {
            this.id = id;
            this.q = new dsUsuariosTableAdapters.usuariosTableAdapter();
            dsUsuarios.usuariosDataTable tabla =
                this.q.GetDataByID(this.id);
            
            if (tabla.Count > 0)
            {
                this.datos = tabla[0];
                this.tiene_datos = true;
                this.tipo_usuario = new IUUsuariosTipos(this.GetIDTipo());
            }
            else
            {
                this.id = -1;
                this.tiene_datos = false;
            }
        }

        public Usuarios(String login)
        {
            this.q = new dsUsuariosTableAdapters.usuariosTableAdapter();
            dsUsuarios.usuariosDataTable tabla =
                this.q.GetDataByEmail(login.Trim().ToLower());
            if (tabla.Count > 0)
            {
                this.datos = tabla[0];
                this.tiene_datos = true;
                this.id = datos.id;
                this.tipo_usuario = new IUUsuariosTipos(this.GetIDTipo());
            }
            else
            {
                this.id = -1;
                this.tiene_datos = false;
            }
        }

        public Boolean SyncGoogleCalendar()
        {
            return Convert.ToBoolean(this.datos.google_calendar);
        }

        public Boolean SetGoogleCalendar(Boolean google_calendar)
        {
            try
            {
                this.q.ActualizarGoogleCalendar(Convert.ToInt32(google_calendar), this.id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean MostrarCalendario()
        {
            return Convert.ToBoolean(this.datos.calendario);
        }

        public String GetTipoUsuario()
        {
            return this.tipo_usuario.GetTipo();
        }

        /// <summary>
        /// Método que nos indica si hay alguna cuenta que use el correo
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Boolean EmailEnUso(String email)
        {
            dsUsuarios.usuariosDataTable tabla =
                this.q.GetDataByEmailIDDiferente(email.Trim().ToLower(), this.id);
            if (tabla.Count==0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean GetAutentico(String clave)
        {
            if (Interfaz.GetMD5Hash(clave.Trim()) == this.GetClave())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean GetUsuarioPrincipal()
        {
            return Convert.ToBoolean(this.datos.usuario_principal);
        }

        public Int64 Insertar(Int64 idcuenta, Int64 idtipo, String email, String clave, String nombre, String apellidos,
            String telefono, String movil, String direccion, String cod_postal, String poblacion,
                    String provincia, String nif, DateTime fecha_alta, Int16 activo, Boolean usuario_principal)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                if (clave.Trim()!=String.Empty)
                {
                    clave = Interfaz.GetMD5Hash(clave);
                }
                this.q.Insertar(idcuenta, idtipo, email, clave, nombre, apellidos, telefono,
                    movil, direccion, cod_postal, poblacion,
                    provincia, nif, fecha_alta, activo,Convert.ToInt32(usuario_principal));
                Int64 last_id = this.q.GetDataByLastID()[0].id;
                trans.Commit();
                return last_id;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, 
                    String.Empty, String.Empty, String.Empty);
                return -1;
            }
        }

        public Boolean Actualizar(Int64 idtipo, String email, String clave,
            String nombre, String apellidos,
            String telefono, String movil, String direccion, 
            String cod_postal, String poblacion,
                    String provincia, String nif, Int16 activo)
        {

            try
            {
                this.q.Actualizar(idtipo, email, nombre, apellidos, telefono,
                    movil, direccion, cod_postal, poblacion,
                    provincia, nif, activo, this.datos.id);
                if (clave!=String.Empty)
                {
                    return this.ActualizarClave(clave.Trim());
                }
                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty);
                return false;
            }
        }

        public Boolean ActualizarClave(String clave)
        {
            clave = Interfaz.GetMD5Hash(clave);
            try
            {
                this.q.ActualizarClave(clave, this.datos.id);
                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty);
                return false;
            }
        }

        public Boolean Eliminar()
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Eliminar(this.datos.id);
                trans.Commit();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty);
                return false;
            }
        }

        public Int64 GetIDByEmail(String email)
        {
            Int64 last_id;

            dsUsuarios.usuariosDataTable tabla = this.q.GetDataByEmail(email);

            if (tabla.Count == 1)
            {
                return tabla[0].id;
            }
            else
            {
                return -1;
            }
        }


        public Int64 GetByLogin(String email, String clave)
        {
            Int64 last_id;

            clave = Interfaz.GetMD5Hash(clave);

            dsUsuarios.usuariosDataTable tabla = this.q.GetDataByLogin(email, clave);

            if (tabla.Count == 1)
            {
                return tabla[0].id;
            }
            else
            {
                return -1;
            }
        }

        public Int64[] GetUsuariosByCuenta(Int64 idcuenta)
        {
            Int64[] idsusuarios;

            dsUsuarios.usuariosDataTable tabla =
                this.q.GetDataByIDCuenta(idcuenta);

            if (tabla.Count > 0)
            {
                idsusuarios = new Int64[tabla.Count];
                for (int i = 0; i < tabla.Count; i++)
                {
                    idsusuarios[i] = tabla[i].id;
                }
            }
            else
            {
                idsusuarios = new Int64[1];
                idsusuarios[0] = -1;
            }


            return idsusuarios;
        }

        public Boolean TieneDatos()
        {
            return this.tiene_datos;
        }

        public Int64 GetID()
        {
            return this.datos.id;
        }

        public Int64 GetIDCuenta()
        {
            return this.datos.idcuenta;
        }

        public DateTime GetFechaAlta()
        {
            return this.datos.fecha_alta;
        }

        public Int64 GetIDTipo()
        {
            return this.datos.idtipo;
        }

        public String GetEmail()
        {
            return this.datos.email;
        }

        public String GetClave()
        {
            return this.datos.clave;
        }

        public String GetNombre()
        {
            return this.datos.nombre;
        }

        public String GetApellidos()
        {
            return this.datos.apellidos;
        }

        public String GetTelefono()
        {
            return this.datos.telefono;
        }

        public String GetMovil()
        {
            return this.datos.movil;
        }

        public String GetDireccion()
        {
            return this.datos.direccion;
        }

        public String GetCodigoPostal()
        {
            return this.datos.cod_postal;
        }

        public String GetPoblacion()
        {
            return this.datos.poblacion;
        }

        public String GetProvincia()
        {
            return this.datos.provincia;
        }

        public String GetNIF()
        {
            return this.datos.nif;
        }

        public Boolean IsActivo()
        {
            if(this.datos.activo == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
