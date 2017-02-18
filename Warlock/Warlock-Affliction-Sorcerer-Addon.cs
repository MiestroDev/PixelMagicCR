// winifix@gmail.com
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertPropertyToExpressionBody

using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using PixelMagic.Helpers;

namespace PixelMagic.Rotation
{
    public class WarlockAffliction : CombatRoutine
    {  
        public override string Name
        {
            get { return "Affliction Warlock"; }
        }

        public override string Class
        {
            get { return "Warlock"; }
        }

        public override void Initialize()
        {
            Log.Write("Welcome to Affliction Warlock", Color.Purple);
            Log.Write("IN ORDER FOR THIS ROTATION TO WORK YOU WILL NEED TO DOWNLOAD AND INSTALL THE ADDON.", Color.Red);
            Log.Write("Poke me on DISCORD for the addon", Color.Blue);
        }       

        public override void Stop()
        {
        }				
		
        public override void Pulse() // Updated for Legion (tested and working for single target)
        {
            AddonCreationPulse();
            PlayerStats();
            AoEStuff();
            if (WoW.IsInCombat && !WoW.PlayerHasBuff("Mount"))
            {
                SelectRotation();
            }

            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.PlayerHasBuff("Mount"))
                {

                    if (WoW.CurrentSoulShards >= 1
                        && WoW.WasLastCasted("Unstable Affliction")
                        && WoW.CanCast("Reap Souls")
                        && WoW.IsSpellInRange("Agony")
                        && !WoW.PlayerIsCasting
                        && !WoW.PlayerHasBuff("Deadwind Harvester")
                        && WoW.PlayerHasBuff("Tormented Souls"))
                    {
                        WoW.CastSpell("Reap Souls");
                        Thread.Sleep(200);
                        return;
                    }

                    if ((!WoW.TargetHasDebuff("Agony") || WoW.TargetDebuffTimeRemaining("Agony") <= 5.4)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Agony") <= 1.5)
                        && WoW.CanCast("Agony")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Agony");
                        return;
                    }

                    if ((WoW.CurrentSoulShards >= 3 || WoW.WasLastCasted("Unstable Affliction")
                        && !WoW.IsMoving
                        && WoW.CanCast("Unstable Affliction")
                        && WoW.IsSpellInRange("Agony")))
                    {
                        WoW.CastSpell("Unstable Affliction");
                        Thread.Sleep(200);
                        return;
                    }

                    if (WoW.CanCast("Life Tap") && !WoW.PlayerIsChanneling && CharInfo.T2 == 3 && !WoW.PlayerHasBuff("Empowered Life Tap"))
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }                   

                    if ((!WoW.TargetHasDebuff("Corruption") || WoW.TargetDebuffTimeRemaining("Corruption") <= 4.2)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Corruption") <= 1)
                        && WoW.CanCast("Corruption")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Corruption");
                        return;
                    }

                    if ((!WoW.TargetHasDebuff("Siphon Life") || WoW.TargetDebuffTimeRemaining("Siphon Life") <= 4.2)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Siphon Life") <= 1)
                        && CharInfo.T4 == 1
                        && WoW.CanCast("Siphon Life")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Siphon Life");
                        return;
                    }                  

                    /*if (WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction2")
                        || (WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction3"))
                        || (WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction4"))
                        || (WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction5"))
                        || (WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction3"))
                        || (WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction4"))
                        || (WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction5"))
                        || (WoW.TargetHasDebuff("Unstable Affliction3") && WoW.TargetHasDebuff("Unstable Affliction4"))
                        || (WoW.TargetHasDebuff("Unstable Affliction3") && WoW.TargetHasDebuff("Unstable Affliction5"))
                        || (WoW.TargetHasDebuff("Unstable Affliction4") && WoW.TargetHasDebuff("Unstable Affliction5"))
                        && !WoW.PlayerIsCasting
                        && WoW.CanCast("Reap Souls")
                        && !WoW.PlayerHasBuff("Deadwind Harvester")
                        && WoW.PlayerHasBuff("Tormented Souls"))
                    {
                        WoW.CastSpell("Reap Souls");
                        return;
                    }*/

                    

