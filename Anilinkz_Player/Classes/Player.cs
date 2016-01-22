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
            ArkVid,
            yUp
        }

        static public void setPlayer(sources source)
        {
            switch (source)
            {
                case sources.ArkVid:
                    new Sources.ArkVid(player, DataHold.VideoList.Dequeue());
                    break;
                case sources.yUp:
                    new Sources.yUp(player, DataHold.VideoList.Dequeue());
                    break;
                default:
                    break;
            }
        }

    }
}
