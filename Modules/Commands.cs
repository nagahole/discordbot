using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.API;
using Discord.Commands;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

//TODO : Add a DiscordUtils class with all of these general methods

namespace Discord_Bot_2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        public static Random rnd = new Random();

        public static string factsfile = Program.folderPath + @"facts.txt";

        public static string copypastafile = Program.folderPath + $"copypastas.txt";

        public static string[] facts;

        public static string[] copypastas;

        public static ulong tempid;

        //LEGACY
        #region
            /*
        [Command("opencase", RunMode = RunMode.Async)]
        public async Task OpenCase()
        {
            double g = rnd.NextDouble();
            string skin = "";

            var e = new EmbedBuilder();
            e.Description = "Opening a ``Falchion Case``";
            await ReplyAsync(embed: e.Build());

            if(g < 0.0025575)//gold
            {
                skin = "Gold";
                skin += " Falchion Knife";
            }
            else if (g < 0.0089514)//red
            {
                skin = "Covert";
                skin += " AWP | Hyper Beast";
            }
            else if (g < 0.0409207)//pink
            {
                skin = "Classified";
                var classified = new string[]
                {
                    "MP7 | Nemisis",
                    "SG 553 | Cyrex"
                };
                skin += $" {classified[rnd.Next(0, classified.Length)]}";
            }
            else if (g < 0.2007672) //prple
            {
                skin = "Restricted";
                var restricted = new string[]
                {
                    "FAMAS | Neural Net",
                    "M4A4 | Evil Daimyo",
                    "MP9 | Ruby Poison Dart",
                    "Negev | Loudmouth",
                    "P2000 | Handgun"
                };
                skin += $" {restricted[rnd.Next(0, restricted.Length)]}";
            }
            else //millispec
            {
                skin = "Milispec";
                var milispecs = new string[]
                {
                    "Galil AR | Rocket Pop",
                    "Glock-18 | Bunsen Burner",
                    "Nova | Ranger",
                    "P90 | Elite Build",
                    "UMP-45 | Riot",
                    "USP-S | Torque"
                };
                skin += $" {milispecs[rnd.Next(0, milispecs.Length)]}";
            }

            double s = rnd.NextDouble();
            if (s < 0.1)
            {
                skin = "StatTrak " + skin;
            }

            var embed = new EmbedBuilder();
            embed.Description = $"You got a {skin}! Float : {(float)rnd.NextDouble()}";
            await ReplyAsync(embed: embed.Build());

        }


        [Command("Spam", RunMode = RunMode.Async)]
        public async Task Spam(IUser user, float delay)
        {
            for (int i = 0; i < 10; i++)
            {
                await ReplyAsync($"Hi " + user.Mention);
                await Task.Delay((int)(delay * 1000f));
            }
        }*/
        #endregion

        //For Hi i'm..
        [Command("m", RunMode = RunMode.Async)]
        public async Task DadJoke(params string[] arg)
        {
            await ReplyAsync($"Hi {string.Join(" ",arg)}, I'm dad");
        }

        [Command("iq", RunMode = RunMode.Async)]
        public async Task IQ(IUser user)
        {
            ulong n = user.Id;
            int a = (int) n.ToString().Select(x => Math.Pow(int.Parse(x.ToString()), 2)).Sum();
            a = (int) Math.Pow((((a % 100) * n.ToString().Length) % 100), Math.PI);
            var embed = new EmbedBuilder();
            embed.Description = $"{user.Mention}'s IQ is an astounding {a % 101 + 50}!";

            await ReplyAsync(embed: embed.Build());
        }

        [Command("iq", RunMode = RunMode.Async)]
        public async Task IQ()
        {
            await IQ(Context.Message.Author);
        }


        [Command("lovelevel", RunMode = RunMode.Async)]
        public async Task LoveLevel(string a, string b)
        {
            int lovelevel = (a.ToLower().ToCharArray().Select(x=>(int)x).Sum() * b.ToLower().ToCharArray().Select(x => (int)x).Sum()) % 101;
            var embed = new EmbedBuilder();
            embed.Description = $"{Char.ToUpper(a[0]) + (a.Length>1? a.Substring(1):"")} and {Char.ToUpper(b[0]) + (b.Length > 1 ? b.Substring(1) : "")} has a love rating of.... {lovelevel}!";
            await ReplyAsync(embed: embed.Build());
        }


        [Command("setmuterole", RunMode = RunMode.Async)]
        public async Task SetMuteRole(IRole role)
        {
            if (!await CheckForOwner(Context.Message.Author))
                return;

            tempid = role.Id;
            var embed = new EmbedBuilder();
            embed.WithDescription($"{role.Mention} has successfully been set as the mute role!");
            await ReplyAsync(embed: embed.Build());
        }

        [Command("mute", RunMode = RunMode.Async)]
        public async Task Mute(IUser user, int seconds = 60, params string[] arg)
        {
            if (!await CheckForOwner(Context.Message.Author))
                return;

            string reason = string.Join(" ", arg);

            var u = Context.Guild.GetUser(user.Id);

            var muted = u.Guild.GetRole(tempid);

            await u.AddRoleAsync(muted);

            var embed1 = new EmbedBuilder();
            embed1.AddField("Muted!",$"{u.Mention} has been muted for {seconds} seconds");
            embed1.AddField("=======",$"Muted by {Context.Message.Author.Mention}");
            if (reason.Length != 0)
            {
                embed1.WithFooter(f => f.Text = $"Reason: {reason}");
            }
            await ReplyAsync(embed: embed1.Build());

            await Task.Delay(seconds * 1000);

            //if (u.Roles.Contains(muted))
            //{
                await u.RemoveRoleAsync(muted);

                var embed2 = new EmbedBuilder();
                embed2.WithDescription($"{u.Mention} has been unmuted from a {seconds} second mute!");
                await ReplyAsync(embed: embed2.Build());
            //}
        }

        [Command("stats", RunMode = RunMode.Async)]
        public async Task Stats(IUser user)
        {
            var u = Context.Guild.GetUser(user.Id);
            var embed = new EmbedBuilder();

            DateTime date = DateTime.Parse(u.JoinedAt.ToString());

            embed.Color = Color.Red;

            embed.WithTitle($"Stats of {u.Nickname}")
                .WithDescription(u.Mention)
                .WithColor(Color.Blue)
                .WithImageUrl(u.GetAvatarUrl())
                .AddField("Join Date", date.ToShortDateString())
                .AddField("Days On Server", Utils.Date.AgeInDays(date))
                .WithFooter(f => f.Text = "To use this command, type $stats (user)")
                .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
   
        }

        [Command("Bully", RunMode = RunMode.Async)]
        public async Task Bully(IUser user, params string[] arg)
        {
            var u = Context.Guild.GetUser(user.Id);
            string n = string.Join(" ",arg);

            if(n.Length < 2 || n.Length > 32)
            {
                var a = new EmbedBuilder();
                a.WithDescription("Error: Name has invalid length ``(2-32)``");
                await ReplyAsync(embed: a.Build());
                return;
            }

            if (u.Nickname == n)
            {
                var e = new EmbedBuilder();
                e.WithDescription($"{u.Mention}'s name is already {n}!");
                e.Color = Color.Red;
                await ReplyAsync(embed: e.Build());
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithDescription($"{Context.Message.Author.Mention} has bullied {user.Mention} into {n}");
            embed.Color = Color.Blue;
            await ReplyAsync(embed: embed.Build());

            await u.ModifyAsync(x => {
                x.Nickname = n;
            });

            
            return;
        }

        [Command("Profile", RunMode = RunMode.Async)]
        public async Task Profile(IUser user)
        {
            await ReplyAsync(user.GetAvatarUrl(size: 512));
        }
        [Command("Profile", RunMode = RunMode.Async)]
        public async Task Profile()
        {
            await Profile(Context.Message.Author);
        }

        [Command("Fact", RunMode = RunMode.Async)]
        public async Task Fact()
        {
            if (facts == null)
            {
                string raw = File.ReadAllText(factsfile);
                facts = JsonConvert.DeserializeObject<string[]>(raw);
            }
            var embed = new EmbedBuilder();

            embed.AddField("Fact", facts[rnd.Next(0, facts.Length)])
            .WithCurrentTimestamp();


            await ReplyAsync(embed : embed.Build());
        }

        [Command("Copypasta", RunMode = RunMode.Async)]
        public async Task Copypasta()
        {
            if (copypastas == null)
            {
                using (StreamReader r = new StreamReader(copypastafile))
                {
                    string raw = r.ReadToEnd();
                    copypastas = JsonConvert.DeserializeObject<string[]>(raw);
                }
            }
            await ReplyAsync(copypastas[rnd.Next(0, copypastas.Length)]);
        }

        [Command("Clearchat", RunMode = RunMode.Async)]
        public async Task Clearchat()
        {
            if (! await CheckForOwner(Context.Message.Author))
                return;
            await ReplyAsync(".\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n.");
        }

        public async Task<bool> CheckForOwner(IUser user)
        {
            if (user != Context.Guild.Owner)
            {
                await OwnerOnlyCommand();
                return false;
            }
            return true;
        }

        public async Task OwnerOnlyCommand()
        {
            await QuickEmbed("Only the owner of the server can execute this command!");
        }

        public async Task QuickEmbed(string str)
        {
            await ReplyAsync(embed: new EmbedBuilder { Description = str }.Build());
        }
    }
}

