using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord_Bot_2.Modules.Money_Module.Structs;

namespace Discord_Bot_2.Modules.Money_Module.Data
{
    class Cases
    {
        public static CaseData[] _cases;
        public static CaseData[] cases
        {
            get
            {
                if (_cases == null)
                {
                    _cases = new CaseData[] { FalchionCase };
                }
                return _cases;
            }
            set
            {
                _cases = value;
            }
        }

        public static readonly CaseData FalchionCase = new CaseData(
            "Falchion Case",
            new SkinData[] { Skins.FALCHIONFade}, //gold
            new SkinData[] { Skins.AWPHyperBeast, Skins.AKAquamarineRevenge }, //covert
            new SkinData[] { Skins.SG553Cyrex, Skins.MP7Nemisis, Skins.MP7Nemisis, Skins.CZ75YellowJacket}, //classified
            new SkinData[] { Skins.M4A4EvilDaimyo, Skins.NEGEVLoudmouth, Skins.P2000Handgun, Skins.MP9RubyPoisonDart, Skins.FAMASNeuralNet}, //restricted
            new SkinData[] { Skins.GALILARRocketPop, Skins.USPSTorque, Skins.P90EliteBuild, Skins.GLOCK18BunsenBurner,
            Skins.UMP45Riot, Skins.NOVARanger} //milispec
            );
    }
}
