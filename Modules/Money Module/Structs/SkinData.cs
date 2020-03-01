using System;
using Discord_Bot_2.Modules.Money_Module;
using System.IO;


namespace Discord_Bot_2.Modules.Money_Module.Structs
{
    [System.Serializable]
    public struct SkinData
    {
        public static Random rnd = new Random();
        public string name { get; set; }
        public string weapon { get; set; }
        public Rarity rarity { get; set; }

        public string imageURL { get; set; }
     
        public int[] worth { get; set; }
        public int[] stattrakWorth { get; set; }
        public string description { get; set; }

        public float floatmin { get; set; }
        public float floatmax { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Name of skin</param>
        /// <param name="gun">Name of gun</param>
        /// <param name="rarity">Rarity of skin</param>
        /// <param name="worth">The value in cents of the skin</param>
        /// <param name="stattrakWorth">The value in cents of the stattrak version of the skin</param>
        /// <param name="description">A description</param>
        /// <param name="min">Min float value default 0</param>
        /// <param name="max">Max float value default 1</param>
        public SkinData(string name, string gun, Rarity rarity, int[] worth, int[] stattrakWorth = null, string imageURL = "",string description = "", float min = 0, float max = 1)
        {
            this.name = name;
            this.weapon = gun;
            this.rarity = rarity;
            this.imageURL = imageURL;
            this.worth = worth;
            this.stattrakWorth = stattrakWorth == null? worth: stattrakWorth;
            this.description = description == ""? CSGOConfigs.LogRarity(rarity) + " weapon skin" : description;
            this.floatmin = min;
            this.floatmax = max;
        }

        public override string ToString()
        {
            return $"{weapon} | {name}";
        }
    }

    /// <summary>
    /// Automatically generates wear value on creation
    /// </summary>
    [System.Serializable]
    public struct SkinInstance
    {
        private static string path = Program.folderPath + "dontedit.txt";
        public uint id { get; set; }
        public SkinData skinData { get; set; }
        public int worth { get; set; }
        public bool stattrak { get; set; }
        public float floatValue { get; set; }
        public Condition condition { get; set; }

        public SkinInstance(SkinData sd)
        {
            skinData = sd;

            stattrak = Program.rnd.Next(0, 11) == 0;

            floatValue = (float)(Program.rnd.NextDouble() * (sd.floatmax - sd.floatmin) + sd.floatmin);
            condition = floatValue < 0.07f ? Condition.FN :
                floatValue < 0.15f ? Condition.MW :
                floatValue < 0.38f ? Condition.FT :
                floatValue < 0.45 ? Condition.WW :
                Condition.BS;

            worth = stattrak ?
                skinData.stattrakWorth[(byte)condition] :
                skinData.worth[(byte)condition];

            string raw = File.ReadAllText(path);
            if(raw.Length == 0)
            {
                File.WriteAllText(path, "1");
                id = 0;
            }
            else
            {
                id = uint.Parse(raw);
                File.WriteAllText(path, $"{id + 1}");
            }
        }
        public void GenerateNewRNG()
        {
            stattrak = Program.rnd.Next(0, 11) == 0;

            floatValue = (float)(Program.rnd.NextDouble() * (skinData.floatmax - skinData.floatmin) + skinData.floatmin);
            condition = floatValue < 0.07f ? Condition.FN :
                floatValue < 0.15f ? Condition.MW :
                floatValue < 0.38f ? Condition.FT :
                floatValue < 0.45 ? Condition.WW :
                Condition.BS;

            worth = stattrak ?
                skinData.stattrakWorth[(byte)condition] :
                skinData.worth[(byte)condition];
        }

        public override string ToString()
        {
            return $"{(stattrak? "Stattrak " : "")}{CSGOConfigs.LogCondition(condition)} {skinData.weapon} | {skinData.name}";
        }
    }
}