namespace Utils
{
    using System;
    using System.Reflection;

    public struct ParamElement
    {
        public Type type { get; private set; }

        public string input { get; private set; }

        public ParamElement(Type t, string s)
        {
            type = t;
            input = s;
        }
    }
    public struct ParamStruct
    {
        private List<ParamElement> melements;
        public List<ParamElement> elements
        {
            get
            {
                if (melements == null)
                {
                    melements = new List<ParamElement>();
                }
                return melements;
            }
            private set
            {
                if (melements == null)
                {
                    melements = new List<ParamElement>();
                }
                melements = value;
            }
        }

        public void AddParam(Type t, string s)
        {
            elements.Add(new ParamElement(t, s));
        }

        public void AddParam(ParamElement t)
        {
            elements.Add(t);
        }
    }


    public class Mathf
    {
        static public float Clamp(float value, float min, float max) => value >= max ? max : value <= min ? min : value;

        static public int Clamp(int value, int min, int max) => value >= max ? max : value <= min ? min : value;

        /// <summary>
        /// Returns a bool of whether the inputed value was within the specific range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static public bool Between(float value, float min, float max) => value >= max ? false : value <= min ? false : true;
        /// <summary>
        /// Returns a bool of whether the inputed value was within the specific range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>

        static public bool Between(int value, int min, int max) => value >= max ? false : value <= min ? false : true;

