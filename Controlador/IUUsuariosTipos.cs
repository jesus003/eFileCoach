using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class IUUsuariosTipos
    {
        Int64 id;
        dsIUUsuariosTiposTableAdapters.iu_usuarios_tiposTableAdapter q;
        dsIUUsuariosTipos.iu_usuarios_tiposRow datos;
        Boolean tiene_datos;

        public IUUsuariosTipos()
        {
            id = -1;
            this.q = new dsIUUsuariosTiposTableAdapters.iu_usuarios_tiposTableAdapter();
            this.tiene_datos = false;
        }

        public IUUsuariosTipos(Int64 id)
        {
            this.id = id;
            this.q = new dsIUUsuariosTiposTableAdapters.iu_usuarios_tiposTableAdapter();
            dsIUUsuariosTipos.iu_usuarios_tiposDataTable tabla =
                this.q.GetDataByID(this.id);

            if(tabla.Count>0)
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

        public Int64 Insertar(Int64 id, String tipo)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(id, tipo);
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

        public Boolean Actualizar(Int64 id, String tipo)
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
