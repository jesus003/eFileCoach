using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FormacionesActividadesAlumnos
    {
        Int64 id;
        dsFormacionesActividadesAlumnosTableAdapters.formaciones_actividades_alumnosTableAdapter q;
        dsFormacionesActividadesAlumnos.formaciones_actividades_alumnosRow datos;
        Boolean tiene_datos;


        public FormacionesActividadesAlumnos()
        {
            this.id = -1;
            this.q = new dsFormacionesActividadesAlumnosTableAdapters.formaciones_actividades_alumnosTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionesActividadesAlumnos(Int64 id)
        {
            this.id = id;

            this.q = new dsFormacionesActividadesAlumnosTableAdapters.formaciones_actividades_alumnosTableAdapter();
            dsFormacionesActividadesAlumnos.formaciones_actividades_alumnosDataTable tabla =
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

        public Int64 Insertar(Int64 idformacion_actividad, Int64 idalumno)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idformacion_actividad, idalumno);
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

        public Boolean InsertarAlumnosAsignadosFormacion(Int64 idformacion, Int64 idactividad)
        {
            try
            {
                this.q.InsertarAlumnosAsignados(idactividad.ToString(), idformacion);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean Actualizar(Int64 idformacion_actividad, Int64 idalumno)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idformacion_actividad, idalumno, this.datos.id);
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

        public Boolean GetAsistencia()
        {
            return Convert.ToBoolean(this.datos.asistencia);
        }

        public Boolean ActualizarAsistencia(Boolean asiste)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.ActualizarAsistencia(Convert.ToInt32(asiste), this.datos.id);
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

        public Boolean Eliminar(Int64 idactividad,Int64 idalumno)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.EliminarIDFormacionIDAlumno(idactividad,idalumno);
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

            dsFormacionesActividadesAlumnos.formaciones_actividades_alumnosDataTable tabla =
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

        public Int64[] GetByIDAlumno(Int64 idalumno)
        {
            Int64[] ids;

            dsFormacionesActividadesAlumnos.formaciones_actividades_alumnosDataTable tabla =
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

        public Int64 GetIDAlumno()
        {
            return this.datos.idalumno;
        }

        public Int64 GetIDFormacionActividad()
        {
            return this.datos.idformacion_actividad;
        }

        public Int64 GetID()
        {
            return this.datos.id;
        }
    }
}
