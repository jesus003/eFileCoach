using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador.Diagnosticos
{
    public class Respuestas
    {
        Int64 idpregunta;
        Int64 idrespuesta;
        dsDiagnosticosTableAdapters.diagnosticos_respuestasTableAdapter adaptador;
        public dsDiagnosticos.diagnosticos_respuestasRow datos;
        Boolean tiene_datos;

        public Respuestas()
        {
            this.idpregunta = -1;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_respuestasTableAdapter();
            this.tiene_datos = false;
        }

        public Respuestas(Int64 idpregunta)
        {
            this.idpregunta = idpregunta;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_respuestasTableAdapter();
            this.tiene_datos = true;
            this.datos = this.adaptador.GetDataByIdPregunta(this.idpregunta)[0];
        }

       


        public Int64 Insertar(Int64 idpregunta, String respuesta,  int orden)
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
                adaptador.Insertar(idpregunta,respuesta,orden);
                Int64 last_id;
                //Obtenemos el ID correspondiente
                last_id = adaptador.GetDataByLastID()[0].id;

                //Validamos la transacción
                trans.Commit();

                this.idrespuesta = last_id;
                this.datos = this.adaptador.GetDataByID(this.idrespuesta)[0];
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

       

     
        public Boolean Eliminar()
        {
            try
            {
                this.adaptador.Eliminar(this.idrespuesta);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean EliminarByIDPregunta(Int64 id_pregunta)
        {
            try
            {
                this.adaptador.EliminarByIDPregunta(id_pregunta);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
