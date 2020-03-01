using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Discord;

namespace Discord_Bot_2.Modules.Money_Module
{
    public class CSGOConfigs
    {
        private static string configPath = Program.folderPath + @"csgocaseconfig.txt";

        static bool init = false;

        private static CSGOConfigsData _config;
        public static CSGOConfigsData config
        {
            get
            {
                if(!init)
                {
                    init = true;
                    string raw = File.ReadAllText(configPath);

                    if (raw.Length == 0)
                    {
                        Console.WriteLine("No values set in csgocaseconfig.txt, defaulting values!");
                        _config = CSGOConfigsData.defaultValues;

                        string json = JsonConvert.SerializeObject(_config);
                        File.WriteAllText(configPath, json);
                        
                        return _config;
                    }

                    var temp = JsonConvert.DeserializeObject<CSGOConfigsData>(raw);
                    _config = temp;
                    Console.WriteLine($"New config's price is {temp.classified}");

                }
                return _config;
            }
            private set
            {
                _config = value;
            }
        }
        public static void UpdateConfigFromJson()
        {
            string raw;
            using (StreamReader r = new StreamReader(configPath))
            {
                raw = r.ReadToEnd();
                Console.WriteLine(raw);
            }
            var temp = JsonConvert.DeserializeObject<CSGOConfigsData>(raw);
            _config = temp;
            Console.WriteLine($"New config's price is {temp.classified}");
        }

        public static string LogRarity(Rarity r)
        {
            return r == Rarity.Gold ? "Gold" :
                r == Rarity.Covert ? "Covert" :
                r == Rarity.Classified ? "Classified" :
                r == Rarity.Restricted ? "Restricted" :
                "Milispec";
        }

        public static Color GetColor(Rarity r)
        {
            return r == Rarity.Gold ? Color.Gold :
                r == Rarity.Covert ? Color.Red :
                r == Rarity.Classified ? Color.Magenta :
                r == Rarity.Restricted ? Color.Purple :
                Color.DarkBlue;
        }
        public static string LogCondition(Condition c)
        {
            return c == Condition.FN ? "Factory New" :
                c == Condition.MW ? "Minimal Wear" :
                c == Condition.FT ? "Field-Tested" :
                c == Condition.WW ? "Well Worn" :
                "Battle-Scarred";
        }

        
    }

    public struct CSGOConfigsData
    {
        public static CSGOConfigsData defaultValues = new CSGOConfigsData(
            2500,
            .7992f,
            .1598f,
            .032f,
            .0064f,
            .0026f
            );

        public int caseprice { get; set; }
        public float milispec { get; set; }
        public float restricted { get; set; }
        public float classified { get; set; }
        public float covert { get; set; }
        public float yellow { get; set; }

        public CSGOConfigsData(int caseprice, float blue, float purple, float pink, float red, float yellow)
        {
            this.caseprice = caseprice;
            this.milispec = blue;
            this.restricted = purple;
            this.classified = pink;
            this.covert = red;
            this.yellow = yellow;
        }
    }

    public enum Condition : byte
    {
        FN,
        MW,
        FT,
        WW,
        BS
    }

    public enum Rarity : byte
    {
        Gold,
        Covert,
        Classified,
        Restricted,
        Milispec
    }
}

