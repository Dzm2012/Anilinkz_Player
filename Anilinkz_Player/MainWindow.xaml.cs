using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace Anilinkz_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            cbAnime.ItemsSource = getPageData().Keys;
        }

        static public Dictionary<string, string> getPageData()
        {
            Dictionary<string, string> nameURL = new Dictionary<string, string>();
            try
            {
                string urlAddress = "http://anilinkz.tv/anime-list";
                string data = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
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
                foreach (string chunk in data.Split(new string[] { "<li " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (chunk[0] != 'c')
                        continue;
                    else
                    {
                        string nameLIREMOVED = chunk.Split(new string[] { "<a href=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                        string number = nameLIREMOVED.Split('>')[2].Split('<')[0];

                        string name = nameLIREMOVED.Split('>')[1].Split('<')[0];
                        name += number;
                        string url = nameLIREMOVED.Split('>')[0];
                        url = url.Remove(0, 1);
                        url = url.Remove(url.Length - 1, 1);
                        foreach(string key in nameURL.Keys)
                        {
                            if (key == name)
                                name += " Duplicate";
                        }
                        nameURL.Add(name, url);
                    }
                }
                Classes.DataHold.AnimeList = nameURL;
            }
            catch(Exception e)
            {

            }
            return nameURL;
        }

        private void BTNstart_Click(object sender, RoutedEventArgs e)
        {
            Classes.Page.GetData(10);
        }

        
    }
}
