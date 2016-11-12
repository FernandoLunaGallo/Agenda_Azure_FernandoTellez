using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Agenda_Azure_FernandoTellez
{
    public class AgendaTabla
    {
        string id;
        string name;
        string apellido;
        string telefono;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "Nombre")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [JsonProperty(PropertyName = "Apellido")]
        public string Ape
        {
            get { return apellido; }
            set { apellido = value; }
        }
        [JsonProperty(PropertyName = "Telefono")]
        public string Tel
        {
            get { return telefono; }
            set { telefono = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}
