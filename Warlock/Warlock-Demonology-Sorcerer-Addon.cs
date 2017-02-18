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
    public class WarlockDemonology : CombatRoutine
    {  
        public override string Name
        {
            get { return "Demonology Warlock"; }
        }

        public override string Class
        {
            get { return "Warlock"; }
        }

        public override void Initialize()
        {
            Log.Write("Welcome to the Demonology Warlock rotation", Color.Purple);
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

            //Dark Pact
            if (WoW.CanCast("Dark Pact")
                && CharInfo.T5 == 3
                && WoW.HealthPercent <= 30
                && !WoW.PlayerHasBuff("Mount"))
            {
                WoW.CastSpell("Dark Pact");
                return;
            }           

            //Shadowfury
            if (DetectKeyPress.GetKeyState(DetectKeyPress.Alt) < 0
                && CharInfo.T3 == 3
                && !WoW.IsMoving
                && WoW.CanCast("Shadowfury"))
            {
                WoW.CastSpell("Shadowfury");
                return;
            }

            if (UseCooldowns)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.PlayerHasBuff("Mount"))
                { 
                    //Doomguard
                    if (WoW.CanCast("Doomguard")
                        && (CharInfo.T6 == 0 || CharInfo.T6 == 2 || CharInfo.T6 == 3)
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Doomguard");
                        return;
                    }

                    //Grimoire of Service
                    if (WoW.CanCast("Grimoire: Felguard")
                        && CharInfo.T6 == 2
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Grimoire: Felguard");
                        return;
                    }

                    //Soul Harvest
                    if (WoW.CanCast("Soul Harvest")
                        && CharInfo.T4 == 3
                        && !WoW.IsMoving
                        && WoW.IsSpellInRange("Doom")
                        && (WoW.PlayerHasBuff("Bloodlust") || WoW.PlayerHasBuff("Time Warp") || WoW.PlayerHasBuff("Netherwinds") || WoW.PlayerHasBuff("Drums of War") || WoW.PlayerHasBuff("Heroism")))
                    {
                        WoW.CastSpell("Soul Harvest");
                        return;
                    }
                }
            }

            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.PlayerHasBuff("Mount"))
                {
                    if ((!WoW.TargetHasDebuff("Doom") || WoW.TargetDebuffTimeRemaining("Doom") <= 1.5)                        
                        && WoW.CanCast("Doom")
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Doom");
                        return;
                    }

                    if (WoW.CanCast("Darkglare")
                        && CharInfo.T7 == 1
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Darkglare");
                        return;
                    }

                    if (WoW.CanCast("Call Dreadstalkers")
                        && (WoW.CurrentSoulShards >= 2 || WoW.TargetHasDebuff("Demonic Calling"))
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Call Dreadstalkers");
                        return;
                    }
                   
                    if (WoW.CanCast("Hand of Guldan")
                        && WoW.CurrentSoulShards >= 4
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Hand of Guldan");
                        return;
                    }

                    if (WoW.CanCast("Demonic Empowerment")
                        && !WoW.IsMoving
                        && !WoW.WasLastCasted("Demonic Empowerment")
                        && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 1.5
                        || WoW.WasLastCasted("Call Dreadstalkers") || WoW.WasLastCasted("Grimoire: Felguard") || WoW.WasLastCasted("Doomguard") || WoW.WasLastCasted("Hand of Guldan")))
                    {
                        WoW.CastSpell("Demonic Empowerment");
                        Thread.Sleep(1000);
                        return;
                    }

                    if (WoW.CanCast("Talkiels Consumption")
                        && WoW.PetHasBuff("Demonic Empowerment")
                        && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 2
                        && WoW.WildImpsCount >= 1
                        && WoW.DreadstalkersCount >= 1
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Talkiels Consumption");
                        return;
                    }

                    if (WoW.CanCast("Felstorm")
                        && WoW.PetHasBuff("Demonic Empowerment") 
                        && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Felstorm");
                        return;
                    }

                    if (WoW.CanCast("Shadowflame")
                        && CharInfo.T1 == 2
                        && !WoW.TargetHasDebuff("Shadowflame")
                        && WoW.CanCast("Shadowflame")
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Shadowflame");
                        return;
                    }

                    if (WoW.CanCast("Life Tap")
                        && WoW.Mana < 60
                        && WoW.HealthPercent > 50)
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Demonwrath")
                        && WoW.Mana > 60
                        && WoW.IsMoving)
                    {
                        WoW.CastSpell("Demonwrath");
                        return;
                    }

                    if ((WoW.CanCast("Shadow Bolt") || WoW.CanCast("Demonbolt"))                        
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Shadow Bolt");
                        WoW.CastSpell("Demonbolt");
                        return;
                    }                  
                }
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.PlayerHasBuff("Mount"))
                {
                    if (WoW.CanCast("Hand of Guldan")
                        && WoW.CurrentSoulShards >= 4
                        && WoW.IsSpellInRange("Doom")
                        && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Hand of Guldan");
                        return;
                    }

                    if (WoW.CanCast("Implosion")
                        && CharInfo.T2 == 3
                        && WoW.WildImpsCount >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Implosion");
                        return;
                    }

                    if (WoW.CanCast("Darkglare")
                        && CharInfo.T7 == 1
                        && WoW.CurrentSoulShards >= 1
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Darkglare");
                        return;
                    }

                    if (WoW.CanCast("Demonic Empowerment")
                       && WoW.CanCast("Felstorm")
                       && !WoW.IsMoving
                       && !WoW.WasLastCasted("Demonic Empowerment")
                       && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 6))
                    {
                        WoW.CastSpell("Demonic Empowerment");
                        Thread.Sleep(2000);
                        return;
                    }

                    if (WoW.CanCast("Felstorm")
                        && WoW.PetHasBuff("Demonic Empowerment")
                        && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6
                        && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Felstorm");
                        return;
                    }                    

                    if (WoW.CanCast("Life Tap")
                        && WoW.Mana < 60
                        && WoW.HealthPercent > 50)
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Demonwrath")
                        && WoW.Mana > 60)
                    {
                        WoW.CastSpell("Demonwrath");
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
AddonName=Quartz
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,686,Shadow Bolt,NumPad1
Spell,157695,Demonbolt,NumPad1
Spell,104316,Call Dreadstalkers,NumPad2
Spell,105174,Hand of Guldan,NumPad3
Spell,193396,Demonic Empowerment,NumPad4
Spell,603,Doom,NumPad5
Spell,193440,Demonwrath,NumPad6
Spell,1454,Life Tap,NumPad7
Spell,205180,Darkglare,NumPad8
Spell,111897,Grimoire: Felguard,NumPad9
Spell,211714,Talkiels Consumption,Add
Spell,205181,Shadowflame,NumPad0
Spell,18540,Doomguard,Decimal
Spell,119914,Felstorm,D4
Spell,196098,Soul Harvest,D0
Spell,196277,Implosion,D7
Spell,30283,Shadowfury,D3
Spell,108416,Dark Pact,Multiply
Aura,2825,Bloodlust
Aura,32182,Heroism
Aura,80353,Time Warp
Aura,160452,Netherwinds
Aura,230935,Drums of War
Aura,603,Doom
Aura,193396,Demonic Empowerment
Aura,205146,Demonic Calling
Aura,205181,Shadowflame
Aura,127271,Mount
*/
