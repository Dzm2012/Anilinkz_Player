using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Windows.Automation;
using System.Text.RegularExpressions;
using OpenQA;
using OpenQA.Selenium.Chrome;
using Selenium.Webdriver.Domify;
using OpenQA.Selenium;
using System.Threading;
using System.Drawing;
using Awesomium.Core;

namespace Anilinkz_Player.Classes
{
    static class Page
    {
        static ChromeDriver driver = null;
        
        static int episodenumber = 1;
        static string URL_TEST = "http://anilinkz.tv/dragonball-episode-";
        static string nextURL = null;
        static bool started = false;
        /// <summary>
        /// This method goes to the AniLinkz url and gets the Video div then passes the data inside the div to the shreder
        /// Then it sets the "Player" browser with the source type and url
        /// </summary>
        static public void GetData(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (driver == null)
                {
                    driver = new ChromeDriver();

                    //This code moves the "Working window" off the monitor comment out thisline if your confused what its doing
                    driver.Manage().Window.Position = new System.Drawing.Point(-2000, 0);
                }

                //Sets the url and adds the video number (this will need to be changed to accomidate a source in the URL)
                driver.Navigate().GoToUrl(URL_TEST + episodenumber.ToString());

                //This looks for the video div, so far it looks like all the divs are named the same but they may not be for different sources
                //If they are not this should be moved into the corrisponding source's class
                var div = driver.Divs(By.Id("Videoads"));
                while (div.Count() < 1)
                    div = driver.Divs(By.Id("Videoads"));
                var doc = driver.Divs(By.Id("Videoads"))[0];
                string tempHTML = doc.InnerHtml;
                string videoUrlFormated = HTMLShredder(tempHTML);
                if (!started)
                {
                    started = true;
                    //The source will need to be determined before setting the player
                    Player.setPlayer(Player.sources.ArkVid, videoUrlFormated);
                }
                DataHold.VideoList.Enqueue(videoUrlFormated);
                episodenumber++;
            }
        }

        /// <summary>
        /// This method will get the source video URL and return just the url from the shredded HTML
        /// </summary>
        static public string HTMLShredder(string html)
        {
            
            string videoUrl = html.Split(new string[] { "<iframe src=\"http://" }, StringSplitOptions.RemoveEmptyEntries)[1];
            string videoUrlFormated = videoUrl.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
            return "http://" + videoUrlFormated;
        }
        
    }
    
}
