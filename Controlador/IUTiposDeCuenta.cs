using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class IUTiposDeCuenta
    {
        Int64 id;
        dsIUTiposDeCuentaTableAdapters.iu_tipos_de_cuentaTableAdapter q;
        dsIUTiposDeCuenta.iu_tipos_de_cuentaRow datos;
        Boolean tiene_datos;

        public IUTiposDeCuenta()
        {
            this.id = -1;
            this.q = new dsIUTiposDeCuentaTableAdapters.iu_tipos_de_cuentaTableAdapter();
            this.tiene_datos = false;
        }

        public IUTiposDeCuenta(Int64 id)
        {
            this.id = id;
            this.q = new dsIUTiposDeCuentaTableAdapters.iu_tipos_de_cuentaTableAdapter();
            dsIUTiposDeCuenta.iu_tipos_de_cuentaDataTable tabla =
                this.q.GetDataByID(this.id);
            
            if(tabla.Count > 0)
            {
                this.datos = tabla[0];
                this.tiene_datos = true;
            }
            else
            {
                this.id = -1;
                this.tiene_datos = false;
            }
        }

        public Int64 Insertar(String tipo_cuenta, Int16 disponible, Int16 orden)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(tipo_cuenta, disponible, orden);
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

        public Boolean Actualizar(String tipo_cuenta, Int16 disponible, Int16 orden)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(tipo_cuenta, disponible, orden, this.datos.id);
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

        public Boolean TieneDatos()
        {
            return this.tiene_datos;
        }

        public Int64 GetID()
        {
            return this.datos.id;
        }

        public String GetTipoCuenta()
        {
            return this.datos.tipo_cuenta;
        }

        public Boolean IsDisponible()
        {
            if(this.datos.disponible == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Int16 GetOrden()
        {
            return this.datos.orden;
        }
    }
}
