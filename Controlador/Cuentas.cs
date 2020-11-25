using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Cuentas
    {
        Int64 id;
        dsCuentasTableAdapters.cuentasTableAdapter q;
        dsCuentas.cuentasRow datos;
        Boolean tiene_datos;
        IUTiposDeCuenta tipo_cuenta;
        Int64 idusuario_principal;

        public Cuentas()
        {
            id = -1;
            this.q = new dsCuentasTableAdapters.cuentasTableAdapter();
            tiene_datos = false;
        }

        public Cuentas(Int64 id)
        {
            this.id = id;
            this.q = new dsCuentasTableAdapters.cuentasTableAdapter();
            dsCuentas.cuentasDataTable tabla =
                this.q.GetDataByID(this.id);
            if (tabla.Count > 0)
            {
                this.datos = tabla[0];
                tiene_datos = true;
                this.tipo_cuenta = new IUTiposDeCuenta(this.GetIDTipo());
            }
            else
            {
                this.id = -1;
                tiene_datos = false;
            }
        }

        public Cuentas(String login)
        {
            this.q = new dsCuentasTableAdapters.cuentasTableAdapter();
            dsCuentas.cuentasDataTable tabla =
                this.q.GetDataByEmail(login);
            if (tabla.Count>0)
            {
                this.tiene_datos = true;
                this.datos = tabla[0];
                this.id = this.datos.id;
                this.tipo_cuenta = new IUTiposDeCuenta(this.GetIDTipo());
            }
            else
            {
                this.id = -1;
                tiene_datos = false;
            }
        }

        public static Int32 GetEncuestasContador(Int64 idcuenta)
        {
            Diagnosticos.dsDiagnosticosTableAdapters.vista_diagnosticos_contadorTableAdapter q
                = new Diagnosticos.dsDiagnosticosTableAdapters.vista_diagnosticos_contadorTableAdapter();
            try
            {
                return q.GetData(idcuenta).Count;
            }
            catch
            {
                return 0;
            }
        }

        public Boolean MostrarCalendario()
        {
            return Convert.ToBoolean(this.datos.calendario);
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

        public Boolean SyncGoogleCalendar()
        {
            return Convert.ToBoolean(this.datos.google_calendar);
        }

        public Byte[] GetLogotipo()
        {
            if (this.datos.IslogotipoNull())
            {
                return null;
            }
            else
            {
                return this.datos.logotipo;
            }
        }

        public Boolean GetPublicaPlantillas()
        {
            if (this.datos.Ispublica_plantillasNull())
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(this.datos.publica_plantillas);
            }
        }

        public Int64 GetIDUsuarioPrincipal()
        {
            try
            {
                dsUsuariosTableAdapters.usuariosTableAdapter q_usuarios =
                    new dsUsuariosTableAdapters.usuariosTableAdapter();
                return q_usuarios.GetDataUsuarioPrincipalByIDCuenta(this.id)[0].id;
            }
            catch
            {
                return -1;
            }
        }

        public String GetTipoDeCuenta()
        {
            return this.tipo_cuenta.GetTipoCuenta();
        }

        public Boolean ActualizarLogo(Byte[] logotipo)
        {
            try
            {
                this.q.ActualizarLogotipo(logotipo, this.id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean GetAutentico(String clave)
        {
            if (Interfaz.GetMD5Hash(clave.Trim())==this.GetClave())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public String GetSMTPFirma()
        {
            try
            {
                return this.datos.smtp_firma;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Método que nos indica si hay alguna cuenta que use el correo
        /// </summary>
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean EmailEnUso(String email)
        {
            dsCuentas.cuentasDataTable tabla =
                this.q.GetDataByEmailIDDiferente(email.Trim().ToLower(), this.id);
            if (tabla.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Int64 Insertar(Int64 idtipo, String email, String clave, String nombre, String apellidos, String telefono, String movil,
            String direccion, String cod_postal, String  poblacion, String provincia, String nif, String cuenta_bancaria, DateTime fecha_alta,
            String ip_alta, Nullable<DateTime> fecha_baja, Int16 activo, String api_mailchimp,
                    String smtp_from, String smtp_usuario, String smtp_clave, Int16 smtp_ssl, Int32 smtp_puerto,
                String smtp_from_txt)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                
                this.q.Insertar(idtipo, email.Trim().ToLower(), clave.Trim(), nombre.Trim(), apellidos.Trim(), 
                    telefono.Trim(), movil.Trim(), direccion.Trim(), cod_postal.Trim(),
                    poblacion.Trim(), provincia.Trim(), nif.Trim(), cuenta_bancaria.Trim(), fecha_alta, ip_alta, fecha_baja, 
                    activo, api_mailchimp.Trim(),
                    smtp_from.Trim(), smtp_usuario.Trim(), smtp_clave.Trim(), smtp_ssl, smtp_puerto,
                    smtp_from_txt.Trim(),String.Empty, String.Empty);
                Int64 last_id = this.q.GetDataByLastID()[0].id;
                trans.Commit();

                //Damos de alta el usuario por defecto
                Usuarios u = new Usuarios();
                if (u.Insertar(last_id,3,email.Trim().ToLower(),"","Usuario", "Principal",String.Empty, String.Empty,
                    String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,DateTime.Now,
                    1,true) !=-1)
                {
                    return last_id;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                trans.Rollback();
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty, e.StackTrace);
                return -1;
            }
        }

        public Boolean Actualizar(Int64 idtipo, String email, String clave, String nombre, String apellidos, String telefono, String movil,
            String direccion, String cod_postal, String poblacion, String provincia, String nif, String cuenta_bancaria, DateTime fecha_alta,
            String ip_alta, Nullable<DateTime> fecha_baja, Int16 activo, String api_mailchimp,
                    String smtp_from, String smtp_usuario, String smtp_clave, Int16 smtp_ssl, Int32 smtp_puerto)
        {
            try
            {
                this.q.Actualizar(idtipo, email, clave, nombre, apellidos, telefono, movil, direccion, cod_postal,
                    poblacion, provincia, nif, cuenta_bancaria, fecha_alta, ip_alta, fecha_baja, activo, api_mailchimp,
                    smtp_from, smtp_usuario, smtp_clave, smtp_ssl, smtp_puerto, this.datos.id);
                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty, e.StackTrace);
                return false;
            }
        }

        public Boolean ActualizarPrincipal(String email, String nombre,
            String apellidos, String telefono, 
            String movil, String direccion, String cod_postal, String poblacion,
            String provincia, String nif)
        {
            try
            {
                this.q.ActualizarPrincipal(email.Trim().ToLower(), nombre.Trim(), apellidos.Trim(),
                    telefono.Trim(), movil.Trim(), direccion.Trim(), cod_postal.Trim(),
                    poblacion.Trim(), provincia.Trim(), nif.Trim(), this.datos.id);
                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty, e.StackTrace);
                return false;
            }
        }

        public Boolean ActualizarSuscripcion(String iban)
        {
            try
            {
                this.q.ActualizarSuscripcion(iban, this.datos.id);
                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty, e.StackTrace);
                return false;
            }
        }

        public Boolean ActualizarCorreo(String api_mailchimp,
                    String smtp_from, String smtp_usuario, String smtp_clave, Int16 smtp_ssl, Int32 smtp_puerto,
                    String smtp_from_txt, String smtp_servidor, String firma)
        {
            try
            {
                this.q.ActualizarCorreo(api_mailchimp.Trim(),
                    smtp_from.Trim(), smtp_usuario.Trim(), smtp_clave.Trim(), smtp_ssl, smtp_puerto,
                    smtp_from_txt.Trim(), smtp_servidor.Trim(), firma,this.datos.id);
                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty, e.StackTrace);
                return false;
            }
        }

        public Boolean ActualizarClave(String clave)
        {
            clave = Interfaz.GetMD5Hash(clave.Trim());
            try
            {
                this.q.ActualizarClave(clave, this.datos.id);
                return true;
            }
            catch (Exception e)
            {
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty, e.StackTrace);
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
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty, e.StackTrace);
                return false;
            }
        }

        public Int64 GetIDByEmail(String email)
        {
            Int64 last_id;

            dsCuentas.cuentasDataTable tabla = this.q.GetDataByEmail(email);

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

            dsCuentas.cuentasDataTable tabla = this.q.GetDataByLogin(email, clave);

            if(tabla.Count==1)
            {
                return tabla[0].id;
            }
            else
            {
                return -1;
            }
        }

        public Boolean TieneDatos()
        {
            return this.tiene_datos;
        }

        public Int64 GetID()
        {
            return this.datos.id;
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

        public String GetCuentaBancaria()
        {
            return this.datos.cuenta_bancaria;
        }

        public DateTime GetFechaAlta()
        {
            return this.datos.fecha_alta;
        }

        public String GetIPAlta()
        {
            return this.datos.ip_alta;
        }

        public Nullable<DateTime> GetFechaBaja()
        {
            if(this.datos.Isfecha_bajaNull())
            {
                return null;
            }
            else
            {
                return this.datos.fecha_baja;
            }
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
            
        public String GetAPIMailchimp()
        {
            return this.datos.api_mailchimp;
        }

        public String GetSMTPFrom()
        {
            return this.datos.smtp_from;
        }

        public String GetSMTPUsuario()
        {
            return this.datos.smtp_usuario;
        }

        public String GetSMTPClave()
        {
            return this.datos.smtp_clave;
        }

        public String GetSMTPFromTxt()
        {
            return this.datos.smtp_from_txt;
        }

        public Boolean GetSMTPSSL()
        {
            return Convert.ToBoolean(this.datos.smtp_ssl);
        }

        public String GetSMTPServidor()
        {
            return this.datos.smtp_servidor;
        }

        public Boolean IsSSLSMTPEnabled()
        {
            if(this.datos.smtp_ssl == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Int32 GetSMTPPuerto()
        {
            return this.datos.smtp_puerto;
        }


    }
}
