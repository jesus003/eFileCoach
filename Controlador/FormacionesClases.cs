using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FormacionesActividades
    {
        Int64 id;
        dsFormacionesActividadesTableAdapters.formaciones_actividadesTableAdapter q;
        dsFormacionesActividades.formaciones_actividadesRow datos;
        Boolean tiene_datos;

        public FormacionesActividades()
        {
            this.id = -1;
            this.q = new dsFormacionesActividadesTableAdapters.formaciones_actividadesTableAdapter();
            this.tiene_datos = false;
        }

        public FormacionesActividades(Int64 id)
        {
            this.id = id;
            this.q = new dsFormacionesActividadesTableAdapters.formaciones_actividadesTableAdapter();
            dsFormacionesActividades.formaciones_actividadesDataTable tabla =
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

        public Int64 Insertar(Int64 idformacion, Int64 idprofesor, Int64 idtipo_actividad
            , Nullable<Int16> individual, Nullable<Decimal> num_horas,
            String titulo, String descripcion, String informe, Int32? numero_sesion,
            String idgooglecalendar)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Insertar(idformacion, idprofesor, idtipo_actividad,
                    individual, num_horas,
                    titulo, descripcion,informe,numero_sesion, idgooglecalendar);
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

        public Int64 Insertar(Int64 idformacion, Int64 idprofesor, Int64 idtipo_actividad
            , Nullable<Int16> individual, Nullable<Decimal> num_horas,
            String titulo, String descripcion,
            Nullable<DateTime> fecha_inicio, Nullable<DateTime> fecha_fin,
            String informe, Int32? numero_sesion, String idgooglecalendar)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.InsertarConFechas(idformacion, idprofesor, idtipo_actividad, individual, num_horas,
                    titulo, descripcion,fecha_inicio,fecha_fin,informe,numero_sesion, idgooglecalendar);
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

        public String GetInforme()
        {
            if (this.datos.IsinformeNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.informe;
                
            }
        }

        //Método que duplica una clase y todos los alumnos asignados a ella
        public Int64 Duplicar(Boolean sesion)
        {
            Int16? individual=null;
            if (this.GetIndividual()!=null)
            {
                individual = Convert.ToInt16(this.GetIndividual());
            }
            Int64 idnueva;
            if (sesion)
            {
                Int32 numero_sesion;
                if (this.GetNumeroSesion()==null)
                {
                    numero_sesion = 1;
                }
                else
                {
                    numero_sesion = (Int32)this.GetNumeroSesion() + 1;
                }
                idnueva = this.Insertar(this.GetIDFormacion(), this.GetIDProfesor(), this.GetIDTipo(), individual,
                (decimal?)this.GetHoras(), "", this.GetNotasPrivadas(), DateTime.Now,
                null, this.GetInforme(), this.GetNumeroSesion()+1,"");
            }
            else
            {
                //Clase
                idnueva = this.Insertar(this.GetIDFormacion(), this.GetIDProfesor(), this.GetIDTipo(), individual,
                (decimal?)this.GetHoras(), "Copia: " + this.GetTitulo(), this.GetNotasPrivadas(), DateTime.Now,
                null, this.GetInforme(), null,"");
            }
            
            if (idnueva!=-1)
            {
                //Damos de alta la lista de asignaciones de alumnos
                dsFormacionesActividadesAlumnosTableAdapters.formaciones_actividades_alumnosTableAdapter q_alumnos_asignados =
                    new dsFormacionesActividadesAlumnosTableAdapters.formaciones_actividades_alumnosTableAdapter();
                dsFormacionesActividadesAlumnos.formaciones_actividades_alumnosDataTable alumnos_asignados =
                    q_alumnos_asignados.GetDataByIDFormacionActividad(this.id);
                FormacionesActividadesAlumnos actividad_alumno = new FormacionesActividadesAlumnos();
                Int32 errores = 0;
                for (int i=0;i<alumnos_asignados.Count;i++)
                {
                    if(actividad_alumno.Insertar(idnueva,alumnos_asignados[i].idalumno)==-1)
                    {
                        errores++;
                    }
                }
                if (errores==0)
                {
                    return idnueva;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public Int32? GetNumeroSesion()
        {
            if (this.datos.Isnumero_sesionNull())
            {
                return null;
            }
            else
            {
                return this.datos.numero_sesion;
            }
        }

        public String GetNumeroSesionTXT()
        {
            if (this.datos.Isnumero_sesionNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.numero_sesion.ToString();
            }
        }

        public Boolean Actualizar(Int64 idprofesor, Int64 idtipo_actividad,
             Nullable<Int16> individual, Nullable<Double> num_horas, String titulo, String descripcion,
             Nullable<DateTime> fecha_inicio, Nullable<DateTime> fecha_fin,
             String lugar, String direccion, String codpos,
             String poblacion, String provincia, String pais,
             String informe, Boolean notificable, Int32? numero_sesion,
             String idgooglecalendar)
        {
            if (q.Connection.State != System.Data.ConnectionState.Open) { q.Connection.Open(); }
            MySqlTransaction trans = q.Connection.BeginTransaction();
            try
            {
                this.q.Actualizar(this.GetIDFormacion(), idprofesor, idtipo_actividad, 
                    individual, (Nullable<Decimal>)num_horas, titulo, descripcion,fecha_inicio,fecha_fin,lugar,
                    direccion,codpos,poblacion,provincia,pais,informe,Convert.ToInt32(notificable),
                    numero_sesion,idgooglecalendar,this.datos.id);
                trans.Commit();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Interfaz.InsertarLog(e.Message + Environment.NewLine + e.StackTrace, 
                    String.Empty, String.Empty, String.Empty);
                return false;
            }
        }

        public Boolean GetNotificable()
        {
            return Convert.ToBoolean(this.datos.notificable);
        }

        public String GetLugar()
        {
            if (this.datos.IslugarNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.lugar;
            }
        }

        public String GetDireccion()
        {
            if (this.datos.IsdireccionNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.direccion;
            }
        }

        public String GetCodPos()
        {
            if (this.datos.IscodposNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.codpos;
            }
        }

        public String GetPoblacion()
        {
            if (this.datos.IspoblacionNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.poblacion;
            }
        }

        public String GetProvincia()
        {
            if (this.datos.IsprovinciaNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.provincia;
            }
        }

        public String GetPais()
        {
            if (this.datos.IspaisNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.pais;
            }
        }


        /// <summary>
        /// Método que devuelve null si no está definido,
        /// true si es individual
        /// false si es colectivo
        /// </summary>
        /// <returns></returns>
        public Nullable<Boolean> GetIndividual()
        {
            if (!this.datos.IsindividualNull())
            {
                return Convert.ToBoolean(this.datos.individual);
            }
            else
            {
                return null;
            }
        }

        public Nullable<DateTime> GetFechaInicio()
        {
            if (!this.datos.Isfecha_inicioNull())
            {
                return this.datos.fecha_inicio;
            }
            else
            {
                return null;
            }
        }

        public Nullable<DateTime> GetFechaFin()
        {
            if (!this.datos.Isfecha_finNull())
            {
                return this.datos.fecha_fin;
            }
            else
            {
                return null;
            }
        }

        public String GetTitulo()
        {
            if (!this.datos.IstituloNull())
            {
                return this.datos.titulo;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// El campo descripción pasa a ser las notas privadas
        /// </summary>
        /// <returns></returns>
        public String GetNotasPrivadas()
        {
            if (!this.datos.IsdescripcionNull())
            {
                return this.datos.descripcion;
            }
            else
            {
                return String.Empty;
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

        public Int64[] GetByIDFormacion(Int64 idFormacion)
        {
            Int64[] ids;

            dsFormacionesActividades.formaciones_actividadesDataTable tabla =
                this.q.GetDataByIDFormacion(idFormacion);

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

        public Int64[] GetByIDActividad(Int64 idactividad)
        {
            Int64[] ids;

            dsFormacionesActividades.formaciones_actividadesDataTable tabla =
               this.q.GetDataByIDTipoActividad(idactividad);

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

        public Int64[] GetByIDProfesor(Int64 idprofesor)
        {
            Int64[] ids;

            dsFormacionesActividades.formaciones_actividadesDataTable tabla =
               this.q.GetDataByIDFormacion(idprofesor);

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

        public Int64 GetIDFormacion()
        {
            return this.datos.idformacion;
        }

        public Int64 GetIDProfesor()
        {
            return this.datos.idprofesor;
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

        public Int64 GetIDTipo()
        {
            return this.datos.idtipo_actividad;
        }

        public Nullable<Double> GetHoras()
        {
            if (!this.datos.Isnum_horasNull())
            {
                return (Nullable<Double>)this.datos.num_horas;
            }
            else
            {
                return null;
            }
        }
    }
}
