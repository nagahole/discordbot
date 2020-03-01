using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_Bot_2.Modules.Money_Module.Structs;
using Discord_Bot_2.Modules.Money_Module.Data;
using Newtonsoft.Json;

//WARNING - SAVETOJSON DOESNT APPLY AUTOMATICALLY ON ADDING COOLDOWNS

namespace Discord_Bot_2.Modules.Money_Module
{
    public class Money : ModuleBase<SocketCommandContext>
    {
        Random rnd = new Random();

        private static string userDataPath = Program.folderPath + @"userdata.txt";
        private static string moneyconfigPath = Program.folderPath + @"moneyconfig.txt";
        private static bool initUserPath = false;
        private static bool initMoneyConfig = false;

        //------------[CONFIGS]--------------//

        //TODO : Make it so these values can be changed from commands, and when it is changed, change the json value and change the variable value. (json value is only read from at initialisation
        //OR Read from configs.txt, which is then deserialized from a struct Settings



        private static MoneyConfig _configs;

        public static MoneyConfig configs
        {
            get
            {
                if (initMoneyConfig) //isnt setup
                {
                    return _configs;
                }

                //first runtime setup
                initMoneyConfig = true;
                string raw = File.ReadAllText(moneyconfigPath);

                if (raw.Length == 0)//is empty
                {
                    _configs = MoneyConfig.defaultValues; //if the txt file is empty, just use default values set by the MoneyConfig struct
                    File.WriteAllText(moneyconfigPath, JsonConvert.SerializeObject(_configs));
                    return _configs;
                }
                _configs = JsonConvert.DeserializeObject<MoneyConfig>(raw); //if it is not empty, use the values in the txt file
                File.WriteAllText(moneyconfigPath, JsonConvert.SerializeObject(_configs));
                return _configs;

            }
            private set //probably wont be using this
            {
                _configs = value;
                string json = JsonConvert.SerializeObject(_configs);
                File.WriteAllText(moneyconfigPath, json);
            }
        }

        //------------[CONFIGS]--------------//

        private static Dictionary<ulong, UserData> _users = new Dictionary<ulong, UserData>();

        public static Dictionary<ulong, UserData> users
        {
            get
            {
                if (!initUserPath)
                {
                    string json = File.ReadAllText(userDataPath);
                    Console.WriteLine($"JSON : \"{json}\"");
                    if (json.Length != 0)
                    {
                        _users = JsonConvert.DeserializeObject<Dictionary<ulong, UserData>>(json);
                    }
                    if (_users == null)
                    {
                        Console.WriteLine("_users is null");
                    }
                    initUserPath = true;
                }
                return _users;
            }
        }

        [Command("json", RunMode = RunMode.Async)]
        public async Task JSON(string str = "userdata")
        {
            switch (str.ToLower())
            {
                case "userdata":
                    await QuickEmbed($"```json\n{File.ReadAllText(userDataPath)}```");
                    break;

                case "moneyconfig":
                    await QuickEmbed($"```json\n{File.ReadAllText(moneyconfigPath)}```");
                    break;

                default:
                    await QuickEmbed("String not recognised");
                    break;
            }
        }

        [Command("flush", RunMode = RunMode.Async)]
        public async Task Flush(string str = "all")
        {
            if (!await CheckForOwner())
                return;

            switch (str.ToLower())
            {
                case "all":
                    File.WriteAllText(userDataPath, "");
                    configs = new MoneyConfig(0, 0, 0, 0, 0, 0, 0, 0, 0);
                    File.WriteAllText(moneyconfigPath, "");
                    users.Clear();

                    await QuickEmbed("Successfully flushed **all** data");
                    break;
                case "userdata":
                    File.WriteAllText(userDataPath, "");
                    users.Clear();

                    await QuickEmbed("Successfully flushed all **userdata**");
                    break;

                case "moneyconfig":
                    configs = new MoneyConfig(0, 0, 0, 0, 0, 0, 0, 0, 0);
                    File.WriteAllText(moneyconfigPath, "");
                    

                    await QuickEmbed("Successfully flushed **moneyconfig**");
                    break;

                default:
                    await QuickEmbed("String not recognised");
                    break;
            }
        }

