using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    static class Dics
    {
        public static Dictionary<int, string> Jobs = new Dictionary<int, string>();
        public static Dictionary<string, string> Skills = new Dictionary<string,string>();
        public static Dictionary<string, string> Buffs = new Dictionary<string, string>();

        public static void Init()
        {
            Jobs.Add(0, "novice");
            Jobs.Add(1, "swordman");
            Jobs.Add(2, "magician");
            Jobs.Add(3, "archer");
            Jobs.Add(4, "acolyte");
            Jobs.Add(5, "merchant");
            Jobs.Add(6, "thief");
            Jobs.Add(7, "knight");
            Jobs.Add(8, "priest");
            Jobs.Add(9, "wizard");
            Jobs.Add(10, "blacksmith");
            Jobs.Add(11, "hunter");
            Jobs.Add(12, "assassin");
            Jobs.Add(14, "crusader");
            Jobs.Add(15, "monk");
            Jobs.Add(16, "sage");
            Jobs.Add(17, "rogue");
            Jobs.Add(18, "alchemist");
            Jobs.Add(19, "bard");
            Jobs.Add(20, "dancer");

            Jobs.Add(4008, "lk");
            Jobs.Add(4009, "hp");
            Jobs.Add(4010, "hw");
            Jobs.Add(4011, "ws");
            Jobs.Add(4012, "sniper");
            Jobs.Add(4013, "assa cross");
            Jobs.Add(4015, "pala");
            Jobs.Add(4016, "champ");
            Jobs.Add(4017, "prof");
            Jobs.Add(4018, "stalker");
            Jobs.Add(4019, "crea");
            Jobs.Add(4020, "clown");
            Jobs.Add(4021, "gypsy");

            Jobs.Add(4001, "high novice");
            Jobs.Add(4002, "high swordi");
            Jobs.Add(4003, "high mage");
            Jobs.Add(4004, "high archer");
            Jobs.Add(4005, "high aco");
            Jobs.Add(4006, "high merch");
            Jobs.Add(4007, "high thief");
        }
    }
}
