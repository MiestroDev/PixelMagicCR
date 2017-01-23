﻿/**
 * Shadow rotation written by Miestro.
 * 
 * TODO:
 *  Add support for other talents.
 *  Add direct support for surrender to madness.
 *  Add support for a list of spells to interrupt.
 *  Check haste for rotation changes
 *  
 * 
 * Published November 27th, 2016
 */

using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PixelMagic.Helpers;


/**
 * Shadow priest rotation.
*/

namespace PixelMagic.Rotation
{
    internal class MiestroShadow : CombatRoutine
    {
        //General constants
        private const int HEALTH_PERCENT_FOR_SWD = 35;
        private const int PANIC_INSANITY_VALUE = 45;
        private const int INTERRUPT_DELAY = 650;

        //Spell Constants
        private const string SHADOW_PAIN = "Shadow Word: Pain";
        private const string VAMPIRIC_TOUCH = "Vampiric Touch";
        private const string VOID_TORRENT = "Void Torrent";
        private const string MIND_FLAY = "Mind Flay";
        private const string MIND_SEAR = "Mind Sear";
        private const string MIND_BLAST = "Mind Blast";
        private const string SHADOW_MEND = "Shadow Mend";
        private const string POWER_WORD_SHIELD = "Power Word: Shield";
        private const string VOID_BOLT = "Void Bolt";
        private const string VOID_ERUPTION = "Void Eruption";
        private const string SURRENDER_MADNESS = "Surrender to Madness";
        private const string SHADOW_DEATH = "Shadow Word: Death";
        private const string SHADOWFORM = "Shadowform";
        private const string SILENCE = "Silence";

        //Aura Constants
        private const string VOIDFORM_AURA = "Voidform";
        private const string SHADOWFORM_AURA = "Shadowform";

        /// <summary>
        ///     Private variable for timing interrupt delay.
        /// </summary>
        private readonly Stopwatch timer = new Stopwatch();

        /**
         * Member Variables
         */

        //Name of the rotation.
        public override string Name => "Shadow Rotation by Miestro";

        //Name of the class.
        public override string Class => "Priest";

        //Settings form, Right side.
        public override Form SettingsForm { get; set; }

        //Initialize, print some details so the user can prepare thier ingame character.
        public override void Initialize()
        {
            Log.Write("Welcome to Miestro's Shadow rotation", Color.Orange);
            Log.Write("Please make sure your specialization is as follows: http://us.battle.net/wow/en/tool/talent-calculator#Xba!0101102", Color.Orange);
            Log.Write("Surrender to madness is not explicitly supported in this build yet, however it can be manually cast.", Color.Orange);
            Log.Write("Note: legendaries are not supported either. If you need one supported or something fixed, please make note of it in the discord.", Color.Orange);
        }

        public override void Stop()
        {
            //Do nothing
        }

        public override void Pulse()
        {
            if (WoW.HealthPercent <= 1)
            {
                //Dead
                return;
            }

            //Heal yourself, Can't do damage if you're dead.
            if (WoW.HealthPercent <= 60)
            {
                if (isPlayerBusy(true, false) && !WoW.PlayerHasBuff(POWER_WORD_SHIELD))
                {
                    castWithRangeCheck(POWER_WORD_SHIELD);
                }
                castWithRangeCheck(SHADOW_MEND);
            }
            //Shield if health is dropping.
            if (WoW.HealthPercent <= 80 && !WoW.PlayerHasBuff(POWER_WORD_SHIELD))
            {
                castWithRangeCheck(POWER_WORD_SHIELD, true);
            }

            //Always have shadowform.
            if (!(WoW.PlayerHasBuff(SHADOWFORM_AURA) || WoW.PlayerHasBuff(VOIDFORM_AURA)))
            {
                castWithRangeCheck(SHADOWFORM);
            }

            if (WoW.HasTarget && WoW.TargetIsEnemy)
            {
                if (!WoW.PlayerHasBuff(VOIDFORM_AURA))
                {
                    //Just so happens that the spell and debuff name are the same, this is not ALWAYS the case.
                    maintainDebuff(VAMPIRIC_TOUCH, VAMPIRIC_TOUCH, 5);
                    maintainDebuff(SHADOW_PAIN, SHADOW_PAIN, 2);
                }

                switch (combatRoutine.Type)
                {
                    //Single target
                    case RotationType.SingleTarget:
                        doRotation();
                        break;

                    //Against 2 or more
                    case RotationType.AOE:
                    case RotationType.SingleTargetCleave:
                        doRotation(false);
                        break;
                }
            }

            //Interrupt after a delay.
            if (WoW.TargetIsCasting && WoW.TargetIsEnemy)
            {
                if (timer.ElapsedMilliseconds >= INTERRUPT_DELAY)
                {
                    castWithRangeCheck(SILENCE);
                }
                else
                {
                    timer.Reset();
                    timer.Start();
                }
            }
        }