        [Command("defaultconfig", RunMode = RunMode.Async)]
        public async Task DefaultConfig()
        {
            if (!await CheckForOwner())
                return;

            configs = MoneyConfig.defaultValues;
            File.WriteAllText(moneyconfigPath, JsonConvert.SerializeObject(configs));

            await QuickEmbed("moneyconfig.txt values have been defaulted!");
        }

        /// <summary>
        /// Updates config from moneyconfig.txt
        /// </summary>
        /// <returns>Nothing</returns>
        [Command("updateconfig", RunMode = RunMode.Async)]
        public async Task UpdateConfig(string arg = "moneyconfig")
        {
            switch (arg.ToLower())
            {
                case "moneyconfig":
                    string raw = File.ReadAllText(moneyconfigPath);

                    if (raw.Length == 0)
                    {
                        _configs = MoneyConfig.defaultValues;
                        File.WriteAllText(moneyconfigPath,
                            JsonConvert.SerializeObject(_configs));
                        await QuickEmbed("moneyconfig.txt is empty! Instead defaulting values");
                    }

                    _configs = JsonConvert.DeserializeObject<MoneyConfig>(raw);
                    var embed = new EmbedBuilder { Description = "Configs has successfully been updated from moneyconfig.txt" };

                    //gets each member
                    string values = UnpackValues(_configs);

                    embed.AddField("Values", values);
                    await ReplyAsync(embed: embed.Build());
                    break;
                case "csgoconfig":
                    CSGOConfigs.UpdateConfigFromJson();

                    var e = new EmbedBuilder { Description = "Configs has successfully been updated from moneyconfig.txt" };

                    //gets each member
                    string v = UnpackValues(CSGOConfigs.config);

                    e.AddField("Values", v);
                    await ReplyAsync(embed: e.Build());
                    break;
                default:
                    await QuickEmbed("File not recognised");
                    break;

            }
        }

        [Command("work", RunMode = RunMode.Async)]
        public async Task Work()
        {

            UserData temp = GetUserData(Context.Message.Author.Id); //temp not getting anything

            //cooldown

            if (temp.HasCooldown(Cooldown.Work))
            {
                await QuickEmbed($"You have to wait " +
                    $"{temp.cooldowns[Cooldown.Work].remainingTime} more seconds before working");
                return;
            }

            temp.AddCooldown(Cooldown.Work, configs.work_cooldown);

            //money

            int moneyearned = (int)(configs.work_base * (1 + (rnd.Next(0, configs.work_random_percentage) / 100f)));

            temp.cash += moneyearned;

            ChangeUserData(Context.Message.Author.Id, temp);

            //ALL THE BACKEND CODE IS DONE. THIS IS JUST DISPLAY

            await QuickEmbed($"{Context.Message.Author.Mention} just worked and got **{moneyearned}** dollars!");
        }

        [Command("deletedata", RunMode = RunMode.Async)]
        public async Task DeleteData(IUser user)
        {
            if (!await CheckForOwner())
                return;

            users.Remove(user.Id);
            SaveUserDataToJson();
            await QuickEmbed($"User {user.Mention}'s data has been erased");
        }

        [Command("givemoney", RunMode = RunMode.Async)]
        public async Task GiveMoney(IUser user, int amount)
        {
            if (!await CheckForOwner())
                return;

            UserData u = GetUserData(user.Id);
            u.cash += amount;
            ChangeUserData(user.Id, u);
            await QuickEmbed($"User {user.Mention}'s has been given **${amount}**!");
            return;
        }

        [Command("Deposit", RunMode = RunMode.Async)]
        public async Task Deposit(int amount)
        {
            UserData d = GetUserData(Context.Message.Author.Id);
            if (amount < 0)
            {
                await EnterNonNegativeValue();
                return;
            }
            if (amount > d.cash)
            {
                await InsufficientFunds();
                return;
            }

            d.cash -= amount;
            d.bank += amount;
            ChangeUserData(Context.Message.Author.Id, d);

            await QuickEmbed($"{Context.Message.Author.Mention} deposited **${amount}** into the bank. New balance: " +
                $"\nCash: **${d.cash}** \nBank: **${d.bank}**");
        }
        [Command("DepositAll", RunMode = RunMode.Async)]
        public async Task DepositAll() //might optimise and remove the GetUserData
        {
            UserData d = GetUserData(Context.Message.Author.Id);
            await Deposit(d.cash);
        }

