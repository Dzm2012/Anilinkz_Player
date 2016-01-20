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
        static bool autoPlay = false;
        public enum sources
        {
            ArkVid
        }

        static public void setPlayer(sources source)
        {
            do
            {
                switch (source)
                {
                    case sources.ArkVid:
                        new Sources.ArkVid(player, DataHold.VideoList.Dequeue());
                        break;
                    default:
                        break;
                }
            }
            while (autoPlay);
            
        }

    }
}
