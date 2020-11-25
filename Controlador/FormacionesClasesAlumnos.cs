using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FormacionesClasesAlumnos
    {
        Int64 id;
        dsFormacionesClasesAlumnosTableAdapters.formaciones_clases_alumnosTableAdapter q;
        dsFormacionesClasesAlumnos.formaciones_clases_alumnosRow datos;
        Boolean tiene_datos;

        public FormacionesClasesAlumnos()
        {
            this.id = -1;
            this.q = new dsFormacionesClasesAlumnosTableAdapters.formaciones_clases_alumnosTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionesClasesAlumnos(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionesClasesAlumnosTableAdapters.formaciones_clases_alumnosTableAdapter();
            dsFormacionesClasesAlumnos.formaciones_clases_alumnosDataTable tabla =
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

        public Int64 Insertar(Int64 idclase, Int64 idalumno)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idclase, idalumno);
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

        public Boolean Actualizar(Int64 idclase, Int64 idalumno)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idclase, idalumno, this.datos.id);
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

        public Int64[] GetByIDClase(Int64 idclase)
        {
            Int64[] ids;

            dsFormacionesClasesAlumnos.formaciones_clases_alumnosDataTable tabla =
                this.q.GetDataByIDClase(idclase);

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

            dsFormacionesClasesAlumnos.formaciones_clases_alumnosDataTable tabla =
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

        public Int64 GetIDAlumno()
        {
            return this.datos.idalumno;
        }

        public Int64 GetIDClase()
        {
            return this.datos.idclase;
        }

    }
}
