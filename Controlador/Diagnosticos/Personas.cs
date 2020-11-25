using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Controlador.Diagnosticos
{
    public class Personas
    {
        Int64 idpersona;
        dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter adaptador;
        dsDiagnosticos.diagnosticos_personasRow datos;
        Boolean tiene_datos;
        Boolean completos;

        public Personas()
        {
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter();
            this.tiene_datos = false;
            this.idpersona = -1;
        }

        public Personas(Int64 idpersona)
        {
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter();
            this.tiene_datos = true;
            this.idpersona = idpersona;
            this.datos = this.adaptador.GetDataByID(this.idpersona)[0];
        }

        public Personas (Int64 idevaluacion,Int64 idcoachee)
        {
            this.adaptador = new dsDiagnosticosTableAdapters.diagnosticos_personasTableAdapter();

            if (adaptador.GetDataByIDEvalIDCoachee(idevaluacion, idcoachee).Rows.Count > 0)
            {
                this.tiene_datos = true;
                this.datos = this.adaptador.GetDataByIDEvalIDCoachee(idevaluacion, idcoachee)[0];
                this.idpersona = datos.id;
            }
            else
            {
                this.tiene_datos = false;
            }
            
        }

        public Boolean ExisteEmail(String email, Int64 idencuesta)
        {
            try
            {
                if (this.adaptador.GetDataByIDEncuestaEmail(email.Trim().ToLower(),idencuesta).Rows.Count>0)
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
                return true;
            }
        }

        public Boolean ExisteEmailEdicion(String email, Int64 idencuesta)
        {
            try
            {
                if (this.adaptador.GetDataByIDEncuestaEmailYoNo(email.Trim().ToLower(), idencuesta,this.idpersona).Rows.Count > 0)
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
                return true;
            }
        }

        public Boolean InsertarEmail(String de, String para, String asunto,
            String cuerpo, DateTime fecha, String ip)
        {
            try
            {
                dsDiagnosticosTableAdapters.diagnosticos_personas_emailsTableAdapter q
                    = new dsDiagnosticosTableAdapters.diagnosticos_personas_emailsTableAdapter();
                q.Insertar(this.idpersona, de, para, asunto, cuerpo, fecha, ip);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Int64 Insertar(Int64 iddiagnostico, Nullable<Int64> idcoachee,
            String nombre, String apellidos, String email,
            Int64? idgrupo)
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
                adaptador.Insertar(iddiagnostico,
                    idcoachee,
                    nombre.Trim().ToLower(),
                    apellidos.Trim().ToLower(),
                    email.Trim().ToLower(),
                    idgrupo);
                Int64 last_id;
                //Obtenemos el ID correspondiente
                last_id = adaptador.GetDataByLastID()[0].id;

                //Validamos la transacción
                trans.Commit();

                this.idpersona = last_id;
                this.datos = this.adaptador.GetDataByID(this.idpersona)[0];
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

        public Boolean Actualizar(String nombre, String apellidos, String email, Int64 idgrupo)
        {
            try
            {
                this.adaptador.Actualizar(nombre, apellidos, email,idgrupo, this.idpersona);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int64 GetIDGrupo()
        {
            if (this.datos.IsidgrupoNull())
            {
                return -1;
            }
            else
            {
                return this.datos.idgrupo;
            }
        }

        public Boolean Eliminar()
        {
            try
            {
                this.adaptador.Eliminar(this.idpersona);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int64 GetIDDiagnostico()
        {
            return this.datos.iddiagnostico;
        }

        public Int64 GetIDCoachee()
        {
            if (this.datos.IsidcoacheeNull())
            {
                return -1;
            }
            else
            {
                return this.datos.idcoachee;
            }
        }

        public String GetNombre()
        {
            if (this.datos.IsnombreNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.nombre;
            }
        }

        public String GetApellidos()
        {
            if (this.datos.IsapellidosNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.apellidos;
            }
        }

        public String GetEmail()
        {
            if (this.datos.IsemailNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.email;
            }
        }

        public Int64 GetIdPersona()
        {
            return this.idpersona;
        }

        public Int16 GetEstado()
        {
            return datos.cerrado;
        }

        public Boolean GetTieneDatos()
        {
            return this.tiene_datos;
        }

        public String GetObservaciones()
        {
            if (this.datos.IsobservacionesNull())
            {
                return String.Empty;
            }
            else
            {
                return this.datos.observaciones;
            }
        }
        public Boolean ActualizarObservaciones(String observacion,Int16 estado)
        {
            if (idpersona != -1) {          
                adaptador.ActualizarObservaciones(observacion, estado, idpersona);
                return (true);
            }
            else
            {
                return false;
            }


        }
        
    }
}
