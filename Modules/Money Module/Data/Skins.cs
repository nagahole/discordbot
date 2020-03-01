using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord_Bot_2.Modules.Money_Module.Structs;

namespace Discord_Bot_2.Modules.Money_Module.Data
{
    public class Skins
    {
        // NAME, WEAPON, DESCRIPTION, MIN, MAX

        //(TEMPLATE)
        public static readonly SkinData template = new SkinData("Name", "Gun", Rarity.Classified, new int[] { 1,2,3,4,5}, new int[] { 2,4,6,8,10});

        //FALCHION CASE
        public static readonly SkinData FALCHIONFade = new SkinData("Fade", "Falchion", Rarity.Gold, new int[] { 80532, 74023, 60034, 50042, 30043 },
            new int[] { 104859, 95621, 80432, 7652, 69852 });

        public static readonly SkinData AWPHyperBeast = new SkinData("Hyper Beast", "AWP", Rarity.Covert, new int[] { 10022, 7022, 4735, 4048, 3168},
            new int[] { 36487, 19075, 10696, 9685, 7089}, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_awp_cu_awp_hyper_beast_light_large.7dd26637e8d50bc129d25ebdbf3e6e410917808e.png");

        public static readonly SkinData AKAquamarineRevenge = new SkinData("Aquamarine Revenge", "AK-47", Rarity.Covert, new int[] { 9606, 5948, 4184, 3557, 3136 },
            new int[] { 29439, 16380, 10518, 7750, 6473}, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_ak47_cu_ak47_courage_alt_light_large.27e4e7d38dc2ce36ffe86bd6ec65d6f525751eaa.png");

        public static readonly SkinData SG553Cyrex = new SkinData("Cyrex", "SG 553", Rarity.Classified, new int[] { 5442, 2704, 1200, 956, 795 },
            new int[] { 18016, 9315, 4210, 2630, 2411 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_sg556_cu_sg553_cyrex_light_large.ef2fb1e4d88e8eb7c0efe12e231a773ca1792a4d.png");

        public static readonly SkinData MP7Nemisis = new SkinData("Nemisis", "MP7", Rarity.Classified, new int[] { 1077, 761, 588 },
            new int[] { 3629, 1744, 1315 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_mp7_cu_mp7_nemsis_light_large.72074e71a27827f41dc5d6f511d2f003c1b60d8a.png", max: 0.32f);

        public static readonly SkinData CZ75YellowJacket = new SkinData("Yellow Jacket", "CZ75", Rarity.Classified, new int[] { 1408, 790, 524, 497, 427 },
            new int[] { 4823, 1920, 1097, 945, 923 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_cz75a_cu_cz75a_chastizer_light_large.d3234c712c3c068adbbfd5718c468c778f2351dd.png");

        public static readonly SkinData M4A4EvilDaimyo = new SkinData("Evil Daimyo", "M4A4", Rarity.Restricted, new int[] { 1254, 804, 555, 651, 592 },
            new int[] { 3947, 2232, 1509, 1568, 1576 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_m4a1_cu_m4a4_evil_daimyo_light_large.c208ba252c0d8902caa973a634cbfa945508a716.png", max: 0.52f);

        public static readonly SkinData NEGEVLoudmouth = new SkinData("Loudmouth", "Negev", Rarity.Restricted, new int[] { 0, 763, 086, 112, 076 },
            new int[] { 0, 3680, 280, 342, 222 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_negev_cu_negev_annihilator_light_large.421039357ccbbcb9ba1456caa1ed2ae4829b5495.png", min: 0.14f, max: 0.65f);

        public static readonly SkinData P2000Handgun = new SkinData("Handgun", "P2000", Rarity.Restricted, new int[] { 464, 185, 104, 75, 76 },
            new int[] { 2470, 552, 262, 227, 222 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_hkp2000_aq_p2000_boom_light_large.39f01b0b86b795bea56300432fecfbf93415ee58.png");

        public static readonly SkinData MP9RubyPoisonDart = new SkinData("Ruby Poison Dart", "MP9", Rarity.Restricted, new int[] { 326, 203, 136, 153, 166 },
            new int[] { 1173, 625, 350, 636, 395 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_mp9_am_mp9_nitrogen_light_large.3a0b5a7cd31a7cfd5f0d90b9a0a1dbfcdb642cca.png", max: 0.50f);

        public static readonly SkinData FAMASNeuralNet = new SkinData("Neural Net", "FAMAS", Rarity.Restricted, new int[] { 315, 185, 116, 161, 112 },
            new int[] { 886, 569, 268, 411, 307 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_famas_am_famas_dots_light_large.dc6f19278bac52ea06b8e3576fa324624f2f82b4.png", max: 0.60f);

        public static readonly SkinData GALILARRocketPop = new SkinData("Rocket Pop", "Galil AR", Rarity.Milispec, new int[] { 904, 281, 99, 60, 43 },
            new int[] { 2542, 648, 232, 168, 124 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_galilar_cu_galilar_particles_light_large.8732f64d53dbc9b0c732641655d4f99124d8cacc.png");

        public static readonly SkinData USPSTorque = new SkinData("Torque", "USP-S", Rarity.Milispec, new int[] { 323, 288, 204, 233, 368 },
            new int[] { 958, 766, 547, 606, 705 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_usp_silencer_cu_usp_progressiv_light_large.91cde781cd0c8502bbbb66f37cc7f1baf2a10c05.png", max: 0.46f);

        public static readonly SkinData P90EliteBuild = new SkinData("Elite Build", "P90", Rarity.Milispec, new int[] { 262, 64, 30, 28, 24 },
            new int[] { 1659, 289, 140, 112, 92 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_p90_cu_p90_mastery_light_large.f58ff489c92ffa8c6e4c42814bad01c352df0ab6.png");

        public static readonly SkinData GLOCK18BunsenBurner = new SkinData("Bunsen Burner", "Glock-18", Rarity.Milispec, new int[] { 147, 49, 32, 92, 43 },
            new int[] { 737, 304, 172, 449, 160 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_glock_aq_glock18_flames_blue_light_large.5fed23d5a32793c25914eeb99b45f1a2b0cb9d6c.png", max: 0.80f);

        public static readonly SkinData UMP45Riot = new SkinData("Riot", "UMP-45", Rarity.Milispec, new int[] { 62, 44, 28, 38, 24 },
            new int[] { 235, 137, 67, 94, 57 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_ump45_cu_ump45_uproar_light_large.04cd84320c4370bced14a3989b0e141cff67ec88.png", max: 0.70f);

        public static readonly SkinData NOVARanger = new SkinData("Ranger", "Nova", Rarity.Milispec, new int[] { 78, 44, 28, 24, 25 },
            new int[] { 259, 112, 68, 54, 54 }, @"https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_nova_cu_nova_ranger_light_large.e3e9d3d47d5707092223a268ef59adb53ce76278.png");


    }
}
