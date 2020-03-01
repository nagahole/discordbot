
namespace Discord_Bot_2.Modules.Money_Module.Structs
{
    [System.Serializable]
    public struct MoneyConfig
    {
        public readonly static MoneyConfig defaultValues = new MoneyConfig(
            WORK_BASE,
            WORK_RANDOM_PERCENTAGE,
            WORK_COOLDOWN,

            COINFLIP_MIN,
            COINFLIP_MAX,

            STEAL_MONEY_PERCENTAGE,
            STEAL_SUCCESS_PERCENTAGE,
            STEAL_COOLDOWN,

            ITEM_DISPLAYED_PER_PAGE

            );

        //DEFAULT VALUES
        public const int WORK_BASE = 500;
        public const int WORK_RANDOM_PERCENTAGE = 100; //500 + 1.[0-2] * 500
        public const int WORK_COOLDOWN = 20;

        public const int COINFLIP_MIN = 5;
        public const int COINFLIP_MAX = 40000;

        public const int STEAL_MONEY_PERCENTAGE = 30;
        public const int STEAL_SUCCESS_PERCENTAGE = 50;
        public const int STEAL_COOLDOWN = 30;

        public const int ITEM_DISPLAYED_PER_PAGE = 10;


        public int work_base { get; set; }
        public int work_random_percentage { get; set; }
        public int work_cooldown { get; set; }

        public int coinflip_min { get; set; }
        public int coinflip_max { get; set; }

        public int steal_money_percentage { get; set; }
        public int steal_success_percentage { get; set; }
        public int steal_cooldown { get; set; }

        public int item_displayed_per_page { get; set; }

        public MoneyConfig(int workbase, int workrandompercentage, int workcooldown, int coinflipmin, 
            int coinflipmax, int stealmoneypercentage, int stealsuccesspercentage, int stealcooldown, int itemdisplayedperpage)
        {
            work_base = workbase;
            work_random_percentage = workrandompercentage;
            work_cooldown = workcooldown;

            coinflip_min = coinflipmin;
            coinflip_max = coinflipmax;

            steal_money_percentage = stealmoneypercentage;
            steal_success_percentage = stealsuccesspercentage;
            steal_cooldown = stealcooldown;

            item_displayed_per_page = itemdisplayedperpage;
        }
    }
}