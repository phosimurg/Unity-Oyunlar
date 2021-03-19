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
using System.IO;

namespace Phoenix_Twitter_Bot
{
    public partial class AnaSayfa : Form
    {


        Thread th, th2;

        ChromeDriverService servis = ChromeDriverService.CreateDefaultService();


        //Kullanıcı Bilgileri Okuma Bölümü
        int poz = 0, uzunluk = 0;
        string Kadi = "", sifre = "", metin = "", metinsirasi = "";

        //oturum bölümü
        int ayniandakioturum = 0, toplamoturum = 0, toplamtweet = 0, acilanoturumsayisi = 0;

        public AnaSayfa()
        {
            InitializeComponent();

            //birden fazla thread kullanmamızı engeller eğer "True" olursa.
            CheckForIllegalCrossThreadCalls = false;

            //açılan konsol ekranını gizleme kodu

            servis.HideCommandPromptWindow = true;


        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {

            //kullanıcı listesi eğer boş değilse ilk sıradaki metni seç
            if (KullaniciListe.Text != "")
            {
                KullaniciListe.SelectedIndex = 0;
            }



        }



        private void ChkAdresCubugu_CheckedChanged(object sender)
        {
            if (ChkAdresCubugu.Checked)
            {
                TxtAdresCubugu.Enabled = true;
            }
            else
            {
                TxtAdresCubugu.Enabled = false;

                TxtAdresCubugu.Text = "https://twitter.com/login";
            }
        }

        private void ChkProxyKullan_CheckedChanged(object sender)
        {
            if (ChkProxyKullan.Checked)
            {
                TxtProxy.Enabled = true;
                TxtPort.Enabled = true;
            }
            else
            {
                TxtProxy.Enabled = false;
                TxtPort.Enabled = false;

                TxtProxy.Text = "";
                TxtPort.Text = "";
            }

        }

        private void BtnTweetTemizle_Click(object sender, EventArgs e)
        {
            TweetList.Clear();
        }

        private void BtnProxyListTemizle_Click(object sender, EventArgs e)
        {
            ProxyList.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (TxtSifre.UseSystemPasswordChar == true)
            {
                TxtSifre.UseSystemPasswordChar = false;
                button3.Text = "Gizle";
            }
            else
            {
                TxtSifre.UseSystemPasswordChar = true;
                button3.Text = "Göster";
            }
        }

        private void BtnKullaniciEkle_Click(object sender, EventArgs e)
        {

            //kullanıcı listesine kullanıcı txt dosyası çektik.
            openFileDialog1.Filter = "Txt Dosyası |*.txt";
            openFileDialog1.ShowDialog();

            StreamReader st = File.OpenText(openFileDialog1.FileName);




            while ((metin = st.ReadLine()) != null)
            {
                KullaniciListe.Items.Add(metin);
            }


            st.Close();

            //kullanıcı listesinde ki ilk sıradaki metni seç
            if (KullaniciListe.Items != null)
            {
                KullaniciListe.SelectedIndex = 0;
            }


        }

        private void AAAOSNumericUPDOWN_Click(object sender, EventArgs e)
        {
            if (AAAOSNumericUPDOWN.Value > TOSNumericUPDOWN.Value)
            {
                AAAOSNumericUPDOWN.Value = TOSNumericUPDOWN.Value;
                MessageBox.Show("Toplam Oturum Sayısı Yetersiz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }


        }

        private void TOSNumericUPDOWN_Click(object sender, EventArgs e)
        {
            if (AAAOSNumericUPDOWN.Value > TOSNumericUPDOWN.Value)
            {
                AAAOSNumericUPDOWN.Value = TOSNumericUPDOWN.Value;
                MessageBox.Show("Toplam Oturum Sayısı Yetersiz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void BtnBaslat_Click(object sender, EventArgs e)
        {

            //proxy kullan aktif değilse ProxysizBasla() ya gitsin. Değilse; ProxysiliBasla() ya gitsin
            if (ChkProxyKullan.Checked == false)
            {
                /////////////////////////
                //LİSTESİZ PROXYSİZ BAŞLA
                /////////////////////////


                //Tek giriş aktif ise işlemleri yap
                if (Radio1.Checked)
                {
                    //kutucuklar boş değilse işlemleri yap
                    if (TxtAdresCubugu.Text != "")
                    {
                        //threadı tanımla ve başlat
                        th = new Thread(ListesizProxysizBasla);
                        th.Start();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Gidilecek Adresi Yazın!");
                    }


                }

                else
                {
                    /////////////////////////
                    //LİSTELİ PROXYSİZ BAŞLA
                    /////////////////////////

                    toplamoturum = Convert.ToInt32(TOSNumericUPDOWN.Value);
                    ayniandakioturum = Convert.ToInt32(AAAOSNumericUPDOWN.Value);


                    //kutucuklar boş değilse işlemleri yap
                    if (TxtAdresCubugu.Text != "")
                    {

                        if (ayniandakioturum == 1)
                        {
                            //AYNI ANDAKİ OTURUM SAYISI 1 İSE 
                            //toplam oturum sayısı kadar sıra sıra açılacak
                            for (int i = 0; i < toplamoturum; i++)
                            {
                                //threadı tanımla ve başlat
                                th = new Thread(ListeliProxysizBasla);
                                th.Start();
                            }

                        }
                        else
                        {
                        //AYNI ANDAKİ OTURUM SAYISI 1 DEN FAZLAYSA 
                        //

                             //oturum sayısı tamamlana kadar dön
                             basadon:
                            if (acilanoturumsayisi < toplamoturum)
                            {



                                for (int i = 0; i < ayniandakioturum; i++)
                                {



                                    //threadı tanımla ve başlat
                                    th = new Thread(ListeliProxysizBasla);
                                    th.Start();

                                    //th2 = new Thread(ListeliProxysizBasla);
                                    //th2.Start();

                                    //oturum saysıı hesaplama
                                    acilanoturumsayisii();

                                    goto basadon;
                                }
                            }
                            //oturumlar hedefe ulaştıysa
                            else
                            {
                                MessageBox.Show("Oturumlar Hedefe Ulaştı..", "Bilgi..", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }


                        }

                    }
                    else
                    {
                        MessageBox.Show("Lütfen Gidilecek Adresi Yazın!");
                    }

                }

            }
            else
            {

                if (TxtAdresCubugu.Text != "")
                {
                    //threadı tanımla ve başlat
                    th = new Thread(ListesizProxysiliBasla);
                    th.Start();

                }
                else
                {
                    MessageBox.Show("Lütfen Gidilecek Adresi Yazın!");
                }
            }



        }

        private void acilanoturumsayisii()
        {

            acilanoturumsayisi++;
            NotifiAcilanOturum.Value = acilanoturumsayisi;


            if (acilanoturumsayisi == toplamoturum)
            {
                acilanoturumsayisi = 0;
                MessageBox.Show("Oturumlar Hedefe Ulaştı..", "Bilgi..", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

        private void ListesizProxysizBasla()
        {

            IWebDriver driver = new ChromeDriver(servis);

            //İstediğimiz sayfaya yönlendirme bölümü
            try
            {
                //hedef adrese git
                driver.Navigate().GoToUrl(TxtAdresCubugu.Text);



                //kullanıcı adı veya eposta gir
                IWebElement TxtKadiAra = driver.FindElement(By.Name("session[username_or_email]"));
                TxtKadiAra.SendKeys(TxtKadi.Text);

                //şifre gir
                IWebElement TxtSifreAra = driver.FindElement(By.Name("session[password]"));
                TxtSifreAra.SendKeys(TxtSifre.Text);

                //giriş yapma butonuna bas
                driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/main/div/div/div[1]/form/div/div[3]/div/div/span/span")).Click();

            }
            catch
            {
                MessageBox.Show("Bir hata meydana geldi.");
            }

        }


        private void ListeliProxysizBasla()
        {



            try
            {




                IWebDriver driver = new ChromeDriver(servis);


                //Kullanıcı listesinde, seçilen metinden kullanıcıadı ve şifre al
                metinsirasi = KullaniciListe.SelectedItems[0].ToString();

                if (metinsirasi != null)
                {
                    uzunluk = metinsirasi.Length;
                }
                poz = metinsirasi.IndexOf(",", 0, uzunluk);
                Kadi = metinsirasi.Substring(0, poz);
                sifre = metinsirasi.Substring(poz + 1, uzunluk - (poz + 1));


                //hedef adrese git
                driver.Navigate().GoToUrl(TxtAdresCubugu.Text);


                //kullanıcı adı veya eposta gir
                IWebElement TxtKadiAra = driver.FindElement(By.Name("session[username_or_email]"));
                TxtKadiAra.SendKeys(Kadi);

                //şifre gir
                IWebElement TxtSifreAra = driver.FindElement(By.Name("session[password]"));
                TxtSifreAra.SendKeys(sifre);

                //giriş yapma butonuna bas
                driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/main/div/div/div[1]/form/div/div[3]/div/div/span/span")).Click();


                //açıldıktan sonra 1 saniye bekle ve ardından oturumu kapat
                Thread.Sleep(1000);
                driver.Quit();

                //kullanıcı listesindeki sırayı 1 arttır
                KullaniciListe.SelectedIndex++;

            }
            catch
            {
                MessageBox.Show("Bir hata meydana geldi.");
            }





        }

        private void ListesizProxysiliBasla()
        {


            //proxy ile hızlı başlama


            ChromeOptions proxy = new ChromeOptions();

            try
            {
                proxy.AddArgument("--proxy-server=" + TxtProxy.Text + ":" + TxtPort.Text);
                IWebDriver driver2 = new ChromeDriver(servis, proxy); // servis ve proxy 

                driver2.Navigate().GoToUrl(TxtAdresCubugu.Text);



            }
            catch
            {

                MessageBox.Show("Girdiğiniz adres hatalı veya proxy de sorun var!");
            }




        }

        private void Radio1_CheckedChanged(object sender)
        {
            if (Radio1.Checked == true)
            {

                //textboxları aç
                TxtKadi.Enabled = true;
                TxtSifre.Enabled = true;

                //kullanıcı butonlarını kapat
                BtnKullaniciEkle.Enabled = false;
                BtnKullaniciTemizle.Enabled = false;


                //numeric up-downları aç
                AAAOSNumericUPDOWN.Enabled = false;
                ATSNumericUPDOWN.Enabled = false;
                TOSNumericUPDOWN.Enabled = false;

            }
            else
            {
                //textboxları kapat
                TxtKadi.Enabled = false;
                TxtSifre.Enabled = false;

                //kullanıcı butonlarını aç
                BtnKullaniciEkle.Enabled = true;
                BtnKullaniciTemizle.Enabled = true;


                //numeric up-downları kapat
                AAAOSNumericUPDOWN.Enabled = true;
                ATSNumericUPDOWN.Enabled = true;
                TOSNumericUPDOWN.Enabled = true;
            }

        }


        private void BtnKullaniciTemizle_Click(object sender, EventArgs e)
        {
            KullaniciListe.Items.Clear();



        }


        private void oturumayarlari()
        {
            /////////////////
            //OTURUM AYARLARI
            /////////////////



            try
            {

                if (AAAOSNumericUPDOWN.Value <= 0 && ATSNumericUPDOWN.Value <= 0 && TOSNumericUPDOWN.Value <= 0)
                {
                    MessageBox.Show("Lütfen Tüm Değerleri Doğru Girin!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    toplamoturum = Convert.ToInt32(TOSNumericUPDOWN.Value);
                    toplamtweet = Convert.ToInt32(ATSNumericUPDOWN.Value);
                    ayniandakioturum = Convert.ToInt32(AAAOSNumericUPDOWN.Value);


                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bir hata meydana geldi..");
            }


        }

    }
}
