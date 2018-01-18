using System;

namespace T.C_Dogruluk_Kontrol
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("TC: ");
            long tc = long.Parse(Console.ReadLine()); // doğum tarihini okuduk
            Console.Write("AD: ");
            string name = Console.ReadLine().ToUpper(); // ismi okuduk ve tamamını büyük harfe dönüştürdük
            Console.Write("SOYAD: ");
            string surname = Console.ReadLine().ToUpper(); // soyadı okuduk ve tamamını büyük harfe dönüştürdük
            Console.Write("DOĞUM TARİHİ(YIL): ");
            int birthdate = int.Parse(Console.ReadLine()); // doğum tarihini okuduk

            Identity control = new Identity(tc, name, surname, birthdate);
            switch (control.check())
            {
                case 1: Console.WriteLine("Girilen T.C teyit edilmiştir."); break;
                case 2: Console.WriteLine("T.C geçersizdir."); break;
                case 3: Console.WriteLine("Girilen T.C ve kişi bilgileri uyuşmamaktadır."); break;
            }
            Console.ReadKey();
        }
        
    }
}
