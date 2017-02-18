// ReSharper disable UnusedMember.Global


using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PixelMagic.Helpers;

namespace PixelMagic.Rotation
{
    public class AssassinationCJ : CombatRoutine
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        public override string Name
        {
            get { return "Rogue-Assassination"; }
        }

        public override string Class
        {
            get { return "Rogue"; }
        }

        public override Form SettingsForm { get; set; }


        public override void Initialize()
        {
            Log.Write("Welcome to Rogue-Assassination", Color.Green);
            Log.Write("Suggested build : MP - Vigor - AP");
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (stopwatch.ElapsedMilliseconds == 0)
            {
                stopwatch.Start();
                Log.WritePixelMagic("The Cooldown toggle button is now Active (Numpad0). The delay is set to 1000ms ( 1 second )", Color.Black);
                return;
            }
            {
                if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_NUMPAD0) < 0)
                {
                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        combatRoutine.UseCooldowns = !combatRoutine.UseCooldowns;
                        WoW.Speak("Cooldowns " + (combatRoutine.UseCooldowns ? "On" : "Off"));
                        stopwatch.Restart();
                    }
                }
                if (combatRoutine.Type == RotationType.SingleTarget)
                {
                    if (WoW.IsSpellInRange("Rupture") && WoW.IsInCombat)


                    {
                        if (UseCooldowns && WoW.CanCast("Kingsbane") && WoW.CurrentComboPoints <= 4 && WoW.Energy >= 35 && !WoW.IsSpellOnCooldown("Kingsbane") &&
                            (WoW.SpellCooldownTimeRemaining("Vendetta") >= 10 || WoW.TargetHasDebuff("Vendetta")))
                        {
                            WoW.CastSpell("Kingsbane");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Vanish") && WoW.CanCast("Garrote") && WoW.Energy >= 45 && !WoW.TargetHasDebuff("Garrote") && !WoW.IsSpellOnCooldown("Garrote") &&
                            WoW.CurrentComboPoints <= 4 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Garrote");
                            return;
                        }
                        if (WoW.TargetHasDebuff("Vendetta") && WoW.CanCast("Fan Of Knives") && WoW.Energy >= 35 && WoW.PlayerHasBuff("FoK") && WoW.PlayerBuffStacks("FoK") == 30 &&
                            WoW.CurrentComboPoints <= 4)
                        {
                            WoW.CastSpell("Fan Of Knives");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Vanish") && WoW.CanCast("Garrote") && WoW.Energy >= 45 && WoW.TargetHasDebuff("Garrote") && WoW.TargetDebuffTimeRemaining("Garrote") <= 3 &&
                            WoW.CurrentComboPoints <= 4 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Garrote");
                            return;
                        }
                        if (WoW.CurrentComboPoints == 4 && WoW.Energy >= 25 && WoW.CanCast("Rupture") && !WoW.TargetHasDebuff("Rupture") && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Rupture");
                            return;
                        }
                        if (WoW.CurrentComboPoints == 4 && WoW.Energy >= 25 && WoW.TargetHasDebuff("Rupture") && WoW.TargetDebuffTimeRemaining("Rupture") <= 6 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Rupture");
                            return;
                        }
                        if (WoW.CurrentComboPoints == 5 && WoW.Energy >= 25 && WoW.CanCast("Rupture") && !WoW.TargetHasDebuff("Rupture") && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Rupture");
                            return;
                        }
                        if (WoW.CurrentComboPoints == 5 && WoW.Energy >= 25 && WoW.TargetHasDebuff("Rupture") && WoW.TargetDebuffTimeRemaining("Rupture") <= 6 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Rupture");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Vanish") && WoW.TargetHasDebuff("Toxins") && WoW.TargetDebuffTimeRemaining("Toxins") <= 1.5 && WoW.Energy >= 35 && WoW.CurrentComboPoints == 4 &&
                            WoW.CanCast("Envenom") && WoW.TargetHasDebuff("Rupture") && WoW.TargetDebuffTimeRemaining("Rupture") > 6 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Vanish") && WoW.TargetHasDebuff("Toxins") && WoW.TargetDebuffTimeRemaining("Toxins") <= 1.5 && WoW.Energy >= 35 && WoW.CurrentComboPoints == 5 &&
                            WoW.CanCast("Envenom") && WoW.TargetHasDebuff("Rupture") && WoW.TargetDebuffTimeRemaining("Rupture") > 6 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Vanish") && !WoW.TargetHasDebuff("Toxins") && WoW.Energy >= 35 && WoW.CurrentComboPoints == 4 && WoW.CanCast("Envenom") &&
                            WoW.TargetHasDebuff("Rupture") && WoW.TargetDebuffTimeRemaining("Rupture") > 6 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Vanish") && !WoW.TargetHasDebuff("Toxins") && WoW.Energy >= 35 && WoW.CurrentComboPoints == 5 && WoW.CanCast("Envenom") &&
                            WoW.TargetHasDebuff("Rupture") && WoW.TargetDebuffTimeRemaining("Rupture") > 6 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Vanish") && WoW.TargetHasDebuff("Toxins") && WoW.TargetHasDebuff("Vendetta") && WoW.Energy >= 140 && WoW.CurrentComboPoints >= 4 &&
                            WoW.CanCast("Envenom") && WoW.TargetHasDebuff("Rupture") && WoW.TargetDebuffTimeRemaining("Rupture") > 6 && WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (WoW.CanCast("Mutilate") && WoW.Energy >= 55 && WoW.CurrentComboPoints <= 3)
                        {
                            WoW.CastSpell("Mutilate");
                            return;
                        }
                        if (!WoW.PlayerHasBuff("Critbuff") && UseCooldowns && WoW.CanCast("Vendetta") && !WoW.IsSpellOnCooldown("Vendetta") && WoW.Energy <= 50)
                        {
                            WoW.CastSpell("Vendetta");
                            return;
                        }
                        /*if (UseCooldowns &&
                        WoW.CanCast("Vanish") &&
                        WoW.CurrentComboPoints >= 5 &&
                        WoW.Energy >= 25 && WoW.TargetHasDebuff("Vendetta") &&
                        (
                            WoW.TargetHasDebuff("Rupture") &&
                            WoW.TargetDebuffTimeRemaining("Rupture") < 10
                        )
                    ) {
                        WoW.CastSpell("Vanish");
                        return;
                    }*/
                        if (UseCooldowns && WoW.CurrentComboPoints == 5 && !WoW.IsSpellOnCooldown("Vanish") && WoW.Energy >= 35 && WoW.TargetHasDebuff("Vendetta"))
                        {
                            WoW.CastSpell("Vanish");
                            return;
                        }
                        if (UseCooldowns && WoW.CurrentComboPoints == 5 && WoW.PlayerHasBuff("Vanish") && WoW.Energy >= 35 && WoW.TargetHasDebuff("Vendetta"))
                        {
                            WoW.CastSpell("Rupture");
                            return;
                        }
                    }
                }


                if (combatRoutine.Type == RotationType.AOE || combatRoutine.Type == RotationType.SingleTargetCleave) // Do AoE Target Stuff here
                {
                    if (WoW.HasTarget && WoW.IsSpellInRange("Rupture") && WoW.IsInCombat)
                    {
                        if (WoW.Energy >= 35 && WoW.CurrentComboPoints <= 4 && WoW.CanCast("Fan Of Knives"))
                        {
                            WoW.CastSpell("Fan Of Knives");
                            return;
                        }
                        if (WoW.Energy >= 35 && WoW.CurrentComboPoints == 4 && WoW.TargetHealthPercent <= 35 && WoW.CanCast("Envenom"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (WoW.Energy >= 35 && WoW.CurrentComboPoints == 5 && WoW.TargetHealthPercent <= 35 && WoW.CanCast("Envenom"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (WoW.Energy >= 25 && WoW.CurrentComboPoints == 4 && WoW.TargetHealthPercent >= 36 && WoW.CanCast("Rupture") && !WoW.TargetHasDebuff("Rupture"))
                        {
                            WoW.CastSpell("Rupture");
                            return;
                        }
                        if (WoW.Energy >= 25 && WoW.CurrentComboPoints == 5 && WoW.TargetHealthPercent >= 36 && WoW.CanCast("Rupture") && !WoW.TargetHasDebuff("Rupture"))
                        {
                            WoW.CastSpell("Rupture");
                            return;
                        }
                        if (WoW.Energy >= 35 && WoW.CurrentComboPoints >= 4 && WoW.TargetHealthPercent >= 36 && WoW.TargetHasDebuff("Rupture") && WoW.CanCast("Envenom"))
                        {
                            WoW.CastSpell("Envenom");
                            return;
                        }
                        if (WoW.CanCast("Garrote") && WoW.Energy >= 45 && !WoW.TargetHasDebuff("Garrote") && !WoW.IsSpellOnCooldown("Garrote") && WoW.CurrentComboPoints <= 4 &&
                            WoW.IsSpellInRange("Garrote"))
                        {
                            WoW.CastSpell("Garrote");
                        }
                    }
                }
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=CreepyJoker
AddonName=Sucstobeyou
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,1943,Rupture,D2
Spell,79140,Vendetta,D5
Spell,1856,Vanish,V
Spell,1329,Mutilate,D1
Spell,703,Garrote,R
Spell,192759,Kingsbane,D3
Spell,32645,Envenom,Q
Spell,51723,Fan Of Knives,X
Aura,1943,Rupture
Aura,1784,Stealth
Aura,703,Garrote
Aura,208693,FoK
Aura,235027,Critbuff
Aura,1856,Vanish
Aura,192425,Toxins
Aura,32645,Envenom
Aura,200802,Agonizing Poison
Aura,193641,Elaborate Planning
Aura,79140,Vendetta
*/
