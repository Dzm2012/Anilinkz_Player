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
using System.Xml.Linq;
using System.IO;

namespace Anilinkz_Player.Classes
{
    static class Page
    {
        static ChromeDriver driver = null;
        
        static string nextURL = null;
        static bool started = false;
        /// <summary>
        /// This method goes to the AniLinkz url and gets the Video div then passes the data inside the div to the shreder
        /// Then it sets the "Player" browser with the source type and url
        /// </summary>
        static public void GetData(int episodeCount,int episodeNumber, string url, List<string> sourcePriority)
        {
            for (int i = 0; i < episodeCount; i++)
            {
                if (driver == null)
                {
                    driver = new ChromeDriver();

                    //This code moves the "Working window" off the monitor comment out thisline if your confused what its doing
                    driver.Manage().Window.Position = new System.Drawing.Point(0, 0);
                }

                //Sets the url and adds the video number (this will need to be changed to accomidate a source in the URL)
                string urlFormated = url + episodeNumber.ToString() + GetSourceTag(url + episodeNumber.ToString(), sourcePriority);
                driver.Navigate().GoToUrl(urlFormated);

                //This looks for the video div, so far it looks like all the divs are named the same but they may not be for different sources
                //If they are not this should be moved into the corrisponding source's class
                var div = driver.Divs(By.Id("Videoads"));
                while (div.Count() < 1)
                    div = driver.Divs(By.Id("Videoads"));
                var doc = driver.Divs(By.Id("Videoads"))[0];
                string tempHTML = doc.InnerHtml;
                string videoUrlFormated = HTMLShredder(tempHTML);
                DataHold.VideoList.Enqueue(videoUrlFormated);
                if (!started)
                {
                    started = true;
                    //The source will need to be determined before setting the player
                    Player.setPlayer(Player.sources.ArkVid);
                }

                episodeNumber++;
            }
        }

        static public string GetSourceTag(string url, List<string> sourcePriority)
        {
            string data = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            string setter = "";
            foreach (string priority in sourcePriority)
            {
                if (data.Split(new string[] { priority }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                {
                    try {
                        string build = data.Split(new string[] { priority }, StringSplitOptions.RemoveEmptyEntries)[1];
                        build = build.Split(new string[] { "href=\"" }, StringSplitOptions.RemoveEmptyEntries).Last();
                        build = build.Split('"')[0].Split('?')[1];
                        setter = build;
                    }
                    catch
                    {

                    }
                    break;
                }
            }
            if (setter != "")
                setter = "?" + setter;
            return setter;
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
