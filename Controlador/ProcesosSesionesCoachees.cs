using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class ProcesosSesionesCoachees
    {
        Int64 id;
        dsProcesosSesionesCoacheesTableAdapters.procesos_sesiones_coacheesTableAdapter q;
        dsProcesosSesionesCoachees.procesos_sesiones_coacheesRow datos;
        Boolean tiene_datos;

        public ProcesosSesionesCoachees()
        {
            this.id = -1;
            this.q = new dsProcesosSesionesCoacheesTableAdapters.procesos_sesiones_coacheesTableAdapter();
            tiene_datos = false;
        }

        public ProcesosSesionesCoachees(Int64 id)
        {
            this.id = id;
            this.q = new dsProcesosSesionesCoacheesTableAdapters.procesos_sesiones_coacheesTableAdapter();
            dsProcesosSesionesCoachees.procesos_sesiones_coacheesDataTable tabla =
                this.q.GetDataByID(this.id);

            if(tabla.Count > 0)
            {
                this.datos = tabla[0];
                this.tiene_datos = false;
            }
            else
            {
                this.tiene_datos = false;
                this.id = -1;
            }
        }

        public Int64 Insertar(Int64 idsesion, Int64 idcoachee)
        {

            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idsesion, idcoachee);
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

        public Boolean Actualizar(Int64 idsesion, Int64 idcoachee)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idsesion, idcoachee, this.datos.id);
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

        public Int64[] GetByIDSesion(Int64 idsesion)
        {

            Int64[] ids;

            dsProcesosSesionesCoachees.procesos_sesiones_coacheesDataTable tabla =
                this.q.GetDataByIDSesion(idsesion);

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

        public Int64[] GetByIDCoachee(Int64 idcoachee)
        {

            Int64[] ids;

            dsProcesosSesionesCoachees.procesos_sesiones_coacheesDataTable tabla =
                this.q.GetDataByIDCoachee(idcoachee);

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

        public Int64 GetIDSesion()
        {
            return this.datos.idsesion;
        }

        public Int64 GetIDCoachee()
        {
            return this.datos.idcoachee;
        }

    }
}
