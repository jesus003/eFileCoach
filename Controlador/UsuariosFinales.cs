using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class UsuariosFinales
    {
        Int64 id;
        dsUsuariosFinalesTableAdapters.usuarios_finalesTableAdapter q;
        dsUsuariosFinales.usuarios_finalesRow datos;
        Boolean tiene_datos;
        IUUsuariosFinalesTipos tipo_cliente;

        public UsuariosFinales()
        {
            this.id = -1;
            this.q = new dsUsuariosFinalesTableAdapters.usuarios_finalesTableAdapter();
            this.tiene_datos = false;
        }

        public static Boolean ExisteEmail(String email, Int64 idcuenta)
        {
            dsUsuariosFinalesTableAdapters.usuarios_finalesTableAdapter q_usuarios_finales
                = new dsUsuariosFinalesTableAdapters.usuarios_finalesTableAdapter();
            if (q_usuarios_finales.GetDataByIDCuentaEmail(idcuenta,email.Trim().ToLower()).Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public UsuariosFinales(Int64 id)
        {
            this.id = id;
            this.q = new dsUsuariosFinalesTableAdapters.usuarios_finalesTableAdapter();
            dsUsuariosFinales.usuarios_finalesDataTable tabla =
                this.q.GetDataByID(this.id);
            if (tabla.Count > 0)
            {
                this.datos = tabla[0];
                tiene_datos = true;
                this.tipo_cliente = new IUUsuariosFinalesTipos(this.GetIDTipo());
            }
            else
            {
                this.id = -1;
                this.tiene_datos = false;
            }
        }

        public UsuariosFinales(String email)
        {
            this.q = new dsUsuariosFinalesTableAdapters.usuarios_finalesTableAdapter();
            dsUsuariosFinales.usuarios_finalesDataTable tabla =
                this.q.GetDataByEmail(email.Trim().ToLower());
            if (tabla.Count > 0)
            {
                this.datos = tabla[0];
                tiene_datos = true;
                this.id = this.datos.id;
                this.tipo_cliente = new IUUsuariosFinalesTipos(this.GetIDTipo());
            }
            else
            {
                this.id = -1;
                this.tiene_datos = false;
            }
        }

        public String GetTipoCliente()
        {
            return this.tipo_cliente.GetTipo();
        }

        public Int64 Insertar(Int64 idcuenta, Int64 idtipo, String email, String password,
            String nombre, String apellidos, String otra_empresa, String nif,
                    String direccion, String cod_postal, String localidad,
                    String provincia, String telefono, String movil, Nullable<DateTime> fecha_nacimiento,
                    String observaciones, DateTime fecha_alta)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                password = Interfaz.GetMD5Hash(password);
                this.q.Insertar(idcuenta, idtipo, email, password, nombre, apellidos, otra_empresa, nif
                    , direccion, cod_postal, localidad, provincia, telefono, movil, fecha_nacimiento, observaciones, fecha_alta);

                Int64 last_id = this.q.GetDataByLastID()[0].id;
                trans.Commit();
                return last_id;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty);
                return -1;
            }
        }

        public Boolean Actualizar(Int64 idtipo, String email, String password,
            String nombre, String apellidos, String otra_empresa, String nif,
                    String direccion, String cod_postal, String localidad,
                    String provincia, String telefono, String movil, Nullable<DateTime> fecha_nacimiento,
                    String observaciones, DateTime fecha_alta)
        {
            try
            {
                this.q.Actualizar(idtipo, email, nombre, apellidos, otra_empresa, nif, 
                    direccion, cod_postal, localidad, provincia, telefono, movil, fecha_nacimiento, 
                    observaciones, this.datos.id);
                if (password!=String.Empty)
                {
                    this.q.ActualizarClave(Interfaz.GetMD5Hash(password.Trim()), this.id);
                }
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

        public Int64 GetByEmail(String email)
        {
            dsUsuariosFinales.usuarios_finalesDataTable tabla = this.q.GetDataByEmail(email);

            if (tabla.Count == 1)
            {
                return tabla[0].id;
            }
            else
            {
                return -1;
            }
        }

        public Int64[] GetByIDCuenta(Int64 idcuenta)
        {
            Int64[] ids;

            dsUsuariosFinales.usuarios_finalesDataTable tabla =
                this.q.GetDataByIDCuenta(idcuenta);

            if(tabla.Count > 0)
            {
                ids = new Int64[tabla.Count];
                for(int i=0; i< tabla.Count; i++)
                {
                    ids[i] = tabla[i].id;
                }
            }
            else
            {
                ids = new Int64[1];
                ids[0] = -1;
            }

            return ids;
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

        public Int64 GetIDTipo()
        {
            return this.datos.idtipo;
        }

        public String GetEmail()
        {
            return this.datos.email;
        }

        public String GetPassword()
        {
            return this.datos.password;
        }

        public String GetNombre()
        {
            return this.datos.nombre;
        }

        public String GetApellidos()
        {
            return this.datos.apellidos;
        }

        public String GetOtraEmpresa()
        {
            return this.datos.otra_empresa;
        }

        public String GetNIF()
        {
            return this.datos.nif;
        }

        public String GetDireccion()
        {
            return this.datos.direccion;
        }

        public String GetCodigoPostal()
        {
            return this.datos.cod_postal;
        }

        public String GetLocalidad()
        {
            return this.datos.localidad;
        }

        public String GetProvincia()
        {
            return this.datos.provincia;
        }

        public String GetTelefono()
        {
            return this.datos.telefono;
        }

        public String GetMovil()
        {
            return this.datos.movil;
        }

        public Nullable<DateTime> GetFechaNacimiento()
        {
            if(this.datos.Isfecha_nacimientoNull())
            {
                return null;
            }
            else
            {
                return this.datos.fecha_nacimiento;
            }
        }

        public String GetObservaciones()
        {
            return this.datos.observaciones;
        }

        public DateTime GetFechaAlta()
        {
            return this.datos.fecha_alta;
        }
        
    }
}
