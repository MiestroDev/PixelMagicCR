// winifix@gmail.com
// ReSharper disable UnusedMember.Global


using System.Drawing;
using System.Windows.Forms;
using PixelMagic.Helpers;

namespace PixelMagic.Rotation
{
    public class HunterSurvival : CombatRoutine
    {
        public override string Name => "Hunter Survival";

        public override string Class => "Hunter";

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Welcome to Hunter Survival", Color.Green);
            WoW.Speak("Welcome to PixelMagic Survival");
        }

        public override void Stop()
        {
        }

        public override void Pulse() // Updated for Legion (tested and working for single target)
        {
            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat)
                {
                    if (WoW.CanCast("Fury of Eagle") && WoW.PlayerHasBuff("Mongoose Fury") && WoW.PlayerBuffStacks("Mongoose Fury") >= 5)
                    {
                        WoW.CastSpell("Fury of Eagle");
                        return;
                    }
                    if (WoW.CanCast("Explosive Trap") && WoW.IsSpellInRange("Raptor Strike") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Explosive Trap");
                        return;
                    }
                    if (WoW.CanCast("Dragonsfire Grenade") && WoW.IsSpellInRange("Raptor Strike") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Dragonsfire Grenade");
                        return;
                    }
                    if (WoW.CanCast("Lacerate") && !WoW.TargetHasDebuff("Lacerate") && (WoW.Focus >= 35) && WoW.IsSpellInRange("Lacerate") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Lacerate");
                        return;
                    }
                    if (WoW.CanCast("Snake Hunter") && (WoW.PlayerSpellCharges("Mongoose Bite") <= 0) && (WoW.PlayerBuffTimeRemaining("Mongoose Fury") >= 6) &&
                        WoW.IsSpellInRange("Mongoose Bite") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Snake Hunter");
                        return;
                    }
                    if (WoW.CanCast("Aspect of the Eagle") && (WoW.PlayerSpellCharges("Mongoose Bite") >= 2) && (WoW.PlayerBuffTimeRemaining("Mongoose Fury") >= 11) &&
                        WoW.IsSpellInRange("Mongoose Bite") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Aspect of the Eagle");
                        return;
                    }
                    if (WoW.CanCast("Mongoose Bite") && (!WoW.TargetHasDebuff("Mongoose Fury") || (WoW.PlayerBuffTimeRemaining("Mongoose Fury") >= 3)) && WoW.IsSpellInRange("Mongoose Bite") &&
                        !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Mongoose Bite");
                        return;
                    }
                    if (WoW.CanCast("Mongoose Bite") && WoW.PlayerHasBuff("Aspect of the Eagle") && WoW.IsSpellInRange("Mongoose Bite") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Mongoose Bite");
                        return;
                    }
                    if (WoW.CanCast("Throwing Axes") && WoW.IsSpellInRange("Throwing Axes") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Throwing Axes");
                        return;
                    }
                    if (WoW.CanCast("Flanking Strike") && (WoW.Focus >= 50) && WoW.IsSpellInRange("Flanking Strike") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Flanking Strike");
                        return;
                    }
                    if (WoW.CanCast("Raptor Strike") && (WoW.Focus >= 25) && WoW.IsSpellInRange("Raptor Strike") && WoW.IsSpellOnCooldown("Lacerate") &&
                        WoW.IsSpellOnCooldown("Flanking Strike") && WoW.IsSpellOnCooldown("Throwing Axes") && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling)
                    {
                        WoW.CastSpell("Raptor Strike");
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                // Do AOE stuff here
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
AddonAuthor=Vectarius
AddonName=PixelMagic
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,190928,Mongoose Bite,D1
Spell,202800,Flanking Strike,D2
Spell,185855,Lacerate,D3
Spell,186270,Raptor Strike,D4
Spell,214579,Carve,D5
Spell,191433,Explosive Trap,D6
Spell,194855,Dragonsfire Grenade,D7
Spell,200163,Throwing Axes,D8
Spell,203415,Fury of Eagle,D9
Spell,186289,Aspect of the Eagle,D0
Spell,201078,Snake Hunter,C
Aura,190931,Mongoose Fury
Aura,185855,Lacerate
Aura,186289,Aspect of the Eagle
*/