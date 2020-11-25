using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Formaciones
    {
        Int64 id;
        dsFormacionesTableAdapters.formacionesTableAdapter q;
        dsFormaciones.formacionesRow datos;

        dsFormacionesObjetivosTableAdapters.formaciones_indicadores_objetivosTableAdapter q_indicadores;

        Boolean tiene_datos;

        public Formaciones()
        {
            this.id = -1;
            this.q = new dsFormacionesTableAdapters.formacionesTableAdapter();
            this.tiene_datos = false;
        }

        public Formaciones(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionesTableAdapters.formacionesTableAdapter();
            dsFormaciones.formacionesDataTable tabla =
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

        public String GetIDGoogleCalendar()
        {
            if (this.datos.IsidgooglecalendarNull())
            {
                return "";
            }
            else
            {
                return this.datos.idgooglecalendar;
            }
        }

        public Boolean NumeroSesionEnUso(Int32 numero_sesion)
        {
            dsFormacionesActividadesTableAdapters.formaciones_actividadesTableAdapter q_proceso =
                new dsFormacionesActividadesTableAdapters.formaciones_actividadesTableAdapter();
            try
            {
                if (q_proceso.GetDataByIDProcesoNumeroSesion(this.GetID(), numero_sesion).Count > 0)
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

        public Boolean NumeroSesionEnUso(Int32 numero_sesion, Int64 idsesion)
        {
            dsFormacionesActividadesTableAdapters.formaciones_actividadesTableAdapter q_proceso =
                new dsFormacionesActividadesTableAdapters.formaciones_actividadesTableAdapter();
            try
            {
                if (q_proceso.GetDataByIDProcesoNumeroSesionYo(this.GetID(), numero_sesion,idsesion).Count > 0)
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

        public static Boolean GetFormadorAsignado(Int64 idformador, Int64 idformacion)
        {
            dsFormacionesTableAdapters.cuantas_formacionesTableAdapter q_cuantas =
                new dsFormacionesTableAdapters.cuantas_formacionesTableAdapter();
            try
            {
                if (q_cuantas.GetData(idformador,idformacion)[0].cuantas>0)
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

        public Boolean InsertarIndicador(String indicador)
        {
            try
            {
                if (this.q_indicadores == null)
                {
                    this.q_indicadores = new dsFormacionesObjetivosTableAdapters.formaciones_indicadores_objetivosTableAdapter();
                }
                this.q_indicadores.Insertar(this.id, indicador, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean EliminarIndicador(Int64 idindicador)
        {
            try
            {
                if (this.q_indicadores == null)
                {
                    this.q_indicadores = new dsFormacionesObjetivosTableAdapters.formaciones_indicadores_objetivosTableAdapter();
                }
                this.q_indicadores.Eliminar(idindicador);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Int64 Insertar(Boolean escoaching, Int64 idcuenta, Int64 idformador, 
            Nullable<Int64> idmateria
            , DateTime fecha_inicio, Nullable<DateTime> fecha_fin,
            String titulo, String descripcion, String objetivos)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idcuenta, idformador, idmateria, fecha_inicio,
                    fecha_fin,titulo, descripcion,Convert.ToInt16(escoaching),objetivos,"");
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

        public Boolean Actualizar(Int64 idformador, Nullable<Int64> idmateria
            , DateTime fecha_inicio, Nullable<DateTime> fecha_fin,
            String titulo, String descripcion, String objetivos,
            String idgooglecalendar)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(idformador, idmateria, 
                    fecha_inicio, fecha_fin, titulo, descripcion,objetivos,idgooglecalendar,this.datos.id);
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

        public Boolean ActualizarGoogleCalendar(String idgooglecalendar)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.ActualizarGoogleCalendar(idgooglecalendar, this.datos.id);
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

        public String GetObjetivos()
        {
            return this.datos.objetivos;
        }

        public String GetTitulo()
        {
            return this.datos.titulo;
        }

        public String GetDescripcion()
        {
            return this.datos.descripcion;
        }

        public Int64[] GetByIDCuenta(Int64 idcuenta, Boolean escoaching)
        {
            Int64[] ids;

            dsFormaciones.formacionesDataTable tabla =
                this.q.GetDataByIDCuenta(idcuenta,Convert.ToInt16(escoaching));

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

        public Int64[] GetByIDMateria(Int64 idmateria)
        {
            Int64[] ids;

            dsFormaciones.formacionesDataTable tabla =
                this.q.GetDataByIDMateria(idmateria);

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

        public Int64[] GetByIDFormador(Int64 idformador)
        {
            Int64[] ids;

            dsFormaciones.formacionesDataTable tabla =
               this.q.GetDataByIDFormador(idformador);

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

        public DateTime GetFechaInicio()
        {
            return this.datos.fecha_inicio;
        }

        public Nullable<Int64> GetIDMateria()
        {
            if (this.datos.IsidmateriaNull())
            {
                return null;
            }
            else
            {
                return this.datos.idmateria;
            }
        }
        
        public Nullable<DateTime> GetFechaFin()
        {
            if (this.datos.Isfecha_finNull())
            {
                return null;
            }
            else
            {
                return this.datos.fecha_fin;
            }
        }

        public Int64 GetIDCuenta()
        {
            return this.datos.idcuenta;
        }

        public Int64 GetID()
        {
            return this.datos.id;
        }

        public Int64 GetIDFormador()
        {
            return this.datos.idformador;
        }
            
    }
}
