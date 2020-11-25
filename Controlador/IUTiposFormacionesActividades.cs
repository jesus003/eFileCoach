using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class IUTiposFormacionesActividades
    {
        Int64 id;
        dsIUFormacionesTiposActividadesTableAdapters.iu_tipos_formaciones_actividadesTableAdapter q;
        dsIUFormacionesTiposActividades.iu_tipos_formaciones_actividadesRow datos;
        Boolean tiene_datos;

        public IUTiposFormacionesActividades()
        {
            this.id = -1;
            this.q = new dsIUFormacionesTiposActividadesTableAdapters.iu_tipos_formaciones_actividadesTableAdapter();
            this.tiene_datos = false;
        }

        public IUTiposFormacionesActividades(Int64 id)
        {
            this.id = id;
            this.q = new dsIUFormacionesTiposActividadesTableAdapters.iu_tipos_formaciones_actividadesTableAdapter();
            dsIUFormacionesTiposActividades.iu_tipos_formaciones_actividadesDataTable tabla =
                this.q.GetDataByID(this.id);

            if(tabla.Count > 0)
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

        public Int64 Insertar(String tipo)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(tipo);
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

        public Boolean Actualizar(String tipo)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(tipo, this.datos.id);
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

        public Boolean TieneDatos()
        {
            return this.tiene_datos;
        }

        public Int64 GetID()
        {
            return this.datos.id;
        }

        public String GetTipo()
        {
            return this.datos.tipo;
        }
    }
}
