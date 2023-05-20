using System.Reflection;
using System.Xml.Linq;

namespace MageloRankings.Models
{
    public class Character
    {
        public string name { get; set; }
        public string last_name { get; set; }
        public string guild_name { get; set; }
        public int level { get; set; }
        public string race { get; set; }
        public string @class { get; set; }
        public string deity { get; set; }
        public string gender { get; set; }
        public int id { get; set; }
        public int weight { get; set; }
        public int aa_points_unspent { get; set; }
        public int aa_points_spent { get; set; }
        public int hp_regen_standing_base { get; set; }
        public int hp_regen_sitting_base { get; set; }
        public int hp_regen_resting_base { get; set; }
        public int hp_regen_standing_total { get; set; }
        public int hp_regen_sitting_total { get; set; }
        public int hp_regen_resting_total { get; set; }
        public int hp_regen_item { get; set; }
        public int hp_regen_item_cap { get; set; }
        public int hp_regen_aa { get; set; }
        public int mana_regen_standing_base { get; set; }
        public int mana_regen_sitting_base { get; set; }
        public int mana_regen_standing_total { get; set; }
        public int mana_regen_sitting_total { get; set; }
        public int mana_regen_item { get; set; }
        public int mana_regen_item_cap { get; set; }
        public int mana_regen_aa { get; set; }
        public int hp_max_total { get; set; }
        public int hp_max_item { get; set; }
        public int mana_max_total { get; set; }
        public int mana_max_item { get; set; }
        public int end_max_total { get; set; }
        public int ac_total { get; set; }
        public int ac_item { get; set; }
        public int ac_shield { get; set; }
        public int ac_avoidance { get; set; }
        public int ac_mitigation { get; set; }
        public int atk_total { get; set; }
        public int atk_item { get; set; }
        public int atk_item_cap { get; set; }
        public int atk_offense { get; set; }
        public int atk_tohit { get; set; }
        public int STR_total { get; set; }
        public int STR_base { get; set; }
        public int STR_item { get; set; }
        public int STR_aa { get; set; }
        public int STR_cap { get; set; }
        public int STA_total { get; set; }
        public int STA_base { get; set; }
        public int STA_item { get; set; }
        public int STA_aa { get; set; }
        public int STA_cap { get; set; }
        public int AGI_total { get; set; }
        public int AGI_base { get; set; }
        public int AGI_item { get; set; }
        public int AGI_aa { get; set; }
        public int AGI_cap { get; set; }
        public int DEX_total { get; set; }
        public int DEX_base { get; set; }
        public int DEX_item { get; set; }
        public int DEX_aa { get; set; }
        public int DEX_cap { get; set; }
        public int CHA_total { get; set; }
        public int CHA_base { get; set; }
        public int CHA_item { get; set; }
        public int CHA_aa { get; set; }
        public int CHA_cap { get; set; }
        public int INT_total { get; set; }
        public int INT_base { get; set; }
        public int INT_item { get; set; }
        public int INT_aa { get; set; }
        public int INT_cap { get; set; }
        public int WIS_total { get; set; }
        public int WIS_base { get; set; }
        public int WIS_item { get; set; }
        public int WIS_aa { get; set; }
        public int WIS_cap { get; set; }
        public int MR_total { get; set; }
        public int MR_item { get; set; }
        public int MR_aa { get; set; }
        public int MR_cap { get; set; }
        public int FR_total { get; set; }
        public int FR_item { get; set; }
        public int FR_aa { get; set; }
        public int FR_cap { get; set; }
        public int CR_total { get; set; }
        public int CR_item { get; set; }
        public int CR_aa { get; set; }
        public int CR_cap { get; set; }
        public int DR_total { get; set; }
        public int DR_item { get; set; }
        public int DR_aa { get; set; }
        public int DR_cap { get; set; }
        public int PR_total { get; set; }
        public int PR_item { get; set; }
        public int PR_aa { get; set; }
        public int PR_cap { get; set; }
        public int damage_shield_item { get; set; }
        public int haste_item { get; set; }

