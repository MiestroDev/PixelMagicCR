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
    public class HunterBeastmastery : CombatRoutine
    {
        private readonly Stopwatch BeastCleave = new Stopwatch();

        public override string Name
        {
            get
            {
                return "Hunter BM";
            }
        }

        public override string Class
        {
            get
            {
                return "Hunter";
            }
        }

        public override void Initialize()
        {
            Log.Write("Welcome to Beastmastery Hunter", Color.Green);
            Log.Write("IN ORDER FOR THIS ROTATION TO WORK YOU WILL NEED TO DOWNLOAD AND INSTALL THE ADDON.", Color.Red);
            Log.Write("Poke me on DISCORD for the addon", Color.Blue);
            Log.Write("Use Scroll Lock key to toggle ST/AOE/CLEAVE auto detection", Color.Blue);
            Log.Write("If Scroll Lock LED is ON ST/AOE/CLEAVE auto detection is ENABLED", Color.Blue);
            Log.Write("If Scroll Lock LED is OFF ST/AOE/CLEAVE auto detection is DISABLED use the manual mode to select ST/AOE/CLEAVE (Default: ALT+S, ALT+A)", Color.Blue);
        }

        public override void Stop()
        {
        }

        public override void Pulse()        // Updated for Legion (tested and working for single target)
        {
            AddonCreationPulse();
            PlayerStats();
            AoEStuff();
            if (WoW.IsInCombat && !WoW.PlayerHasBuff("Mount"))
            {
                SelectRotation();
            }

            //Healthstone - Potion
            if ((WoW.CanCast("Healthstone") || WoW.CanCast("Potion"))
                && (WoW.ItemCount("Healthstone") >= 1 || WoW.ItemCount("Potion") >= 1)
                && (!WoW.ItemOnCooldown("Healthstone") || !WoW.ItemOnCooldown("Potion"))
                && !WoW.PlayerHasBuff("Feign Death")
                && WoW.HealthPercent <= 30
                && !WoW.PlayerHasBuff("Mount")
                && WoW.HealthPercent != 0)
            {
                WoW.CastSpell("Healthstone");
                WoW.CastSpell("Potion");
                return;
            }

            //Exhilaration
            if (WoW.CanCast("Exhilaration")
                && WoW.HealthPercent <= 20
                && !WoW.PlayerHasBuff("Mount")
                && !WoW.PlayerHasBuff("Feign Death")
                && WoW.HealthPercent != 0)
            {
                WoW.CastSpell("Exhilaration");
                return;
            }

            //Call pet
            if (!WoW.HasPet
                && !WoW.PlayerHasBuff("Mount")
                && !WoW.PlayerHasBuff("Feign Death")
                && WoW.HealthPercent != 0)
            {                
                WoW.CastSpell("Call Pet");
                return;
            }

            //Revive Pet
            if ((!WoW.HasPet || WoW.PetHealthPercent < 1)
                && !WoW.PlayerHasBuff("Mount")
                && !WoW.PlayerHasBuff("Feign Death")
                && WoW.HealthPercent != 0)
            {
                WoW.CastSpell("Heart of the Phoenix");
                WoW.CastSpell("Revive Pet");
                return;
            }            

            //Voley
            if (WoW.CanCast("Voley")
                && !WoW.PlayerHasBuff("Feign Death")
                && !WoW.PlayerHasBuff("Voley")
                && WoW.HealthPercent != 0
                && CharInfo.T6 == 3)
            {
                WoW.CastSpell("Voley");
                return;
            }

            if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerHasBuff("Mount") && !WoW.PlayerIsChanneling && !WoW.PlayerHasBuff("Feign Death") && WoW.HealthPercent != 0)
            {
                //Stampede
                if (DetectKeyPress.GetKeyState(DetectKeyPress.Shift) < 0
                    && WoW.CanCast("Stampede")
                    && CharInfo.T7 == 1)
                {
                    WoW.CastSpell("Stampede");
                    return;
                }

                //Intimidation //Binding Shot
                if (DetectKeyPress.GetKeyState(DetectKeyPress.Ctrl) < 0                    
                    && ((WoW.CanCast("Intimidation") && CharInfo.T5 == 3) || (WoW.CanCast("Binding Shot") && CharInfo.T5 == 1)))
                {
                    WoW.CastSpell("Binding Shot");
                    WoW.CastSpell("Intimidation");
                    return;
                }                

                //Cooldowns
                if (UseCooldowns)
                { 
                    //Aspect of the Wild
                    if (WoW.CanCast("Aspect of the Wild")
                        && !WoW.PlayerHasBuff("Aspect of the Turtle")
                        && WoW.PlayerHasBuff("Bestial Wrath")
                        && WoW.PlayerBuffTimeRemaining("Bestial Wrath") >= 12)
                    {
                        WoW.CastSpell("Aspect of the Wild");
                        return;
                    }                    
                }

                //Legendary Trinket
                if (WoW.CanCast("Kil'jaeden's Burning Wish")
                    && (((WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 15 && WoW.SpellCooldownTimeRemaining("Dire Beast") > 5)) || WoW.PlayerHasBuff("Bestial Wrath"))
                    && !WoW.ItemOnCooldown("Kil'jaeden's Burning Wish")
                    && WoW.IsSpellInRange("Cobra Shot"))
                {
                    WoW.CastSpell("Kil'jaeden's Burning Wish");
                }

                //SINGLE TARGET

                        //Bestial Wrath
                        if ((combatRoutine.Type == RotationType.SingleTarget || combatRoutine.Type == RotationType.SingleTargetCleave)
                            && WoW.CanCast("Bestial Wrath")
                            && !WoW.PlayerHasBuff("Aspect of the Turtle")
                            && WoW.IsSpellInRange("Cobra Shot")
                            && (WoW.Focus >= 110))
                        {
                            WoW.CastSpell("Bestial Wrath");
                            WoW.CastSpell("Kill Command");
                            if (WoW.CanCast("A Murder of Crows")
                                && WoW.IsSpellInRange("Cobra Shot")
                                && CharInfo.T6 == 1
                                && WoW.Focus >= 30)
                            {
                                WoW.CastSpell("A Murder of Crows");                                
                            } 
                            
                            return;
                        }

                        //A Murder of Crows
                        if (combatRoutine.Type == RotationType.SingleTarget
                            && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 23
                            && WoW.CanCast("A Murder of Crows")
                            && WoW.IsSpellInRange("Cobra Shot")
                            && CharInfo.T6 == 1
                            && WoW.Focus >= 25)                            
                        {
                            WoW.CastSpell("A Murder of Crows");
                            return;
                        }

                        //Kill Command
                        if (combatRoutine.Type == RotationType.SingleTarget
                            && WoW.CanCast("Kill Command")
                            && WoW.Focus >= 100)
                        {
                            WoW.CastSpell("Kill Command");
                            return;
                        }

                        // Dire beast
                        if (combatRoutine.Type == RotationType.SingleTarget
                            && WoW.CanCast("Dire Beast")
                            && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 3
                            && WoW.IsSpellInRange("Cobra Shot") 
                            && CharInfo.T2 != 2)
                        {
                            WoW.CastSpell("Dire Beast");
                            WoW.CastSpell("Titan's Thunder");
                            return;
                        }

                        //Dire Frenzy
                        if (combatRoutine.Type == RotationType.SingleTarget
                            && WoW.CanCast("Dire Frenzy")
                            && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 6
                            && WoW.IsSpellInRange("Cobra Shot")
                            && CharInfo.T2 == 2)
                        {
                            WoW.CastSpell("Titan's Thunder");
                            WoW.CastSpell("Dire Frenzy");                            
                            return;
                        }

                        //Kill Command
                        if (combatRoutine.Type == RotationType.SingleTarget
                            && WoW.CanCast("Kill Command")
                            && (WoW.SpellCooldownTimeRemaining("Bestial Wrath") - WoW.SpellCooldownTimeRemaining("Kill Command") > 2 || (WoW.SpellCooldownTimeRemaining("Bestial Wrath") >= 23 && WoW.SpellCooldownTimeRemaining("Dire Beast") - WoW.SpellCooldownTimeRemaining("Kill Command") > 2))
                            && WoW.Focus >= 25)
                        {
                            WoW.CastSpell("Kill Command");
                            return;
                        }

                        //Chimaera Shot
                        if (combatRoutine.Type == RotationType.SingleTarget
                            && WoW.CanCast("Chimaera Shot")
                            && WoW.IsSpellInRange("Cobra Shot")
                            && CharInfo.T2 == 3
                            && WoW.Focus < 90)
                        {
                            WoW.CastSpell("Chimaera Shot");
                            return;
                        }

                        //Cobra Shot
                        if (combatRoutine.Type == RotationType.SingleTarget
                            && ((WoW.Focus >= 110) || (WoW.PlayerHasBuff("Bestial Wrath") && (WoW.Focus >= 40)))
                            && WoW.IsSpellInRange("Cobra Shot")
                            && WoW.CanCast("Cobra Shot")
                            && !WoW.CanCast("Bestial Wrath"))
                        {
                            WoW.CastSpell("Cobra Shot");
                            return;
                        }

                //AOE

                    //Bestial Wrath
                    if (combatRoutine.Type == RotationType.AOE
                        && WoW.CanCast("Bestial Wrath")
                        && WoW.IsSpellInRange("Cobra Shot")
                        && !WoW.PlayerHasBuff("Aspect of the Turtle"))
                    {
                        WoW.CastSpell("Bestial Wrath");
                        return;
                    }

                    // Dire beast
                    if (combatRoutine.Type == RotationType.AOE
                        && WoW.CanCast("Dire Beast")
                        && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 3
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T2 != 2)
                    {
                        WoW.CastSpell("Dire Beast");
                        WoW.CastSpell("Titan's Thunder");
                        return;
                    }

                    //Dire Frenzy
                    if (combatRoutine.Type == RotationType.AOE
                        && WoW.CanCast("Dire Frenzy")
                        && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 6
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T2 == 2)
                    {
                        WoW.CastSpell("Dire Frenzy");
                        WoW.CastSpell("Titan's Thunder");
                        return;
                    }

                    //Barrage
                    if (combatRoutine.Type == RotationType.AOE
                        && WoW.CanCast("Barrage")
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T6 == 2
                        && (WoW.Focus >= 60))
                    {
                        WoW.CastSpell("Barrage");
                        return;
                    }                    

                    //Multishot
                    if (combatRoutine.Type == RotationType.AOE
                        && WoW.CanCast("Multi-Shot")
                        && WoW.IsSpellInRange("Multi-Shot")
                        && WoW.Focus >= 40)
                    {
                        WoW.CastSpell("Multi-Shot");
                        return;
                    }

                    //Chimaera Shot
                    if (combatRoutine.Type == RotationType.AOE
                        && WoW.CanCast("Chimaera Shot")
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T2 == 3
                        && WoW.Focus < 80)
                    {
                        WoW.CastSpell("Chimaera Shot");
                        return;
                    }

                //CLEAVE

                    //A Murder of Crows
                    if (combatRoutine.Type == RotationType.SingleTarget
                        && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 23
                        && WoW.CanCast("A Murder of Crows")
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T6 == 1
                        && WoW.Focus >= 25)
                    {
                        WoW.CastSpell("A Murder of Crows");
                        return;
                    }

                    //Multishot - Beast Cleave uptime
                    if (combatRoutine.Type == RotationType.SingleTargetCleave
                        && WoW.CanCast("Multi-Shot")
                        && (!WoW.PetHasBuff("BeastCleave") || WoW.PetBuffTimeRemaining("BeastCleave") <= 1)
                        && WoW.IsSpellInRange("Multi-Shot")
                        && !WoW.CanCast("Bestial Wrath")                       
                        && WoW.Focus >= 40)  
                    {
                        WoW.CastSpell("Multi-Shot");                        
                        return;
                    }

                    // Dire beast
                    if (combatRoutine.Type == RotationType.SingleTargetCleave
                        && WoW.CanCast("Dire Beast")
                        && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 3
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T2 != 2)
                    {
                        WoW.CastSpell("Dire Beast");
                        WoW.CastSpell("Titan's Thunder");
                        return;
                    }

                    //Dire Frenzy
                    if (combatRoutine.Type == RotationType.SingleTargetCleave
                        && WoW.CanCast("Dire Frenzy")
                        && WoW.SpellCooldownTimeRemaining("Bestial Wrath") > 6
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T2 == 2)
                    {
                        WoW.CastSpell("Dire Frenzy");
                        WoW.CastSpell("Titan's Thunder");
                        return;
                    }

                    //Barrage
                    if (combatRoutine.Type == RotationType.SingleTargetCleave
                        && WoW.CanCast("Barrage")
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T6 == 2
                        && (WoW.Focus >= 60))
                    {
                        WoW.CastSpell("Barrage");
                        return;
                    }

                    //Kill Command
                    if (combatRoutine.Type == RotationType.SingleTarget
                        && WoW.CanCast("Kill Command")
                        && (WoW.SpellCooldownTimeRemaining("Bestial Wrath") - WoW.SpellCooldownTimeRemaining("Kill Command") > 2 || (WoW.SpellCooldownTimeRemaining("Bestial Wrath") >= 23 && WoW.SpellCooldownTimeRemaining("Dire Beast") - WoW.SpellCooldownTimeRemaining("Kill Command") > 2))
                        && WoW.Focus >= 25)
                    {
                        WoW.CastSpell("Kill Command");
                        return;
                    }            

                    //Chimaera Shot
                    if (combatRoutine.Type == RotationType.SingleTargetCleave
                        && WoW.CanCast("Chimaera Shot")
                        && WoW.IsSpellInRange("Cobra Shot")
                        && CharInfo.T2 == 3
                        && WoW.Focus < 80)
                    {
                        WoW.CastSpell("Chimaera Shot");
                        return;
                    }

                    //Cobra Shot
                    if (combatRoutine.Type == RotationType.SingleTargetCleave
                        && ((WoW.Focus >= 110) || (WoW.PlayerHasBuff("Bestial Wrath") && (WoW.Focus >= 40)))
                        && WoW.IsSpellInRange("Cobra Shot")
                        && WoW.CanCast("Cobra Shot")
                        && !WoW.CanCast("Bestial Wrath"))
                    {
                        WoW.CastSpell("Cobra Shot");
                        return;
                    }

                    //Mend Pet
                    if (WoW.HasPet
                        && WoW.CanCast("Mend Pet")
                        && WoW.PetHealthPercent <= 70
                        && !WoW.PlayerHasBuff("Feign Death"))
                    {
                        WoW.CastSpell("Mend Pet");
                        return;
                    }
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
                if(Control.IsKeyLocked(Keys.Scroll))
                {
                    if (npcCount >= 4 && !WoW.TargetIsPlayer)
                        combatRoutine.ChangeType(RotationType.AOE);
                    if ((npcCount == 2 || npcCount == 3) && !WoW.TargetIsPlayer)
                        combatRoutine.ChangeType(RotationType.SingleTargetCleave);
                    if ((npcCount <= 1))
                        combatRoutine.ChangeType(RotationType.SingleTarget);            
                }
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
Spell,136,Mend Pet,D8
Spell,982,Revive Pet,D8
Spell,883,Call Pet,D7
Spell,5512,Healthstone,D1
Spell,127834,Potion,D1
Spell,19577,Intimidation,D9
Spell,109248,Binding Shot,D9
Spell,193455,Cobra Shot,NumPad2
Spell,109304,Exhilaration,NumPad7
Spell,120679,Dire Beast,NumPad1
Spell,217200,Dire Frenzy,NumPad1
Spell,34026,Kill Command,NumPad3
Spell,131894,A Murder of Crows,NumPad4
Spell,120360,Barrage,NumPad4
Spell,194386,Voley,NumPad4
Spell,2643,Multi-Shot,NumPad5
Spell,207068,Titan's Thunder,Add
Spell,19574,Bestial Wrath,NumPad9
Spell,55709,Heart of the Phoenix,D8
Spell,144259,Kil'jaeden's Burning Wish,D2
Spell,193530,Aspect of the Wild,Divide
Spell,53209,Chimaera Shot,D0
Spell,201430,Stampede,NumPad6
Aura,120694,Dire Beast
Aura,5384,Feign Death
Aura,19574,Bestial Wrath
Aura,127271,Mount
Aura,186265,Aspect of the Turtle
Aura,118455,BeastCleave
Aura,194386,Voley
Item,5512,Healthstone
Item,127834,Potion
Item,144259,Kil'jaeden's Burning Wish
*/
