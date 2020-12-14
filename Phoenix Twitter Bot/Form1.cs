using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace Phoenix_Twitter_Bot
{
    public partial class Form1 : Form
    {

        Thread th,th2,th3;

        ChromeDriverService servis = ChromeDriverService.CreateDefaultService();


        public Form1()
        {
            InitializeComponent();

            //birden fazla thread kullanmamızı engeller eğer "True" olursa.
            CheckForIllegalCrossThreadCalls=false;

            //açılan konsol ekranını gizleme kodu
            
            servis.HideCommandPromptWindow = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                //threadı tanımla ve başlat
                th = new Thread(hizlibaslama);
                th.Start();

            }
            else
            {
                MessageBox.Show("Lütfen Gidilecek Adresi Yazın!");
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!="" && textBox2.Text!="" && textBox3.Text != "")
            {
                //threadı tanımla ve başlat
                th2 = new Thread(hizlibaslama2);
                th2.Start();
            }
            else
            {
                MessageBox.Show("Lütfen Tüm Kutucukları Doldurun!");
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                //threadı tanımla ve başlat
                th3 = new Thread(yazveara);
                th3.Start();

            }
            else
            {
                MessageBox.Show("Lütfen Tüm Kutuları Doldurun!");
            }

        }


        // hızlı başlatma metodu içinde yönlendirme bölümü
        private void hizlibaslama()
        {

            

            IWebDriver driver = new ChromeDriver(servis);

            //İstediğimiz sayfaya yönlendirme bölümü
            try
            {
                driver.Navigate().GoToUrl(textBox3.Text);
            }
            catch
            {
                MessageBox.Show("Bir hata meydana geldi.");
            }
            

        }

        private void hizlibaslama2()
        {
            //proxy ile hızlı başlama


            ChromeOptions proxy = new ChromeOptions();

            try
            {
                proxy.AddArgument("--proxy-server=" + textBox1.Text + ":" + textBox2.Text);
                IWebDriver driver2 = new ChromeDriver(servis,proxy); // servis ve proxy 

                driver2.Navigate().GoToUrl(textBox3.Text);

               

            }
            catch 
            {

                MessageBox.Show("Girdiğiniz adres hatalı veya proxy de sorun var!");
            }




        }

       

        private void yazveara()
        {

            try
            {
                //adrese gitme
                IWebDriver driver3 = new ChromeDriver(servis);
                driver3.Navigate().GoToUrl(textBox3.Text);

                Thread.Sleep(3000);//3 saniye yükleme süresi olsun ardından site açılsın


                //google da arama yaptırma bölümü
                IWebElement ara = driver3.FindElement(By.Name("q"));
                ara.SendKeys(textBox4.Text);

                

                //arama butonuna tıklama
                ara.FindElement(By.XPath("/html/body/div[2]/div[2]/form/div[2]/div[1]/div[3]/center/input[1]")).Click();

                

            }
            catch
            {

                MessageBox.Show("Girdiğiniz adres hatalı veya proxy de sorun var.");

            }

        }

       
    }
}
