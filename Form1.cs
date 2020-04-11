using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
namespace taobaoSellPic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string dirName = "图片";

        bool isCatching = false;
        List<string> urls = new List<string>();
        List<string> pics = new List<string>();
        List<string> names = new List<string>();
        int inx=0;
        //string tpl = "";
        string path = System.Environment.CurrentDirectory;
        private void button1_Click(object sender, EventArgs e)
        {
            urls = new List<string>();
            urls.AddRange(  textBox1.Text.Split("\r\n".ToCharArray()));
            inx=0;
            //tpl = NCShop.Common.Utils.ReadTextFile(path + "/tpl.db",System.Text.Encoding.UTF8);

            
            timer1.Start();
           
         
        }
        void start(){
            if (inx > urls.Count-1)
            {
                timer1.Stop();
                label1.Text = "好了";
                return;

            }
            if (urls[inx].Trim() == "")
            {
                inx++;
                return;
            }
            label1.Text = "正在抓第" + (inx + 1) + "个页面 ";

            isCatching = true;
            pics = new List<string>();
            names = new List<string>();

            HtmlWeb htmlWeb = new HtmlWeb();
            Regex styleRex = new Regex("(//.*?jpg)");
            Regex nameRex = new Regex("<span>(.*?)</span>");

            HtmlAgilityPack.HtmlDocument doc;

            try {
                doc=htmlWeb.LoadFromBrowser(urls[inx]);
            }
            catch
            {
                doc= htmlWeb.LoadFromBrowser(urls[inx]);

            }


            inx++;
            var  linkList= doc.DocumentNode.SelectNodes("//div[@class='tb-sku']/dl/dd/ul/li");

      
            foreach (var link in linkList)
            {
                Match item = styleRex.Match(link.InnerHtml);
                if (item.Success)
                {
                    pics.Add("https:" + item.Groups[1].Value + "");

                    Match item2 = nameRex.Match(link.InnerHtml);
                    if (item2.Success)
                    {
                        names.Add("" + item2.Groups[1].Value + "");
                    }
                }
               

            }
       

        }
        void out2File()
        {
            try
            {
               
                Directory.CreateDirectory(dirName);
                for (int i = 0; i < pics.Count; i++)
                {
                    System.Net.WebClient WC = new WebClient();
                    WC.DownloadFile(pics[i], path + "/" + dirName + "/" + "" + names[i] + ".jpg");
                 
                    WC.Dispose();
                
                }
                isCatching = false;
           
            }
            catch (Exception e)
            {

                isCatching = false;
            }
           
          
        }

       
        

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isCatching)
            {
                start();
                out2File();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe",System.Environment.CurrentDirectory+Path.DirectorySeparatorChar+ dirName);
        }
    }
}
