// winifix@gmail.com
// ReSharper disable UnusedMember.Global
// Todo: right range of Throw Glaive spell 204157


using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PixelMagic.GUI;
using PixelMagic.Helpers;

namespace PixelMagic.Rotation
{
    public class DemonHunterVengJE : CombatRoutine
    {
        private readonly Stopwatch interruptwatch = new Stopwatch();
        public override string Name => "PixelMagic Vengeance by Jedix";

        public override string Class => "Demon Hunter";

        public override Form SettingsForm
        {
            get { return new frmVengance(); }
            set { }
        }

        public override void Initialize()
        {
            Log.DrawHorizontalLine();
            WoW.Speak("Welcome to PixelMagic Vengeance by Jedix");
            Log.WritePixelMagic("Welcome to PixelMagic Vengeance by Jedix", Color.Green);
            Log.Write("Suggested build: 3332333", Color.Green);
            Log.Write("IMPORTANT!", Color.Red);
            Log.Write("You can use any talent instead of Fracture, just set bot to aoe - he will never use a Fracture", Color.Black);
            Log.Write(
                "Also need to do a macro in wow to make Sigil of Flame work, macro #showtooltip /cast [mod:shift,target=player][nomod,target=player][mod:ctrl,target=player] Sigil of Flame /cast [mod:alt] Sigil of Flame /script UIErrorsFrame:Clear()",
                Color.Black);
            Log.Write("Put it in position of Sigil of Flame, when u need to cast it ranged just hold ALT and click", Color.Black);
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (WoW.IsInCombat)
            {
                interruptwatch.Start();
            }

            if (UseCooldowns)
            {
            }

            //if (WoW.PlayerHasBuff("Mount")) return;

            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat) //Pull need to do by yourself - to prevent a mess
                {
                    if (WoW.HealthPercent < 30 && !WoW.IsSpellOnCooldown("Metamorphasis"))
                    {
                        WoW.Speak("Metamorphosis");
                        Log.Write("Health low < 30% using CDs...", Color.Red);
                        WoW.CastSpell("Metamorphasis"); // Off the GCD no return needed
                    }

                    if (WoW.CanCast("Throw Glaive") && !WoW.IsSpellInRange("Soul Carver") && WoW.IsSpellInRange("Fel Eruption")) //Need to implement range 30y for Throw Glaive spell 204157
                    {
                        WoW.CastSpell("Throw Glaive");
                        return;
                    }

                    if (!WoW.IsSpellInRange("Soul Carver")) // If we are out of melee range return
                        return;

                    if (WoW.TargetIsCasting && interruptwatch.ElapsedMilliseconds > 1200)
                    {
                        if (!WoW.IsSpellOnCooldown("Sigal of Silence") && WoW.WasLastCasted("Arcane Torrent"))
                        {
                            WoW.Speak("Interrupting spell");
                            WoW.CastSpell("Sigal of Silence");
                            interruptwatch.Reset();
                            interruptwatch.Start();
                            return;
                        }

                        if (!WoW.IsSpellOnCooldown("Arcane Torrent") && WoW.WasLastCasted("Sigal of Silence"))
                        {
                            WoW.Speak("Interrupting spell");
                            WoW.CastSpell("Arcane Torrent");
                            interruptwatch.Reset();
                            interruptwatch.Start();
                            return;
                        }
                    }

                    if (WoW.CanCast("Fiery Brand") && !WoW.TargetHasDebuff("Fiery Demise"))
                    {
                        WoW.CastSpell("Fiery Brand");
                    }

                    if (WoW.CanCast("Soul Carver") && WoW.SpellCooldownTimeRemaining("Fiery Brand") >= 10)
                    {
                        WoW.CastSpell("Soul Carver");
                        return;
                    }

                    if (WoW.CanCast("Spirit Bomb") && WoW.PlayerHasBuff("Soul Fragments") && !WoW.TargetHasDebuff("Frailty"))
                    {
                        WoW.CastSpell("Spirit Bomb");
                        return;
                    }

                    if (WoW.CanCast("Sigil of Flame") && WoW.TargetHasDebuff("Fiery Demise") && WoW.TargetDebuffTimeRemaining("Fiery Demise") >= 7)
                    {
                        //WoW.CastSpellOnMe("Sigil of Flame");  // Use this if you not using "Concentrated Sigal's" talent - this is a little buggy!!!
                        WoW.CastSpell("Sigil of Flame"); // NB must have "Concentrated Sigal's" talent 
                        return;
                    }

                    if (WoW.CanCast("Fel Eruption") && WoW.Pain >= 10 && WoW.TargetHasDebuff("Fiery Demise"))
                    {
                        WoW.CastSpell("Fel Eruption");
                        return;
                    }

                    if (WoW.CanCast("Soul Barrier") && WoW.Pain >= 10 && WoW.PlayerHasBuff("Soul Fragments") && WoW.PlayerBuffStacks("Soul Fragments") >= 4)
                    {
                        WoW.CastSpell("Soul Barrier");
                        return;
                    }

                    if (WoW.CanCast("Demon Spikes") && !WoW.PlayerHasBuff("Demon Spikes") && (WoW.Pain == 100 || WoW.HealthPercent < 80 && WoW.Pain >= 20)) // to not waste cd and pain
                    {
                        WoW.CastSpell("Demon Spikes");
                    }

                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 30 && WoW.HealthPercent < 25) // Extra save, when we are on too low hp
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }

                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 80 && WoW.HealthPercent < 50 && !WoW.CanCast("Demon Spikes"))
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }

                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 60 && WoW.HealthPercent < 50 && WoW.PlayerHasBuff("Demon Spikes"))
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }

                    if (WoW.CanCast("Fracture") && WoW.Pain >= 20 && WoW.HealthPercent >= 50 && WoW.PlayerHasBuff("Demon Spikes"))
                    {
                        WoW.CastSpell("Fracture");
                        return;
                    }

                    if (WoW.CanCast("Fracture") && WoW.Pain >= 80 && WoW.HealthPercent >= 50 && !WoW.CanCast("Demon Spikes"))
                    {
                        WoW.CastSpell("Fracture");
                        return;
                    }

                    if (WoW.CanCast("Immolation Aura"))
                    {
                        WoW.CastSpell("Immolation Aura");
                        return;
                    }

                    if (WoW.CanCast("Sigil of Flame"))
                    {
                        //WoW.CastSpellOnMe("Sigil of Flame");  // Use this if you not using "Concentrated Sigal's" talent - this is a little buggy!!!
                        WoW.CastSpell("Sigil of Flame"); // NB must have "Concentrated Sigal's" talent 
                        return;
                    }

                    if (WoW.CanCast("Fel Eruption") && WoW.Pain >= 10)
                    {
                        WoW.CastSpell("Fel Eruption");
                        return;
                    }

                    if (WoW.CanCast("Sever") && WoW.PlayerHasBuff("Metamorphasis")) // Pain Generator
                    {
                        WoW.CastSpell("Sever");
                        return;
                    }

                    if (WoW.CanCast("Shear") && !WoW.PlayerHasBuff("Metamorphasis")) // Pain Generator
                    {
                        WoW.CastSpell("Shear");
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.SingleTargetCleave)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat) //Pull need to do by yourself - to prevent a mess
                {
                    if (WoW.HealthPercent < 30 && !WoW.IsSpellOnCooldown("Metamorphasis"))
                    {
                        WoW.Speak("Metamorphosis");
                        Log.Write("Health low < 30% using CDs...", Color.Red);
                        WoW.CastSpell("Metamorphasis"); // Off the GCD no return needed
                    }

                    if (!WoW.IsSpellInRange("Soul Carver") && !WoW.IsSpellOnCooldown("Throw Glaive") && WoW.IsSpellInRange("Fel Eruption"))
                        //Need to implement range 30y for Throw Glaive spell 204157
                    {
                        WoW.CastSpell("Throw Glaive");
                        return;
                    }

                    if (!WoW.IsSpellInRange("Soul Carver")) // If we are out of melee range return
                        return;

                    if (WoW.TargetIsCasting && interruptwatch.ElapsedMilliseconds > 1200)
                    {
                        if (!WoW.IsSpellOnCooldown("Sigal of Silence") && WoW.WasLastCasted("Arcane Torrent"))
                        {
                            WoW.Speak("Interrupting spell");
                            WoW.CastSpell("Sigal of Silence");
                            interruptwatch.Reset();
                            interruptwatch.Start();
                            return;
                        }

                        if (!WoW.IsSpellOnCooldown("Arcane Torrent") && WoW.WasLastCasted("Sigal of Silence"))
                        {
                            WoW.Speak("Interrupting spell");
                            WoW.CastSpell("Arcane Torrent");
                            interruptwatch.Reset();
                            interruptwatch.Start();
                            return;
                        }
                    }

                    if (WoW.CanCast("Fiery Brand") && !WoW.TargetHasDebuff("Fiery Demise"))
                    {
                        WoW.CastSpell("Fiery Brand");
                    }

                    if (WoW.CanCast("Soul Carver") && WoW.SpellCooldownTimeRemaining("Fiery Brand") >= 10)
                    {
                        WoW.CastSpell("Soul Carver");
                        return;
                    }

                    if (WoW.CanCast("Spirit Bomb") && WoW.PlayerHasBuff("Soul Fragments") && !WoW.TargetHasDebuff("Frailty"))
                    {
                        WoW.CastSpell("Spirit Bomb");
                        return;
                    }

                    if (WoW.CanCast("Sigil of Flame") && WoW.TargetHasDebuff("Fiery Demise") && WoW.TargetDebuffTimeRemaining("Fiery Demise") >= 7)
                    {
                        //WoW.CastSpellOnMe("Sigil of Flame");  // Use this if you not using "Concentrated Sigal's" talent - this is a little buggy!!!
                        WoW.CastSpell("Sigil of Flame"); // NB must have "Concentrated Sigal's" talent 
                        return;
                    }

                    if (WoW.CanCast("Fel Eruption") && WoW.Pain >= 10 && WoW.TargetHasDebuff("Fiery Demise"))
                    {
                        WoW.CastSpell("Fel Eruption");
                        return;
                    }

                    if (WoW.CanCast("Soul Barrier") && WoW.Pain >= 10 && WoW.PlayerHasBuff("Soul Fragments") && WoW.PlayerBuffStacks("Soul Fragments") >= 4)
                    {
                        WoW.CastSpell("Soul Barrier");
                        return;
                    }

                    if (WoW.CanCast("Demon Spikes") && !WoW.PlayerHasBuff("Demon Spikes") && (WoW.Pain == 100 || WoW.HealthPercent < 90 && WoW.Pain >= 20)) // to not waste cd and pain
                    {
                        WoW.CastSpell("Demon Spikes");
                    }

                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 30 && WoW.HealthPercent < 25) // Extra save, when we are on too low hp
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }

                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 60 && WoW.PlayerHasBuff("Demon Spikes"))
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }
                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 80 && !WoW.CanCast("Demon Spikes"))
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }

                    if (WoW.CanCast("Immolation Aura"))
                    {
                        WoW.CastSpell("Immolation Aura");
                        return;
                    }

                    if (WoW.CanCast("Sigil of Flame"))
                    {
                        //WoW.CastSpellOnMe("Sigil of Flame");  // Use this if you not using "Concentrated Sigal's" talent - this is a little buggy!!!
                        WoW.CastSpell("Sigil of Flame"); // NB must have "Concentrated Sigal's" talent 
                        return;
                    }

                    if (WoW.CanCast("Fel Eruption") && WoW.Pain >= 10)
                    {
                        WoW.CastSpell("Fel Eruption");
                        return;
                    }

                    if (WoW.CanCast("Sever") && WoW.PlayerHasBuff("Metamorphasis")) // Pain Generator
                    {
                        WoW.CastSpell("Sever");
                        return;
                    }

                    if (WoW.CanCast("Shear") && !WoW.PlayerHasBuff("Metamorphasis")) // Pain Generator
                    {
                        WoW.CastSpell("Shear");
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && !WoW.PlayerIsChanneling && WoW.IsInCombat) //Pull need to do by yourself - to prevent a mess
                {
                    if (WoW.HealthPercent < 30 && !WoW.IsSpellOnCooldown("Metamorphasis"))
                    {
                        WoW.Speak("Metamorphosis");
                        Log.Write("Health low < 30% using CDs...", Color.Red);
                        WoW.CastSpell("Metamorphasis"); // Off the GCD no return needed
                    }

                    if (!WoW.IsSpellInRange("Soul Carver") && !WoW.IsSpellOnCooldown("Throw Glaive") && WoW.IsSpellInRange("Fel Eruption"))
                        //Need to implement range 30y for Throw Glaive spell 204157
                    {
                        WoW.CastSpell("Throw Glaive");
                        return;
                    }

                    if (!WoW.IsSpellInRange("Soul Carver")) // If we are out of melee range return
                        return;

                    if (WoW.TargetIsCasting && interruptwatch.ElapsedMilliseconds > 1200)
                    {
                        if (!WoW.IsSpellOnCooldown("Sigal of Silence") && WoW.WasLastCasted("Arcane Torrent"))
                        {
                            WoW.Speak("Interrupting spell");
                            WoW.CastSpell("Sigal of Silence");
                            interruptwatch.Reset();
                            interruptwatch.Start();
                            return;
                        }

                        if (!WoW.IsSpellOnCooldown("Arcane Torrent") && WoW.WasLastCasted("Sigal of Silence"))
                        {
                            WoW.Speak("Interrupting spell");
                            WoW.CastSpell("Arcane Torrent");
                            interruptwatch.Reset();
                            interruptwatch.Start();
                            return;
                        }
                    }

                    if (WoW.CanCast("Fiery Brand") && !WoW.TargetHasDebuff("Fiery Demise"))
                    {
                        WoW.CastSpell("Fiery Brand");
                    }

                    if (WoW.CanCast("Soul Carver") && WoW.SpellCooldownTimeRemaining("Fiery Brand") >= 10)
                    {
                        WoW.CastSpell("Soul Carver");
                        return;
                    }

                    if (WoW.CanCast("Spirit Bomb") && WoW.PlayerHasBuff("Soul Fragments") && !WoW.TargetHasDebuff("Frailty"))
                    {
                        WoW.CastSpell("Spirit Bomb");
                        return;
                    }

                    if (WoW.CanCast("Sigil of Flame"))
                    {
                        //WoW.CastSpellOnMe("Sigil of Flame");  // Use this if you not using "Concentrated Sigal's" talent - this is a little buggy!!!
                        WoW.CastSpell("Sigil of Flame"); // NB must have "Concentrated Sigal's" talent 
                        return;
                    }

                    if (WoW.CanCast("Fel Eruption") && WoW.Pain >= 10 && WoW.TargetHasDebuff("Fiery Demise"))
                    {
                        WoW.CastSpell("Fel Eruption");
                        return;
                    }

                    if (WoW.CanCast("Soul Barrier") && WoW.Pain >= 10 && WoW.PlayerHasBuff("Soul Fragments") && WoW.PlayerBuffStacks("Soul Fragments") >= 4)
                    {
                        WoW.CastSpell("Soul Barrier");
                        return;
                    }

                    if (WoW.CanCast("Demon Spikes") && !WoW.PlayerHasBuff("Demon Spikes") && (WoW.Pain == 100 || WoW.HealthPercent < 80 && WoW.Pain >= 20)) // to not waste cd and pain
                    {
                        WoW.CastSpell("Demon Spikes");
                    }

                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 30 && WoW.HealthPercent < 25) // Extra save, when we are on too low hp
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }

                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 60 && WoW.PlayerHasBuff("Demon Spikes"))
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }
                    if (WoW.CanCast("Soul Cleave") && WoW.Pain >= 80 && !WoW.CanCast("Demon Spikes"))
                    {
                        WoW.CastSpell("Soul Cleave");
                        return;
                    }

                    if (WoW.CanCast("Immolation Aura"))
                    {
                        WoW.CastSpell("Immolation Aura");
                        return;
                    }

                    if (WoW.CanCast("Fel Eruption") && WoW.Pain >= 10)
                    {
                        WoW.CastSpell("Fel Eruption");
                        return;
                    }

                    if (WoW.CanCast("Sever") && WoW.PlayerHasBuff("Metamorphasis")) // Pain Generator
                    {
                        WoW.CastSpell("Sever");
                        return;
                    }

                    if (WoW.CanCast("Shear") && !WoW.PlayerHasBuff("Metamorphasis")) // Pain Generator
                    {
                        WoW.CastSpell("Shear");
                    }
                }
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Jedix
AddonName=Pawn
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,203782,Shear,G
Spell,235964,Sever,G
Spell,228477,Soul Cleave,E
Spell,207407,Soul Carver,NumPad5
Spell,178740,Immolation Aura,D1
Spell,204596,Sigil of Flame,NumPad4
Spell,204157,Throw Glaive,D3
Spell,207682,Sigal of Silence,NumPad1
Spell,202719,Arcane Torrent,D8
Spell,187827,Metamorphasis,NumPad2
Spell,204021,Fiery Brand,NumPad3
Spell,218679,Spirit Bomb,NumPad9
Spell,203720,Demon Spikes,Q
Spell,189110,Infernal Strike,MButton
Spell,209795,Fracture,D2
Spell,227225,Soul Barrier,D6
Spell,211881,Fel Eruption,D5
Aura,203819,Demon Spikes
Aura,212818,Fiery Demise
Aura,187827,Metamorphasis
Aura,224509,Frailty
Aura,203981,Soul Fragments
Aura,41252,Mount
Aura,207472,Magnum Opus
Item,80610,Mana
*/