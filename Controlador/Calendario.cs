using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Calendario
    {
        dsCalendarioTableAdapters.vista_calendarioTableAdapter q;
        dsCalendario.vista_calendarioRow datos;
        String identificador;
        Int64 id;

        public Calendario()
        {
            this.q = new dsCalendarioTableAdapters.vista_calendarioTableAdapter();
        }

        public Calendario(String identificador)
        {
            this.q = new dsCalendarioTableAdapters.vista_calendarioTableAdapter();
            this.identificador = identificador;
            this.id = Int64.Parse(identificador.Split(':')[1]);
            this.datos = this.q.GetDataByID(this.identificador)[0];
        }

        public String GetURL()
        {
            return this.datos.url;
        }

        public Int64 GetID()
        {
            return this.id;
        }

    }
}
