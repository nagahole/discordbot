using System.Collections.Generic;
using System;
using Discord_Bot_2.Modules.Money_Module;
using System.Linq;

namespace Discord_Bot_2.Modules.Money_Module.Structs
{
    [System.Serializable]
    public struct UserData
    {
        public int cash { get; set; }
        public int bank { get; set; }

        public int skinTotal { get; set; }

        #region
        private Dictionary<Cooldown, CooldownData> _cooldowns;
        public Dictionary<Cooldown, CooldownData> cooldowns
        {
            get
            {
                if (_cooldowns == null)
                    _cooldowns = new Dictionary<Cooldown, CooldownData>();
                return _cooldowns;
            }
            set { _cooldowns = value; }
        }

        private List<SkinInstance> _skins;

        public List<SkinInstance> skins
        {
            get
            {
                if (_skins == null)
                    _skins = new List<SkinInstance>();
                return _skins;
            }
            set
            {
                skinTotal = value.Select(x => { Console.WriteLine(x.worth); return x.worth; }).Sum(); //updates worht every time inventory updates
                _skins = value;
            }
        }

        #endregion

        public UserData(int money = 0, int bank = 0)
        {
            this.cash = money;
            this.bank = bank;
            skinTotal = 0;
            _cooldowns = new Dictionary<Cooldown, CooldownData>();
            _skins = new List<SkinInstance>();
        }

        //methods

        public void AddCooldown(Cooldown type, int length)
        {
            cooldowns.Add(type, new CooldownData(length, DateTime.Now));
        }

        public bool HasCooldown(Cooldown type)
        {
            CooldownData data;
            if (!cooldowns.TryGetValue(type, out data)) //contains == false : doesnt contain
            {
                return false;
            }

            if (data.elapsedTime >= data.lengthInSeconds) //cooldown is over
            {
                cooldowns.Remove(type);
                return false;
            }

            //cooldown not over
            return true;
        }
    }
}