        [Command("Withdraw", RunMode = RunMode.Async)]
        public async Task Withdraw(int amount)
        {
            UserData d = GetUserData(Context.Message.Author.Id);
            if (amount < 0)
            {
                await EnterNonNegativeValue();
                return;
            }
            if (amount > d.bank)
            {
                await InsufficientFunds();
                return;
            }

            d.cash += amount;
            d.bank -= amount;
            ChangeUserData(Context.Message.Author.Id, d);

            await QuickEmbed($"{Context.Message.Author.Mention} withdrew **${amount}** into the bank. New balance: " +
                $"\nCash: **${d.cash}** \nBank: **${d.bank}**");
        }
        [Command("WithdrawAll", RunMode = RunMode.Async)]
        public async Task WithdrawAll() //might optimise and remove the GetUserData
        {
            UserData d = GetUserData(Context.Message.Author.Id);
            await Withdraw(d.bank);
        }

        [Command("jump", RunMode = RunMode.Async)]
        public async Task Jump(IUser target)
        {
            UserData d = GetUserData(Context.Message.Author.Id);

            if (d.HasCooldown(Cooldown.Steal))
            {
                CooldownData cd = d.cooldowns[Cooldown.Steal];
                await QuickEmbed($"You have to wait {cd.remainingTime} before stealing again!");
                return;
            }

            d.AddCooldown(Cooldown.Steal, configs.steal_cooldown);

            UserData t = GetUserData(target.Id);

            if (rnd.Next(0, 100) <= configs.steal_success_percentage) //successful
            {
                int stealamount = (int)(t.cash * configs.steal_money_percentage / 100f);

                d.cash += stealamount;
                t.cash -= stealamount;

                users[Context.Message.Author.Id] = d; //didn't use method because this is used twice
                users[target.Id] = t;

                SaveUserDataToJson();
                await QuickEmbed($"{Context.Message.Author.Mention} just stole **${stealamount}** from {target.Mention}!");
                return;
            }
            SaveUserDataToJson();

            await QuickEmbed($"{Context.Message.Author.Mention} has been caught stealing from {target.Mention}!");
        }

        [Command("bal", RunMode = RunMode.Async)]
        public async Task Bal(IUser user)
        {
            UserData d = GetUserData(user.Id);
            SaveUserDataToJson(); //incase this is a new user

            await QuickEmbed($"{user.Mention} has \n" +
                $"**${d.cash}** cash and **${d.bank}** in bank");
        }
        [Command("bal", RunMode = RunMode.Async)]
        public async Task Bal()
        {
            await Bal(Context.Message.Author);
        }

        //--------------------[BUYING]--------------------//
        [Command("buycase", RunMode = RunMode.Async)]
        public async Task BuyCase()
        {
            UserData user = GetUserData(Context.Message.Author.Id);
            string shop = "";
            
            for(int i = 0; i < Cases.cases.Length; i ++)
            {
                shop += $"{i} - {Cases.cases[i].name}\n";
                Console.WriteLine(Cases.cases[i].name + Cases.cases.Length.ToString());
            }

            var e = new EmbedBuilder();
            e.WithDescription($"The current cost of cases is ``{CSGOConfigs.config.caseprice}`` coins")
                .AddField("Avaliable Cases", shop);

            await ReplyAsync(embed: e.Build());
        }

        [Command("buycase", RunMode = RunMode.Async)]
        public async Task BuyCase(int n)
        {
            UserData user = GetUserData(Context.Message.Author.Id);
            
            if(CSGOConfigs.config.caseprice > user.cash)
            {
                await QuickEmbed($"Insufficient funds! A case costs ``${CSGOConfigs.config.caseprice}``");
                return;
            }
            if(n >= Cases.cases.Length || n < 0 )
            {
                await QuickEmbed($"Please enter a number between 0 and {Cases.cases.Length - 1}");
                return;
            }

            Console.WriteLine($"There are {Cases.cases.Length} cases aval");

            user.cash -= CSGOConfigs.config.caseprice;
            SkinInstance skin = Cases.cases[n].OpenCase();
            user.skins.Add(skin);

            ChangeUserData(Context.Message.Author.Id, user);

            var embed = new EmbedBuilder();

            embed.WithDescription($"{Context.Message.Author.Mention} just unboxed a")
                .AddField("---------------", $"**{skin.ToString()}** \n" +
                $"from a {Cases.cases[n].name}!\n" +
                $"Value **${skin.worth}**")
                .WithColor(CSGOConfigs.GetColor(skin.skinData.rarity));

            if(skin.skinData.imageURL != "")
            {
                embed.WithImageUrl(skin.skinData.imageURL);
            }


            await ReplyAsync(embed: embed.Build());
        }

