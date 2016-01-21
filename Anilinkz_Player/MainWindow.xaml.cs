using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Anilinkz_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string episodeURL = "";
        public MainWindow()
        {
            InitializeComponent();
            cbAnime.ItemsSource = getAnimeListData().Keys;
        }

        static public Dictionary<string, string> getAnimeListData()
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
                        foreach (string key in nameURL.Keys)
                        {
                            if (key == name)
                                name += " Duplicate";
                        }
                        nameURL.Add(name, url);
                    }
                }
                Classes.DataHold.AnimeList = nameURL;
            }
            catch (Exception e)
            {

            }
            return nameURL;
        }

        private List<string> getSources()
        {
            Dictionary<string, string> nameURL = new Dictionary<string, string>();
            List<string> sources = new List<string>();
            try
            {
                string urlAddress = "http://anilinkz.tv" + Classes.DataHold.AnimeList[cbAnime.SelectedValue.ToString()];
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

                string front = data.Split(new string[] { "-episode-" }, StringSplitOptions.RemoveEmptyEntries)[0].Split('<')[data.Split(new string[] { "-episode-" }, StringSplitOptions.RemoveEmptyEntries)[0].Split('<').Length - 1];
                string end = data.Split(new string[] { "-episode-" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('>')[0];
                string complete = front.Split(new string[] { "href=\"" }, StringSplitOptions.RemoveEmptyEntries)[1] + "-episode-" + end.Remove(end.Length - 1, 1);


                urlAddress = "http://anilinkz.tv" + complete;
                episodeURL = "http://anilinkz.tv" + front.Split(new string[] { "href=\"" }, StringSplitOptions.RemoveEmptyEntries)[1] + "-episode-";
                data = "";

                request = (HttpWebRequest)WebRequest.Create(urlAddress);
                response = (HttpWebResponse)request.GetResponse();
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

                string section = data.Split(new string[] { "If the video above doesn't work, try a different video source below.</span>" }, StringSplitOptions.RemoveEmptyEntries)[1];
                section = section.Split(new string[] { "<br class=\"clr\"/>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                foreach (string chunk in section.Split('>'))
                {
                    if (chunk.Length < 1 || chunk[0] == ' ' || chunk[0] == '<')
                        continue;
                    sources.Add(chunk.Split('<')[0]);
                }
            }
            catch (Exception ex)
            {

            }
            return sources;
        }

        private void BTNstart_Click(object sender, RoutedEventArgs e)
        {
            List<string> priorityOrder = new List<string>();
            if(cbPriority1.SelectedValue!=null && cbPriority1.SelectedValue.ToString() != "")
                priorityOrder.Add(cbPriority1.SelectedValue.ToString());
            if (cbPriority2.SelectedValue != null && cbPriority2.SelectedValue.ToString() != "")
                priorityOrder.Add(cbPriority2.SelectedValue.ToString());
            if (cbPriority3.SelectedValue != null && cbPriority3.SelectedValue.ToString() != "")
                priorityOrder.Add(cbPriority3.SelectedValue.ToString());
            if (cbPriority4.SelectedValue != null && cbPriority4.SelectedValue.ToString() != "")
                priorityOrder.Add(cbPriority4.SelectedValue.ToString());
            if (cbPriority5.SelectedValue != null && cbPriority5.SelectedValue.ToString() != "")
                priorityOrder.Add(cbPriority5.SelectedValue.ToString());

            Classes.Page.GetData(10, 10, episodeURL, priorityOrder);
        }

        private void cbAnime_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<string> sources = getSources();
            List<string> temp = sources;
            for (int i = 0; i < sources.Count && i < 5; i++)
                temp[i] = sources[i];
            sources = temp;
            foreach (string source in sources)
            {
                ComboBox cb = new ComboBox();
                if (cbPriority1.Visibility == Visibility.Hidden)
                    cb = cbPriority1;
                else if (cbPriority2.Visibility == Visibility.Hidden)
                    cb = cbPriority2;
                else if (cbPriority3.Visibility == Visibility.Hidden)
                    cb = cbPriority3;
                else if (cbPriority4.Visibility == Visibility.Hidden)
                    cb = cbPriority4;
                else if (cbPriority5.Visibility == Visibility.Hidden)
                    cb = cbPriority5;
                cb.Visibility = Visibility.Visible;
                cb.ItemsSource = sources;
                cb.SelectedItem = source;
            }
        }
        private void Priority_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<ComboBox> cbList = new List<ComboBox>();
            if(cbPriority1.Items.Count>0)
                cbList.Add(cbPriority1);
            if (cbPriority2.Items.Count > 0)
                cbList.Add(cbPriority2);
            if (cbPriority3.Items.Count > 0)
                cbList.Add(cbPriority3);
            if (cbPriority4.Items.Count > 0)
                cbList.Add(cbPriority4);
            if (cbPriority5.Items.Count > 0)
                cbList.Add(cbPriority5);

            ComboBox Changed = new ComboBox();

            switch (((ComboBox)sender).Name)
            {
                case "cbPriority1":
                    cbList.Remove(cbPriority1);
                    Changed = cbPriority1;
                    break;
                case "cbPriority2":
                    cbList.Remove(cbPriority2);
                    Changed = cbPriority2;
                    break;
                case "cbPriority3":
                    cbList.Remove(cbPriority3);
                    Changed = cbPriority3;
                    break;
                case "cbPriority4":
                    cbList.Remove(cbPriority4);
                    Changed = cbPriority4;
                    break;
                case "cbPriority5":
                    cbList.Remove(cbPriority5);
                    Changed = cbPriority5;
                    break;
            }

            setPrioritys(cbList, Changed);
        }
        private void setPrioritys(List<ComboBox> Boxes, ComboBox changed)
        {
            ComboBox needsToChange = new ComboBox();
            foreach (ComboBox cb in Boxes)
            {
                if(cb.SelectedValue == changed.SelectedValue)
                {
                    //needsToChange = cb;
                    cb.SelectedValue = "";
                    break;
                }
            }
        }
    }
}
