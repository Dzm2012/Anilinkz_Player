using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anilinkz_Player.Classes
{
    static class Player
    {
        static ChromeDriver player = null;
        public enum sources
        {
            ArkVid
        }

        static public void setPlayer(sources source, string URL)
        {
            switch(source)
            {
                case sources.ArkVid:
                    new Sources.ArkVid(player, URL);
                    break;
                default:
                    break;
            }
            
        }

    }
}
