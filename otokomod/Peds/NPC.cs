using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace otokomod.Peds
{
    internal class NPC
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public Vector3 Coords { get; set; }

        NPC()
        {
            Name = "Bob";
            Job = "non";
            Coords = new Vector3(-535, -280, 37);
        }

        NPC(string name, string job, Vector3 coords)
        {
            Name = name;
            Job = job;
            Coords = coords;
        }

    }
}