        //Begin custom metrics
        public int avg_resists { get; set; }
        public Character(string charData, char delimiter = '\t')
        {
            string[] split = charData.Split(delimiter);

            name = split[0];
            last_name = split[1];
            guild_name = split[2];
            level = int.Parse(split[3]);
            race = split[4];
            @class = split[5];
            deity = split[6];
            gender = split[7];
            id = int.Parse(split[8]);
            weight = int.Parse(split[9]);
            aa_points_unspent = int.Parse(split[10]);
            aa_points_spent = int.Parse(split[11]);
            hp_regen_standing_base = int.Parse(split[12]);
            hp_regen_sitting_base = int.Parse(split[13]);
            hp_regen_resting_base = int.Parse(split[14]);
            hp_regen_standing_total = int.Parse(split[15]);
            hp_regen_sitting_total = int.Parse(split[16]);
            hp_regen_resting_total = int.Parse(split[17]);
            hp_regen_item = int.Parse(split[18]);
            hp_regen_item_cap = int.Parse(split[19]);
            hp_regen_aa = int.Parse(split[20]);
            mana_regen_standing_base = int.Parse(split[21]);
            mana_regen_sitting_base = int.Parse(split[22]);
            mana_regen_standing_total = int.Parse(split[23]);
            mana_regen_sitting_total = int.Parse(split[24]);
            mana_regen_item = int.Parse(split[25]);
            mana_regen_item_cap = int.Parse(split[26]);
            mana_regen_aa = int.Parse(split[27]);
            hp_max_total = int.Parse(split[28]);
            hp_max_item = int.Parse(split[29]);
            mana_max_total = int.Parse(split[30]);
            mana_max_item = int.Parse(split[31]);
            end_max_total = int.Parse(split[32]);
            ac_total = int.Parse(split[33]);
            ac_item = int.Parse(split[34]);
            ac_shield = int.Parse(split[35]);
            ac_avoidance = int.Parse(split[36]);
            ac_mitigation = int.Parse(split[37]);
            atk_total = int.Parse(split[38]);
            atk_item = int.Parse(split[39]);
            atk_item_cap = int.Parse(split[40]);
            atk_offense = int.Parse(split[41]);
            atk_tohit = int.Parse(split[42]);
            STR_total = int.Parse(split[43]);
            STR_base = int.Parse(split[44]);
            STR_item = int.Parse(split[45]);
            STR_aa = int.Parse(split[46]);
            STR_cap = int.Parse(split[47]);
            STA_total = int.Parse(split[48]);
            STA_base = int.Parse(split[49]);
            STA_item = int.Parse(split[50]);
            STA_aa = int.Parse(split[51]);
            STA_cap = int.Parse(split[52]);
            AGI_total = int.Parse(split[53]);
            AGI_base = int.Parse(split[54]);
            AGI_item = int.Parse(split[55]);
            AGI_aa = int.Parse(split[56]);
            AGI_cap = int.Parse(split[57]);
            DEX_total = int.Parse(split[58]);
            DEX_base = int.Parse(split[59]);
            DEX_item = int.Parse(split[60]);
            DEX_aa = int.Parse(split[61]);
            DEX_cap = int.Parse(split[62]);
            CHA_total = int.Parse(split[63]);
            CHA_base = int.Parse(split[64]);
            CHA_item = int.Parse(split[65]);
            CHA_aa = int.Parse(split[66]);
            CHA_cap = int.Parse(split[67]);
            INT_total = int.Parse(split[68]);
            INT_base = int.Parse(split[69]);
            INT_item = int.Parse(split[70]);
            INT_aa = int.Parse(split[71]);
            INT_cap = int.Parse(split[72]);
            WIS_total = int.Parse(split[73]);
            WIS_base = int.Parse(split[74]);
            WIS_item = int.Parse(split[75]);
            WIS_aa = int.Parse(split[76]);
            WIS_cap = int.Parse(split[77]);
            MR_total = int.Parse(split[78]);
            MR_item = int.Parse(split[79]);
            MR_aa = int.Parse(split[80]);
            MR_cap = int.Parse(split[81]);
            FR_total = int.Parse(split[82]);
            FR_item = int.Parse(split[83]);
            FR_aa = int.Parse(split[84]);
            FR_cap = int.Parse(split[85]);
            CR_total = int.Parse(split[86]);
            CR_item = int.Parse(split[87]);
            CR_aa = int.Parse(split[88]);
            CR_cap = int.Parse(split[89]);
            DR_total = int.Parse(split[90]);
            DR_item = int.Parse(split[91]);
            DR_aa = int.Parse(split[92]);
            DR_cap = int.Parse(split[93]);
            PR_total = int.Parse(split[94]);
            PR_item = int.Parse(split[95]);
            PR_aa = int.Parse(split[96]);
            PR_cap = int.Parse(split[97]);
            damage_shield_item = int.Parse(split[98]);
            haste_item = int.Parse(split[99]);

            // begin custom metrics
            avg_resists = (PR_total + MR_total + DR_total + FR_total + CR_total) / 5;
        }
    }
}