        [Command("Pay", RunMode = RunMode.Async)]
        public async Task Pay(IUser user, int amount)
        {
            if(amount < 0)
            {
                await EnterNonNegativeValue();
                return;
            }

            UserData payer = GetUserData(Context.Message.Author.Id);

            if(amount > payer.cash)
            {
                await InsufficientFunds();
                return;
            }

            UserData payee = GetUserData(user.Id);

            payee.cash += amount;
            payer.cash -= amount;

            users[user.Id] = payee;
            users[Context.Message.Author.Id] = payer;

            SaveUserDataToJson();

            await QuickEmbed($"{Context.Message.Author.Mention} gave **${amount}** to {user.Mention}");
        }

        [Command("inspect", RunMode = RunMode.Async)]
        public async Task Inspect(int index)
        {
            if(index < 0)
            {
                await EnterNonNegativeValue();
                return;
            }
            List<SkinInstance> skins = GetUserData(Context.Message.Author.Id).skins;
            if(index >= skins.Count)
            {
                await QuickEmbed($"Please enter a number between 0 and {skins.Count - 1}");
                return;
            }
            SkinInstance skin = skins[index];

            var e = new EmbedBuilder();
            e.WithDescription($"{Context.Message.Author.Mention}'s **{skin.skinData.weapon} {skin.skinData.name}**")
                .AddField("=======================",
                $"Value: **${skin.worth}**\n" +
                $"Float: **{skin.floatValue}**\n" +
                $"Condition: **{CSGOConfigs.LogCondition(skin.condition)}**\n" +
                $"Rarity: **{CSGOConfigs.LogRarity(skin.skinData.rarity)}**\n" +
                $"Stattrak? **{(skin.stattrak? "YES" : "NO")}**\n" +
                $"ID: **{skin.id}**\n" +
                $"Description: *{skin.skinData.description}*")
                .WithColor(CSGOConfigs.GetColor(skin.skinData.rarity));
            if(skin.skinData.imageURL != "")
            {
                e.WithImageUrl(skin.skinData.imageURL);
            }
            await ReplyAsync(embed: e.Build());
            

        }

        [Command("Skins", RunMode = RunMode.Async)]
        public async Task Skins(int page = 1)
        {
            UserData user = GetUserData(Context.Message.Author.Id);
            string inv = "";
            int total = 0;
            int pages = (int) Math.Ceiling(user.skins.Count / (float) configs.item_displayed_per_page);

            if(page > pages)
            {
                await QuickEmbed($"You only have {pages} pages of skins!");
                return;
            }

            for (int i = configs.item_displayed_per_page * page - configs.item_displayed_per_page; 
                i < user.skins.Count && i < configs.item_displayed_per_page * page; 
                i ++)
            {
                SkinInstance item = user.skins[i];
                inv += $"[{i}] - **${item.worth}** ``{item.ToString()}`` \n";
                total += item.worth;
            }

            var e = new EmbedBuilder();
            e.WithDescription($"{Context.Message.Author.Mention}'s skins")
            .AddField("---------------", inv)
                .AddField("Inventory Worth:", $"**${total}**")
                .WithFooter(x => x.Text = $"Displaying page {page} / {pages}");
                
            await ReplyAsync(embed: e.Build());
        }

