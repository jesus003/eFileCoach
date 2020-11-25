using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FormacionesActividadesFechas
    {
        Int64 id;
        dsFormacionesActividadesFechasTableAdapters.formaciones_actividades_fechasTableAdapter q;
        dsFormacionesActividadesFechas.formaciones_actividades_fechasRow datos;
        Boolean tiene_datos;

        public FormacionesActividadesFechas()
        {
            this.id = -1;
            this.q = new dsFormacionesActividadesFechasTableAdapters.formaciones_actividades_fechasTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionesActividadesFechas(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionesActividadesFechasTableAdapters.formaciones_actividades_fechasTableAdapter();

            dsFormacionesActividadesFechas.formaciones_actividades_fechasDataTable tabla =
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

        public Int64 Insertar(Int64 idformacion_actividad, DateTime fecha_inicio, DateTime fecha_fin)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idformacion_actividad, fecha_inicio, fecha_fin);
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

        public Boolean Actualizar(Int64 idformacion_actividad, DateTime fecha_inicio, DateTime fecha_fin)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idformacion_actividad, fecha_inicio, fecha_fin, this.datos.id);
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

        public Int64[] GetByIDFormacionActividad(Int64 idformacion_actividad)
        {
            Int64[] ids;

            dsFormacionesActividadesFechas.formaciones_actividades_fechasDataTable tabla =
                this.q.GetDataByIDFormacionActividad(idformacion_actividad);

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

        public Int64 GetIDFormacionActividad()
        {
            return this.datos.idformacion_actividad;
        }

        public DateTime GetFechaInicio()
        {
            return this.datos.fecha_inicio;
        }

        public DateTime GetFechaFin()
        {
            return this.datos.fecha_fin;
        }
    }
}
