using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;

namespace Annie_O_Matic
{
    internal class AnnieMenu
    {
        public static Menu Config;
        public static Orbwalking.Orbwalker Orbwalker;
        

        public static void CreateMenu()
        {
            
            Config = new Menu("Annie-O-Matic", "Annie-O-Matic", true);
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));


            var clearMenu = new Menu("Clear Settings", "Clear Settings");
            {
                var laneMenu = new Menu("Lane Settings", "Lane Settings");
                {
                    laneMenu.AddItem(new MenuItem("clearMenu.laneMenu.useq", "Use [Q]")).SetValue(false);
                    laneMenu.AddItem(new MenuItem("clearMenu.laneMenu.useqlast", "Use [Q] To Last Hit")).SetValue(true);
                    laneMenu.AddItem(new MenuItem("clearMenu.laneMenu.usew", "Use [W]")).SetValue(false);
                    laneMenu.AddItem(new MenuItem("clearMenu.laneMenu.manalimit", "Mana X Limit (%)"))
                        .SetValue(new Slider(60, 1, 100));
                    laneMenu.AddItem(new MenuItem("clearMenu.laneMenu.usewslider", "Use [W] If Hits X Enemies"))
                        .SetValue(new Slider(3, 1, 5));
                    laneMenu.AddItem(new MenuItem("clearMenu.laneMenu.mambo",
                        "MAMBO LIKE A CRAZY  [! DANGER LOW MANA RISK !] ")).SetValue(false);
                    clearMenu.AddSubMenu(laneMenu);
                }
           
                var lastMenu = new Menu("Last Hit Settings", "Last Hit Settings");
                {
                    
                    lastMenu.AddItem(new MenuItem("clearMenu.lastMenu.useqlast", "Use [Q] To Last Hit")).SetValue(true);
                    clearMenu.AddSubMenu(lastMenu);
                }
                Config.AddSubMenu(clearMenu);
            }

            Config.AddToMainMenu();
        }
    }
}
           