        [Command("Skins", RunMode = RunMode.Async)]
        public async Task Skins(IUser u, int page = 1)
        {
            UserData user = GetUserData(u.Id);
            string inv = "";
            int total = 0;
            int pages = (int)Math.Ceiling(user.skins.Count / (float)configs.item_displayed_per_page);

            if (page > pages)
            {
                await QuickEmbed($"{u.Mention} only has {pages} pages of skins!");
                return;
            }

            for (int i = configs.item_displayed_per_page * page - configs.item_displayed_per_page;
                i < user.skins.Count && i < configs.item_displayed_per_page * page;
                i++)
            {
                SkinInstance item = user.skins[i];
                inv += $"[{i}] - **${item.worth}** ``{item.ToString()}`` \n";
                total += item.worth;
            }

            var e = new EmbedBuilder();
            e.WithDescription($"{u.Mention}'s skins")
            .AddField("---------------", inv)
                .AddField("Inventory Worth:", $"**${total}**")
                .WithFooter(x => x.Text = $"Displaying page {page} / {pages}");

            await ReplyAsync(embed: e.Build());
        }

        [Command("sellskin", RunMode = RunMode.Async)]
        public async Task SellSkin(string arg)
        {
            switch(arg.ToLower())
            {
                case "gold":
                case "golds":
                    await SellAllSkinsOfRarity(Rarity.Gold);
                    break;

                case "covert":
                case "coverts":
                case "red":
                case "reds":
                    await SellAllSkinsOfRarity(Rarity.Covert);
                    break;

                case "classified":
                case "classifieds":
                case "pink":
                case "pinks":
                    await SellAllSkinsOfRarity(Rarity.Classified);
                    break;

                case "restricted":
                case "restricteds":
                case "purple":
                case "purples":
                    await SellAllSkinsOfRarity(Rarity.Restricted);
                    break;

                case "milispec":
                case "milispecs":
                case "blue":
                case "blues":
                    await SellAllSkinsOfRarity(Rarity.Milispec);
                    break;

                case "all":
                    await SellAll();
                    break;

                default:
                    await QuickEmbed("Rarity not recognised");
                    break;
                   
            }
        }
        public async Task SellAll()
        {
            UserData user = GetUserData(Context.Message.Author.Id);
            int moneymade = 0;
            if(user.skins.Count == 0)
            {
                await QuickEmbed("You have no skins to sell!");
                return;
            }
            int sold = user.skins.Count;

            for (int i = user.skins.Count - 1; i >= 0; i--) //needs to reverse because list gets edited throughout the looop
            {
                moneymade += user.skins[i].worth;
            }

            user.skins.Clear();
            user.cash += moneymade;

            ChangeUserData(Context.Message.Author.Id, user);

            await QuickEmbed($"{Context.Message.Author.Mention} sold all his skins ({sold}) for **${moneymade}**!");
        }

        public async Task SellAllSkinsOfRarity(Rarity rarity)
        {
            await QuickEmbed($"Selling skins of rarity {CSGOConfigs.LogRarity(rarity)}");
            UserData user = GetUserData(Context.Message.Author.Id);
            int numberSold = 0;
            int moneymade = 0;
            for(int i = user.skins.Count - 1; i >= 0; i --) //needs to reverse because list gets edited throughout the looop
            {
                SkinInstance s = user.skins[i];
                if(s.skinData.rarity == rarity)
                {
                    numberSold++;
                    moneymade += s.worth;
                    user.skins.RemoveAt(i);
                }
            }

            if (numberSold == 0)
            {
                await QuickEmbed($"No {CSGOConfigs.LogRarity(rarity)} skins found in your inventory!");
                return;
            }

            user.cash += moneymade;
            ChangeUserData(Context.Message.Author.Id, user);
            await QuickEmbed($"{Context.Message.Author.Mention} sold {numberSold} {CSGOConfigs.LogRarity(rarity)} skins for **${moneymade}**");
        }

        [Command("SellSkinNo", RunMode = RunMode.Async)]
        public async Task SellSkinNo(int index)
        {
            UserData user = GetUserData(Context.Message.Author.Id);
            if(index < 0)
            {
                await EnterNonNegativeValue();
                return;
            }
            if(user.skins.Count == 0)
            {
                await QuickEmbed("You have no skins to sell!");
                return;
            }
            if(user.skins.Count <= index)
            {
                await QuickEmbed($"Please enter a number between 0 and {user.skins.Count - 1}");
                return;
            }

            SkinInstance skin = user.skins[index];
            user.skins.RemoveAt(index);
            user.cash += skin.worth;
            ChangeUserData(Context.Message.Author.Id, user);

            await QuickEmbed($"{Context.Message.Author.Mention} just sold a {skin.ToString()} for **${skin.worth}**");
        }

