using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controlador;
using Google.Apis.Auth.OAuth2.Flows;
using System.Net;
using System.Web.Script.Serialization;
using Google.Apis.Auth.OAuth2.Responses;

/// <summary>
/// Descripción breve de Calendario
/// </summary>
public class Calendario
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/.credentials/calendar-dotnet-quickstart.json
    static string[] Scopes = { CalendarService.Scope.CalendarEvents };
    static string ApplicationName = "Calendario efileCoach";

    String salida;
    CalendarService servicio;

    /// <summary>
    /// Este método va a recibir delante del número la C o la U para saber si es cuenta o usuario
    /// </summary>
    /// <param name="id"></param>
    public Calendario(Int64 id, Boolean escuenta)
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
        try
        {
            String idtoken = "";
            if (escuenta)
            {
                idtoken = "C" + id.ToString();
            }
            else
            {
                idtoken = "U" + id.ToString();
            }
            UserCredential credential;
            Depuracion.Registro("Entramos a crear el objeto 1");
            
            using (var stream =
                    new FileStream(HttpContext.Current.Server.MapPath("~/App_Data/G/credenciales.json"), 
                    FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath =
                    HttpContext.Current.Server.MapPath("~/App_Data/G/token" + 
                    Cifrado.cifrarParaUrl(idtoken.ToString()) + ".json");
                Depuracion.Registro("credPath: " + credPath);

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Depuracion.Registro("Credential file saved to: " + credPath);

                Depuracion.Registro("Entramos a crear el objeto 2");

                this.servicio = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }

        }
        catch (Exception exc)
        {
            Depuracion.Registro("Calendario.Constructor: " + exc.Message + Environment.NewLine + exc.StackTrace +
                Environment.NewLine + exc.StackTrace);
        }
    }

    public class GoogleToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }

    public String NuevoEvento(String titulo, String notas, DateTime fecha_inicio, DateTime fecha_fin)
    {
        try
        {
            Event newEvent = new Event()
            {
                Summary = titulo,
                //Location = "800 Howard St., San Francisco, CA 94103",
                Description = notas,
                Start = new EventDateTime()
                {
                    DateTime = fecha_inicio,
                    TimeZone = "Europe/Madrid",
                },
                End = new EventDateTime()
                {
                    DateTime = fecha_fin,
                    TimeZone = "Europe/Madrid",
                }
                //},
                //Reminders = new Event.RemindersData()
                //{
                //    UseDefault = false,
                //    Overrides = new EventReminder[] {
                //        new EventReminder() { Method = "email", Minutes = 24 * 60 },
                //        new EventReminder() { Method = "sms", Minutes = 10 },
                //    }
                //}
            };
            String calendarId = "primary";
            EventsResource.InsertRequest request = this.servicio.Events.Insert(newEvent, calendarId);
            Event createdEvent = request.Execute();
            //Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
            return createdEvent.Id;
        }
        catch (Exception ex)
        {
            Depuracion.Registro("Calendario.NuevoEvento: " + ex.Message + Environment.NewLine + ex.StackTrace);
            return String.Empty;
        }
    }

    public Boolean ActualizarEvento(String titulo, String notas, 
        DateTime fecha_inicio, DateTime fecha_fin,
        String idevento)
    {
        try
        {
            Event evento = new Event()
            {
                Summary = titulo,
                //Location = "800 Howard St., San Francisco, CA 94103",
                Description = notas,
                Start = new EventDateTime()
                {
                    DateTime = fecha_inicio,
                    TimeZone = "Europe/Madrid",
                },
                End = new EventDateTime()
                {
                    DateTime = fecha_fin,
                    TimeZone = "Europe/Madrid",
                }
                //},
                //Reminders = new Event.RemindersData()
                //{
                //    UseDefault = false,
                //    Overrides = new EventReminder[] {
                //        new EventReminder() { Method = "email", Minutes = 24 * 60 },
                //        new EventReminder() { Method = "sms", Minutes = 10 },
                //    }
                //}
            };
            String idcalendario = "primary";
            EventsResource.UpdateRequest request = this.servicio.Events.Update(evento, idcalendario, idevento);
            Event createdEvent = request.Execute();
            //Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
            return true;
        }
        catch (Exception ex)
        {
            Depuracion.Registro("Calendario.ActualizarEvento: " + ex.Message + Environment.NewLine + ex.StackTrace);
            return false;
        }
    }

    public Boolean EliminarEvento(String idevento)
    {
        try
        {
            String idcalendario = "primary";
            EventsResource.DeleteRequest request = this.servicio.Events.Delete(idcalendario, idevento);
            String resultado = request.Execute();
            return true;
        }
        catch (Exception ex)
        {
            Depuracion.Registro("Calendario.EliminarEvento: " + ex.Message + Environment.NewLine + ex.StackTrace);
            return false;
        }
    }

    public void LeerEventosProximos()
    {
        try
        {
            //service.Events.Insert()

            // Define parameters of request.
            EventsResource.ListRequest request = servicio.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.TimeMax = DateTime.Now.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            //request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            this.salida = String.Empty;
            salida += ("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    salida += String.Format("ID: {2} - {0} ({1}) <br />", eventItem.Summary, when, eventItem.Id);
                }
            }
            else
            {
                salida += ("No upcoming events found.");
            }
            //Console.Read();
        }
        catch (Exception ex)
        {
            Depuracion.Registro("Calendario.LeerEventosProximos: " + ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }

    public Event GetEvento(String idevento)
    {
        try
        {
            EventsResource.GetRequest request = servicio.Events.Get("primary", idevento);
            Event evento = request.Execute();
            return evento;
        }
        catch (Exception exc)
        {
            Depuracion.Registro("Calendario.GetEvento: " + exc.Message + Environment.NewLine + exc.StackTrace);
            return null;
        }
    }

    public void LeerEventos(DateTime fecha_inico, DateTime fecha_fin)
    {
        try
        {
            //service.Events.Insert()
            // Define parameters of request.
            EventsResource.ListRequest request = servicio.Events.List("primary");
            request.TimeMin = fecha_inico;
            request.TimeMax = fecha_fin;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            //request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            this.salida = String.Empty;
            salida += ("Próximos eventos:<br />");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    salida += String.Format("ID: {2} - {0} ({1}) <br />", eventItem.Summary, when, eventItem.Id);
                }
            }
            else
            {
                salida += ("No upcoming events found.");
            }
            //Console.Read();
        }
        catch (Exception ex)
        {
            Depuracion.Registro("Calendario.LeerEventos: " + ex.Message + Environment.NewLine + ex.StackTrace);
        }
        
    }

    public String GetSalida()
    {
        return this.salida;
    }
}