using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Webdriver.Domify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anilinkz_Player.Classes.Sources
{
    class ArkVid : Source
    {
        ChromeDriver player = null;
        Thread timeChecker = null;

        public ArkVid(ChromeDriver player, string url)
        {
            this.player = player;
            SetPlayer(url);
        }
        /// <summary>
        /// This will set the Player Browser to a URL
        /// </summary>
        public void SetPlayer(string url)
        {
            player = new ChromeDriver();
            
            player.Navigate().GoToUrl(url);
            //This code clicks the play button automatically through Java Script injection
            player.ExecuteJavascript("document.getElementsByClassName('vjs-big-play-button')[0].click();");
            
            if (timeChecker == null)
            {
                timeChecker = new Thread(() => { CheckTime(); });
                timeChecker.Start();
            }
        }

        public void CheckTime()
        {
            while (true)
            {
                //These are strings containing the times from the video player 
                string currentTimeDiv = player.Divs(By.ClassName("vjs-current-time-display"))[0].Text;
                string endTimeDiv = player.Divs(By.ClassName("vjs-duration-display"))[0].Text;
                if (timeCheck(currentTimeDiv, endTimeDiv))
                    break;
                System.Threading.Thread.Sleep(1000);
            }

        }

        public bool timeCheck(string start, string end)
        {
            string startFormated = start.Remove(0, 13);
            startFormated = startFormated.Replace(":", "");

            string endFormated = end.Remove(0, 14);
            endFormated = endFormated.Replace(":", "");

            int startFinalForm = Convert.ToInt32(startFormated);
            int endFinalForm = Convert.ToInt32(endFormated);

            //checks for video add time
            if (endFinalForm < 300)
                return false;
            if (startFinalForm == endFinalForm)
                return true;
            return false;
        }
    }
}