        //-------------------[BETTING]-------------------//

        [Command("Coinflip", RunMode = RunMode.Async)]
        public async Task Coinflip(int amount, string str)
        {
            await Coinflip(str, amount);
        }

        [Command("Coinflip", RunMode = RunMode.Async)]
        public async Task Coinflip(string str, int amount)
        {
            str = str.ToLower();

            UserData ud = GetUserData(Context.Message.Author.Id);

            if (str != "heads" && str != "head" && str != "h" && str != "tails" && str != "tail" && str != "t")
            {
                await QuickEmbed("Please enter \"heads\" or \"tails\"");
                return;
            }

            if (amount < configs.coinflip_min || amount > configs.coinflip_max)
            {
                await QuickEmbed($"Bet amount must be between {configs.coinflip_min} and {configs.coinflip_max}");
                return;
            }

            if (amount > ud.cash)
            {
                await InsufficientFunds();
                return;
            }

            var embed = new EmbedBuilder();

            if (rnd.Next(0, 2) == 0)//heads 
            {
                if (str == "heads" || str == "head" || str == "h")//heads sucessful
                {
                    ud.cash += amount;
                    embed.WithDescription($"{Context.Message.Author.Mention} just won **${amount}** on heads! \nNew balance : $**{ud.cash}**");
                }
                else //heads unsuccesful
                {
                    ud.cash -= amount;
                    embed.WithDescription($"Sorry {Context.Message.Author.Mention}, it was heads \nNew balance : $**{ud.cash}**");
                }
            }
            else//tails
            {
                if (str == "tails" || str == "tail" || str == "t")
                {
                    ud.cash += amount;
                    embed.WithDescription($"{Context.Message.Author.Mention} just won **${amount}** on tails! \nNew balance : $**{ud.cash}**");
                }
                else //heads unsuccesful
                {
                    ud.cash -= amount;
                    embed.WithDescription($"Sorry {Context.Message.Author.Mention}, it was tails \nNew balance : $**{ud.cash}**");
                }
            }

            ChangeUserData(Context.Message.Author.Id, ud);

            await ReplyAsync(embed: embed.Build());
        }

        //GENERAL METHODS

        /// <summary>
        /// Returns true if the imput user is owner
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CheckForOwner(IUser user)
        {
            if(user != Context.Guild.Owner)
            {
                await OwnerOnlyCommand();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the message sender is owner
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> CheckForOwner()
        {
            if (Context.Message.Author != Context.Guild.Owner)
            {
                await OwnerOnlyCommand();
                return false;
            }
            return true;
        }

        public async Task EnterNonNegativeValue()
        {
            await QuickEmbed("Please enter a non-negative value");
        }

        public async Task OwnerOnlyCommand()
        {
            await QuickEmbed("Only the owner of the server can execute this command!");
        }

        public async Task InsufficientFunds()
        {
            await QuickEmbed("You have insufficient funds!");
        }

        /// <summary>
        /// Writes a quick embed in description format
        /// </summary>
        /// <returns></returns>
        public async Task QuickEmbed(string str)
        {
            await ReplyAsync(embed: new EmbedBuilder { Description = str }.Build());
        }

        public static string UnpackValues<T>(T obj)
        {
            string ret = "";
            string regex = @"[\w]+";
            foreach (var field in typeof(T).GetFields(BindingFlags.Instance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Public))
            {
                ret += $"{Regex.Match(field.Name, regex).Value} : ``{field.GetValue(obj)}``\n";
            }
            return ret;
        }

        public static UserData GetUserData(ulong id)
        {
            CreateUserIfNotExisting(id);
            return users[id];
        }

        public static ulong CreateUserIfNotExisting(ulong id)
        {
            if (!users.ContainsKey(id))
            {
                Console.WriteLine("Created new user with id " + id.ToString());
                users.Add(id, new UserData());
            }
            return id;
        }

        public static void SaveUserDataToJson()
        {
            string json = JsonConvert.SerializeObject(users);
            File.WriteAllText(userDataPath, json);
        }


        /// <summary>
        /// Always use this. Just makes sure to save to JSON afterwards
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newData"></param>
        public static void ChangeUserData(ulong id, UserData newData)
        {
            users[id] = newData;
            SaveUserDataToJson();
        }
    }
}
