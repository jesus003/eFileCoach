using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador.Diagnosticos
{
    public class Grupos
    {
        Nullable<Int64> idgrupo;
        dsDiagnosticosTableAdapters.diagnosticos_personas_gruposTableAdapter q;
        dsDiagnosticos.diagnosticos_personas_gruposRow datos;
        public Grupos()
        {
            this.idgrupo = null;
            this.q = new dsDiagnosticosTableAdapters.diagnosticos_personas_gruposTableAdapter();
        }

        public Grupos(Int64 idgrupo)
        {
            this.idgrupo = idgrupo;
            this.q = new dsDiagnosticosTableAdapters.diagnosticos_personas_gruposTableAdapter();
            this.datos = this.q.GetDataByID((Int64)this.idgrupo)[0];
        }

        public Boolean Eliminar()
        {
            try
            {
                this.q.Delete((Int64)this.idgrupo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean Actualizar(String nombre, Decimal ponderacion)
        {
            try
            {
                this.q.Actualizar(nombre, ponderacion, (Int64)this.idgrupo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean Nuevo(Int64 iddiagnostico, String nombre,
            String descripcion, Decimal ponderacion)
        {
            try
            {
                this.q.Insertar(iddiagnostico, nombre, descripcion, ponderacion);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