        static public int Above(int val, int thres) => val < thres ? thres : val;

        static public int Below(int val, int thres) => val > thres ? thres : val;
    }

    public class CC
    {
        static public void Add(ref int n, int amount)
        {
            Console.WriteLine($"{n} += {amount}");
            n += amount;
        }

        /// <summary>
        /// WARNING : ASSUMES THAT STRING IS INPUTTED CORRECTLY
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        static public dynamic Parse(Type t, string s)
        {
            if (t == typeof(bool))
            {
                return bool.Parse(s);
            }
            else if (t == typeof(int))
            {
                return int.Parse(s);
            }
            else if (t == typeof(float))
            {
                return float.Parse(s);
            }
            else if (t == typeof(string))
            {
                return s;
            }
            Console.WriteLine("Null");
            return null;
        }

        static public dynamic Parse(ParamElement e)
        {
            Type t = e.type;

            string s = e.input;

            if (t == typeof(bool))
            {
                return bool.Parse(s);
            }
            else if (t == typeof(int))
            {
                return int.Parse(s);
            }
            else if (t == typeof(float))
            {
                return float.Parse(s);
            }
            else if (t == typeof(string))
            {
                return s;
            }

            Console.WriteLine("Null");
            return null;
        }

        public static string GetBlock(string input, int min, int max)
        {

            int block = 0;
            int br = 0;
            string retVal = "";
            bool metchar = false; //has met something that is not whitespace

            for (int i = 0; i < input.Length; i++)
            {
                string c = input[i].ToString();

                if ((c == " " || c == "(") && br == 0 && metchar == true)
                {
                    block++;
                    metchar = false;
                }

                if (block >= max)
                {
                    break;
                }

                if (block >= min)
                {
                    retVal += c;
                }

                metchar = c == " " ? metchar : true;
                //placed at the end because if placed at the front a ( will always never create a new block (assuming correct syntax)
                br += c == "(" ? 1 : c == ")" ? -1 : 0; //adds 1 if there is ( and -1 if there is )
            }

            //Console.WriteLine("Block generated is '" + retVal.Trim() + "'");
            return retVal.Trim();
        }

