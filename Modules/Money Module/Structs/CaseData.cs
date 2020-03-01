using System;
using System.Collections.Generic;

namespace Discord_Bot_2.Modules.Money_Module.Structs
{
    [System.Serializable]
    public struct CaseData
    {
        public string name { get; private set; }

        SkinData[] specials;
        SkinData[] coverts;
        SkinData[] classified;
        SkinData[] restricted;
        SkinData[] milispecs;

        public CaseData(string name, SkinData[] specials, SkinData[] coverts, SkinData[] classified, SkinData[] restricted, SkinData[] milispecs)
        {
            Console.WriteLine($"New created case' name is set to {name}");
            this.name = name;
            this.specials = specials;
            this.coverts = coverts;
            this.classified = classified;
            this.restricted = restricted;
            this.milispecs = milispecs;
        }

        public SkinInstance OpenCase()
        {
            var config = CSGOConfigs.config;
            float luck = (float) Program.rnd.NextDouble();

            return luck <= config.yellow ? new SkinInstance(specials[Program.rnd.Next(0, specials.Length)]) :
                luck <= config.yellow + config.covert ? new SkinInstance(coverts[Program.rnd.Next(0, coverts.Length)]) :
                luck <= config.yellow + config.covert + config.classified ? new SkinInstance(classified[Program.rnd.Next(0, classified.Length)]) :
                luck <= config.yellow + config.covert + config.classified + config.restricted ? new SkinInstance(restricted[Program.rnd.Next(0, restricted.Length)]) :
                new SkinInstance(milispecs[Program.rnd.Next(0, milispecs.Length)]);
        }
    }
}