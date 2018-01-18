using System.IO;
using System.Net;
using System.Xml;
using System;
/*
	Muhammet Tayyip Muslu
	muslu.net
	tayyipmuslu@gmail.com
*/
namespace T.C_Dogruluk_Kontrol
{
    class Identity
    {
        private long tc;
        private string name;
        private string surname;
        private int year;

        public Identity(long tc, string firstname, string lastname, int birthYear)
        {
            Tc = tc;
            Firstname = firstname;
            Lastname = lastname;
            BirthYear = birthYear;
        }
        public long Tc
        {
            set {
                if (value > 0 && value < 99999999999 && value.ToString().Length == 11)
                tc = value;
                else Console.WriteLine("Hata: T.C Kimlik numarası 11 haneli ve sayısal değerde olmalıdır.");
            }
            get { return tc; }
        }
        public string Firstname
        {
            set { name = value.ToString().ToUpper(); }
            get { return name; }
        }
        public string Lastname
        {
            set { surname = value.ToString().ToUpper(); }
            get { return surname; }
        }
        public int BirthYear
        {
            set { year = value; }
            get { return year; }
        }
        public int check()
        {
            if (firstControl(Tc) == 1)
            {
                if (secondControl()) return 1;
                else return 3;
            }
            else
            {
                return 2;
            }
        }
        private byte firstControl(long tc)
        {
            if (tc > 0  && tc < 99999999999 && tc.ToString().Length == 11)
            {
                char[] arr = tc.ToString().ToCharArray();
                int sumEven = 0, sumOdd = 0, sumFirst10 = 0, i = 0;

                if (arr[0] == '0') return 2; // ilk rakam 0 olamaz
                
                while (i <= 8)
                {
                    int temp = int.Parse(arr[i].ToString());
                    sumFirst10 += temp;
                    if (i % 2 == 1) sumEven += temp;
                    else sumOdd += temp;
                    i++;
                }
                sumFirst10 += int.Parse(arr[9].ToString());
                // 10 ve 11. hane kontrolü yapılıyor.
                if (((sumEven * 9) + (sumOdd * 7)) % 10 == int.Parse(arr[9].ToString()) && (sumFirst10 % 10 == int.Parse(arr[10].ToString())))
                    return 1;
                else
                    return 2;

            }
            else
                return 0;
        }
        private bool secondControl()
        {
            var uri = new Uri("https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx"); // T.C kimlik sorgulama servisine bu sayfadan sorgu yapacağız

            var request = (HttpWebRequest)WebRequest.CreateDefault(uri); // bir istek oluşturduk
            // istek bilgileri  bkz : https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx?op=TCKimlikNoDogrula
            request.ContentType = "text/xml; charset=utf-8";
            request.Method = "POST";
            request.Accept = "text/xml";
            request.Headers.Add("SOAPAction", "http://tckimlik.nvi.gov.tr/WS/TCKimlikNoDogrula");

            // SOAP xml dosyası ve bilgileri göndereceğimiz XML
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
  <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" >
         
           <soap:Body>
          
              <TCKimlikNoDogrula xmlns=""http://tckimlik.nvi.gov.tr/WS"">
           
                 <TCKimlikNo>" + Tc + @"</TCKimlikNo>
           
                 <Ad>" + Firstname + @"</Ad>
           
                 <Soyad>" + Lastname + @"</Soyad>
           
                 <DogumYili>" + BirthYear + @"</DogumYili>
           
               </TCKimlikNoDogrula>
           
             </soap:Body>
            </soap:Envelope>";

            // xml i okuyoruz
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(xml);
            // sunucuya istek gönderiyoruz
            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
            // gelen cevabı burada okuyoruz.
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd(); // cevap yine xml olarak dönüyor. Biz xml taglarından kurtulduğumuzda bize true veya false değeri kalır
                    // tc geçerli ise true, değil ise false değeri döner
                    return bool.Parse(System.Text.RegularExpressions.Regex.Replace(soapResult, "<.*?>", String.Empty));
                }
            }
        }

    }
}
