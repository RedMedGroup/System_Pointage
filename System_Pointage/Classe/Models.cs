using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Pointage.Classe
{
    public class Models
    {
        public class AgentStatus
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public string Statut { get; set; }
            public int Jour { get; set; }
            public string Poste { get; set; }
            public DateTime CalculatedDate { get; set; } 
        }
   
    }
}
