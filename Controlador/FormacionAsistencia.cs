using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FormacionAsistencia
    {
        Int64 id;
        dsFormacionAsistenciaTableAdapters.formacion_asistenciaTableAdapter q;
        dsFormacionAsistencia.formacion_asistenciaRow datos;
        Boolean tiene_datos;

        public FormacionAsistencia()
        {
            this.id = -1;
            this.q = new dsFormacionAsistenciaTableAdapters.formacion_asistenciaTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionAsistencia(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionAsistenciaTableAdapters.formacion_asistenciaTableAdapter();
            dsFormacionAsistencia.formacion_asistenciaDataTable tabla =
                this.q.GetDataByID(this.id);

            if (tabla.Count > 0)
            {
                this.tiene_datos = true;
                this.datos = tabla[0];
            }
            else
            {
                this.tiene_datos = false;
                this.id = -1;
            }
        }

        public Int64 Insertar(Int64 idactividad_fecha, Int64 idalumno, Int16 presente, Int16 retraso)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idactividad_fecha, idalumno, presente, retraso);
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

        public Boolean Actualizar(Int64 idactividad_fecha, Int64 idalumno, Int16 presente, Int16 retraso)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idactividad_fecha, idalumno, presente, retraso, this.datos.id);
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

        public Int64[] GetByIDActividadFecha(Int64 idactividad_fecha)
        {
            Int64[] ids;

            dsFormacionAsistencia.formacion_asistenciaDataTable tabla =
               this.q.GetDataByIDActividadFecha(idactividad_fecha);

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

        public Int64[] GetByIDAlumno(Int64 idalumno)
        {
            Int64[] ids;

            dsFormacionAsistencia.formacion_asistenciaDataTable tabla =
               this.q.GetDataByIDAlumno(idalumno);

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

        public Int64 GetIDActividadFecha()
        {
            return this.datos.idactividad_fecha;
        }

        public Int64 GetIDAlumno()
        {
            return this.datos.idalumno;
        }

        public Boolean IsPresente()
        {
            if(this.datos.presente == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean IsRetraso()
        {
            if (this.datos.retraso == 1)
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
