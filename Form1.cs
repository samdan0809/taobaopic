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
namespace taobaoSellPic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string dirName = "";

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
           
            string html = HttpSend.getSend(urls[inx],"",null,"UTF-8");
            inx++;
            Regex a = new Regex("style=\"background:url\\((\\S+)_\\S+\\)");
            Regex b = new Regex("<a\\s+\\S+\\s+\\S+\\s+\\S+\\s+\\S+>\\s+<span>(\\S+)</span>\\s+</a>");
            /*pic path*/
            MatchCollection mcs= a.Matches(html);
            foreach (Match item in mcs)
            {
                if (item.Success)
                {
                    pics.Add("http:" + item.Groups[1].Value + "");
                }
                
            }

            /*name*/
            MatchCollection NameMcs = b.Matches(html);
            foreach (Match item in NameMcs)
            {
                if (item.Success)
                {
                    names.Add("" + item.Groups[1].Value + "");
                }
            }
           
        }
        void out2File()
        {
            try
            {
                dirName = "图片";
                Directory.CreateDirectory(dirName);
                for (int i = 0; i < pics.Count; i++)
                {
                    System.Net.WebClient WC = new WebClient();
                    WC.DownloadFile(pics[i], path + "/" + dirName + "/" + "" + names[i] + ".jpg");
                    WC.Dispose();
                
                }
                isCatching = false;
           
            }
            catch (Exception)
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

        

        
    }
}
