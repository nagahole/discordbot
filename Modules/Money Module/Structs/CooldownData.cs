using System;

namespace Discord_Bot_2.Modules.Money_Module.Structs
{
    [System.Serializable]
    public struct CooldownData
    {
        public int lengthInSeconds { get; }
        public DateTime timeSet { get; }

        public int elapsedTime
        {
            get
            {
                return (int)(DateTime.Now - timeSet).TotalSeconds;
            }
        }

        public int remainingTime
        {
            get
            {
                return lengthInSeconds - elapsedTime;
            }
        }

        public CooldownData(int lengthInSeconds, DateTime timeSet)
        {
            this.lengthInSeconds = lengthInSeconds;
            this.timeSet = timeSet;
        }
    }
}