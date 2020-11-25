using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
   public class FormacionesTiposValoraciones
    {
        Int64 id;
        dsFormacionesTiposValoracionesTableAdapters.formaciones_tipos_valoracionesTableAdapter q;
        dsFormacionesTiposValoraciones.formaciones_tipos_valoracionesRow datos;
        Boolean tiene_datos;

        public FormacionesTiposValoraciones()
        {
            this.id = -1;
            this.q = new dsFormacionesTiposValoracionesTableAdapters.formaciones_tipos_valoracionesTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionesTiposValoraciones(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionesTiposValoracionesTableAdapters.formaciones_tipos_valoracionesTableAdapter();
            dsFormacionesTiposValoraciones.formaciones_tipos_valoracionesDataTable tabla = this.q.GetDataByID(this.id);

            if(tabla.Count > 0)
            {
                this.tiene_datos = true;
                this.datos = tabla[0];
            }
            else
            {
                this.id = -1;
                this.tiene_datos = false;
            }
        }

        public Int64 GetIDTipoDato()
        {
            return this.datos.idtipovaloracion;
        }

        public Int64 Insertar(Int64 idactividadformacion, String nombre, Int16 orden,
            Int64 idtipodato)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(nombre,idactividadformacion,orden, idtipodato);
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

        public Boolean Actualizar(String nombre, Int32 orden)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(nombre,(Int16)orden, this.datos.id);
                trans.Commit();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, 
                    String.Empty, String.Empty, String.Empty);
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

        public Int64[] GetByIDActividadFormacion(Int64 idactividadformacion)
        {
            Int64[] ids;

            dsFormacionesTiposValoraciones.formaciones_tipos_valoracionesDataTable tabla =
               this.q.GetDataByIDActividadFormacion(idactividadformacion);

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

        public Int64 GetIDActividadFormacion()
        {
            return this.datos.idactividadformacion;
        }

        public String GetNombre()
        {
            return this.datos.nombre;
        }
        
    }
}
