using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        bool opertorCase = false;           // Operator durumu. Hesap makinesinin o an bir işlem yapıp yapmadığını kontrol eder.
        double resultNumber = 0;            // İşlem sonucu elde edilen değer.
        string operatorSymbol = "";         // Yapılan işlem.
        string reserveOpSym = "";           // Yapılan işlem bilgisini sonraki işlemde kullanmak için tutar.
        double reserveResult = 0;           // Elde edilen değeri sonraki işlemde kullanmak için tutar.
        string reserveNum;                  // İşleme alınan ikinci sayıyı sonraki işlemde de kullanabilmek için tutar.
        string newResult = "";              // Elde eilen değeri sonraki işlemde kullanmak veya operasyonu değiştirmek için kullanır.
        bool priority = false;              // İşlem önceliğini bozmamak için kullanılır.

        public Calculator()
        {
            InitializeComponent();
        }

        private void resultsScreen_TextChanged(object sender, EventArgs e)
        {

        }


        private void Sizing(object sender, EventArgs e)     // Boyutlandırma işlemi.
        {
            Button[,] buttonArray = { { button7, button8, button9, button16 }, { button4, button5, button6, button15 }, { button1, button2, button3, button14 }, { button10, button11, button12, button13 } };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    buttonArray[i, j].Width = (this.Size.Width - 40) / 4;
                    buttonArray[i, j].Height = (this.Size.Height - 75) / 6;
                    writingScreen.Height = button1.Height;
                    writingScreen.Width = this.Size.Width - 25;
                    writingScreen.Location = new Point (5, 5);
                    resultScreen.Height = button1.Height;
                    resultScreen.Width = this.Size.Width - 25;
                    resultScreen.Location = new Point(5, writingScreen.Height + 10);
                    buttonArray[i, j].Font = new Font("Tahoma", Convert.ToInt32(button1.Height * 0.5));
                    writingScreen.Font = new Font("Tahoma", Convert.ToInt32(button1.Height * 0.5) /2);
                    resultScreen.Font = new Font("Tahoma", Convert.ToInt32(button1.Height * 0.4));
                    buttonArray[i, j].Location = new Point(j * button1.Width + 5*j + 5 , (i+1) * button1.Height + writingScreen.Height + 5*i + 15);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void NumberClick(object sender, EventArgs e)        // Numaralara tıklama işlemi.
        {
            Button button = (Button)sender;

            if (resultScreen.Text == "0" || opertorCase || resultScreen.Text == "Sıfıra bölünmez." || resultScreen.Text == "Tanımsız.") // Sonuç 0'sa veya hesap makinesi bir işlem yapıyorsa, sonuç ekranında "Sıfıra bölünmez." veya "Tanımsız." yazıyorsa;
                resultScreen.Clear();                               // Sonuç ekranını temizle. Yani girilen sayının 0 ile başlamasını engeller veya ikinci sayıyı girleceği zaman ilk sayıyı temziler.
            if ( writingScreen.Text.EndsWith("="))                  // Her hangi bir işlem yapılmışsa ve bir operatör girilmeden bir sayı girilmek isteniyorsa;
                writingScreen.Clear();                              // İşlem ekranı temizle.

            reserveNum = button.Text;                               // Girilen sayıyı sakla.
            opertorCase = false;                                    // Operatör durumunu kapat.
            resultScreen.Text += button.Text;                       // Tıklanan butonun değerini sonuç ekranında yazan sayıya ekle.
        }

        private void OperatorCalculus(object sender, EventArgs e)   // Operatör İşlemi.
        {
            Button button = (Button)sender;
            string newOpt = button.Text;                            // Tıklanan butonun değerini sakla.

            if (opertorCase == true || writingScreen.Text == "0 =") // Operatör durumu kapalı ise veya "0 =" işlemi yapılmışsa (İşlem sonucu üzerinden yeni bir işlem yapılacaksa);
            {                
                newResult = writingScreen.Text.Substring(0, writingScreen.TextLength - 2);  // İşlem ekranında yazan yazının son iki karakteri (eşittir ve boşluk) harici sakla.
                writingScreen.Text = resultScreen.Text + " " + newOpt;                      // İşlem ekranına Sonuç değerini ve operatörün işaretini gir.                                
            }

            else
            {
                opertorCase = true;      // Operatör durumunu aç.

                if (resultScreen.Text != "Sıfıra bölünmez." || resultScreen.Text == "Tanımsız.")        // Sonuç ekranında "Sıfıra bölünmez." yazısı veya "Tanımsız." yazısı yoksa;
                {
                    writingScreen.Text = writingScreen.Text + " " + resultScreen.Text + " " + newOpt;   // İşlem ekranına sonuç değeri ve operatör işaretini ekle.
                }

                switch (operatorSymbol)     // Operatör işaretini kontrol et.
                {
                    case "+":       //Operatör işareti "+" ise;
                        reserveResult = resultNumber + Double.Parse(reserveNum);                            // Sonuç değerini ve sonuç ekranında yazan değeri toplayıp sakla.
                        resultScreen.Text = (resultNumber + Double.Parse(resultScreen.Text)).ToString();    // Sonuç ekranı yazısına sonuç değeri ve sonuç ekranında yazan değeri toplayıp ata.
                        priority = true;    // İşlem önceliğini aç.
                        break;      // İşlemi sonlandır.

                    case "-":       //Operatör işareti "+" ise;
                        reserveResult = resultNumber - Double.Parse(reserveNum);                            // Sonuç değerinden sonuç ekranında yazan değeri çıkarıp sakla.
                        resultScreen.Text = (resultNumber - Double.Parse(resultScreen.Text)).ToString();    // Sonuç ekranı yazısına sonuç değerinden sonuç ekranında yazan değeri çıkarıp ata.
                        priority = true;    // İşlem önceliğini aç.
                        break;      // İşlemi sonlandır.

                    case "×":       //Operatör işareti "×" ise;
                        reserveResult = resultNumber * Double.Parse(reserveNum);                            // Sonuç değerini ve sonuç ekranında yazan değeri çarpıp sakla.
                        resultScreen.Text = (resultNumber * Double.Parse(resultScreen.Text)).ToString();    // Sonuç ekranı yazısına sonuç değeri ve sonuç ekranında yazan değeri çarpıp ata.
                        break;      // İşlemi sonlandır.

                    case "÷":       //Operatör işareti "÷" ise;
                        if (resultScreen.Text == "0" && resultNumber == 0)      // Sonuç ekranında "0" yazıyorsa ve sonuç 0'sa ( 0 ÷ 0 = )
                        {
                            resultScreen.Text = "Tanımsız.";                    // Sonuç ekranına "Tanımsız." yaz.
                        }

                        else if (resultScreen.Text == "0")                      // Sonuç ekranında "0" değeri varsa (bir sayıyıyı 0'a bölmek istiyorsa);
                        {
                            resultScreen.Text = "Sıfıra bölünmez."; // Sonuç ekranına 0 değerini ata.
                        }

                        else
                        {
                            reserveResult = resultNumber / Double.Parse(reserveNum);                        // Sonuç değerini sonuç ekranında yazan değere bölüp sakla.
                            resultScreen.Text = (resultNumber / Double.Parse(resultScreen.Text)).ToString();// Sonuç ekranı yazısına sonuç değerini sonuç ekranında yazan değere bölüp ata.            
                        }
                        break;      // İşlemi sonlandır.
                }
            }
            
            if (resultScreen.Text == "Sıfıra bölünmez." || resultScreen.Text == "Tanımsız.")        // Sonuç ekranında "Sıfıra bölünmez." yazısı veya "Tanımsız." yazısı varsa;
            {
                
                resultNumber = 0;                               // Sonuç değerini 0'a eşitle.
                resultScreen.Text = resultNumber.ToString();    // Sonuç ekranına sonuç değerini (0) yazdır.
                writingScreen.Text = (resultNumber + " " + newOpt).ToString();    // İşlem ekranına sonuç değerini ve en son girilen operatör değerini gir ( "0 +" gibi).
                operatorSymbol = newOpt;                        // Operatör simgesine en son girilen operatör değerini gir.
                reserveOpSym = operatorSymbol;                  // Yedek operatör simgesine operatör simgesini ata.
            }

            else
            {
                resultNumber = Double.Parse(resultScreen.Text); // Sonuç değerine sonuç ekranında yazan değeri ata.
                resultScreen.Text = resultNumber.ToString();    // Sonuç ekranına sonuç değerini gir.
                operatorSymbol = newOpt;                        // Operatör simgesine en son girilen operatör değerini gir.
                reserveOpSym = operatorSymbol;                  // Yedek operatör simgesine operatör simgesini ata.
                newResult = resultNumber.ToString();            // Sonuç değerini yedek sonuç değerine ata.

                if ((newOpt == "×" || newOpt == "÷") && priority == true)   // Yeni operatör "×" veya "÷"se ve toplama çıkarma işlemi yapılmışsa;
                {
                    writingScreen.Text = resultScreen.Text + " " + newOpt;  // İşlem ekranına  sonuç değerini yaz ve yeni operatörü ekle.
                    priority = false;                                       // Toplama çıkarma işlemi kontrolünü kapat.
                }
            }
        }

        private void button12_Click(object sender, EventArgs e) // C İşlemi
        {
            resultScreen.Text = "0";                // Sonuç değerine 0 ver.
            writingScreen.Text = "";                // İşlem ekranını temizle.
            operatorSymbol = "";                    // Operatör simgesini kaldır.
            resultNumber = 0;                       // Sonuç değerine 0 ver.
            reserveResult = 0;                      // Yedek sonuç değerine 0 ver.
            reserveNum = "";                        // İşleme alınan ikinci sayıyı temizle.
            opertorCase = false;                    // Operatör işlemini kaldır.
            newResult = "";                         // Yedek sonuç değerine 0 ver.
            reserveOpSym = "";                      // Yedek operatör simgesini kaldır.
        }

        private void button11_Click(object sender, EventArgs e) // Eşittir İşlemi.
        {
            opertorCase = true;                                 // Operatör durumunu aç.
            bool equalCase = false;                             // Eşittir durumunu kontrol eder.

            if (writingScreen.Text.EndsWith("="))               // İşlem ekranı "=" ile bitiyorsa (daha önce bir işlem yapılmışsa);
            {
                writingScreen.Text = resultNumber + " " + reserveOpSym + " " + reserveNum + " " + "=";  // İşlem ekranına sonuç değerini, yedeklenen operatör simgesini ve yedeklenen ikinci sayıyı gir.
                operatorSymbol = reserveOpSym;                  // Operatör simgesine yedeklenen operatör simgesini ata.
                equalCase = true;                               // Eşittir durumunu aç.

                if ( resultScreen.Text == "0" && opertorCase == false)                  // Sonuç Ekranında da "0" yazıyorsa ve operatör durumu kapalı ise;
                {
                    resultScreen.Text = "0";                    // Sonuç değerine 0 ver.
                    writingScreen.Text = "";                    // İşlem ekranını temizle.
                    operatorSymbol = "";                        // Operatör simgesini kaldır.
                    resultNumber = 0;                           // Sonuç değerine 0 ver.
                    reserveResult = 0;                          // Yedek sonuç değerine 0 ver.
                    reserveNum = "";                            // İşleme alınan ikinci sayıyı temizle.
                    opertorCase = false;                        // Operatör işlemini kaldır.
                    newResult = "";                             // Yedek sonuç değerine 0 ver.
                    reserveOpSym = "";                          // Yedek operatör simgesini kaldır.
                }
            }

            else
            {
                writingScreen.Text = writingScreen.Text + " " + resultScreen.Text + " " + "=";  // İşlem ekranına sonuç değerini ve "=" işaretini ekle.
            }

            switch (operatorSymbol)
            {
                case "+":
                    reserveResult = resultNumber + Double.Parse(reserveNum);                            // Sonuç değerini ve sonuç ekranında yazan değeri toplayıp sakla.
                    resultScreen.Text = (resultNumber + Double.Parse(resultScreen.Text)).ToString();    // Sonuç ekranı yazısına sonuç değeri ve sonuç ekranında yazan değeri toplayıp ata.
                    break;  // İşlemi sonlandır.

                case "-":
                    reserveResult = resultNumber - Double.Parse(reserveNum);                            // Sonuç değerinden sonuç ekranında yazan değeri çıkarıp sakla.
                    resultScreen.Text = (resultNumber - Double.Parse(resultScreen.Text)).ToString();    // Sonuç ekranı yazısına sonuç değerinden sonuç ekranında yazan değeri çıkarıp ata.
                    break;  // İşlemi sonlandır.

                case "×":
                    reserveResult = resultNumber * Double.Parse(reserveNum);                            // Sonuç değerini ve sonuç ekranında yazan değeri çarpıp sakla.
                    resultScreen.Text = (resultNumber * Double.Parse(resultScreen.Text)).ToString();    // Sonuç ekranı yazısına sonuç değeri ve sonuç ekranında yazan değeri çarpıp ata.
                    break;  // İşlemi sonlandır.

                case "÷":
                    if (resultScreen.Text == "0" &&  resultNumber == 0)     // Sonuç ekranında "0" yazıyorsa ve sonuç 0'sa ( 0 ÷ 0 = )
                    {
                        resultScreen.Text = "Tanımsız.";                    // Sonuç ekranına "Tanımsız." yaz.
                    }

                    else if (resultScreen.Text == "0")                      // Sonuç ekranında "0" değeri varsa (bir sayıyıyı 0'a bölmek istiyorsa);
                    {
                        resultScreen.Text = "Sıfıra bölünmez.";             // Sonuç ekranına "Sıfıra bölünmez" yaz.
                    }

                    else
                    {
                        reserveResult = resultNumber / Double.Parse(reserveNum);                        // Sonuç değerini sonuç ekranında yazan değere bölüp sakla.
                        resultScreen.Text = (resultNumber / Double.Parse(resultScreen.Text)).ToString();// Sonuç ekranı yazısına sonuç değerini sonuç ekranında yazan değere bölüp ata.
                    }
                    break;  // İşlemi sonlandır.
            }

            if (resultScreen.Text == "Sıfıra bölünmez." || resultScreen.Text == "Tanımsız.")    // Sonuç ekranında "Sıfıra bölünmez." yazısı varsa;
            {
                opertorCase = false;                // Operatör durumunu kapat.
                operatorSymbol = "";                // Operatör simgesini kaldır.
                writingScreen.Clear();              // İşlem ekranını temizle.
                resultNumber = 0;                   // Sonuç değerine 0 ver.
                opertorCase = false;                // Operatör durumunu kapat.
            }

            else if (resultNumber == 0 && (operatorSymbol != "+" || operatorSymbol != "-"))     // Sonuç değeri 0'a eşitse ve operatör simgesi "+" veya "-" ise;
            {
                if (writingScreen.Text.StartsWith("0"))             // İşlem ekranında yazan yazı 0 İle başlıyorsa;
                {
                    resultNumber = reserveResult;                   // Sonuç değerine yedeklenen sonuç değerini ata.
                    resultScreen.Text = resultNumber.ToString();    // Sonuç ekranına sonuç değerini yazdır.
                }

                else
                {
                    writingScreen.Text = resultNumber.ToString() + " " + "=";   // İşlem ekranına sonuç değerini ve "=" işaretini yazdır.
                }

                operatorSymbol = "";                        // Operatör Simgesini kaldır.
                opertorCase = false;                        // Operatör durumunu kapat.

            }

            else if (equalCase == true)                     // Eşittir durumu açıksa;
            {
                resultNumber = reserveResult;               // Sonuç değerine yedeklenen değeri ata.
                resultScreen.Text = resultNumber.ToString();// Sonuç ekranına sonuç değerini yazdır.
                reserveOpSym = operatorSymbol;              // Yedeklenen operatör simgesine geçerli operatör simgesini ata.
                operatorSymbol = "";                        // Operatör simgesini kaldır.
                newResult = resultNumber.ToString();        // Yedek sonuca sonuç değerini ata.
            }

            else
            {
                resultNumber = Double.Parse(resultScreen.Text); // Sonuç değerine sonuç ekranı değerini ata.
                resultScreen.Text = resultNumber.ToString();    // Sonuç ekranına sonuç değerini yazdır.
                reserveOpSym = operatorSymbol;                  // Yedeklenen operatör simgesine geçerli operatör simgesini ata.
                operatorSymbol = "";                            // Operatör simgesini kaldır.
                newResult = resultNumber.ToString();            // Yedek sonuca sonuç değerini ata.
            }
        }
    }
}
