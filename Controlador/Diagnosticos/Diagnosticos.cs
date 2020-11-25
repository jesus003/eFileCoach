using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Controlador.Diagnosticos
{
    public class Diagnosticos
    {
        Int64 iddiagnostico;
        dsDiagnosticosTableAdapters.diagnosticosTableAdapter adaptador;
        dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter adaptador_dimensiones;
        dsDiagnosticos.diagnosticosRow datos;
        Boolean tiene_datos;
        dsDiagnosticos.diagnosticos_dimensionesDataTable datos_dimensiones;

        public Diagnosticos()
        {
            this.iddiagnostico = -1;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticosTableAdapter();
            this.tiene_datos = false;
            this.adaptador_dimensiones = new dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter();
        }

        public Diagnosticos(Int64 iddiagnostico)
        {
            this.iddiagnostico = iddiagnostico;
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticosTableAdapter();
            dsDiagnosticos.diagnosticosDataTable temp = this.adaptador.GetDataByID(this.iddiagnostico);
            if (temp.Count>0)
            {
                this.tiene_datos = true;
                this.datos = temp[0];
                this.adaptador_dimensiones = new dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter();
                this.datos_dimensiones = this.adaptador_dimensiones.GetDataByIDDiagnostico(this.iddiagnostico);
            }
            else
            {
                this.tiene_datos = false;
            }
            
        }

        public Boolean GetTieneDatos()
        {
            return this.tiene_datos;
        }

        public Int32 GetContadorDimensiones()
        {
            return this.datos_dimensiones.Rows.Count;
        }

        public Decimal GetSumaPonderacionGrupos()
        {
            try
            {
                dsDiagnosticosTableAdapters.grupos_ponderacionTableAdapter q_ponderacion =
                new dsDiagnosticosTableAdapters.grupos_ponderacionTableAdapter();
                return (Decimal)q_ponderacion.GetData(this.iddiagnostico)[0].suma;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Método que nos devuleve el IDDimension del índice que le pasemos
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public Int64 GetIDDimension(Int32 indice)
        {
            if (indice<this.datos_dimensiones.Rows.Count)
            {
                return this.datos_dimensiones[indice].id;
            }
            else
            {
                return -1;
            }
        }

        public Int32 GetCuantosGruposResponden()
        {
            try
            {
                dsDiagnosticosTableAdapters.vista_diagnosticos_contador_gruposTableAdapter q_vista =
                    new dsDiagnosticosTableAdapters.vista_diagnosticos_contador_gruposTableAdapter();
                Int32 contador = q_vista.GetData(this.iddiagnostico)[0].grupos_con_respuesta;
                return contador;

            }
            catch
            {
                return 0;
            }
        }

        public Int32 GetCuantosGrupos()
        {
            try
            {
                dsDiagnosticosTableAdapters.vista_diagnosticos_contador_gruposTableAdapter q_vista =
                     new dsDiagnosticosTableAdapters.vista_diagnosticos_contador_gruposTableAdapter();
                Int32 contador = q_vista.GetData(this.iddiagnostico)[0].cuantos_grupos;
                return contador;
            }
            catch
            {
                return 0;
            }
        }

        public Int64 GetIDUsuario()
        {
            return this.datos.idusuario;
        }

        public Int64 GetID()
        {
            return this.iddiagnostico;
        }

        public Int64 GetIDCliente()
        {
            if (this.datos.IsidclienteNull())
            {
                return -1;
            }
            else
            {
                return this.datos.idcliente;
            }
        }

        /// <summary>
        /// Método que nos facilita el siguiente orden de las dimensiones de una evaluación
        /// </summary>
        /// <returns></returns>
        public Int32 GetOrdenSiguiente()
        {
            dsDiagnosticos.diagnosticos_dimensionesDataTable tabla =
                new dsDiagnosticos.diagnosticos_dimensionesDataTable();
            dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter q_temp =
                new dsDiagnosticosTableAdapters.diagnosticos_dimensionesTableAdapter();
            tabla = q_temp.GetDataProximoOrden(this.iddiagnostico);
            if (tabla.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                return tabla[0].orden + 1;
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

        public Int64 GetIDEstado()
        {
            return this.datos.idestado;
        }

        public String GetPlantillaUsada()
        {
            return this.datos.plantilla_usada;
        }

        public Nullable<Int64> GetContadorSinEnviar()
        {
            try
            {
                dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter adaptadorpersonas = new dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter();
                Nullable<Int64> contador = Convert.ToInt64(adaptadorpersonas.ContarPorDiagnosticoEstado(this.iddiagnostico, 0));

                return contador;

            }
            catch
            {
                return -1;
            }
        }

        public Nullable<Int64> GetContadorEspera()
        {
            try
            {
                dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter adaptadorpersonas = new dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter();
                Nullable<Int64> contador = Convert.ToInt64(adaptadorpersonas.ContarPorDiagnosticoEstado(this.iddiagnostico,1));
                
                return contador;
              
            }
            catch
            {
                return -1;
            }
        }
            
            
            
            
        public Nullable<Int64> GetContadorCerrados()
        {
            try
            {
                dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter adaptadorpersonas = new dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter();
                Nullable<Int64> contador = Convert.ToInt64( adaptadorpersonas.ContarPorDiagnosticoEstado(iddiagnostico, 2));

                return contador;

            }
            catch
            {
                return -1;
            }
        }

        public Int64 GetContadorPersonas()
        {
            dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter adaptadorpersonas = new dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter();
            return adaptadorpersonas.GetDataByIddiagnosticoConEmail(iddiagnostico).Rows.Count;
        }

        public Boolean ActualizarEstado(Int16 estado)
        {
            try
            {
                adaptador.ActualizarEstado(estado, this.iddiagnostico);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean Actualizar(String nombre, DateTime fecha, Int64 idestado)
        {
            try
            {
                this.adaptador.Actualizar(nombre, fecha, idestado, this.iddiagnostico);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idusuario"></param>
        /// <param name="idcliente"></param>
        /// <param name="nombre"></param>
        /// <param name="fecha"></param>
        /// <param name="idestado"></param>
        /// <param name="plantilla_usada"></param>
        /// <returns></returns>
        public Int64 Insertar(Int64 idusuario, Nullable<Int64> idcliente, String nombre, 
            DateTime fecha, Int64 idestado, String plantilla_usada, Int64 idtipo)
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
                adaptador.Insertar(idusuario, idcliente, nombre, fecha, idestado, plantilla_usada,idtipo);
                Int64 last_id;
                //Obtenemos el ID correspondiente
                last_id = adaptador.GetDataByLastID()[0].id;

                //Validamos la transacción
                trans.Commit();

                this.iddiagnostico = last_id;
                this.datos = this.adaptador.GetDataByID(this.iddiagnostico)[0];
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

        public Boolean Eliminar()
        {
            try
            {
                this.adaptador.Eliminar(this.iddiagnostico);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Método que comprueba que todas las dimensiones contengan como mínimo una pregunta
        /// </summary>
        /// <returns>True si todas tienen como mínimo una pregunta y false si alguna no contiene ninguna</returns>
        public Boolean ContarPreguntasDiagnostico()
        {
            if (this.iddiagnostico != -1)
            {
                Diagnosticos diagnostico = new Diagnosticos(iddiagnostico);
                Boolean enc =true;
                if (diagnostico.GetContadorDimensiones() > 0)
                { 
                for(int i = 0; i < diagnostico.GetContadorDimensiones()&&enc; i++)
                {
                    Int32 contador = Convert.ToInt32(adaptador.ContarPreguntasPorDimension(diagnostico.GetIDDimension(i)));
                    if (contador<=0)
                    {
                        enc = false;
                    }
                }

                }
                else
                {
                    enc = false;
                }
                return enc;
            }
            else
            {
                return false;
            }
        }

        public Boolean TieneAutoevaluacion()
        {

            if (this.iddiagnostico != -1)
            {
                if (datos.IsidclienteNull())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        
    }
}