        /// <summary>
        ///     Do the rotation.
        /// </summary>
        private void doRotation(bool isSingleTarget = true)
        {
            bool ignoreMovement = WoW.PlayerHasBuff(SURRENDER_MADNESS);

            if (WoW.Insanity >= 100 || WoW.PlayerHasBuff(VOIDFORM_AURA))
            {
                //Expend insanity in voidform.
                if (WoW.HasTarget && !WoW.PlayerHasBuff(VOIDFORM_AURA))
                {
                    castWithRangeCheck(VOID_ERUPTION);
                }
                else
                {
                    /* Disabled, allow player to cast.
                    if (WoW.PlayerBuffStacks(VOIDFORM_AURA)<5) {
                        castWithRangeCheck(SHADOWFIEND);
                    }
                    */

                    //If we can, cast it.
                    castWithRangeCheck(VOID_BOLT);

                    //Cast it.
                    castWithRangeCheck(VOID_TORRENT, ignoreMovement);
                    if (WoW.LastSpell.Equals(VOID_TORRENT))
                    {
                        Thread.Sleep(4000); //Sleep while void torrent is casting.
                    }

                    //If the boss health is at or below 35% cast SW:D
                    if (WoW.TargetHealthPercent <= HEALTH_PERCENT_FOR_SWD)
                    {
                        if (WoW.PlayerSpellCharges(SHADOW_DEATH) == 2 && WoW.Insanity <= 70)
                        {
                            //If we have 2 charges, always cast
                            castWithRangeCheck(SHADOW_DEATH);
                        }
                        else if (WoW.PlayerSpellCharges(SHADOW_DEATH) == 1)
                        {
                            //If we have 1 charge, only cast if at high insanity and mindblast is off CD or extremely low insanity
                            if ((WoW.Insanity > PANIC_INSANITY_VALUE && !(WoW.IsSpellOnCooldown(MIND_BLAST) || WoW.IsSpellOnCooldown(VOID_BOLT))) || WoW.Insanity <= calculateInsanityDrain())
                            {
                                castWithRangeCheck(SHADOW_DEATH);
                            }
                        }
                    }

                    //If we can, cast it.
                    castWithRangeCheck(MIND_BLAST);

                    //If we have high stacks, cast shadowfiend.
                    /* Disabled, allow player to cast. 
                    if (WoW.PlayerBuffStacks(VOIDFORM_AURA)>=15) {
                        castWithRangeCheck(SHADOWFIEND);
                    }
                    */

                    if (!isPlayerBusy(ignoreChanneling: false))
                    {
                        //Always fill with mind flay on single target.
                        castWithRangeCheck(MIND_FLAY);
                    }
                }
            }
            else
            {
                //Build up insanity
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.TargetIsVisible)
                {
                    //If we can, cast mind blast.
                    if (castWithRangeCheck(MIND_BLAST))
                    {
                        return;
                    }

                    //If we don't have anything else to do, cast Mind flay.
                    if (!isPlayerBusy(ignoreChanneling: false))
                    {
                        if (isSingleTarget)
                        {
                            castWithRangeCheck(MIND_FLAY);
                        }
                        else
                        {
                            castWithRangeCheck(MIND_SEAR);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Calculate the amount of insanity currently drained per second
        /// </summary>
        /// <returns>The amount of insanity drained per second</returns>
        private float calculateInsanityDrain()
        {
            return 9 + (WoW.PlayerBuffStacks(VOIDFORM_AURA) - 1/2);
        }

        /// <summary>
        ///     Get whether we can cast spells based on what the player is currently doing.
        /// </summary>
        /// <param name="ignoreMovement">Can we ignore movement</param>
        /// <param name="ignoreChanneling"></param>
        /// <returns>True if we can not currently cast another spell.</returns>
        private bool isPlayerBusy(bool ignoreMovement = false, bool ignoreChanneling = true)
        {
            var canCast = WoW.PlayerIsCasting || (WoW.PlayerIsChanneling && !ignoreChanneling) || (WoW.IsMoving && ignoreMovement);
            return canCast;
        }

        /// <summary>
        ///     Cast a spell by name. Will check range, cooldown, and visibility. After the spell is cast, the thread will sleep
        ///     for GCD.
        /// </summary>
        /// <param name="spellName">The name of the spell in the spell databse.</param>
        /// <param name="ignoreMovement">Can we cast while moving.</param>
        /// <param name="ignoreChanneling"></param>
        /// <returns>True if the spell was cast, false if it was not.</returns>
        private bool castWithRangeCheck(string spellName, bool ignoreMovement = false, bool ignoreChanneling = true)
        {
            //Can't do range check.
            if (!isPlayerBusy(ignoreMovement, ignoreChanneling) && WoW.CanCast(spellName))
            {
                WoW.CastSpell(spellName);
                if (WoW.IsSpellOnGCD(spellName))
                {
                    Thread.Sleep(WoW.SpellCooldownTimeRemaining(spellName));
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Maintain a debuff if it is not currently on the target or if it's about to expire.
        /// </summary>
        /// <param name="debuffName">The name of the debuff we are maintaining.</param>
        /// <param name="spellName">The name of the spell that applies the debuff.</param>
        /// <param name="minTimeToExpire">The minimum amount of time to allow on the debuff before renewing.</param>
        /// <returns>True if the debuff was renewed, otherwise fasle.</returns>
        private void maintainDebuff(string debuffName, string spellName, float minTimeToExpire)
        {
            if (!WoW.TargetHasDebuff(debuffName) || (WoW.TargetDebuffTimeRemaining(debuffName) < minTimeToExpire))
            {
                castWithRangeCheck(spellName);
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Miestro
AddonName=PixelMagic
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,589,Shadow Word: Pain,E
Spell,34914,Vampiric Touch,U
Spell,205065,Void Torrent,B
Spell,15407,Mind Flay,D2
Spell,48045,Mind Sear,D5
Spell,8092,Mind Blast,D1
Spell,186263,Shadow Mend,A
Spell,17,Power Word: Shield,S
Spell,205448,Void Bolt,Z
Spell,228260,Void Eruption,Z
Spell,193223,Surrender to Madness,F
Spell,34433,Shadowfiend,D7
Spell,32379,Shadow Word: Death,D3
Spell,232698,Shadowform,W
Spell,15487,Silence,D0
Spell,47585,Dispersion,K
Aura,232698,Shadowform
Aura,34914,Vampiric Touch
Aura,589,Shadow Word: Pain
Aura,197937,Lingering Insanity
Aura,194249,Voidform
Aura,193223,Surrender to Madness
Aura,17,Power Word: Shield
*/