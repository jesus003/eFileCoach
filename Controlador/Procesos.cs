using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Procesos
    {
        Int64 id;
        dsProcesosTableAdapters.procesosTableAdapter q;
        dsProcesos.procesosRow datos;
        Boolean tiene_datos;

        public Procesos()
        {
            this.id = -1;
            this.q = new dsProcesosTableAdapters.procesosTableAdapter();
            this.tiene_datos = false;
        }

        public Procesos(Int64 id)
        {
            this.id = id;
            this.q = new dsProcesosTableAdapters.procesosTableAdapter();
            dsProcesos.procesosDataTable tabla =
                this.q.GetDataByID(id);

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

        

        public static Boolean GetCoachAsignado(Int64 idformador, Int64 idproceso)
        {
            dsFormacionesTableAdapters.cuantas_formacionesTableAdapter q_cuantas =
                new dsFormacionesTableAdapters.cuantas_formacionesTableAdapter();
            try
            {
                if (q_cuantas.GetData(idformador, idproceso)[0].cuantas > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public Int64 Insertar(Int64 idcuenta, Int64 idcoach, DateTime fecha_inicio,
            Nullable<DateTime>fecha_fin, Int16 disponible)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idcuenta, idcoach, fecha_inicio, fecha_fin, disponible);
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

        public Boolean Actualizar(Int64 idcuenta, Int64 idcoach, DateTime fecha_inicio,
            Nullable<DateTime> fecha_fin, Int16 disponible)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idcuenta, idcoach, fecha_inicio, fecha_fin, disponible, this.datos.id);
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

        public Int64[] GetByIDCuenta(Int64 idcuenta)
        {
            Int64[] ids;

            dsProcesos.procesosDataTable tabla =
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

        public Int64[] GetByIDCoach(Int64 idcoach)
        {
            Int64[] ids;

            dsProcesos.procesosDataTable tabla =
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

        public Int64 GetIDCuenta()
        {
            return this.datos.idcuenta;
        }

        public Int64 GetIDCoach()
        {
            return this.datos.idcoach;
        }

        public DateTime GetFechaInicio()
        {
            return this.datos.fecha_inicio;
        }

        public Nullable<DateTime> GetFechaFin()
        {
            if(this.datos.Isfecha_finNull())
            {
                return null;
            }
            else
            {
                return this.datos.fecha_fin;
            }
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


    }
}
