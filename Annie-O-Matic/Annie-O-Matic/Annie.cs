using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;



namespace Annie_O_Matic 
{
    internal class Annie : AnnieMenu
    {
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }

        #region Spells and Menu
        // Menu and Orbwalker
       
        
        //Spells
        private static Spell Q = new Spell(SpellSlot.Q, 625);
        private static Spell W = new Spell(SpellSlot.W, 625);
        private static Spell E = new Spell(SpellSlot.E);
        private static Spell R = new Spell(SpellSlot.R, 600);
        
        
        #endregion

        public static void Load(EventArgs args)
        {

            if (Player.ChampionName != "Annie")
            {
                return;
            }

            CreateMenu();
            W.SetSkillshot(0.5f, 250f, float.MaxValue, false, SkillshotType.SkillshotCone);
            R.SetSkillshot(0.2f, 250f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Game.OnUpdate += OnGameUpdate;
        }

        private static void LastHit()
        {
            var canQ = Config.Item("clearMenu.lastHitMenu.useqlast").GetValue<bool>();

            var minionNumber =
               MinionManager.GetMinions(Player.Position, Q.Range, MinionTypes.All,
                   MinionTeam.NotAlly).FirstOrDefault();

            if (minionNumber == null)
                return;

            var minion = minionNumber;
            var minionhp = minion.Health;

            if (minionhp <= Q.GetDamage(minion) && canQ && Q.IsReady())
            {
                Q.Cast(minion);
            }
        }

        private static void LaneClear()
        {
            var canQ = Config.Item("farmMenu.laneClearMenu.useq").GetValue<bool>();
            var canW = Config.Item("farmMenu.laneClearMenu.usew").GetValue<bool>();
            var mambo = Config.Item("farmMenu.laneClearMenu.mambo").GetValue<bool>();
            var minMana = Config.Item("farmMenu.laneClearMenu.manalimit").GetValue<Slider>().Value;

            #region Q Usage
            var minionNumber =
                 MinionManager.GetMinions(Player.Position, Q.Range,
                 MinionTypes.All, MinionTeam.NotAlly).FirstOrDefault();

            if (minionNumber == null)
                return;

            var minion = minionNumber;

            var minionhp = minion.Health;



            if (canQ && Q.IsReady() && minion.IsValidTarget(Q.Range) && minionhp <= Q.GetDamage(minion) && minionhp > Player.GetAutoAttackDamage(minion) && Player.ManaPercent >= minMana)
            {
                Q.Cast(minion);
            }
            
            #endregion

            #region W Usage
            
            var wMinionNumber =
                MinionManager.GetMinions(Player.Position,
                W.Range, MinionTypes.All, MinionTeam.NotAlly);

            if (minionNumber == null)
                return;

            var wPredict = W.GetLineFarmLocation(wMinionNumber);

            if (canW && W.IsReady() && minion.IsValidTarget(W.Range) && minionhp <= W.GetDamage(minion) && Player.ManaPercent >= minMana && wPredict.MinionsHit >= 2)
            {
                W.Cast(minion);
            } 
            #endregion


        }

        private static void OnGameUpdate(EventArgs args)
        {
            Orbwalker.SetAttack(true);

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit();
                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    break;
            }
        }

        private static void DamageCalc()
        {
            var target = TargetSelector.GetTarget(Player.CastRange, TargetSelector.DamageType.Magical);

            var qDamage = Q.GetDamage(target);
            var wDamage = W.GetDamage(target);
            var rDamage = R.GetDamage(target);

            var totalDamage = qDamage + wDamage + rDamage;    
        }

        
    }
}
