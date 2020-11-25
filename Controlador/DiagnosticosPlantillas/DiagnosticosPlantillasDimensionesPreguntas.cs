using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Controlador.DiagnosticosPlantillas
{
    public class DiagnosticosPlantillasDimensionesPreguntas
    {
        Int64 idpregunta;
        dsPlantillasTableAdapters.diagnosticos_plantillas_dimensiones_preguntasTableAdapter q;
        dsPlantillas.diagnosticos_plantillas_dimensiones_preguntasRow datos;
        Boolean tiene_datos;

        public DiagnosticosPlantillasDimensionesPreguntas()
        {
            this.idpregunta = -1;
            this.tiene_datos = false;
            this.q = new dsPlantillasTableAdapters.diagnosticos_plantillas_dimensiones_preguntasTableAdapter();
        }

        public DiagnosticosPlantillasDimensionesPreguntas(Int64 idpregunta)
        {
            this.idpregunta = idpregunta;
            this.q = new dsPlantillasTableAdapters.diagnosticos_plantillas_dimensiones_preguntasTableAdapter();
            this.datos = this.q.GetDataByID(this.idpregunta)[0];
            this.tiene_datos = true;
        }

        public Int64 GetID()
        {
            return this.idpregunta;
        }

        public Int64 GetIDDimension()
        {
            return this.datos.iddimension;
        }

        public Int64 GetIDTipo()
        {
            return this.datos.idtipo;
        }
        public int GetNumRespuestas()
        {
            return this.datos.num_respuestas;
        }

        public String GetPregunta()
        {
            return this.datos.pregunta;
        }

        public Int32 GetOrden()
        {
            return this.datos.orden;
        }

        public Boolean Actualizar(String pregunta,Int32 orden)
        {
            try
            {
                this.q.Actualizar(pregunta, orden, this.GetIDDimension(), this.idpregunta);
                return true;
            }
            catch (Exception ex)
            {
                //Aquí registraríamos la excepción insertando en base de datos el mensaje
                return false;
            }
        }

        public Boolean ActualizarPregunta(String pregunta)
        {
            try
            {
                this.q.Actualizar(pregunta, this.GetOrden(), this.GetIDDimension(), this.idpregunta);
                return true;
            }
            catch (Exception ex)
            {
                //Aquí registraríamos la excepción insertando en base de datos el mensaje
                return false;
            }
        }

        public Int64 Insertar(Int64 iddimension, String pregunta, Int32 orden, Int64 idtipo, int num_respuestas)
        {
            try
            {
                //Abrimos la conexión
                if (q.Connection.State != System.Data.ConnectionState.Open)
                {
                    q.Connection.Open();
                }
                //Iniciamos la transacción
                MySqlTransaction trans = q.Connection.BeginTransaction();
                try
                {
                    //Realizamos el alta en la tabla ejemplo
                    q.Insertar(iddimension, pregunta, orden, idtipo, num_respuestas);
                    Int64 last_id;
                    //Obtenemos el ID correspondiente
                    last_id = q.GetDataByLastID()[0].id;
                    //Validamos la transacción
                    trans.Commit();

                    this.idpregunta = last_id;
                    this.tiene_datos = true;
                    this.datos = this.q.GetDataByID(this.idpregunta)[0];

                    return (last_id);
                }
                catch (Exception exc)
                {
                    //Si algo falla, hacemos el RollBack de la transacción y devolvemos -1
                    trans.Rollback();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public Boolean Eliminar()
        {
            try
            {
                this.q.Eliminar(this.idpregunta);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
