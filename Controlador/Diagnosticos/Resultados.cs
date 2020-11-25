using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Controlador.Diagnosticos
{
    public class Resultados
    {
        Int64 idresultado;
        dsDiagnosticosTableAdapters.diagnosticos_resultadosTableAdapter adaptador;
        dsDiagnosticos.diagnosticos_resultadosRow datos;
        Boolean tiene_datos;

        public Resultados()
        {
            this.idresultado = -1;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_resultadosTableAdapter();
            this.tiene_datos = false;
        }

        public Resultados(Int64 idresultado)
        {
            this.idresultado = idresultado;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_resultadosTableAdapter();
            this.tiene_datos = true;
            this.datos = this.adaptador.GetDataByID(this.idresultado)[0];
        }

        public Boolean ComprobarResultados(Int64 idpersona)
        {
            
            if (Convert.ToInt64(adaptador.ContarPreguntasSinResponder(idpersona)) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean Comprobar(Int64 idpersona,Int32 iddimension)
        {
            dsDiagnosticosTableAdapters.vrespuestas_preguntasTableAdapter adaptadorcomprobar=new dsDiagnosticosTableAdapters.vrespuestas_preguntasTableAdapter();
            dsDiagnosticos.vrespuestas_preguntasDataTable tabla = adaptadorcomprobar.GetDataByPersonaIdDimension(idpersona, iddimension);
            if (tabla.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Int64 Insertar(Int64 idpersona, Int64 idpregunta, 
            DateTime fecha, Int32? resultado, String resultado_texto, String ip)
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
                adaptador.Insertar(idpersona,idpregunta,fecha,resultado,ip,resultado_texto);
                Int64 last_id;
                //Obtenemos el ID correspondiente
                last_id = adaptador.GetDataByLastID()[0].id;

                //Validamos la transacción
                trans.Commit();

                this.idresultado = last_id;
                this.datos = this.adaptador.GetDataByID(this.idresultado)[0];
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

        public Boolean ActualizarNumero(Int32 resultado, String ip)
        {
            try
            {
                this.adaptador.ActualizarNumero(DateTime.Now, resultado,ip,this.idresultado);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean ActualizarTexto(String resultado_texto, String ip)
        {
            try
            {
                this.adaptador.ActualizarTexto(DateTime.Now,ip, resultado_texto, this.idresultado);
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
                this.adaptador.Eliminar(this.idresultado);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int64 GetIDPersona()
        {
            return this.datos.idpersona;
        }

        public Int64 GetIDPregunta()
        {
            return this.datos.idpregunta;
        }

        public DateTime GetFecha()
        {
            return this.datos.fecha;
        }

        public Int32 GetResultado()
        {
            return this.datos.resultado;
        }

    }
}
