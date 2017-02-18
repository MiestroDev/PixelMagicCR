// winifix@gmail.com
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertPropertyToExpressionBody

using System;
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

        public override void Stop()
        {
        }

        public override void Pulse() // Updated for Legion (tested and working for single target)/
        {
            //Dark Pact
            if (WoW.CanCast("Dark Pact") && DarkPact && WoW.HealthPercent <= 30 && !WoW.PlayerHasBuff("Mount"))
            {
                WoW.CastSpell("Dark Pact");
                return;
            }

            //Shadowfury
            if (DetectKeyPress.GetKeyState(DetectKeyPress.Alt) < 0 && Shadowfury && !WoW.IsMoving && WoW.CanCast("Shadowfury"))
            {
                WoW.CastSpell("Shadowfury");
                return;
            }

            if (UseCooldowns)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.PlayerHasBuff("Mount"))
                {
                    //Doomguard
                    if (WoW.CanCast("Doomguard") && !GrimoireofSupremacy && WoW.CurrentSoulShards >= 1 && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Doomguard");
                        return;
                    }

                    //Grimoire of Service
                    if (WoW.CanCast("Grimoire: Felguard") && GrimoireofService && WoW.CurrentSoulShards >= 1 && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Grimoire: Felguard");
                        return;
                    }

                    //Soul Harvest
                    if (WoW.CanCast("Soul Harvest") && SoulHarvest && !WoW.IsMoving && WoW.IsSpellInRange("Doom") &&
                        (WoW.PlayerHasBuff("Bloodlust") || WoW.PlayerHasBuff("Time Warp") || WoW.PlayerHasBuff("Netherwinds") || WoW.PlayerHasBuff("Drums of War") ||
                         WoW.PlayerHasBuff("Heroism")))
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
                    if ((!WoW.TargetHasDebuff("Doom") || WoW.TargetDebuffTimeRemaining("Doom") <= 1.5) && WoW.CanCast("Doom") && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Doom");
                        return;
                    }

                    if (WoW.CanCast("Darkglare") && SummonDarkglare && WoW.CurrentSoulShards >= 1 && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Darkglare");
                        return;
                    }

                    if (WoW.CanCast("Call Dreadstalkers") && (WoW.CurrentSoulShards >= 2 || WoW.TargetHasDebuff("Demonic Calling")) && WoW.IsSpellInRange("Doom") && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Call Dreadstalkers");
                        return;
                    }

                    if (WoW.CanCast("Hand of Guldan") && WoW.CurrentSoulShards >= 4 && WoW.IsSpellInRange("Doom") && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Hand of Guldan");
                        return;
                    }

                    if (WoW.CanCast("Demonic Empowerment") && !WoW.IsMoving && !WoW.WasLastCasted("Demonic Empowerment") &&
                        (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 1.5 || WoW.WasLastCasted("Call Dreadstalkers") ||
                         WoW.WasLastCasted("Grimoire: Felguard") || WoW.WasLastCasted("Doomguard") || WoW.WasLastCasted("Hand of Guldan")))
                    {
                        WoW.CastSpell("Demonic Empowerment");
                        Thread.Sleep(1000);
                        return;
                    }

                    if (WoW.CanCast("Talkiels Consumption") && WoW.PetHasBuff("Demonic Empowerment") && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 2 && WoW.WildImpsCount >= 1 &&
                        WoW.DreadstalkersCount >= 1 && WoW.IsSpellInRange("Doom") && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Talkiels Consumption");
                        return;
                    }

                    if (WoW.CanCast("Felstorm") && WoW.PetHasBuff("Demonic Empowerment") && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6 && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Felstorm");
                        return;
                    }

                    if (WoW.CanCast("Shadowflame") && Shadowflame && !WoW.TargetHasDebuff("Shadowflame") && WoW.CanCast("Shadowflame") && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Shadowflame");
                        return;
                    }

                    if (WoW.CanCast("Life Tap") && WoW.Mana < 60 && WoW.HealthPercent > 50)
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Demonwrath") && WoW.Mana > 60 && WoW.IsMoving)
                    {
                        WoW.CastSpell("Demonwrath");
                        return;
                    }

                    if ((WoW.CanCast("Shadow Bolt") || WoW.CanCast("Demonbolt")) && WoW.IsSpellInRange("Doom") && !WoW.IsMoving)
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
                    if (WoW.CanCast("Hand of Guldan") && WoW.CurrentSoulShards >= 4 && WoW.IsSpellInRange("Doom") && !WoW.IsMoving)
                    {
                        WoW.CastSpell("Hand of Guldan");
                        return;
                    }

                    if (WoW.CanCast("Implosion") && Implosion && WoW.WildImpsCount >= 1 && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Implosion");
                        return;
                    }

                    if (WoW.CanCast("Darkglare") && SummonDarkglare && WoW.CurrentSoulShards >= 1 && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Darkglare");
                        return;
                    }

                    if (WoW.CanCast("Demonic Empowerment") && WoW.CanCast("Felstorm") && !WoW.IsMoving && !WoW.WasLastCasted("Demonic Empowerment") &&
                        (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 6))
                    {
                        WoW.CastSpell("Demonic Empowerment");
                        Thread.Sleep(2000);
                        return;
                    }

                    if (WoW.CanCast("Felstorm") && WoW.PetHasBuff("Demonic Empowerment") && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6 && WoW.IsSpellInRange("Doom"))
                    {
                        WoW.CastSpell("Felstorm");
                        return;
                    }

                    if (WoW.CanCast("Life Tap") && WoW.Mana < 60 && WoW.HealthPercent > 50)
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }

                    if (WoW.CanCast("Demonwrath") && WoW.Mana > 60)
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

        #region Talents

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

        private CheckBox ShadowyInspirationBox;
        private CheckBox ShadowflameBox;
        private CheckBox DemonicCallingBox;
        private CheckBox ImpendingDoomBox;
        private CheckBox ImprovedDreadstalkersBox;
        private CheckBox ImplosionBox;
        private CheckBox DemonicCircleBox;
        private CheckBox MortalCoilBox;
        private CheckBox ShadowfuryBox;
        private CheckBox HandofDoomBox;
        private CheckBox PowerTripBox;
        private CheckBox SoulHarvestBox;
        private CheckBox DemonSkinBox;
        private CheckBox BurningRushBox;
        private CheckBox DarkPactBox;
        private CheckBox GrimoireofSupremacyBox;
        private CheckBox GrimoireofServiceBox;
        private CheckBox GrimoireofSynergyBox;
        private CheckBox SummonDarkglareBox;
        private CheckBox DemonboltBox;
        private CheckBox SoulConduitBox;

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Welcome to the Demonology Warlock rotation", Color.Purple);
            Log.Write("READ ME:", Color.Red);
            Log.Write("Please select your talents from the Rotation settings menu", Color.Red);
            Log.Write("Some talents won't impact your rotation, they are there for future use if necesary", Color.Red);
            Log.Write("The script is not retard proof so please don't select more than 1 talent on each row", Color.Red);
            Log.Write("NOT supported talents: Mortal Coil, Burning Rush", Color.Red);
            Log.Write("Soul Harvest used automatically if you have Bloodlust and Cooldowns activated, Dark Pact activates at 30% or less HP", Color.Red);
            Log.Write("Doomguard and Grimoire: Felguard only cast if you have Cooldowns enabled)", Color.Red);
            Log.Write("To cast Shadowfury you need to use the macro instead of the spell, it will be cast at your mouse when you press the ALT key:", Color.Red);
            Log.Write("#Showtooltip /cast [@cursor] Shadowfury", Color.Red);
            Log.Write("The rotation is based on Icy Veins data", Color.Red);
            WoW.Speak("Welcome to PixelMagic Demonology Warlock");

            SettingsForm = new Form {Text = "Talents", StartPosition = FormStartPosition.CenterScreen, Width = 555, Height = 350, ShowIcon = true};

            var lblShadowyInspirationText = new Label //ShadowyInspiration LABEL
            {Text = "Shadowy Inspiration", Size = new Size(150, 15), Left = 30, Top = 30};
            SettingsForm.Controls.Add(lblShadowyInspirationText); //ShadowyInspiration TEXT

            ShadowyInspirationBox = new CheckBox {Checked = ShadowyInspiration, TabIndex = 2, Size = new Size(15, 15), Left = 15, Top = 30};
            SettingsForm.Controls.Add(ShadowyInspirationBox); //ShadowyInspiration BOX

            var lblShadowflameText = new Label //Shadowflame LABEL
            {Text = "Shadowflame", Size = new Size(150, 15), Left = 210, Top = 30};
            SettingsForm.Controls.Add(lblShadowflameText); //Shadowflame TEXT

            ShadowflameBox = new CheckBox {Checked = Shadowflame, TabIndex = 4, Size = new Size(15, 15), Left = 195, Top = 30};
            SettingsForm.Controls.Add(ShadowflameBox); //Shadowflame BOX		

            var lblDemonicCallingText = new Label // Demonic Calling LABEL
            {Text = "Demonic Calling", Size = new Size(150, 15), Left = 390, Top = 30};
            SettingsForm.Controls.Add(lblDemonicCallingText); //Demonic Calling TEXT

            DemonicCallingBox = new CheckBox {Checked = DemonicCalling, TabIndex = 6, Size = new Size(15, 15), Left = 375, Top = 30};
            SettingsForm.Controls.Add(DemonicCallingBox); // Demonic Calling BOX

            var lblImpendingDoomText = new Label // ImpendingDoom LABEL
            {Text = "Impending Doom", Size = new Size(150, 15), Left = 30, Top = 60};
            SettingsForm.Controls.Add(lblImpendingDoomText); //ImpendingDoom TEXT

            ImpendingDoomBox = new CheckBox {Checked = ImpendingDoom, TabIndex = 8, Size = new Size(15, 15), Left = 15, Top = 60};
            SettingsForm.Controls.Add(ImpendingDoomBox); //ImpendingDoom Box

            var lblImprovedDreadstalkersText = new Label //Improved Dreadstalkers label
            {Text = "Improved Dreadstalkers", Size = new Size(150, 15), Left = 210, Top = 60};
            SettingsForm.Controls.Add(lblImprovedDreadstalkersText); //Improved Dreadstalkers text

            ImprovedDreadstalkersBox = new CheckBox {Checked = ImprovedDreadstalkers, TabIndex = 10, Size = new Size(15, 15), Left = 195, Top = 60};
            SettingsForm.Controls.Add(ImprovedDreadstalkersBox); //Improved Dreadstalkers box

            var lblImplosionText = new Label //Implosion label
            {Text = "Implosion", Size = new Size(150, 15), Left = 390, Top = 60};
            SettingsForm.Controls.Add(lblImplosionText); //Implosion text

            ImplosionBox = new CheckBox {Checked = Implosion, TabIndex = 12, Size = new Size(15, 15), Left = 375, Top = 60};
            SettingsForm.Controls.Add(ImplosionBox); //Implosion box	

            var lblDemonicCircleText = new Label //Demonic Circle label
            {Text = "Demonic Circle", Size = new Size(150, 15), Left = 30, Top = 90};
            SettingsForm.Controls.Add(lblDemonicCircleText); //Demonic Circle text

            DemonicCircleBox = new CheckBox {Checked = DemonicCircle, TabIndex = 12, Size = new Size(15, 15), Left = 15, Top = 90};
            SettingsForm.Controls.Add(DemonicCircleBox); //Demonic Circle box			

            var lblMortalCoilText = new Label //Mortal Coil LABEL
            {Text = "Mortal Coil", Size = new Size(150, 15), Left = 210, Top = 90};
            SettingsForm.Controls.Add(lblMortalCoilText); //Mortal Coil TEXT

            MortalCoilBox = new CheckBox {Checked = MortalCoil, TabIndex = 4, Size = new Size(15, 15), Left = 195, Top = 90};
            SettingsForm.Controls.Add(MortalCoilBox); //Mortal Coil BOX		

            var lblShadowfuryText = new Label // Shadowfury LABEL
            {Text = "Shadowfury", Size = new Size(150, 15), Left = 390, Top = 90};
            SettingsForm.Controls.Add(lblShadowfuryText); //Shadowfury TEXT

            ShadowfuryBox = new CheckBox {Checked = Shadowfury, TabIndex = 6, Size = new Size(15, 15), Left = 375, Top = 90};
            SettingsForm.Controls.Add(ShadowfuryBox); // Shadowfury BOX

            var lblHandofDoomText = new Label //Hand of Doom LABEL
            {Text = "Hand of Doom", Size = new Size(150, 15), Left = 30, Top = 120};
            SettingsForm.Controls.Add(lblHandofDoomText); //Hand of Doom TEXT

            HandofDoomBox = new CheckBox {Checked = HandofDoom, TabIndex = 2, Size = new Size(15, 15), Left = 15, Top = 120};
            SettingsForm.Controls.Add(HandofDoomBox); //Hand of Doom BOX

            var lblPowerTripText = new Label //Power Trip LABEL
            {Text = "Power Trip", Size = new Size(150, 15), Left = 210, Top = 120};
            SettingsForm.Controls.Add(lblPowerTripText); //Power Trip TEXT

            PowerTripBox = new CheckBox {Checked = PowerTrip, TabIndex = 4, Size = new Size(15, 15), Left = 195, Top = 120};
            SettingsForm.Controls.Add(PowerTripBox); //Power Trip BOX		

            var lblSoulHarvestText = new Label // Soul Harvest LABEL
            {Text = "Soul Harvest", Size = new Size(150, 15), Left = 390, Top = 120};
            SettingsForm.Controls.Add(lblSoulHarvestText); //Soul Harvest TEXT

            SoulHarvestBox = new CheckBox {Checked = SoulHarvest, TabIndex = 6, Size = new Size(15, 15), Left = 375, Top = 120};
            SettingsForm.Controls.Add(SoulHarvestBox); // Soul Harvest BOX

            var lblDemonSkinText = new Label //Demon Skin LABEL
            {Text = "Demon Skin", Size = new Size(150, 15), Left = 30, Top = 150};
            SettingsForm.Controls.Add(lblDemonSkinText); //Demon Skin TEXT

            DemonSkinBox = new CheckBox {Checked = DemonSkin, TabIndex = 2, Size = new Size(15, 15), Left = 15, Top = 150};
            SettingsForm.Controls.Add(DemonSkinBox); //Demon Skin BOX

            var lblBurningRushText = new Label //Burning Rush LABEL
            {Text = "Burning Rush", Size = new Size(150, 15), Left = 210, Top = 150};
            SettingsForm.Controls.Add(lblBurningRushText); //Burning Rush TEXT

            BurningRushBox = new CheckBox {Checked = BurningRush, TabIndex = 4, Size = new Size(15, 15), Left = 195, Top = 150};
            SettingsForm.Controls.Add(BurningRushBox); //Burning Rush BOX		

            var lblDarkPactText = new Label //Dark Pact LABEL
            {Text = "Dark Pact", Size = new Size(150, 15), Left = 390, Top = 150};
            SettingsForm.Controls.Add(lblDarkPactText); //Dark Pact TEXT

            DarkPactBox = new CheckBox {Checked = DarkPact, TabIndex = 6, Size = new Size(15, 15), Left = 375, Top = 150};
            SettingsForm.Controls.Add(DarkPactBox); //Dark Pact BOX

            var lblGrimoireofSupremacyText = new Label //Grimoire of Supremacy LABEL
            {Text = "Grimoire of Supremacy", Size = new Size(150, 15), Left = 30, Top = 180};
            SettingsForm.Controls.Add(lblGrimoireofSupremacyText); //Grimoire of Supremacy TEXT

            GrimoireofSupremacyBox = new CheckBox {Checked = GrimoireofSupremacy, TabIndex = 2, Size = new Size(15, 15), Left = 15, Top = 180};
            SettingsForm.Controls.Add(GrimoireofSupremacyBox); //Grimoire of Supremacy BOX

            var lblGrimoireofServiceText = new Label //Grimoire of Service LABEL
            {Text = "Grimoire of Service", Size = new Size(150, 15), Left = 210, Top = 180};
            SettingsForm.Controls.Add(lblGrimoireofServiceText); //Grimoire of Service TEXT

            GrimoireofServiceBox = new CheckBox {Checked = GrimoireofService, TabIndex = 4, Size = new Size(15, 15), Left = 195, Top = 180};
            SettingsForm.Controls.Add(GrimoireofServiceBox); //Grimoire of Service BOX		

            var lblGrimoireofSynergyText = new Label //Grimoire of Synergy LABEL
            {Text = "Grimoire of Synergy", Size = new Size(150, 15), Left = 390, Top = 180};
            SettingsForm.Controls.Add(lblGrimoireofSynergyText); //Grimoire of Synergy TEXT

            GrimoireofSynergyBox = new CheckBox {Checked = GrimoireofSynergy, TabIndex = 6, Size = new Size(15, 15), Left = 375, Top = 180};
            SettingsForm.Controls.Add(GrimoireofSynergyBox); // Grimoire of Synergy BOX

            var lblSummonDarkglareText = new Label //Summon Darkglare LABEL
            {Text = "Summon Darkglare", Size = new Size(150, 15), Left = 30, Top = 210};
            SettingsForm.Controls.Add(lblSummonDarkglareText); //Summon Darkglare TEXT

            SummonDarkglareBox = new CheckBox {Checked = SummonDarkglare, TabIndex = 2, Size = new Size(15, 15), Left = 15, Top = 210};
            SettingsForm.Controls.Add(SummonDarkglareBox); //Summon Darkglare BOX

            var lblDemonboltText = new Label //Demonbolt LABEL
            {Text = "Demonbolt", Size = new Size(150, 15), Left = 210, Top = 210};
            SettingsForm.Controls.Add(lblDemonboltText); //Demonbolt TEXT

            DemonboltBox = new CheckBox {Checked = Demonbolt, TabIndex = 4, Size = new Size(15, 15), Left = 195, Top = 210};
            SettingsForm.Controls.Add(DemonboltBox); //Demonbolt BOX		

            var lblSoulConduitText = new Label //Soul Conduit LABEL
            {Text = "Soul Conduit", Size = new Size(150, 15), Left = 390, Top = 210};
            SettingsForm.Controls.Add(lblSoulConduitText); //Soul Conduit TEXT

            SoulConduitBox = new CheckBox {Checked = SoulConduit, TabIndex = 6, Size = new Size(15, 15), Left = 375, Top = 210};
            SettingsForm.Controls.Add(SoulConduitBox); //Soul Conduit BOX

            var cmdSave = new Button //Save Button
            {Text = "Save", Width = 40, Height = 20, Left = 445, Top = 270, Size = new Size(80, 30)};

            ShadowyInspirationBox.Checked = ShadowyInspiration;
            ShadowflameBox.Checked = Shadowflame;
            DemonicCallingBox.Checked = DemonicCalling;
            ImpendingDoomBox.Checked = ImpendingDoom;
            ImprovedDreadstalkersBox.Checked = ImprovedDreadstalkers;
            ImplosionBox.Checked = Implosion;
            DemonicCircleBox.Checked = DemonicCircle;
            MortalCoilBox.Checked = MortalCoil;
            ShadowfuryBox.Checked = Shadowfury;
            HandofDoomBox.Checked = HandofDoom;
            PowerTripBox.Checked = PowerTrip;
            SoulHarvestBox.Checked = SoulHarvest;
            DemonSkinBox.Checked = DemonSkin;
            BurningRushBox.Checked = BurningRush;
            DarkPactBox.Checked = DarkPact;
            GrimoireofSupremacyBox.Checked = GrimoireofSupremacy;
            GrimoireofServiceBox.Checked = GrimoireofService;
            GrimoireofSynergyBox.Checked = GrimoireofSynergy;
            SummonDarkglareBox.Checked = SummonDarkglare;
            DemonboltBox.Checked = Demonbolt;
            SoulConduitBox.Checked = SoulConduit;

            cmdSave.Click += CmdSave_Click;
            ShadowyInspirationBox.CheckedChanged += ShadowyInspiration_Click;
            ShadowflameBox.CheckedChanged += Shadowflame_Click;
            DemonicCallingBox.CheckedChanged += DemonicCalling_Click;
            ImpendingDoomBox.CheckedChanged += ImpendingDoom_Click;
            ImprovedDreadstalkersBox.CheckedChanged += ImprovedDreadstalkers_Click;
            ImplosionBox.CheckedChanged += Implosion_Click;
            DemonicCircleBox.CheckedChanged += DemonicCircle_Click;
            MortalCoilBox.CheckedChanged += MortalCoil_Click;
            ShadowfuryBox.CheckedChanged += Shadowfury_Click;
            HandofDoomBox.CheckedChanged += HandofDoom_Click;
            PowerTripBox.CheckedChanged += PowerTrip_Click;
            SoulHarvestBox.CheckedChanged += SoulHarvest_Click;
            DemonSkinBox.CheckedChanged += DemonSkin_Click;
            BurningRushBox.CheckedChanged += BurningRush_Click;
            DarkPactBox.CheckedChanged += DarkPact_Click;
            GrimoireofSupremacyBox.CheckedChanged += GrimoireofSupremacy_Click;
            GrimoireofServiceBox.CheckedChanged += GrimoireofService_Click;
            GrimoireofSynergyBox.CheckedChanged += GrimoireofSynergy_Click;
            SummonDarkglareBox.CheckedChanged += SummonDarkglare_Click;
            DemonboltBox.CheckedChanged += Demonbolt_Click;
            SoulConduitBox.CheckedChanged += SoulConduit_Click;

            SettingsForm.Controls.Add(cmdSave);
            lblShadowyInspirationText.BringToFront();
            lblShadowflameText.BringToFront();
            lblDemonicCallingText.BringToFront();
            lblImpendingDoomText.BringToFront();
            lblImprovedDreadstalkersText.BringToFront();
            lblImplosionText.BringToFront();
            lblDemonicCircleText.BringToFront();
            lblMortalCoilText.BringToFront();
            lblShadowfuryText.BringToFront();
            lblHandofDoomText.BringToFront();
            lblPowerTripText.BringToFront();
            lblSoulHarvestText.BringToFront();
            lblDemonSkinText.BringToFront();
            lblBurningRushText.BringToFront();
            lblDarkPactText.BringToFront();
            lblGrimoireofSupremacyText.BringToFront();
            lblGrimoireofServiceText.BringToFront();
            lblGrimoireofSynergyText.BringToFront();
            lblSummonDarkglareText.BringToFront();
            lblDemonboltText.BringToFront();
            lblSoulConduitText.BringToFront();

            Log.Write("Shadowy Inspiration = " + ShadowyInspiration + "                                             " + "Shadowflame = " + Shadowflame +
                      "                                                      " + "Demonic Calling = " + DemonicCalling);
            Log.Write("Impending Doom = " + ImpendingDoom + "                                                 " + "Improved Dreadstalkers = " + ImprovedDreadstalkers +
                      "                                     " + "Implosion = " + Implosion);
            Log.Write("Demonic Circle = " + DemonicCircle + "                                                     " + "Mortal Coil = " + MortalCoil +
                      "                                                           " + "Shadowfury = " + Shadowfury);
            Log.Write("Hand of Doom = " + HandofDoom + "                                                      " + "Power Trip = " + PowerTrip +
                      "                                                           " + "Soul Harvest = " + SoulHarvest);
            Log.Write("Demon Skin = " + DemonSkin + "                                                           " + "Burning Rush = " + BurningRush +
                      "                                                       " + "Dark Pact = " + DarkPact);
            Log.Write("Grimoire of Supremacy = " + GrimoireofSupremacy + "                                       " + "Grimoire of Service = " + GrimoireofService +
                      "                                              " + "Grimoire of Synergy = " + GrimoireofSynergy);
            Log.Write("Summon Darkglare = " + SummonDarkglare + "                                              " + "Demonbolt = " + Demonbolt +
                      "                                                           " + "Soul Conduit = " + SoulConduit);
        }

        private void CmdSave_Click(object sender, EventArgs e)
        {
            ShadowyInspiration = ShadowyInspirationBox.Checked;
            Shadowflame = ShadowflameBox.Checked;
            DemonicCalling = DemonicCallingBox.Checked;
            ImpendingDoom = ImpendingDoomBox.Checked;
            ImprovedDreadstalkers = ImprovedDreadstalkersBox.Checked;
            Implosion = ImplosionBox.Checked;
            DemonicCircle = DemonicCircleBox.Checked;
            MortalCoil = MortalCoilBox.Checked;
            Shadowfury = ShadowfuryBox.Checked;
            HandofDoom = HandofDoomBox.Checked;
            PowerTrip = PowerTripBox.Checked;
            SoulHarvest = SoulHarvestBox.Checked;
            DemonSkin = DemonSkinBox.Checked;
            BurningRush = BurningRushBox.Checked;
            DarkPact = DarkPactBox.Checked;
            GrimoireofSupremacy = GrimoireofSupremacyBox.Checked;
            GrimoireofService = GrimoireofServiceBox.Checked;
            GrimoireofSynergy = GrimoireofSynergyBox.Checked;
            SummonDarkglare = SummonDarkglareBox.Checked;
            Demonbolt = DemonboltBox.Checked;
            SoulConduit = SoulConduitBox.Checked;
            MessageBox.Show("Settings saved", "PixelMagic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SettingsForm.Close();
        }

        private void ShadowyInspiration_Click(object sender, EventArgs e)
        {
            ShadowyInspiration = ShadowyInspirationBox.Checked;
        }

        private void Shadowflame_Click(object sender, EventArgs e)
        {
            Shadowflame = ShadowflameBox.Checked;
        }

        private void DemonicCalling_Click(object sender, EventArgs e)
        {
            DemonicCalling = DemonicCallingBox.Checked;
        }

        private void ImpendingDoom_Click(object sender, EventArgs e)
        {
            ImpendingDoom = ImpendingDoomBox.Checked;
        }

        private void ImprovedDreadstalkers_Click(object sender, EventArgs e)
        {
            ImprovedDreadstalkers = ImprovedDreadstalkersBox.Checked;
        }

        private void Implosion_Click(object sender, EventArgs e)
        {
            Implosion = ImplosionBox.Checked;
        }

        private void DemonicCircle_Click(object sender, EventArgs e)
        {
            DemonicCircle = DemonicCircleBox.Checked;
        }

        private void MortalCoil_Click(object sender, EventArgs e)
        {
            MortalCoil = MortalCoilBox.Checked;
        }

        private void Shadowfury_Click(object sender, EventArgs e)
        {
            Shadowfury = ShadowfuryBox.Checked;
        }

        private void HandofDoom_Click(object sender, EventArgs e)
        {
            HandofDoom = HandofDoomBox.Checked;
        }

        private void PowerTrip_Click(object sender, EventArgs e)
        {
            PowerTrip = PowerTripBox.Checked;
        }

        private void SoulHarvest_Click(object sender, EventArgs e)
        {
            SoulHarvest = SoulHarvestBox.Checked;
        }

        private void DemonSkin_Click(object sender, EventArgs e)
        {
            DemonSkin = DemonSkinBox.Checked;
        }

        private void BurningRush_Click(object sender, EventArgs e)
        {
            BurningRush = BurningRushBox.Checked;
        }

        private void DarkPact_Click(object sender, EventArgs e)
        {
            DarkPact = DarkPactBox.Checked;
        }

        private void GrimoireofSupremacy_Click(object sender, EventArgs e)
        {
            GrimoireofSupremacy = GrimoireofSupremacyBox.Checked;
        }

        private void GrimoireofService_Click(object sender, EventArgs e)
        {
            GrimoireofService = GrimoireofServiceBox.Checked;
        }

        private void GrimoireofSynergy_Click(object sender, EventArgs e)
        {
            GrimoireofSynergy = GrimoireofSynergyBox.Checked;
        }

        private void SummonDarkglare_Click(object sender, EventArgs e)
        {
            SummonDarkglare = SummonDarkglareBox.Checked;
        }

        private void Demonbolt_Click(object sender, EventArgs e)
        {
            Demonbolt = DemonboltBox.Checked;
        }

        private void SoulConduit_Click(object sender, EventArgs e)
        {
            SoulConduit = SoulConduitBox.Checked;
        }

        public static bool ShadowyInspiration
        {
            get
            {
                var ShadowyInspiration = ConfigFile.ReadValue("WarlockDemonology", "Shadowy Inspiration").Trim();

                return ShadowyInspiration != "" && Convert.ToBoolean(ShadowyInspiration);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Shadowy Inspiration", value.ToString()); }
        }

        private static bool Shadowflame
        {
            get
            {
                var Shadowflame = ConfigFile.ReadValue("WarlockDemonology", "Shadowflame").Trim();

                return Shadowflame != "" && Convert.ToBoolean(Shadowflame);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Shadowflame", value.ToString()); }
        }

        private static bool DemonicCalling
        {
            get
            {
                var DemonicCalling = ConfigFile.ReadValue("WarlockDemonology", "Demonic Calling").Trim();

                return DemonicCalling != "" && Convert.ToBoolean(DemonicCalling);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Demonic Calling", value.ToString()); }
        }

        private static bool ImpendingDoom
        {
            get
            {
                var ImpendingDoom = ConfigFile.ReadValue("WarlockDemonology", "Impending Doom").Trim();

                return ImpendingDoom != "" && Convert.ToBoolean(ImpendingDoom);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Impending Doom", value.ToString()); }
        }

        private static bool ImprovedDreadstalkers
        {
            get
            {
                var ImprovedDreadstalkers = ConfigFile.ReadValue("WarlockDemonology", "Improved Dreadstalkers").Trim();

                return ImprovedDreadstalkers != "" && Convert.ToBoolean(ImprovedDreadstalkers);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Improved Dreadstalkers", value.ToString()); }
        }

        private static bool Implosion
        {
            get
            {
                var Implosion = ConfigFile.ReadValue("WarlockDemonology", "Implosion").Trim();

                return Implosion != "" && Convert.ToBoolean(Implosion);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Implosion", value.ToString()); }
        }

        private static bool DemonicCircle
        {
            get
            {
                var DemonicCircle = ConfigFile.ReadValue("WarlockDemonology", "Demonic Circle").Trim();

                return DemonicCircle != "" && Convert.ToBoolean(DemonicCircle);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Demonic Circle", value.ToString()); }
        }

        private static bool MortalCoil
        {
            get
            {
                var MortalCoil = ConfigFile.ReadValue("WarlockDemonology", "Mortal Coil").Trim();

                return MortalCoil != "" && Convert.ToBoolean(MortalCoil);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Mortal Coil", value.ToString()); }
        }

        private static bool Shadowfury
        {
            get
            {
                var Shadowfury = ConfigFile.ReadValue("WarlockDemonology", "Shadowfury").Trim();

                return Shadowfury != "" && Convert.ToBoolean(Shadowfury);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Shadowfury", value.ToString()); }
        }

        private static bool HandofDoom
        {
            get
            {
                var HandofDoom = ConfigFile.ReadValue("WarlockDemonology", "Hand of Doom").Trim();

                return HandofDoom != "" && Convert.ToBoolean(HandofDoom);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Hand of Doom", value.ToString()); }
        }

        private static bool PowerTrip
        {
            get
            {
                var PowerTrip = ConfigFile.ReadValue("WarlockDemonology", "Power Trip").Trim();

                return PowerTrip != "" && Convert.ToBoolean(PowerTrip);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Power Trip", value.ToString()); }
        }

        private static bool SoulHarvest
        {
            get
            {
                var SoulHarvest = ConfigFile.ReadValue("WarlockDemonology", "Soul Harvest").Trim();

                return SoulHarvest != "" && Convert.ToBoolean(SoulHarvest);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Soul Harvest", value.ToString()); }
        }

        private static bool DemonSkin
        {
            get
            {
                var DemonSkin = ConfigFile.ReadValue("WarlockDemonology", "Demon Skin").Trim();

                return DemonSkin != "" && Convert.ToBoolean(DemonSkin);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Demon Skin", value.ToString()); }
        }

        private static bool BurningRush
        {
            get
            {
                var BurningRush = ConfigFile.ReadValue("WarlockDemonology", "Burning Rush").Trim();

                return BurningRush != "" && Convert.ToBoolean(BurningRush);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Burning Rush", value.ToString()); }
        }

        private static bool DarkPact
        {
            get
            {
                var DarkPact = ConfigFile.ReadValue("WarlockDemonology", "Dark Pact").Trim();

                return DarkPact != "" && Convert.ToBoolean(DarkPact);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Dark Pact", value.ToString()); }
        }

        private static bool GrimoireofSupremacy
        {
            get
            {
                var GrimoireofSupremacy = ConfigFile.ReadValue("WarlockDemonology", "Grimoire of Supremacy").Trim();

                return GrimoireofSupremacy != "" && Convert.ToBoolean(GrimoireofSupremacy);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Grimoire of Supremacy", value.ToString()); }
        }

        private static bool GrimoireofService
        {
            get
            {
                var GrimoireofService = ConfigFile.ReadValue("WarlockDemonology", "Grimoire of Service").Trim();

                return GrimoireofService != "" && Convert.ToBoolean(GrimoireofService);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Grimoire of Service", value.ToString()); }
        }

        private static bool GrimoireofSynergy
        {
            get
            {
                var GrimoireofSynergy = ConfigFile.ReadValue("WarlockDemonology", "Grimoire of Synergy").Trim();

                return GrimoireofSynergy != "" && Convert.ToBoolean(GrimoireofSynergy);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Grimoire of Synergy", value.ToString()); }
        }

        private static bool SummonDarkglare
        {
            get
            {
                var SummonDarkglare = ConfigFile.ReadValue("WarlockDemonology", "Summon Darkglare").Trim();

                return SummonDarkglare != "" && Convert.ToBoolean(SummonDarkglare);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Summon Darkglare", value.ToString()); }
        }

        private static bool Demonbolt
        {
            get
            {
                var Demonbolt = ConfigFile.ReadValue("WarlockDemonology", "Demonbolt").Trim();

                return Demonbolt != "" && Convert.ToBoolean(Demonbolt);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Demonbolt", value.ToString()); }
        }

        private static bool SoulConduit
        {
            get
            {
                var SoulConduit = ConfigFile.ReadValue("WarlockDemonology", "Soul Conduit").Trim();

                return SoulConduit != "" && Convert.ToBoolean(SoulConduit);
            }
            set { ConfigFile.WriteValue("WarlockDemonology", "Soul Conduit", value.ToString()); }
        }
    }

    #endregion
}

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