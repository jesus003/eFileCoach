using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FormacionesMaterias
    {
        Int64 id;
        dsFormacionesMateriasTableAdapters.formaciones_materiasTableAdapter q;
        dsFormacionesMaterias.formaciones_materiasRow datos;
        Boolean tiene_datos;

        public FormacionesMaterias()
        {
            this.id = -1;
            this.q = new dsFormacionesMateriasTableAdapters.formaciones_materiasTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionesMaterias(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionesMateriasTableAdapters.formaciones_materiasTableAdapter();
            dsFormacionesMaterias.formaciones_materiasDataTable tabla =
                this.q.GetDataByID(this.id);

            if(tabla.Count > 0)
            {
                this.datos = tabla[0];
                this.tiene_datos = true;
            }
            else
            {
                this.tiene_datos = false;
                this.id = -1;
            }
        }

        public Int64 Insertar(Int64 idcuenta, String nombre)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idcuenta, nombre);
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

        public Boolean Actualizar(Int64 idcuenta, String nombre)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idcuenta, nombre, this.datos.id);
                trans.Commit();
                return false;
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
                return false;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, String.Empty, String.Empty, String.Empty);
                return false;
            }
        }

        public Int64[] GetByIDCuenta(Int64 idcuenta)
        {
            Int64[] ids;

            dsFormacionesMaterias.formaciones_materiasDataTable tabla =
               this.q.GetDataByIDCuenta(idcuenta);

            if (tabla.Count > 0)
            {
                ids = new Int64[tabla.Count];
                for (int i = 0; i < tabla.Count; i++)
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

        public String GetNombre()
        {
            return this.datos.nombre;
        }
    }
}