                    if (WoW.CanCast("Reap Souls") && !WoW.PlayerIsCasting && !WoW.PlayerHasBuff("Deadwind Harvester") && WoW.PlayerHasBuff("Tormented Souls"))
                    {
                        WoW.CastSpell("Reap Souls");
                        return;
                    }

                    if (WoW.CanCast("Felhunter") && CharInfo.T6 == 2 && !WoW.IsSpellOnCooldown("Felhunter") && WoW.IsSpellInRange("Agony") && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting)
                    {
                        WoW.CastSpell("Felhunter");
                        return;
                    }

                    if (WoW.CanCast("Unstable Affliction") && !WoW.IsMoving && CharInfo.T2 == 1 && !WoW.IsMoving && WoW.IsSpellInRange("Unstable Affliction") && !WoW.PlayerIsChanneling && WoW.CurrentSoulShards >= 1
                        && (!WoW.TargetHasDebuff("Unstable Affliction1") || !WoW.TargetHasDebuff("Unstable Affliction2") || !WoW.TargetHasDebuff("Unstable Affliction3") || !WoW.TargetHasDebuff("Unstable Affliction4") || !WoW.TargetHasDebuff("Unstable Affliction5")
                        || (WoW.TargetDebuffTimeRemaining("Unstable Affliction1") <= 1.5) || (WoW.TargetDebuffTimeRemaining("Unstable Affliction2") <= 1.5) || (WoW.TargetDebuffTimeRemaining("Unstable Affliction3") <= 1.5)
                        || (WoW.TargetDebuffTimeRemaining("Unstable Affliction4") <= 1.5) || (WoW.TargetDebuffTimeRemaining("Unstable Affliction5") <= 1.5)))
                    {
                        WoW.CastSpell("Unstable Affliction");
                        Thread.Sleep(200);
                        return;
                    }