        /// <summary>
        /// Only for primitve classes
        /// </summary>
        /// <param name="input"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool TypeMatch(List<string> input, List<Type> types)
        {
            if (input.Count != types.Count)
            {
                return false;
            }

            for (int i = 0; i < input.Count; i++)
            {
                Type type = types[i];
                if (type == typeof(bool))
                {
                    bool r;
                    if (!bool.TryParse(input[i], out r)) { return false; }
                }
                else if (type == typeof(int))
                {
                    int r;
                    if (!int.TryParse(input[i], out r)) { return false; }
                }
                else if (type == typeof(float))
                {
                    float r;
                    if (!float.TryParse(input[i], out r)) { return false; }
                }

                //string doesn't need to be checked since it will always be valid
            }

            return true;
        }





        static public List<string> GetParams(string input) //input example "(2321, asd ,3123, sdasd)"
        {
            List<string> retVal = new List<string>();
            string curVal = "";
            int br = 0, com = 0;

            Console.WriteLine($"Looking for params in \"{input}\"");

            for (int i = 0; i < input.Length; i++)
            {
                string c = input[i].ToString();

                com += c == "," ? 1 : 0;

                if ((c == "," || c == ")") && br == 1) //separator or to check if last bracket
                {
                    retVal.Add(curVal.Trim());
                    curVal = "";
                }
                else if (br > 0) //to exclude first pair of brackets
                {
                    curVal += c;
                }

                //placed at the end because if placed at the front a ( will always never create a new block (assuming correct syntax)
                br += c == "(" ? 1 : c == ")" ? -1 : 0; //adds 1 if there is ( and -1 if there is )
            }

            Console.WriteLine($"{com} commas detected and {retVal.Count} items found in param!");

            return retVal;
        }

        static public List<Type> GetMethodTypes(string methodName, Type fromClass)
        {
            List<Type> retVal = new List<Type>();

            var param = fromClass.GetMethod(methodName).GetParameters();

            for (int i = 0; i < param.Length; i++)
            {
                retVal.Add(param[i].ParameterType);
            }
            return retVal;
        }

        static public List<Type> GetMethodTypes(MethodInfo methodInfo)
        {
            List<Type> retVal = new List<Type>();

            var param = methodInfo.GetParameters();

            for (int i = 0; i < param.Length; i++)
            {
                retVal.Add(param[i].ParameterType);
            }
            return retVal;
        }

        static public void LogList(List<string> vs)
        {
            Console.WriteLine($"Logging {vs.Count} items from a List<string>...");

            Console.Write("[");
            for (int i = 0; i < vs.Count; i++)
            {
                Console.Write($"'{vs[i]}'" + ((i != vs.Count - 1) ? "," : ""));
            }
            Console.Write("]");
        }

        static public void LogList(List<int> vs)
        {
            Console.WriteLine($"Logging {vs.Count} items from a List<int>...");

            Console.Write("[");
            for (int i = 0; i < vs.Count; i++)
            {
                Console.Write($"'{vs[i]}'" + ((i != vs.Count - 1) ? "," : ""));
            }
            Console.Write("]");
        }

