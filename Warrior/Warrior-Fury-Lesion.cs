// winifix@gmail.com
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertPropertyToExpressionBody

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using PixelMagic.Helpers;
using PixelMagic.GUI;


namespace PixelMagic.Rotation
{
    public class FuryWarrior : CombatRoutine
    {
		
		private CheckBox TrueFrothingZerkerFalseMassacre;
		private CheckBox MplusAoE;
		
		
		 public override string Name => "Fury Warrior";

        public override string Class => "Warrior";
		
		
		        
		private static bool TFZFMint
        {
            get
            {
                var TFZFMint = ConfigFile.ReadValue("FuryWarrior", "TFZFMint").Trim();

                return TFZFMint != "" && Convert.ToBoolean(TFZFMint);
            }
            set { ConfigFile.WriteValue("FuryWarrior", "TFZFMint", value.ToString()); }
        }
		
		private static bool MplusAoEint
		{
            get
            {
                var MplusAoE = ConfigFile.ReadValue("FuryWarrior", "MplusAoE").Trim();

                return MplusAoE != "" && Convert.ToBoolean(MplusAoE);
            }
            set { ConfigFile.WriteValue("FuryWarrior", "MplusAoE", value.ToString()); }
        }
        
		public override Form SettingsForm { get; set; }
		
		
        public override void Initialize()
        {
            Log.Write("Welcome to Fury Warrior", Color.Green);
			Log.Write("Talent build 1 : ", Color.Green);
			Log.Write("Written based on WoWHead Guide ", Color.Green);
			Log.Write("ST Raid build = 2333(1or2)32 ", Color.Green);
			Log.Write("M+ AoE build = 1113(1or2)32  ", Color.Green);
			
			
			
			SettingsForm = new Form {Text = "Settings", StartPosition = FormStartPosition.CenterScreen, Width = 350, Height = 250, ShowIcon = false};

            //var picBox = new PictureBox {Left = 0, Top = 0, Width = 800, Height = 100, Image = TopLogo};
            //SettingsForm.Controls.Add(picBox);

            var lblTFZFMintText = new Label //12; 114 LEFT is first value, Top is second.
            {
                Text = "Tick for Frothing, Untick for Massacre",
                Size = new Size(188, 13), //81; 13
                Left = 12,
                Top = 14
            };
            SettingsForm.Controls.Add(lblTFZFMintText); //113; 114 

            TrueFrothingZerkerFalseMassacre = new CheckBox {Checked = TFZFMint, TabIndex = 2, Size = new Size(15, 14), Left = 200, Top = 14};
            SettingsForm.Controls.Add(TrueFrothingZerkerFalseMassacre);
			//
			 var lblMplusAoEintText = new Label //12; 114 LEFT is first value, Top is second.
            {
                Text = "Tick for M+/AoE",
                Size = new Size(188, 13), //81; 13
                Left = 12,
                Top = 29
            };
            SettingsForm.Controls.Add(lblMplusAoEintText); //113; 114 

            MplusAoE = new CheckBox {Checked = MplusAoEint, TabIndex = 2, Size = new Size(15, 14), Left = 200, Top = 29};
            SettingsForm.Controls.Add(MplusAoE);
			
			
			
			var cmdSave = new Button {Text = "Save", Width = 65, Height = 25, Left = 15, Top = 150, Size = new Size(120, 48)};
			
			TrueFrothingZerkerFalseMassacre.Checked = TFZFMint;
			MplusAoE.Checked = MplusAoEint;
			
			cmdSave.Click += CmdSave_Click;
			TrueFrothingZerkerFalseMassacre.CheckedChanged += TFZFM_Click;
			MplusAoE.CheckedChanged += MplusAoE_Click;
			
			SettingsForm.Controls.Add(cmdSave);
            lblTFZFMintText.BringToFront();
			lblMplusAoEintText.BringToFront();
			
			Log.Write("---------------------------------------------------------", Color.Blue);
            Log.Write("IF True = Frothing Zerk, IF False = Massacre = ", Color.Red);
			Log.Write("=" + TFZFMint, Color.Blue);
           	Log.Write("---------------------------------------------------------", Color.Blue);
		}	
			private void CmdSave_Click(object sender, EventArgs e)
        {
            TFZFMint = TrueFrothingZerkerFalseMassacre.Checked;
            			
            MessageBox.Show("Settings saved", "PixelMagic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SettingsForm.Close();
        }
			
        private void TFZFM_Click(object sender, EventArgs e)
        {
            TFZFMint = TrueFrothingZerkerFalseMassacre.Checked;
        }
		
		private void MplusAoE_Click(object sender, EventArgs e)
        {
            MplusAoEint = MplusAoE.Checked;
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (combatRoutine.Type == RotationType.SingleTarget)  // Do Single Target Stuff here
            {
			/*
			Endless Rage / War Machine
			Double Time
			Avatar
			Warpaint
			Frothing Zerker/Massacre
			Inner Rage
			Reckless Abandon
			*/
				
								
			//AOE on Press
			if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_KEY_Z) < 0)
			{
				if (WoW.HasTarget&&WoW.IsInCombat&&WoW.TargetIsEnemy)
				{
				 

				if (TFZFMint)
						{
							if (WoW.CanCast("Whirlwind")
								&& !WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Whirlwind"); return;}
							
							if (WoW.CanCast("Rampage")
								&& WoW.Rage >= 100
								&& WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Rampage"); return;}
							
							if (WoW.CanCast("Bloodthirst")
								&& !WoW.PlayerHasBuff("Enrage")
								&& WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Bloodthirst"); return;}
							
							if (WoW.CanCast("Execute")
								&& WoW.IsSpellOverlayed("Execute"))
							{WoW.CastSpell("Execute"); return;}
							
							if (WoW.CanCast("Raging Blow")
								&& !WoW.CanCast("Bloodthirst")
								&& !WoW.CanCast("Rampage"))
							{WoW.CastSpell("Raging Blow"); return;}
							
							if (WoW.CanCast("Bloodthirst")
								&& WoW.PlayerHasBuff("Enrage")
								&& WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Bloodthirst"); return;}
							
						}
						
						if (!TFZFMint)
						{
							if (WoW.CanCast("Whirlwind")
								&& !WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Whirlwind"); return;}
							
							if (WoW.CanCast("Rampage")
								&& !WoW.PlayerHasBuff("Enrage"))
							{WoW.CastSpell("Rampage"); return;}
							
							if (WoW.CanCast("Bloodthirst")
								&& !WoW.PlayerHasBuff("Enrage")
								&& WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Bloodthirst"); return;}
							
							if (WoW.CanCast("Execute")
								&& WoW.IsSpellOverlayed("Execute"))
							{WoW.CastSpell("Execute"); return;}
							
							if (WoW.CanCast("Raging Blow")
								&& !WoW.CanCast("Bloodthirst")
								&& !WoW.CanCast("Rampage"))
							{WoW.CastSpell("Raging Blow"); return;}
							
							if (WoW.CanCast("Bloodthirst")
								&& WoW.PlayerHasBuff("Enrage")
								&& WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Bloodthirst"); return;}
						}
				
				
				}
			}

                //Normal ST rotation
                if (WoW.HasTarget && WoW.IsInCombat && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
                {
                    if (WoW.TargetHealthPercent >= 20)
                        //When targets are above 20%. Not in Execute phase. (need to change this to TTExecute)
                        //Single target with Reckless Abandon -  Battle Cry  Avatar  Rampage -  Raging Blow -  Odyn's Fury -  Bloodthirst - Raging Blow -  Furious Slash -  Bloodthirst
						if (MplusAoEint)
						{
							if (WoW.CanCast("Whirlwind")
								&& !WoW.PlayerHasBuff("Meat Cleaver"))
							{WoW.CastSpell("Whirlwind"); return;}
							
							
							if (WoW.CanCast("Whirlwind")
								&& !WoW.CanCast("Bloodthirst")
								&& !WoW.CanCast("Raging Blow")
								&& !WoW.CanCast("Rampage")
								)
							{WoW.CastSpell("Whirlwind"); return;}
							
							if (WoW.CanCast("Raging Blow")
                                && !WoW.CanCast("Bloodthirst"))
                            { WoW.CastSpell("Raging Blow"); return; }
							
							
							
							
						}
                        if (TFZFMint)
                        {
                            if (WoW.CanCast("Rampage")
                                && WoW.Rage >= 100)
                            { WoW.CastSpell("Rampage"); return; }

                            if (WoW.CanCast("Bloodthirst")
                                && !WoW.PlayerHasBuff("Enrage"))
                            { WoW.CastSpell("Bloodthirst"); return; }

                            if (WoW.CanCast("Execute")
                                && WoW.IsSpellOverlayed("Execute"))
                            { WoW.CastSpell("Execute"); return; }

                            if (!MplusAoEint && WoW.CanCast("Raging Blow")
                                && !WoW.CanCast("Bloodthirst"))
                            { WoW.CastSpell("Raging Blow"); return; }

                            if (WoW.CanCast("Bloodthirst")
                                && WoW.PlayerHasBuff("Enrage"))
                            { WoW.CastSpell("Bloodthirst"); return; }

                            if (WoW.CanCast("Furious Slash")
                                && !WoW.CanCast("Bloodthirst")
                                && !WoW.CanCast("Raging Blow"))
                            { WoW.CastSpell("Furious Slash"); return; }
                        }

                    if (!TFZFMint)
                    {
                        if (WoW.CanCast("Rampage")
                            && !WoW.PlayerHasBuff("Enrage"))
                        { WoW.CastSpell("Rampage"); return; }

                        if (WoW.CanCast("Bloodthirst")
                            && !WoW.PlayerHasBuff("Enrage"))
                        { WoW.CastSpell("Bloodthirst"); return; }

                        if (WoW.CanCast("Execute")
                            && WoW.IsSpellOverlayed("Execute"))
                        { WoW.CastSpell("Execute"); return; }

                        if (!MplusAoEint && WoW.CanCast("Raging Blow")
                            && !WoW.CanCast("Bloodthirst"))
                        { WoW.CastSpell("Raging Blow"); return; }

                        if (WoW.CanCast("Bloodthirst")
                            && WoW.PlayerHasBuff("Enrage"))
                        { WoW.CastSpell("Bloodthirst"); return; }

                        if (WoW.CanCast("Furious Slash")
                            && !WoW.CanCast("Bloodthirst")
                            && !WoW.CanCast("Raging Blow"))
                        { WoW.CastSpell("Furious Slash"); return; }
                    }


                    if (WoW.TargetHealthPercent <= 20)

                    {
                        if (TFZFMint)
                        {
                            if (WoW.CanCast("Rampage")
                                && WoW.Rage >= 100)
                            { WoW.CastSpell("Rampage"); return; }

                            if (WoW.CanCast("Bloodthirst")
                                && !WoW.PlayerHasBuff("Enrage"))
                            { WoW.CastSpell("Bloodthirst"); return; }

                            if (WoW.CanCast("Execute")
                                && WoW.IsSpellOverlayed("Execute")
                                && WoW.Rage > 25)
                            { WoW.CastSpell("Execute"); return; }

                            if (WoW.CanCast("Execute")
                                && WoW.Rage > 25)
                            { WoW.CastSpell("Execute"); return; }

                            if (!MplusAoEint && WoW.CanCast("Raging Blow")
                                && !WoW.CanCast("Bloodthirst"))
                            { WoW.CastSpell("Raging Blow"); return; }

                            if (WoW.CanCast("Bloodthirst")
                                && WoW.PlayerHasBuff("Enrage"))
                            { WoW.CastSpell("Bloodthirst"); return; }

                            if (WoW.CanCast("Furious Slash")
                                && !WoW.CanCast("Bloodthirst")
                                && !WoW.CanCast("Raging Blow"))
                            { WoW.CastSpell("Furious Slash"); return; }
                        }

                        if (!TFZFMint)
                        {
                            if (WoW.CanCast("Execute")
                                && !WoW.PlayerHasBuff("Enrage")
                                && !WoW.PlayerHasBuff("Massacre"))
                            { WoW.CastSpell("Execute"); return; }

                            if (WoW.CanCast("Rampage")
                                && WoW.PlayerHasBuff("Massacre"))
                            { WoW.CastSpell("Rampage"); return; }

                            if (WoW.CanCast("Execute")
                                && WoW.PlayerHasBuff("Enrage"))
                            { WoW.CastSpell("Execute"); return; }

                            if (WoW.CanCast("Bloodthirst")
                                && WoW.Rage < 25)
                            { WoW.CastSpell("Bloodthirst"); return; }

                            if (!MplusAoEint && WoW.CanCast("Raging Blow")
                                && WoW.Rage < 25)
                            { WoW.CastSpell("Raging Blow"); return; }

                        }
                    }

                }	
			}
				
				
							
			
					
			
            if (combatRoutine.Type == RotationType.AOE)
		    {
				// AOE stuff here
			}
            if (combatRoutine.Type == RotationType.SingleTargetCleave)
            {
                // Do Single Target Cleave stuff here if applicable else ignore this one
            }
			
		}
	}
}

	
	


			
            
    


/*
[AddonDetails.db]
AddonAuthor=Dupe
AddonName=badgggggggggerui
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,205545,Odyn's Fury,D1
Spell,23881,Bloodthirst,D2
Spell,184367,Rampage,D3
Spell,85288,Raging Blow,D4
Spell,100130,Furious Slash,D5
Spell,118000,Dragon Roar,F1
Spell,1719,Battle Cry,F3
Spell,5308,Execute,Z
Spell,190411,Whirlwind,D6
Aura,184362,Enrage
Aura,85739,Meat Cleaver
*/