                    if (WoW.IsInCombat && WoW.Mana < 70 && WoW.HealthPercent > 70 && WoW.CanCast("Life Tap"))
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Haunt") && CharInfo.T1 == 1 && !WoW.IsSpellOnCooldown("Haunt") && WoW.IsSpellInRange("Agony") && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Haunt");
                        return;
                    }

                    if (WoW.CanCast("Drain Soul") && WoW.IsSpellInRange("Agony") && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Drain Soul");
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.PlayerHasBuff("Mount")) // Do AOE stuff here
                {
                    if (WoW.CanCast("Agony") && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Seed of Corruption") && (!WoW.TargetHasDebuff("Agony") || (WoW.TargetDebuffTimeRemaining("Agony") <= 5.4)))
                    {
                        WoW.CastSpell("Agony");
                        return;
                    }

                    if (WoW.CanCast("Corruption") && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Seed of Corruption") && (!WoW.TargetHasDebuff("Corruption") || (WoW.TargetDebuffTimeRemaining("Corruption") <= 4.2)))
                    {
                        WoW.CastSpell("Corruption");
                        return;
                    }                    

                    if (WoW.CanCast("Seed of Corruption") && WoW.IsSpellInRange("Agony") && !WoW.TargetHasDebuff("Seed of Corruption") && !WoW.IsMoving && WoW.CurrentSoulShards >= 1)
                    {
                        WoW.CastSpell("Seed of Corruption");
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.SingleTargetCleave)
            {
                // Do Single Target Cleave stuff here if applicable else ignore this one
            }            
        }

        #region Talents functions
        public struct char_data
        {
            public int T1;
            public int T2;
            public int T3;
            public int T4;
            public int T5;
            public int T6;
            public int T7;
            public float Mana;
            public string Spec;
            public string Race;

            private char_data(int p1, int p2, int p3, int p4, int p5, int p6, int p7, float mana, string spec, string race)
            {
                T1 = p1;
                T2 = p2;
                T3 = p3;
                T4 = p4;
                T5 = p5;
                T6 = p6;
                T7 = p7;
                Mana = mana;
                Spec = spec;
                Race = race;
            }
        }

        public string[] Race = new string[] { "None", "Human", "Dwarf", "NightElf", "Gnome", "Dreanei", "Pandaren", "Orc", "Undead", "Tauren", "Troll", "BloodElf", "Goblin", "none" };
        public string[] Spec = new string[] { "None", "Blood", "Frost", "Unholy", "Havoc", "Vengeance", "Balance", "Feral", "Guardian", "Restoration", "Beast Mastery", "Marksmanship", "Survival", "Arcane", "Fire", "Frost", "Brewmaster", "Mistweaver", "Windwalker", "Holy", "Protection", "Retribution", "Discipline", "HolyPriest", "Shadow", "Assassination", "Outlaw", "Subtlety", "Elemental", "Enhancement", "RestorationShaman", "Affliction", "Arms", "Fury", "Protection", "none" };
        private int npcCount, players;
        private bool Nameplates = false;
        private Color pixelColor = Color.FromArgb(0);
        private double hastePct;
        private char_data CharInfo = new char_data();
        private bool AddonEmbeded = false;
        private bool RangeLib = false;

        private void PlayerStats()
        {
            // Playerstats start at row 1,  column 21
            // t1 t2 t3
            // t4 t5 t7
            // t7 +-haste hastePCT
            // Spec, Mana, Race
            int postive = 0;
            if ((Convert.ToDouble(pixelColor.R) == 255))
                hastePct = 0f;
            else
                hastePct = (Convert.ToSingle(pixelColor.R) * 100f / 255f);
            int spec, race;
            pixelColor = WoW.GetBlockColor(1, 21);
            CharInfo.T1 = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.R) * 100 / 255));
            CharInfo.T2 = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.G) * 100 / 255));
            CharInfo.T3 = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.B) * 100 / 255));
            pixelColor = WoW.GetBlockColor(2, 21);
            CharInfo.T4 = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.R) * 100 / 255));
            CharInfo.T5 = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.G) * 100 / 255));
            CharInfo.T6 = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.B) * 100 / 255));
            pixelColor = WoW.GetBlockColor(3, 21);
            CharInfo.T7 = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.R) * 100 / 255));
            spec = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.G) * 100 / 255));
            race = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.B) * 100 / 255));
            pixelColor = WoW.GetBlockColor(4, 21);
            CharInfo.Mana = (Convert.ToSingle(pixelColor.B) * 100 / 255);
            postive = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.G) / 255));
            if ((Convert.ToDouble(pixelColor.B) == 255))
                hastePct = 0f;
            else
                if (postive == 1)
                    hastePct = (Convert.ToSingle(pixelColor.R) * 100f / 255f) * (-1);
                else
                    hastePct = (Convert.ToSingle(pixelColor.G) * 100f / 255f);
            if (race > 13)
                race = 0;
            if (spec > 34)
                spec = 0;
            //Log.Write ("Char Race :" + race + " Spec : " + spec);
            CharInfo.Race = Race[race];
            CharInfo.Spec = Spec[spec];
            Log.Write(" T1 " + CharInfo.T1 + " T2 " + CharInfo.T2 + " T3 " + CharInfo.T3 + " T4 " + CharInfo.T4 + " T5 " + CharInfo.T5 + " T6 " + CharInfo.T6 + " T7 " + CharInfo.T7);
            //Log.Write("Char Haste " + hastePct + " Mana :" + CharInfo.Mana + " Race : " +CharInfo.Race + " Spec : "  +CharInfo.Spec ) ;

        }

        private void AoEStuff()
        {
            Color pixelColor = Color.FromArgb(0);
            pixelColor = WoW.GetBlockColor(11, 20);
            npcCount = Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.G) * 100 / 255));
            if (Convert.ToInt32(Math.Round(Convert.ToSingle(pixelColor.B) / 255)) == 1)
                Nameplates = true;
            else
                Nameplates = false;
        }

        private void SelectRotation()
        {
            if (Nameplates)
            {
                if (npcCount >= 4 && !WoW.TargetIsPlayer)
                    combatRoutine.ChangeType(RotationType.AOE);
              /*  if ((npcCount == 2 || npcCount == 3) && !WoW.TargetIsPlayer)
                    combatRoutine.ChangeType(RotationType.SingleTargetCleave);*/
                if (npcCount <= 1)
                    combatRoutine.ChangeType(RotationType.SingleTarget);
            }
        }

        private bool AddonEdited = false;
        private static string AddonName = ConfigFile.ReadValue("PixelMagic", "AddonName");
        private static string AddonEmbedName = "BossLib.xml";// Initialization variables		
        private static string LuaAddon = "Shaman.lua";
        public static string CustomLua
        {
            get
            {
                var customLua = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + LuaAddon);
                return customLua;
            }
        }
        public void RangeLibCopy()
        {
            try
            {
                string fileName = "text.txt";
                string sourcePath = string.Concat(AppDomain.CurrentDomain.BaseDirectory + "LibSpellRange-1.0\\");
                string sourcePathSub = string.Concat(AppDomain.CurrentDomain.BaseDirectory + "LibSpellRange-1.0\\lib\\LibStub\\");
                string targetPath = string.Concat("" + WoW.AddonPath + "\\" + AddonName + "\\lib\\LibSpellRange-1.0\\");
                string targetPathSub = string.Concat("" + WoW.AddonPath + "\\" + AddonName + "\\lib\\LibSpellRange-1.0\\lib\\LibStub\\");
                string destFile = "text.txt";

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                    Log.Write("Base target:" + targetPath);
                }
                if (!Directory.Exists(targetPathSub))
                {
                    Log.Write("Sub target:" + targetPathSub);
                    Directory.CreateDirectory(targetPathSub);
                }
                if (!Directory.Exists(sourcePath))
                    Log.Write("Dirctory doesn't exist:" + sourcePath);
                if (!Directory.Exists(sourcePathSub))
                    Log.Write("Dirctory doesn't exist:" + sourcePathSub);
                if (Directory.Exists(sourcePath))
                {
                    string[] files = Directory.GetFiles(sourcePath);
                    foreach (string s in files)
                    {
                        Log.Write("Generating file" + s);
                        fileName = Path.GetFileName(s);
                        destFile = Path.Combine(targetPath, fileName);
                        File.Copy(s, destFile, true);
                    }
                }
                if (Directory.Exists(sourcePathSub))
                {
                    string[] files = Directory.GetFiles(sourcePathSub);

                    foreach (string s in files)
                    {
                        Log.Write("Generating Sub file" + s);
                        fileName = Path.GetFileName(s);
                        destFile = Path.Combine(targetPathSub, fileName);
                        File.Copy(s, destFile, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "PixelMagic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Thread.Sleep(2000);
            RangeLib = true;
        }

        private void AddonEmbedEdit()
        {
            try
            {

                Log.Write("Addon emedding Editing in progress");
                if (!File.Exists(Path.Combine("" + WoW.AddonPath + "\\" + AddonName + "\\", AddonEmbedName)))
                {
                    string addonlua = " < Ui xmlns = \"http://www.blizzard.com/wow/ui/\" xmlns: xsi = \"http://www.w3.org/2001/XMLSchema-instance \" xsi: schemaLocation = \"http://www.blizzard.com/wow/ui/ ..\\FrameXML\\UI.xsd\" >" + Environment.NewLine
                + "< Script file = \"lib\\LibSpellRange-1.0\\LibSpellRange-1.0.lua\" />" + Environment.NewLine
                + "</ Ui >" + Environment.NewLine;
                    File.WriteAllText("" + WoW.AddonPath + "\\" + AddonName + "\\" + AddonEmbedName, addonlua);
                    Log.Write("Addon Embedding complete");

                }
                AddonEmbeded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "PixelMagic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void AddonCreationPulse()
        {

            // Editing the addon
            if (AddonEdited == false)
            {
                Log.Write("Editing Addon");
                AddonEdit();
                Log.Write("Editing Addon Complete");
                Thread.Sleep(2000);

            }
            if (AddonEmbeded == false)
            {
                Log.Write("embedingin rangecheck");
                AddonEmbedEdit();
                Log.Write("embedingin RangeCheck Complete");
                Thread.Sleep(2000);
            }
            if (RangeLib == false)
            {
                RangeLibCopy();
                WoW.Reload();
            }
            Thread.Sleep(350);
        }
        private void AddonEdit()
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + LuaAddon))
                {
                    string addonlua = File.ReadAllText("" + WoW.AddonPath + "\\" + AddonName + "\\" + AddonName + ".lua");
                    Log.Write("Addon Editing in progress");
                    addonlua = addonlua.Replace("if name == \"Wild Imps\"", "if (name == \"Wild Imps\" or name == \"Spirit Wolf\" or name == \"Totem Mastery\")");
                    addonlua = addonlua.Replace("and(startTime + duration - GetTime() > 1.6) ", "and(startTime + duration - GetTime() > (1.5 / (1 + (GetHaste() / 100) ))) ");
                    addonlua = addonlua.Replace("end" + Environment.NewLine + Environment.NewLine + "local function InitializeTwo()", Environment.NewLine);
                    addonlua = addonlua.Replace("	--print (\"Initialising Spell Charges Frames\")", "end" + Environment.NewLine + "local function InitializeTwo()" + Environment.NewLine + "	--print (\"Initialising Spell Charges Frames\")" + Environment.NewLine);
                    addonlua = addonlua.Replace("IsSpellInRange(name, \"target\")", "LibStub(\"SpellRange-1.0\").IsSpellInRange(name, \"target\")");
                    addonlua = addonlua.Replace("local function InitializeOne()", Environment.NewLine + CustomLua + Environment.NewLine + "local function InitializeOne()");
                    addonlua = addonlua.Replace("InitializeOne()" + Environment.NewLine + "            InitializeTwo()", "InitializeOne()" + Environment.NewLine + "            InitializeTwo()" + Environment.NewLine + "            InitializeThree()");
                    addonlua = addonlua.Replace("healthFrame:SetScript(\"OnUpdate\", updateHealth)", "");
                    addonlua = addonlua.Replace("powerFrame: SetScript(\"OnUpdate\", updatePower)", "");
                    addonlua = addonlua.Replace("	targetHealthFrame: SetScript(\"OnUpdate\", updateTargetHealth) ", "");
                    addonlua = addonlua.Replace("unitCombatFrame: SetScript(\"OnUpdate\", updateCombat)", "");
                    addonlua = addonlua.Replace("unitPowerFrame:SetScript(\"OnUpdate\", updateUnitPower)", "");
                    addonlua = addonlua.Replace("isTargetFriendlyFrame: SetScript(\"OnUpdate\", updateIsFriendly)", "");
                    addonlua = addonlua.Replace("playerIsCastingFrame: SetScript(\"OnUpdate\", updatePlayerIsCasting)", "");
                    addonlua = addonlua.Replace("hasTargetFrame: SetScript(\"OnUpdate\", hasTarget)", "");
                    addonlua = addonlua.Replace("targetIsCastingFrame: SetScript(\"OnUpdate\", updateTargetIsCasting)", "");
                    addonlua = addonlua.Replace("hasteFrame: SetScript(\"OnUpdate\", updateHaste)", "");
                    addonlua = addonlua.Replace("unitIsVisibleFrame: SetScript(\"OnUpdate\", updateUnitIsVisible)", "");
                    addonlua = addonlua.Replace("petHealthFrame: SetScript(\"OnUpdate\", updatePetHealth)", "");
                    addonlua = addonlua.Replace("wildPetsFrame: SetScript(\"OnUpdate\", updateWildPetsFrame)", "");
                    addonlua = addonlua.Replace("cooldownframes[spellId]:SetScript(\"OnUpdate\", updateSpellCooldowns)", "");
                    addonlua = addonlua.Replace("spellInRangeFrames[spellId]:SetScript(\"OnUpdate\", updateSpellInRangeFrames)", "");
                    addonlua = addonlua.Replace("targetDebuffFrames[debuffId]:SetScript(\"OnUpdate\", updateTargetDebuffs)", "");
                    addonlua = addonlua.Replace("updateSpellChargesFrame[spellId]:SetScript(\"OnUpdate\", updateSpellCharges)", "");
                    addonlua = addonlua.Replace("TargetBuffs[buffId]:SetScript(\"OnUpdate\", updateTargetBuffs)", "");
                    addonlua = addonlua.Replace("PlayerMovingFrame: SetScript(\"OnUpdate\", PlayerNotMove)", "");
                    addonlua = addonlua.Replace("AutoAtackingFrame: SetScript(\"OnUpdate\", AutoAtacking)", "");
                    addonlua = addonlua.Replace("targetIsPlayerFrame: SetScript(\"OnUpdate\", updateIsPlayer)", "");
                    addonlua = addonlua.Replace("flagFrame: SetScript(\"OnUpdate\", updateFlag)", "");
                    addonlua = addonlua.Replace("targetLastSpellFrame[i]:SetScript(\"OnUpdate\", updateTargetCurrentSpell)", "");
                    addonlua = addonlua.Replace("targetArena1Frame[i]:SetScript(\"OnUpdate\", updateArena1Spell)", "");
                    addonlua = addonlua.Replace("targetArena2Frame[i]:SetScript(\"OnUpdate\", updateArena2Spell)", "");
                    addonlua = addonlua.Replace("targetArena3Frame[i]:SetScript(\"OnUpdate\", updateArena3Spell)", "");
                    addonlua = addonlua.Replace("buffFrames[buffId]:SetScript(\"OnUpdate\", updateMyBuffs)", "");
                    addonlua = addonlua.Replace("itemframes[itemId]:SetScript(\"OnUpdate\", updateItemCooldowns)", "");
                    addonlua = addonlua.Replace("spellOverlayedFrames[spellId]:SetScript(\"OnUpdate\", updateIsSpellOverlayedFrames)", "");
                    addonlua = addonlua.Replace("playerDebuffFrames[debuffId]:SetScript(\"OnUpdate\", updatePlayerDebuffs)", "");
                    File.WriteAllText("" + WoW.AddonPath + "\\" + AddonName + "\\" + AddonName + ".lua", addonlua);
                    AddonEdited = true;
                }
                else
                    Log.Write("Adddon failed to load, Pleaes check " + LuaAddon + " Is in PM.EXE directory,Rotation will not function with out it");
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "PixelMagic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Log.Write("Editing Addon Complete");
        }

        public override Form SettingsForm { get; set; }
        public class DetectKeyPress
        {
            public static int Num1 = 0x31;
            public static int Num2 = 0x32;
            public static int Num3 = 0x33;
            public static int Num4 = 0x34;
            public static int Num5 = 0x35;
            public static int Num6 = 0x36;
            public static int Numpad0 = 0x60;
            public static int Numpad1 = 0x61;
            public static int Numpad2 = 0x62;
            public static int Numpad3 = 0x63;
            public static int Numpad4 = 0x64;
            public static int Numpad5 = 0x65;
            public static int Numpad6 = 0x66;
            public static int Numpad7 = 0x67;
            public static int Numpad8 = 0x68;
            public static int Numpad9 = 0x69;
            public static int NumpadDot = 0x6E;
            public static int NumpadADD = 0x6B;

            public static int Shift = 0x10;
            public static int Ctrl = 0x11;
            public static int Alt = 0x12;

            public static int Z = 0x5A;
            public static int X = 0x58;
            public static int C = 0x43;
            public static int V = 0x56;
            public static int Slash = 0xDC;

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            internal static extern short GetKeyState(int virtualKeyCode);
        }
    }
}
        #endregion

/*
[AddonDetails.db]
AddonAuthor=Sorcerer
AddonName=PixelMagic
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,980,Agony,NumPad1
Spell,63106,Siphon Life,NumPad2
Spell,172,Corruption,NumPad3
Spell,30108,Unstable Affliction,NumPad4
Spell,216698,Reap Souls,NumPad5
Spell,1454,Life Tap,NumPad7
Spell,48181,Haunt,NumPad8
Spell,198590,Drain Soul,Add
Spell,27243,Seed of Corruption,NumPad0
Spell,111897,Felhunter,NumPad9
Aura,980,Agony
Aura,27243,Seed of Corruption
Aura,146739,Corruption
Aura,63106,Siphon Life
Aura,233490,Unstable Affliction1
Aura,233496,Unstable Affliction2
Aura,233497,Unstable Affliction3
Aura,233498,Unstable Affliction4
Aura,233499,Unstable Affliction5
Aura,216708,Deadwind Harvester
Aura,216695,Tormented Souls
Aura,235156,Empowered Life Tap
Aura,127271,Mount
*/
