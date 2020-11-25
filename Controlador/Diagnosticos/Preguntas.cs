using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Controlador.Diagnosticos
{
    public class Preguntas
    {
        Int64 idpregunta;
        dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter adaptador;
        dsDiagnosticos.diagnosticos_dimensiones_preguntasRow datos;
        Boolean tiene_datos;
        Dimensiones dimension;

        public Preguntas()
        {
            this.idpregunta = -1;
            this.tiene_datos = false;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter();
        }

        public Preguntas(Int64 idpregunta)
        {
            this.idpregunta = idpregunta;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter();
            this.tiene_datos = true;
            this.datos = this.adaptador.GetDataByID(this.idpregunta)[0];
        }

        public Int64 Insertar(Int64 iddimension, String pregunta, Int32 orden, Int64 idtipo, int num_respuestas)
        {
            //Abrimos la conexión
            if (adaptador.Connection.State != System.Data.ConnectionState.Open)
            {
                adaptador.Connection.Open();
            }
            //Iniciamos la transacción
            MySqlTransaction trans = adaptador.Connection.BeginTransaction();
            try
            {
                //Realizamos el alta en la tabla ejemplo
                adaptador.Insertar(iddimension, pregunta, orden,idtipo,num_respuestas);
                Int64 last_id;
                //Obtenemos el ID correspondiente
                last_id = adaptador.GetDataByLastID()[0].id;

                //Validamos la transacción
                trans.Commit();

                this.idpregunta = last_id;
                this.datos = this.adaptador.GetDataByID(this.idpregunta)[0];
                this.tiene_datos = true;

                return (last_id);
            }
            catch (Exception exc)
            {
                //Si algo falla, hacemos el RollBack de la transacción y devolvemos -1
                trans.Rollback();
                return -1;
            }

        }

        public Int64 GetIDTipo()
        {
            return this.datos.idtipo;
        }

        public Boolean Actualizar(String pregunta, Int32 orden)
        {
            try
            {
                this.adaptador.Actualizar(pregunta, orden, this.idpregunta);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        

        public Boolean Eliminar()
        {
            try
            {
                this.adaptador.Eliminar(this.idpregunta);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int64 GetIDDimension()
        {
            return this.datos.iddimension;
        }
        public Int64 GetNumRespuestas()
        {
            return this.datos.num_respuestas;
        }

        /// <summary>
        /// Método que nos facilita el objeto de la Dimensión a la que pertenece la pregunta
        /// </summary>
        /// <returns></returns>
        public Dimensiones GetDimension()
        {
            if (this.dimension == null)
            {
                this.dimension = new Dimensiones(this.GetIDDimension());
            }
            return dimension;
        }

        public String GetPregunta()
        {
            return this.datos.pregunta;
        }

        public Int32 GetOrden()
        {
            return this.datos.orden;
        }
    }
}
