using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Invitados
    {
        Int64 id;
        dsInvitadosTableAdapters.invitadosTableAdapter q;
        dsInvitados.invitadosRow datos;
        Boolean tiene_datos;

        public Invitados()
        {
            this.id = -1;
            this.q = new dsInvitadosTableAdapters.invitadosTableAdapter();
            this.tiene_datos = false;
        }

        public Invitados(Int64 id)
        {
            this.id = id;
            this.q = new dsInvitadosTableAdapters.invitadosTableAdapter();
            dsInvitados.invitadosDataTable tabla = this.q.GetDataByID(this.id);

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

        public Int64 Insertar(Int64 idcuenta)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idcuenta);
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

        public Boolean Actualizar(Int64 idcuenta)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                
                this.q.Actualizar(idcuenta, this.datos.id);
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

            dsInvitados.invitadosDataTable tabla =
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
    }
}
