using Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tareas_programadas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["tokencillo"] == Interfaz.GetValor("token_tareas_programadas"))
                {
                    this.Cron();
                }
                else
                {
                    Response.Redirect("/");
                }
            }
        }
        catch
        {
            Response.Redirect("/");
        }
    }

    private void Cron()
    {
        //Buscamos las clases para alumnos de mañana y mandamos las notificaciones
        dsCron.vista_actividades_cronDataTable actividades;
        Controlador.dsCronTableAdapters.vista_actividades_cronTableAdapter q =
            new Controlador.dsCronTableAdapters.vista_actividades_cronTableAdapter();
        actividades = q.GetData();
        for (int i=0;i<actividades.Count;i++)
        {
            try
            {
                FormacionesActividades actividad = new FormacionesActividades(actividades[i].id_formaciones_actividades);
                Cuentas cuenta = new Cuentas(actividades[i].idcuenta);
                String asunto = Interfaz.GetValor("notificacion_dia_alumno_asunto");

                String cuerpo = Interfaz.GetValor("notificacion_dia_alumno_cuerpo");

                if (cuenta.GetLogotipo() != null)
                {
                    cuerpo = cuerpo.Replace("[logo]", "https://my.efilecoach.com/logo.aspx?"
                        + Cifrado.cifrarParaUrl(cuenta.GetID().ToString()));
                }
                else
                {
                    cuerpo = cuerpo.Replace("[logo]", "https://my.efilecoach.com/Images/logo_efile_coach.jpg");
                }

                if (cuenta.GetIDTipo() == 1)
                {
                    cuerpo = cuerpo.Replace("[logo_pie]", "https://my.efilecoach.com/Images/logo_pie.png");
                }

                String lugar = actividades[i].lugar;
                if (lugar != String.Empty)
                {
                    lugar = " en: <br /><br />" + lugar;
                    if (actividad.GetDireccion()!=String.Empty)
                    {
                        lugar += " " + actividad.GetDireccion();
                    }
                    if (actividad.GetCodPos()!=String.Empty)
                    {
                        lugar += " <br />" + actividad.GetCodPos();
                    }
                    if (actividad.GetPoblacion()!=String.Empty)
                    {
                        lugar += " " + actividad.GetPoblacion();
                    }
                    if (actividad.GetProvincia()!=String.Empty)
                    {
                        lugar += " (" + actividad.GetProvincia() + ")";
                    }
                    lugar = lugar + "<br /><br />";
                }

                cuerpo = cuerpo.Replace("[nombre]", actividades[i].nombre_alumno);
                cuerpo = cuerpo.Replace("[titulo]", actividades[i].titulo);
                cuerpo = cuerpo.Replace("[fecha]", actividades[i].fecha_inicio.ToString("dd/MM/yyyy"));
                cuerpo = cuerpo.Replace("[actividad_programada]", actividades[i].tipo_actividad);
                cuerpo = cuerpo.Replace("[hora]", actividades[i].fecha_inicio.ToString("HH:mm"));
                cuerpo = cuerpo.Replace("[lugar]", lugar);
                cuerpo = cuerpo.Replace("[email_profesor]", actividades[i].email_profesor);
                cuerpo = cuerpo.Replace("[nombre_usuario]", actividades[i].nombre_profesor
                    + " " + actividades[i].apellidos_profesor);
                cuerpo = cuerpo.Replace("[nombre_propietario_cuenta]", cuenta.GetNombre());
                Email.sendEmailCuenta("efileCoach<no-responder@efilecoach.com>", actividades[i].email_alumno, asunto, cuerpo, "",cuenta);
            }
            catch
            {

            }
        }

        //Buscamos las sesiones para coachees de mañana
        dsCron.vista_actividades_cronDataTable actividades_profesor = q.GetDataByProfesor();
        for (int j=0;j<actividades_profesor.Count;j++)
        {
            Cuentas cuenta = new Cuentas(actividades_profesor[j].idcuenta);
            String listado = String.Empty;
            String asunto = Interfaz.GetValor("notificacion_dia_profesor_asunto");
            String cuerpo = Interfaz.GetValor("notificacion_dia_profesor_cuerpo");
            if (cuenta.GetLogotipo() != null)
            {
                cuerpo = cuerpo.Replace("[logo]", "https://my.efilecoach.com/logo.aspx?"
                    + Cifrado.cifrarParaUrl(cuenta.GetID().ToString()));
            }
            else
            {
                cuerpo = cuerpo.Replace("[logo]", "https://my.efilecoach.com/Images/logo_efile_coach.jpg");
            }

            if (cuenta.GetIDTipo() == 1)
            {
                cuerpo = cuerpo.Replace("[logo_pie]", "https://my.efilecoach.com/Images/logo_pie.png");
            }

            actividades = q.GetDataByIDProfesor(actividades_profesor[j].idprofesor);
            for (int i = 0; i < actividades.Count; i++)
            {
                try
                {
                    FormacionesActividades actividad = new FormacionesActividades(actividades[i].id_formaciones_actividades);
                    Formaciones formacion = new Formaciones(actividades[i].idformacion);
                    String lugar = actividades[i].lugar;
                    if (lugar != String.Empty)
                    {
                        lugar = " en " + lugar;
                        if (actividad.GetDireccion() != String.Empty)
                        {
                            lugar += " " + actividad.GetDireccion();
                        }
                        if (actividad.GetCodPos() != String.Empty)
                        {
                            lugar += " " + actividad.GetCodPos();
                        }
                        if (actividad.GetPoblacion() != String.Empty)
                        {
                            lugar += " " + actividad.GetPoblacion();
                        }
                        if (actividad.GetProvincia() != String.Empty)
                        {
                            lugar += " (" + actividad.GetProvincia() + ")";
                        }
                    }
                    listado += String.Format("<li><a href='{0}'>{2} ({1})</a> a las {5} con {3} {4}{6}.</li>",
                        "https://my.efilecoach.com/actividad.aspx?id=" + Cifrado.cifrarParaUrl(actividad.GetID().ToString()),
                        actividades[i].tipo_actividad,
                        actividades[i].titulo,
                        actividades[i].nombre_alumno,
                        actividades[i].apellidos_alumno,
                        actividades[i].fecha_inicio.ToString("HH:mm"),
                        lugar);
                }
                catch
                {

                }
            }
            if (listado != String.Empty)
            {
                cuerpo = cuerpo.Replace("[fecha]", actividades[j].fecha_inicio.ToString("dd/MM/yyyy"));
                cuerpo = cuerpo.Replace("[nombre_propietario_cuenta]", cuenta.GetNombre());
                cuerpo = cuerpo.Replace("[lista_actividades]", listado);
                
                Email.sendEmailCuenta("efileCoach<no-responder@efilecoach.com>", 
                    actividades_profesor[0].email_profesor, asunto, cuerpo, "",
                    cuenta);
            }
        }

        

    }
}