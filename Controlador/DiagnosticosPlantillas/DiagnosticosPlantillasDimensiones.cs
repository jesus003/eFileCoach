using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controlador.DiagnosticosPlantillas;
using Controlador.DiagnosticosPlantillas.dsPlantillasTableAdapters;

namespace Controlador.DiagnosticosPlantillas
{
    public class DiagnosticosPlantillasDimensiones
    {
        Int64 id;
        diagnosticos_plantillas_dimensionesTableAdapter adaptador;
        dsPlantillas.diagnosticos_plantillas_dimensionesRow datos;

        diagnosticos_plantillas_dimensiones_preguntasTableAdapter adaptador_preguntas;

        DiagnosticosPlantillasDimensionesPreguntas[] preguntas;

        Boolean tiene_datos;


        public DiagnosticosPlantillasDimensiones()
        {
            this.id = -1;
            this.tiene_datos = false;
            this.adaptador = new diagnosticos_plantillas_dimensionesTableAdapter();
            this.preguntas = new DiagnosticosPlantillasDimensionesPreguntas[0];
        }

        public DiagnosticosPlantillasDimensiones(Int64 id)
        {
            this.preguntas = new DiagnosticosPlantillasDimensionesPreguntas[0];
            this.id = id;
            this.adaptador = new diagnosticos_plantillas_dimensionesTableAdapter();
            dsPlantillas.diagnosticos_plantillas_dimensionesDataTable tabla = this.adaptador.GetDataByID(this.id);
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

        public DiagnosticosPlantillasDimensionesPreguntas [] GetPreguntas()
        {
            if (this.adaptador_preguntas==null)
            {
                this.adaptador_preguntas = new diagnosticos_plantillas_dimensiones_preguntasTableAdapter();
                dsPlantillas.diagnosticos_plantillas_dimensiones_preguntasDataTable tabla_preguntas =
                    this.adaptador_preguntas.GetDataByIDDimension(this.id);
                if (tabla_preguntas.Count>0)
                {
                    this.preguntas = new DiagnosticosPlantillasDimensionesPreguntas[tabla_preguntas.Count];
                    for (int i=0;i< tabla_preguntas.Count;i++)
                    {
                        this.preguntas[i] = new DiagnosticosPlantillasDimensionesPreguntas(tabla_preguntas[i].id);
                    }
                }
            }
            return this.preguntas;
        }

        public Int64 Insertar(Int64 idplantilla, String titulo, Int32 orden)
        {
            try
            {
                if (adaptador.Connection.State != System.Data.ConnectionState.Open)
                {
                    adaptador.Connection.Open();
                }
                MySqlTransaction trans = adaptador.Connection.BeginTransaction();
                this.adaptador.Insertar(idplantilla, titulo, orden);
                dsPlantillas.diagnosticos_plantillas_dimensionesDataTable tabla_temporal = this.adaptador.GetDataByUltimo();
                trans.Commit();
                return tabla_temporal[0].id;
            }
            catch
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

        public Boolean Actualizar(String titulo)
        {
            try
            {
                this.adaptador.Actualizar(this.GetIDPlantilla(), titulo, this.id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean SetOrden(Int32 orden)
        {
            try
            {
                this.adaptador.ActualizarOrden(orden, this.id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Método que devuelve el ID de la dimensión
        /// </summary>
        /// <returns></returns>
        public Int64 GetID()
        {
            return this.id;
        }

        public Int64 GetIDPlantilla()
        {
            return this.datos.idplantilla;
        }

        public String GetTitulo()
        {
            return this.datos.titulo;
        }

        public Int32 GetOrden()
        {
            return this.datos.orden;
        }

        public Int32 GetOrdenSiguientePregunta()
        {
            dsPlantillas.diagnosticos_plantillas_dimensiones_preguntasDataTable tabla;
            dsPlantillasTableAdapters.diagnosticos_plantillas_dimensiones_preguntasTableAdapter q_aux =
                new diagnosticos_plantillas_dimensiones_preguntasTableAdapter();
            tabla = q_aux.GetDataByOrdenDESC(this.id);
            if (tabla.Rows.Count>0)
            {
                return (tabla[0].orden + 1);
            }
            else
            {
                return 1;
            }
        }

        public DataTable GetDimensionesPorPlantilla(Int64 idplantilla)
        {
            return this.adaptador.GetDataByIdPlantilla(idplantilla);
        }
    }
}

