using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Controlador.Diagnosticos
{
    public class Dimensiones
    {
        Int64 iddimension;
        Boolean tiene_datos;
        dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter adaptador;
        dsDiagnosticos.diagnosticos_dimensionesRow datos;

        public Dimensiones()
        {
            this.iddimension = -1;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter();
            this.tiene_datos = false;
        }

        public Dimensiones(Int64 iddimension)
        {
            this.iddimension = iddimension;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter();
            this.datos = this.adaptador.GetDataByID(this.iddimension)[0];
            this.tiene_datos = true;
        }

        public Int64[] DevolverPreguntas()
        {
            if (tiene_datos == true)
            {
                dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter adaptadortabla = new dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter();
                dsDiagnosticos.diagnosticos_dimensiones_preguntasDataTable tabla = adaptadortabla.GetDataByIDDimension(this.iddimension);
                Int64[] idspreguntas = new Int64[tabla.Rows.Count];
                for (int i = 0; i < idspreguntas.Length; i++)
                {
                    idspreguntas[i] = tabla[i].id;
                }

                return idspreguntas;
            }

            return new Int64[0];
        }

        public Int64 Insertar(Int64 iddiagnostico, String titulo, Int32 orden)
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
                adaptador.Insertar(iddiagnostico, titulo, orden);
                Int64 last_id;
                //Obtenemos el ID correspondiente
                last_id = adaptador.GetDataByLastID()[0].id;

                //Validamos la transacción
                trans.Commit();

                this.iddimension = last_id;
                this.datos = this.adaptador.GetDataByID(this.iddimension)[0];
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

        public Boolean Actualizar(String titulo, Int32 orden)
        {
            try
            {
                this.adaptador.Actualizar(titulo, orden, this.iddimension);
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
                this.adaptador.Eliminar(this.iddimension);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int64 GetIDDimension()
        {
            return this.iddimension;
        }

        public Int64 GetIDDiagnostico()
        {
            return this.datos.iddiagnostico;
        }
        
        public String GetTitulo()
        {
            return this.datos.titulo;
        }

        public Int32 GetOrden()
        {
            return this.datos.orden;
        }

        /// <summary>
        /// Método que nos da el orden de la siguiente pregunta de la dimensión
        /// </summary>
        /// <returns></returns>
        public Int32 GetOrdenSiguiente()
        {
            dsDiagnosticos.diagnosticos_dimensiones_preguntasDataTable tabla =
                new dsDiagnosticos.diagnosticos_dimensiones_preguntasDataTable();
            dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter q_temp =
                new dsDiagnosticosTableAdapters.diagnosticos_dimensiones_preguntasTableAdapter();
            tabla = q_temp.GetDataOrdenSiguiente(this.iddimension);
            if (tabla.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                return tabla[0].orden + 1;
            }
        }

    }
}