        static public void LogList(List<float> vs)
        {
            Console.WriteLine($"Logging {vs.Count} items from a List<float>...");

            Console.Write("[");
            for (int i = 0; i < vs.Count; i++)
            {
                Console.Write($"'{vs[i]}'" + ((i != vs.Count - 1) ? "," : ""));
            }
            Console.Write("]");
        }

        static public void LogList(List<bool> vs)
        {
            Console.WriteLine($"Logging {vs.Count} items from a List<bool>...");

            Console.Write("[");
            for (int i = 0; i < vs.Count; i++)
            {
                Console.Write($"'{(vs[i] ? 'T' : 'F')}'" + ((i != vs.Count - 1) ? "," : ""));
            }
            Console.Write("]");
        }

        static public void LogList(List<Type> vs)
        {
            Console.WriteLine($"Logging {vs.Count} items from a List<Type>...");

            Console.Write("[");
            for (int i = 0; i < vs.Count; i++)
            {
                Console.Write($"'{vs[i].ToString()}'" + ((i != vs.Count - 1) ? "," : ""));
            }
            Console.Write("]");
        }
    }

    public class Legacy
    {
        static List<string> strue = new List<string>() { "True", "true", "t", "1" }; //strings that can reflect to true

        static List<string> sfalse = new List<string>() { "False", "false", "f", "0" }; //strings that can reflet to false

        /// <summary>
        /// checks for float
        /// </summary>
        /// <returns>returns a bool value on whether the inputed string can be parsed to a float</returns>
        static public bool IsFloat(string s)
        {
            int dec = 0;
            for (int i = 0; i < s.Length; i++)
            {
                string c = s[i].ToString();
                dec += c == "." ? 1 : 0;

                if (!"-1234567890.".Contains(c) || dec > 1) { return false; };
            }
            return true;
        }

        static public bool IsInt(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!"-1234567890".Contains(s[i].ToString())) { return false; };
            }
            return true;
        }

        static public bool IsBool(string s)
        {
            for (int i = 0; i < strue.Count || i < sfalse.Count; i++)
            {
                if (i < strue.Count)
                {
                    if (strue[i] == s)
                    {
                        return true;
                    }
                }
                if (i < sfalse.Count)
                {
                    if (sfalse[i] == s)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }

    public class Date
    {
        public static int AgeInYears(DateTime date)
        {
              var dateT = DateTime.Now;
              return (dateT.Month >= date.Month && dateT.Day >= date.Day) ? dateT.Year - date.Year : dateT.Year - date.Year - 1;
        }

        public static int AgeInMonths(DateTime date)
        {
            var dateT = DateTime.Now;
            return (AgeInYears(date) * 12 + 12 + ((dateT.Day >= date.Day) ? dateT.Month - date.Month : dateT.Month - date.Month - 1));
        }

        public static int AgeInDays(DateTime date)
        {
            var now = DateTime.Now;

            int retVal = 0;

            //basically loops through each month in first and current year, then just adds the inbetweens
            for (int y = date.Year; y <= now.Year; y++)
            {
                if (y != date.Year && y != now.Year)
                {
                    CC.Add(ref retVal, DateTime.IsLeapYear(y) ? 366 : 365); //leap year vs not leap year
                }
                else
                {
                    if (y == now.Year) //current year
                    {
                        for (int m = now.Year == date.Year? date.Month:1; m < now.Month; m++)
                        {
                            CC.Add(ref retVal, DateTime.DaysInMonth(y, m));
                        }
                        CC.Add(ref retVal, (now.Day) - ((now.Year == date.Year && now.Month == date.Month)? date.Day:0));
                    }
                    else if (y == date.Year) //birth year
                    {
                        for (int m = date.Month; m <= 12; m++) //itterates through the months
                        {
                            CC.Add(ref retVal, DateTime.DaysInMonth(y, m)); //gets the day per month
                        }
                        CC.Add(ref retVal, - (now.Day - 1));
                    }
                    
                }
            }

            return retVal;
        }
    }
}