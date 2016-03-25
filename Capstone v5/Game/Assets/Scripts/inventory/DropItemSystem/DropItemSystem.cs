using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DropItemSystem;

namespace DropItemSystem.UnityStuff
{
    public static class DropSystem
    {
        //private static string[] Rarity = new string[4] { "Bronze","Silver","Gold", "Platinum" };
        
        /// <summary>
        /// This Drops the item. It uses a hash table and Random Numbers
        /// to determine out of all of our items, in the correct zones, 
        /// which one will drop
        /// </summary>
        public static int GetRarity(Zone currentZone)
        {
            //Dice rolls to get Rarity Multiplier
            //We start with the least chance then progress to a Higher chance rarity as we go along.
            if (DiceRoll.Chance(currentZone.GetPlatinum))
                return 5;
            else if (DiceRoll.Chance(currentZone.GetGold))
                return 3;
            else if (DiceRoll.Chance(currentZone.GetSilver))
                return 2;
            else if (DiceRoll.Chance(currentZone.GetBronze))
                return 1;

            else
                return 0;
        }
        /*
        public static item DropItem(Zone currentZone, int playerLevel)
        {
            item itemToDrop = new item(GetRarity(currentZone), currentZone.ZoneKey);
            itemToDrop.SetItemType(DiceRoll.CustomRoll(0, 5));
            itemToDrop.SetItemStats(playerLevel);
            return itemToDrop;
        } */
    }

    public class Zone
    {
        public int ZoneKey;
        public string ZoneTitle;

        private double bronze;
        private double silver;
        private double gold;
        private double platinum;

        public double GetBronze { get { return bronze; } }
        public double GetSilver{ get { return silver; } }
        public double GetGold { get { return gold; } }
        public double GetPlatinum { get { return platinum; } }

        public Zone() { }
        public Zone (int zoneNumber)
        {
            ZoneKey = zoneNumber;
            MakeRarities();
        }

        private void MakeRarities()
        {            
            if (ZoneKey <= 3)
            {
                bronze = .7;
                silver = .3;
                gold = .05;
                platinum = 0.0;
            }
            else if (ZoneKey > 3 && ZoneKey <= 7)
            {
                bronze = .7;
                silver = .2;
                gold = .1;
                platinum = .05;
            }
            else
            {
                bronze = 1;
                silver = .7;
                gold = .2;
                platinum = .07;
            }
        }
    }
}
