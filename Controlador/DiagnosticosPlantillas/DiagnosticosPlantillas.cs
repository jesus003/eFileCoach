using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controlador.DiagnosticosPlantillas;
using Controlador.DiagnosticosPlantillas.dsPlantillasTableAdapters;
using System.Collections;

namespace Controlador.DiagnosticosPlantillas
{
    public class DiagnosticosPlantillas
    {
        Int64 id;
        diagnosticos_plantillasTableAdapter adaptador;
        dsPlantillas.diagnosticos_plantillasRow datos;

        dsPlantillasTableAdapters.diagnosticos_plantillas_dimensionesTableAdapter q_dimensiones;
        DiagnosticosPlantillasDimensiones[] dimensiones;

        Boolean tiene_datos;

        public DiagnosticosPlantillas()
        {
            this.id = -1;
            this.tiene_datos = false;
            this.adaptador = new diagnosticos_plantillasTableAdapter();
            this.dimensiones = new DiagnosticosPlantillasDimensiones[0];
        }

        public DiagnosticosPlantillas(Int64 id)
        {
            this.dimensiones = new DiagnosticosPlantillasDimensiones[0];
            this.id = id;
            this.adaptador = new diagnosticos_plantillasTableAdapter();
            dsPlantillas.diagnosticos_plantillasDataTable tabla = this.adaptador.GetDataByID(this.id);
            if (tabla.Count > 0)
            {
                this.datos = tabla[0];
                this.tiene_datos = true;
            }
            else
            {
                this.tiene_datos = false;
            }
        }

        /// <summary>
        /// Nos indica el orden de la siguiente dimensión de la plantilla
        /// </summary>
        /// <returns></returns>
        public Int32 GetOrdenSiguiente()
        {
            dsPlantillas.diagnosticos_plantillas_dimensionesDataTable tabla = new dsPlantillas.diagnosticos_plantillas_dimensionesDataTable();
            dsPlantillasTableAdapters.diagnosticos_plantillas_dimensionesTableAdapter q_temp = new diagnosticos_plantillas_dimensionesTableAdapter();
            tabla = q_temp.GetDataByOrdenDESC(this.id);
            if (tabla.Rows.Count==0)
            {
                return 1;
            }
            else
            {
                return tabla[0].orden + 1;
            }
        }

        /// <summary>
        /// Método que nos devuelve un array de objetos dimension
        /// </summary>
        /// <returns></returns>
        public DiagnosticosPlantillasDimensiones[] GetDimensiones()
        {
            if (this.q_dimensiones==null)
            {
                this.q_dimensiones = new diagnosticos_plantillas_dimensionesTableAdapter();
                dsPlantillas.diagnosticos_plantillas_dimensionesDataTable tabla_dimensiones =
                    this.q_dimensiones.GetDataByIdPlantilla(this.id);
                if (tabla_dimensiones.Count>0)
                {
                    this.dimensiones = new DiagnosticosPlantillasDimensiones[tabla_dimensiones.Count];
                    for (int i = 0; i < tabla_dimensiones.Rows.Count; i++)
                    {
                        DiagnosticosPlantillasDimensiones dimension = new DiagnosticosPlantillasDimensiones(tabla_dimensiones[i].id);
                        dimensiones[i] = dimension;
                    }
                }
                
            }
            return this.dimensiones;
        }

        public Int64 Insertar(Int64 idusuario,String nombre,DateTime fecha,
            Boolean disponible, Boolean publico, Int64 idtipo)
        {
            try
            {
                if (adaptador.Connection.State != System.Data.ConnectionState.Open) 
                { 
                    adaptador.Connection.Open(); 
                }
                MySqlTransaction trans = adaptador.Connection.BeginTransaction();
                this.adaptador.Insertar(idusuario,nombre,fecha,
                    Convert.ToInt16(disponible),Convert.ToInt16(publico),idtipo);
                dsPlantillas.diagnosticos_plantillasDataTable tabla_temporal = 
                    this.adaptador.GetDataLastID();
                trans.Commit();
                return tabla_temporal[0].id;
            }
            catch (Exception exc)
            {
                return -1;
            }
        }

        public Boolean Eliminar()
        {
            try
            {
                this.adaptador.Eliminar(this.id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean Actualizar(String nombre,DateTime fecha,Boolean disponible, Boolean publico)
        {
            try
            {
                this.adaptador.Actualizar(nombre, fecha, Convert.ToInt16(disponible),
                    Convert.ToInt16(publico), this.id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public String GetNombre()
        {
            return this.datos.nombre;
        }

        public DateTime GetFecha()
        {
          return this.datos.fecha;
        }

        public Int64 GetIDTipo()
        {
            return this.datos.idtipo;
        }

        public Boolean GetDisponible()
        {
            return Convert.ToBoolean(this.datos.disponible);
        }

        public Boolean GetPublica()
        {
            return Convert.ToBoolean(this.datos.publico);
        }
    }
}
