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

namespace Anilinkz_Player.Classes
{
    class Page
    {
        ChromeDriver driver = null;
        public void GetData()
        {
            string Url = "";
            if (driver == null)
            {
                driver = new ChromeDriver();

                driver.Manage().Window.Position = new System.Drawing.Point(-2000, 0);
                driver.Navigate().GoToUrl(Url);
                System.Threading.Thread.Sleep(3000);

                var doc = driver.Divs(By.ClassName("container-fluid"))[0];
                string tempHTML = doc.InnerHtml;
                HTMLShredder(tempHTML);
            }
            else
            {
                var doc = driver.Divs(By.ClassName("container-fluid"))[0];
                string tempHTML = doc.InnerHtml;
                HTMLShredder(tempHTML);
            }
        }
    }
}
