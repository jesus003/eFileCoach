using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FormacionesActividadesValoracionesAlumnos
    {
        Int64 id;
        dsFormacionesActividadesValoracionesAlumnosTableAdapters.formaciones_actividades_valoraciones_alumnosTableAdapter q;
        dsFormacionesActividadesValoracionesAlumnos.formaciones_actividades_valoraciones_alumnosRow datos;
        Boolean tiene_datos;

        public FormacionesActividadesValoracionesAlumnos()
        {
            this.id = -1;
            this.q = new dsFormacionesActividadesValoracionesAlumnosTableAdapters.formaciones_actividades_valoraciones_alumnosTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionesActividadesValoracionesAlumnos(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionesActividadesValoracionesAlumnosTableAdapters.formaciones_actividades_valoraciones_alumnosTableAdapter();
            dsFormacionesActividadesValoracionesAlumnos.formaciones_actividades_valoraciones_alumnosDataTable tabla =
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

        public Int64 Insertar(Int64 idformacion_actividad, Int64 idalumno, Int64 idmagnitud, Int32 valor)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idformacion_actividad, idalumno, idmagnitud, valor);
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

        public Boolean Actualizar(Int64 idformacion_actividad, Int64 idalumno, Int64 idmagnitud, Int32 valor)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idformacion_actividad, idalumno, idmagnitud, valor, this.datos.id);
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

            dsFormacionesActividadesValoracionesAlumnos.formaciones_actividades_valoraciones_alumnosDataTable tabla =
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

            dsFormacionesActividadesValoracionesAlumnos.formaciones_actividades_valoraciones_alumnosDataTable tabla =
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

        public Int64 GetIDFormacionActividad()
        {
            return this.datos.idformacion_actividad;
        }

        public Int64 GetIDAlumno()
        {
            return this.datos.idalumno;
        }

        public Int64 GetIDMagnitud()
        {
            return this.datos.idmagnitud;
        }

        public Int32 GetValor()
        {
            return this.datos.valor;
        }

    }
}
