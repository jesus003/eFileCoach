using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class ProcesosSesiones
    {
        Int64 id;
        dsProcesosSesionesTableAdapters.procesos_sesionesTableAdapter q;
        dsProcesosSesiones.procesos_sesionesRow datos;
        Boolean tiene_datos;

        public ProcesosSesiones()
        {
            this.id = -1;
            this.q = new dsProcesosSesionesTableAdapters.procesos_sesionesTableAdapter();
            this.tiene_datos = false;
        }

        public ProcesosSesiones(Int64 id)
        {
            this.id = id;
            this.q = new dsProcesosSesionesTableAdapters.procesos_sesionesTableAdapter();

            dsProcesosSesiones.procesos_sesionesDataTable tabla = this.q.GetDataByID(this.id);
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

        public Int64 Insertar(Int64 idproceso, Int64 idcoach)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idproceso, idcoach);
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

        public Boolean Actualizar(Int64 idproceso, Int64 idcoach)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idproceso, idcoach, this.datos.id);
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

        public Int64[] GetByIDProceso(Int64 idproceso)
        {
            Int64[] ids;

            dsProcesosSesiones.procesos_sesionesDataTable tabla =
                this.q.GetDataByIDProceso(idproceso);

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

        public Int64[] GetByIDCoach(Int64 idcoach)
        {
            Int64[] ids;

            dsProcesosSesiones.procesos_sesionesDataTable tabla =
                this.q.GetDataByIDCoach(idcoach);

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

        public Int64 GetIDProceso()
        {
            return this.datos.idproceso;
        }

        public Int64 GetIDCoach()
        {
            return this.datos.idcoach;
        }
    }
}
