using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using System.Net;
using Newtonsoft.Json;


//fix SOLD PROBLEM!
//ver. 1.0 23.08 - 23.10.17 
//ver. 2.0 add auto buy
//ver 2.1 add auto sell 
//ver 2.2 add file management, database management
//ver 3.0 cs:go

namespace SUPERPROJECT228322
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].ValueType = typeof(string);
            dataGridView1.Columns[1].Name = "CS.Money";
            dataGridView1.Columns[2].Name = "Loot";
            dataGridView1.Columns[3].Name = "CSGOTRADE";
            dataGridView1.Columns[4].Name = "CSgosell";
            dataGridView1.Columns[5].Name = "% dif";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                try
                {
                    dataGridView1[4, i].Value = Math.Round((Convert.ToDouble(dataGridView1[2, i].Value) * 0.87 - Convert.ToDouble(dataGridView1[1, i].Value)) /
                        Convert.ToDouble(dataGridView1[2, i].Value), 4);
                }
                catch
                {
                    dataGridView1[4, i].Value = "error";
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = @"http://steamcommunity.com/market/priceoverview/?country=US&currency=1&appid=570&market_hash_name={0}";
            for (int i = 0; i < 20; i++)
            {
                string itemName = Convert.ToString(dataGridView1[0, i].Value);
                string jsonURL = string.Format(url, itemName);
                try
                {
                    string json = new WebClient().DownloadString(jsonURL);
                    string pattern = @"([0-9]+.[0-9]*)";
                    Regex steamregex = new Regex(pattern);
                    Match matches = steamregex.Match(json);
                    double a = Convert.ToDouble(matches.Groups[1].Value.Replace(".", ","));
                    dataGridView1[5, i].Value = Convert.ToDouble(Math.Round((a * 0.87 - Convert.ToDouble(dataGridView1[1, i].Value)) / a, 4));
                    System.Threading.Thread.Sleep(200);
                }
                catch
                {
                    dataGridView1[5, i].Value = 0;
                }
            }
        }




        private void button2_Click(object sender, EventArgs e)
        {

            OpenQA.Selenium.Chrome.ChromeOptions options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.AddArguments(@"user-data-dir=C:\Users\DAle\AppData\Local\Google\Chrome\User Data\Default");
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            //Browser.Manage().Window.Maximize();

            /*IWebElement itemsearch;
            itemsearch = this.Browser.FindElement(By.ClassName("item"));
            var action = new OpenQA.Selenium.Interactions.Actions(this.Browser);
            action.ContextClick(itemsearch);
            action.Perform();*/
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(1000); //was 500
            Browser.Navigate().GoToUrl("https://dota.money");
            System.Threading.Thread.Sleep(6000);
            /*IWebElement USD;
            USD = Browser.FindElement(By.LinkText("USD"));
            USD.Click();
            System.Threading.Thread.Sleep(1000);
            USD = Browser.FindElement(By.LinkText("RUB"));
            USD.Click();*/
            System.Threading.Thread.Sleep(3000);
            Cursor.Position = new Point(1408, 794);
            mouse_event((int)(MouseEventFlags.MIDDLEDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(500);
            Cursor.Position = new Point(1408, 1260);
            System.Threading.Thread.Sleep(22000);
            mouse_event((int)(MouseEventFlags.MIDDLEUP), 0, 0, 0, 0);
            dataGridView1.Columns[1].Name = "Dota.Money";
            dataGridView1.Columns[2].Name = "Steam";
            dataGridView1.Columns[3].Name = "Diff";
            dataGridView1.Columns[4].Name = "%%";
            dataGridView1.Columns[5].Name = "steamprice";
            List<IWebElement> Items2 = Browser.FindElements(By.CssSelector("#inventory_bots div.item")).ToList();
            int m = 0;
            foreach (var element in Items2)
            {
                try
                {
                    string hash = element.GetAttribute("market_hash_name");
                    if ((m <= 0 || hash != Convert.ToString(dataGridView1[0, m - 1].Value)) && hash != "Taunt: Party On? Party Off!")
                    {

                        System.Threading.Thread.Sleep(300);
                        string cost = element.GetAttribute("cost");
                        double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                        dataGridView1.Rows.Add(hash, a, 0, 0, 0, 0);
                        string url = @"http://api.steamapis.com/market/item/570/{0}?api_key={1}";
                        //int appID = 570;
                        string api_key;
                        if (m % 2 != 0)
                            api_key = "teJlN3fVPVf0VHclNqnKqv0M";
                        else
                            api_key = "VbL2nE5MXl-eh6UaArhJgkZQ";
                        string itemName = hash;
                        string jsonURL = string.Format(url, itemName, api_key);
                        double b;
                        try
                        {
                            string json = new WebClient().DownloadString(jsonURL);
                            string pattern1 = @"(?:lowest_sell_order"":)([0-9]+\.?[0-9]*)";
                            Regex steamregex = new Regex(pattern1);
                            Match matches = steamregex.Match(json);
                            b = Convert.ToDouble(matches.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            b = 0;
                        }
                        dataGridView1[2, m].Value = b;
                        dataGridView1[3, m].Value = Math.Round(Convert.ToDouble(dataGridView1[2, m].Value) * 0.87 - Convert.ToDouble(dataGridView1[1, m].Value), 4);
                        m++;
                    }
                }
                catch
                {
                }
                System.Threading.Thread.Sleep(300);
            }
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                try
                {
                    dataGridView1[4, i].Value = Math.Round((Convert.ToDouble(dataGridView1[2, i].Value) * 0.87 - Convert.ToDouble(dataGridView1[1, i].Value)) /
                        Convert.ToDouble(dataGridView1[2, i].Value), 4);
                }
                catch
                {
                    dataGridView1[4, i].Value = "error";
                }


        }

        /*if (n == 0 || hash != Convert.ToString(dataGridView1[0, m - 1].Value))
        {
            string cost = element.GetAttribute("cost");
            double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
            dataGridView1.Rows.Add(hash, a, 0, 0, 0, 0);
            IWebElement localitem = Items2[n];
            var action = new OpenQA.Selenium.Interactions.Actions(this.Browser);
            action.ContextClick(localitem);
            action.Perform();
            System.Threading.Thread.Sleep(300);
            IWebElement insteam = Browser.FindElement(By.LinkText("IN STEAM"));
            insteam.Click();
            System.Threading.Thread.Sleep(300);
            Browser.SwitchTo().Window(Browser.WindowHandles[1]);
            System.Threading.Thread.Sleep(200);
            IWebElement steamprice;
            steamprice = Browser.FindElement(By.CssSelector("#searchResultsRows span span"));
            string steamstr = Convert.ToString(steamprice.Text);
            int i = 1;
            while(steamstr == "Продано!")
            {
                List<IWebElement> steamprice1 = Browser.FindElements(By.CssSelector("div.market_listing_right_cell.market_listing_their_price > span > span.market_listing_price.market_listing_price_with_fee")).ToList();
                steamstr = Convert.ToString(steamprice1[i].Text);
                i++;
            }
            string steampattern = @"([0-9]+\,?[0-9]*)";
            Regex steamregex = new Regex(steampattern);
            Match matches = steamregex.Match(steamstr);
            foreach (Match steammatch in steamregex.Matches(steamstr))                   
                dataGridView1[2, m].Value = Convert.ToDouble(steammatch.Groups[1].Value);
            dataGridView1[3, m].Value = Math.Round(Convert.ToDouble(dataGridView1[2, m].Value) * 0.87 - Convert.ToDouble(dataGridView1[1, m].Value), 4);
            Browser.Close();
            Browser.SwitchTo().Window(Browser.WindowHandles[0]);
            m++;
            /*string url = @"http://steamcommunity.com/market/listings/570/itemName={0}";
            //int appID = 570;
            string itemName = hash;
            string jsonURL = string.Format(url, itemName);
            string json = new WebClient().DownloadString(jsonURL);
            string pattern1 = @"([0-9]+,[0-9]*)";
            Regex steamregex = new Regex(pattern1);
            Match matches = steamregex.Match(json);
            double b = Convert.ToDouble(matches.Groups[1].Value);
            dataGridView1[2, m].Value = b;
        }*/



        /*for (int i = 0; i < Items2.Count; i++, n++)
        {
            IWebElement localitem = Items2[i];
            var action = new OpenQA.Selenium.Interactions.Actions(this.Browser);
            action.ContextClick(localitem);
            action.Perform();
            System.Threading.Thread.Sleep(300);
            IWebElement insteam = Browser.FindElement(By.LinkText("IN STEAM"));
            insteam.Click();
            System.Threading.Thread.Sleep(300);
            Browser.SwitchTo().Window(Browser.WindowHandles[1]);
            System.Threading.Thread.Sleep(200);
            IWebElement steamprice = Browser.FindElement(By.CssSelector("#searchResultsRows span span"));
            string steamstr = Convert.ToString(steamprice.Text);
            string steampattern = @"([0-9]+\.?[0-9]*)";
            Regex steamregex = new Regex(steampattern);
            Match matches = steamregex.Match(steamstr);
            foreach (Match steammatch in steamregex.Matches(steamstr))
            {
                dataGridView1[2,i].Value = Convert.ToDouble(steammatch.Groups[1].Value);
            }
            dataGridView1[3, i].Value = Convert.ToDouble(dataGridView1[2, i].Value)*0.87 - Convert.ToDouble(dataGridView1[1, i].Value);
            Browser.Close();
            Browser.SwitchTo().Window(Browser.WindowHandles[0]);
        }*/




        /*for (int i = 0; i < Items.Count; i++)
        {
            pricelist += Items[i].Text + " ";
        }


        string[] str = new string[Items.Count];
        string pattern = @"([0-9]+\.?[0-9]*)";
        Regex regex = new Regex(pattern);
        foreach (Match match in regex.Matches(pricelist))
        {
            for (int i = 0; i < Items.Count; i++)
                str[i] = match.Groups[1].Value;
            textBox1.Text += match.ToString() + "\r\n";
        }

        double[] arr = new double[Items.Count];

        string[] names = new string[textBox1.Lines.Count() - 1];
        for (int i = 0; i < names.Length; i++)
            names[i] = textBox1.Lines[i];

        for (int i = 0; i < names.Length; i++)
        {
            textBox1.Text += names[i];
        }

        for (int i = 0; i < Items.Count; i++)
        {
            arr[i] = double.Parse(names[i].Replace(".", ","));
        }
        int n = 0;

        textBox1.Clear(); 

         /*for (int i = 0; i < arr.Length; i++) RESULT!!!!!!!!!!!!!!!!!!!
         {
             textBox1.Text += arr[i] + "\r\n";
         }*/



        /*for (int i = 0; i < arr.Length; i++, n++)
        {
            IWebElement localitem;
            localitem = Items[i];
            var action = new OpenQA.Selenium.Interactions.Actions(this.Browser);
            action.ContextClick(localitem);
            action.Perform();
            System.Threading.Thread.Sleep(300);
            IWebElement insteam;
            insteam = Browser.FindElement(By.LinkText("IN STEAM"));
            insteam.Click();
            System.Threading.Thread.Sleep(300);
            Browser.SwitchTo().Window(Browser.WindowHandles[1]);
            System.Threading.Thread.Sleep(200);
            IWebElement steamprice;
            steamprice = Browser.FindElement(By.CssSelector("#searchResultsRows span span"));
            textBox1.Text += steamprice.Text + "\r\n";
            string steamstr = textBox1.Lines[0];
            string steampattern = @"([0-9]+\.?[0-9]*)";
            Regex steamregex = new Regex(steampattern);
            Match matches = steamregex.Match(steamstr);
            textBox1.Text += matches.ToString() + "\r\n";
            foreach (Match steammatch in steamregex.Matches(steamstr))
            {               
                steamstr = steammatch.Groups[1].Value;
                //textBox1.Text += steammatch.ToString() + "\r\n";
            }
            textBox1.Text += steamstr + "\r\n";
            string steamnames = "";
            if (textBox1.Lines[1] != "Продано!")
                steamnames = textBox1.Lines[1];
            else
                break;
            double steamarr = 0;

            steamarr = double.Parse(steamnames.Replace(".", ","));
            if (arr[n] * 1.15 < steamarr)
                textBox2.Text += steamarr + " > " + arr[n] + "\r\n";
            textBox1.Clear();
            System.Threading.Thread.Sleep(400);
            Browser.Close();
            Browser.SwitchTo().Window(Browser.WindowHandles[0]);
            while (arr[n] == arr[n + 1])
            {
                n++;
                i++;
            }
        }*/



        private void button5_Click(object sender, EventArgs e)
        {
            Browser.Quit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }
        public void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //mouse_event(MouseEventFlags.MOVE | MouseEventFlags.ABSOLUTE, x * 65536 / 1366, y * 65536 / 768); //for notebook
            System.Threading.Thread.Sleep(6000);
            for (int k = 0; k < (int)numericUpDown1.Value; k++)
            {
                int x = 525;
                int y = 360;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Cursor.Position = new Point(x, y);
                        mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                        System.Threading.Thread.Sleep(100);
                        mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                        System.Threading.Thread.Sleep(100);
                        mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                        System.Threading.Thread.Sleep(100);
                        mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                        System.Threading.Thread.Sleep(100);
                        x += 100;
                    }
                    x -= 400;
                    y += 100;
                }
                y -= 400;
                Cursor.Position = new Point(873, 753);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                System.Threading.Thread.Sleep(50);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                System.Threading.Thread.Sleep(500);
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);

            this.Cursor = new Cursor(Cursor.Current.Handle);
            textBox1.Text += Cursor.Position.X + "   " + Cursor.Position.Y + "\r\n";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            for (int k = 0; k < (int)numericUpDown1.Value; k++)
            {
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                System.Threading.Thread.Sleep(100);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                System.Threading.Thread.Sleep(150);
                mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
                System.Threading.Thread.Sleep(100);
                mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
                System.Threading.Thread.Sleep(400);
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            Browser.Navigate().GoToUrl("http://cs.money/");
            System.Threading.Thread.Sleep(3000);
            Cursor.Position = new Point(925, 820);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(150);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(2000);
            Cursor.Position = new Point(1481, 601);
            mouse_event((int)(MouseEventFlags.MIDDLEDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(500);
            Cursor.Position = new Point(1481, 1260);
            System.Threading.Thread.Sleep(20000);
            mouse_event((int)(MouseEventFlags.MIDDLEUP), 0, 0, 0, 0);

            List<IWebElement> CSItems = Browser.FindElements((By.CssSelector("html body div div div div div #block-desktop-bot #block-items-bot div #inventory_bot div.item"))).ToList();
            /*for (int i = 0; i < CSItems.Count; i++)
            {              
                dataGridView1.Rows.Add(CSItems[i].Text);
                
            }*/
            int l = 0;
            foreach (var element in CSItems)
            {
                string hash = element.GetAttribute("hash");
                if (l == 0)
                {
                    dataGridView1.Rows.Add(hash, 0, 0, 0);
                    string cost = element.GetAttribute("cost");
                    double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                    dataGridView1[1, l].Value = a;
                    l++;
                }
                else if (hash != Convert.ToString(dataGridView1[0, l - 1].Value))
                {
                    dataGridView1.Rows.Add(hash, 0, 0, 0);
                    string cost = element.GetAttribute("cost");
                    double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                    dataGridView1[1, l].Value = a;
                    l++;
                }
                /*string cost = element.GetAttribute("cost");
                Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                dataGridView1[1, l].Value = cost;*/

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Browser.Navigate().GoToUrl("https://loot.farm/ru/index.html");
            System.Threading.Thread.Sleep(3000);
            Cursor.Position = new Point(1408, 794);
            mouse_event((int)(MouseEventFlags.MIDDLEDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(500);
            Cursor.Position = new Point(1408, 1260);
            System.Threading.Thread.Sleep(100000);
            mouse_event((int)(MouseEventFlags.MIDDLEUP), 0, 0, 0, 0);

            List<IWebElement> CSItems2 = Browser.FindElements((By.CssSelector("#bots_inv div div div b"))).ToList();
            double[] pricelist = new double[CSItems2.Count];
            string[] str = new string[CSItems2.Count];
            string pattern = @"([0-9]+\.?[0-9]*)";
            Regex regex = new Regex(pattern);
            for (int i = 0; i < CSItems2.Count; i++)
            {
                foreach (Match match in regex.Matches(CSItems2[i].Text))
                {
                    pricelist[i] = Convert.ToDouble(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            /*for (int i = 0; i < CSItems2.Count; i++)
            {
                textBox1.Text += CSItems2[i].Text + "\r\n";
                pricelist[i]=Convert.ToDouble(textBox1.Lines[i].Replace(".", ","));
            }*/
            List<IWebElement> CSItems = Browser.FindElements((By.CssSelector("#bots_inv div div.itemblock"))).ToList();
            int j = 0;
            int m = dataGridView1.RowCount;
            foreach (var element in CSItems)
            {
                string hash = element.GetAttribute("data-name");
                for (int i = 0; i < m; i++)
                {
                    if (hash == Convert.ToString(dataGridView1[0, i].Value))
                    {
                        dataGridView1[2, i].Value = pricelist[j];
                        break;
                    }
                    else if ((i == m - 2) && hash != Convert.ToString(dataGridView1[0, dataGridView1.RowCount - 2].Value))//all right!
                    {
                        dataGridView1.Rows.Add(hash, 0, pricelist[j], 0);
                        break;
                    }
                }
                j++;
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            Browser.Navigate().GoToUrl("https://www.csgotrade.me/");
            System.Threading.Thread.Sleep(8000);
            Cursor.Position = new Point(1400, 750);
            mouse_event((int)(MouseEventFlags.MIDDLEDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(500);
            Cursor.Position = new Point(1400, 1260);
            System.Threading.Thread.Sleep(60000);
            mouse_event((int)(MouseEventFlags.MIDDLEUP), 0, 0, 0, 0);

            List<IWebElement> CSItems = Browser.FindElements(By.CssSelector("#bot_inventory div.images")).ToList();
            /*for (int i = 0; i < CSItems.Count; i++)
            {              
                dataGridView1.Rows.Add(CSItems[i].Text);
                
            }*/
            int m = dataGridView1.RowCount;
            foreach (var element in CSItems)
            {
                string hash = element.GetAttribute("market_hash_name");
                for (int i = 0; i < m - 1; i++)
                {
                    if (hash == Convert.ToString(dataGridView1[0, i].Value))
                    {
                        string cost = element.GetAttribute("cost");
                        double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                        dataGridView1[3, i].Value = a;
                        //textBox1.Text += hash + "\r\n " + Convert.ToString(dataGridView1[0, i].Value) + "\r\n";
                        break;
                        /*if (l == 0)
                        {
                            dataGridView1.Rows.Add(hash);
                            string cost = element.GetAttribute("cost");
                            double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                            dataGridView1[2, l].Value = a;
                            l++;
                        }
                        if (l > 0 && hash != Convert.ToString(dataGridView1[0, l - 1].Value))
                        {
                            dataGridView1.Rows.Add(hash);
                            string cost = element.GetAttribute("cost");
                            double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                            dataGridView1[2, l].Value = a;
                            l++;
                        }*/
                    }
                    else if ((i == m - 2) && hash != Convert.ToString(dataGridView1[0, dataGridView1.RowCount - 2].Value))
                    {
                        //textBox1.Text += hash + "\r\n " + Convert.ToString(dataGridView1[0,dataGridView1.RowCount - 2].Value) + "\r\n";
                        string cost = element.GetAttribute("cost");
                        double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                        dataGridView1.Rows.Add(hash, 0, 0, a);
                        break;
                    }
                    /*string cost = element.GetAttribute("cost");
                    Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                    dataGridView1[1, l].Value = cost;*/

                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Browser.Navigate().GoToUrl("https://csgosell.com/");
            System.Threading.Thread.Sleep(4000);
            Cursor.Position = new Point(1400, 750);
            mouse_event((int)(MouseEventFlags.MIDDLEDOWN), 0, 0, 0, 0);
            System.Threading.Thread.Sleep(500);
            Cursor.Position = new Point(1400, 1260);
            System.Threading.Thread.Sleep(50000);
            mouse_event((int)(MouseEventFlags.MIDDLEUP), 0, 0, 0, 0);

            List<IWebElement> CSItems = Browser.FindElements(By.CssSelector("#botsIv div.itemImgDiv")).ToList();

            int m = dataGridView1.RowCount;
            foreach (var element in CSItems)
            {
                string hash = element.GetAttribute("original-name");
                for (int i = 0; i < m - 1; i++)
                {
                    if (hash == Convert.ToString(dataGridView1[0, i].Value))
                    {
                        string cost = element.GetAttribute("value");
                        double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                        dataGridView1[4, i].Value = a;
                        break;

                    }
                    else if ((i == m - 2) && hash != Convert.ToString(dataGridView1[0, dataGridView1.RowCount - 2].Value))
                    {
                        string cost = element.GetAttribute("value");
                        double a = Convert.ToDouble(cost, System.Globalization.CultureInfo.InvariantCulture);
                        dataGridView1.Rows.Add(hash, 0, 0, 0, a);
                        break;
                    }
                }

            }

        }
        double Min(double a, double b)
        {
            if ((a < b ? a : b) > 0)
            {
                return a < b ? a : b;
            }
            else
                return a > b ? a : b;

        }

        private void button13_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView1[4, i].Value = Convert.ToDouble(0);
                double max = 0, min = 0;
                max = Math.Max(Convert.ToDouble(dataGridView1[1, i].Value), Math.Max(Convert.ToDouble(dataGridView1[2, i].Value), Convert.ToDouble(dataGridView1[3, i].Value)));
                if (max > 0)
                {
                    min = Min(Convert.ToDouble(dataGridView1[1, i].Value), Min(Convert.ToDouble(dataGridView1[2, i].Value), Convert.ToDouble(dataGridView1[3, i].Value)));
                    if (min > 0)
                    {
                        if (max > 0 && min > 0 && max-min > 0.01)
                            dataGridView1[4, i].Value = Math.Round((max - min) / max, 5);
                    }

                }                   

            }

        }

        private void button14_Click(object sender, EventArgs e) //autobuy
        {
            button1_Click(button1, null);
            for (int i = 0; i < 20; i++)
                if(Convert.ToDouble(dataGridView1[5, i].Value) > 0.095)
                {

                }
                  
        }

        private void button15_Click(object sender, EventArgs e) //autosell
        {
            Browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            Browser.Navigate().GoToUrl("http://steamcommunity.com/id/dale-/inventory/");
            var sort = Browser.FindElement(By.CssSelector("#filter_tag_show span"));
            //IWebElement sort = Browser.FindElement(By.XPath("//[contains(text(),'Показать доп. фильтры')]")); //*[@id="filter_tag_show"]/span/text()[contains(.,'Показать доп. фильтры')] //body/a[text()[contains(.,'Ссылка')]]
            sort.Click();
            System.Threading.Thread.Sleep(1000);
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_misc_marketable"));
            sort.Click();
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_Quality_unique"));
            sort.Click();
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_Rarity_Rarity_Common"));
            sort.Click();
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_Rarity_Rarity_Uncommon"));
            sort.Click();
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_Rarity_Rarity_Rare"));
            sort.Click();
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_Rarity_Rarity_Mythical"));
            sort.Click();
            sort = Browser.FindElement(By.CssSelector("#tags_76561198108881200_570_2 div.econ_tag_filter_collapsable_tags_showlink.whiteLink"));
            sort.Click();
            System.Threading.Thread.Sleep(300);
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_Type_wearable"));
            sort.Click();
            sort = Browser.FindElement(By.CssSelector("#tag_filter_570_2_Type_bundle"));
            sort.Click();
            System.Threading.Thread.Sleep(10000);

            var pages = Browser.FindElement(By.CssSelector("#pagebtn_next"));
            while (pages.GetAttribute("class") == "pagecontrol_element pagebtn")
            {
                List<IWebElement> itemimage = Browser.FindElements(By.ClassName("itemHolder")).ToList(); //style = "display: block;" , By.XPath("//input[@style='display: block']"))
                var count = 0;
                foreach (var element in itemimage)
                {
                    if (count > 24)
                    {
                        break;
                    }
                    string hash = element.GetAttribute("style");
                    if (hash == "display: block;")
                    {
                        element.Click();
                        count++;
                        System.Threading.Thread.Sleep(500);

                        /*List<IWebElement> itempricelist = Browser.FindElements(By.XPath("#listings_group div table tbody tr td")).ToList(); //#listings_group div table tbody tr td //*[@id='listings_group']/div[1]/table/tbody/tr[2]/td[1]
                        foreach (var element2 in itempricelist)
                            Console.WriteLine(itempricelist);
                        count++;
                        System.Threading.Thread.Sleep(500);*/
                    }

                }
                pages.Click();
                System.Threading.Thread.Sleep(1000);
            }
            //market_commodity_orders_table
        }
    }
}


