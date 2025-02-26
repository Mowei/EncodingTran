using EncodingTran.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingTran
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<int> codePages = new List<int>() { 932, 20932, 51932, 50220, 50221, 50222, 65001, 1200, 12000, 12001, 65000, 1252, 936 };

            var badstringFromDatabase = "ƒ`ƒƒƒlƒ‹ƒp[ƒgƒi[‚Ì‘I‘ð\"";

            foreach (var codepage in codePages)
            {
                var recovered = Encoding.GetEncoding(codepage).GetBytes(badstringFromDatabase);

                var invalidByte = false;
                for (int i = 0; i < recovered.Length; i++)
                {
                    //find utf-8_replacement_character EF BF BD
                    i = recovered.ToList().FindIndex(i, x => x == 0xEF);
                    if(i == -1) { break; }
                    if (i + 3 <= recovered.Length)
                    {
                        var bytes = recovered.ToList().Skip(i).Take(3).ToArray();
                        if (bytes[0] == 0xef && bytes[1] == 0xbf && bytes[2] == 0xbd)
                        {
                            invalidByte = true;
                            break;
                        }
                    }

                }
                if(invalidByte) {
                    //基本上是轉壞了無法復原
                    Console.WriteLine("! Find utf-8 replacement character " + codepage);
                    continue; 
                }


                Console.WriteLine("read codePage " + codepage);
                foreach (var codepage2 in codePages)
                {
                    ConvertCodePage(codepage2, recovered);
                }
            }

            //To JP
            /*
        var badstringFromDatabase = "ƒ`ƒƒƒlƒ‹ƒp[ƒgƒi[‚Ì‘I‘ð";
        var hopefullyRecovered = Encoding.GetEncoding(1252).GetBytes(badstringFromDatabase);
        var oughtToBeJapanese = Encoding.GetEncoding(932).GetString(hopefullyRecovered);

        var badstringFromDatabase = "ƒ`ƒƒƒlƒ‹ƒp[ƒgƒi[‚Ì‘I‘ð";
        var recovered1 = System.Text.Encoding.GetEncoding(932).GetBytes(badstringFromDatabase); //Shift JIS
        var recovered2 = System.Text.Encoding.GetEncoding(20932).GetBytes(badstringFromDatabase); //EUC
        var recovered3 = System.Text.Encoding.GetEncoding(51932).GetBytes(badstringFromDatabase); //EUC
        var recovered4 = System.Text.Encoding.GetEncoding(50220).GetBytes(badstringFromDatabase); //ISO-2022-JP
        var recovered5 = System.Text.Encoding.GetEncoding(50221).GetBytes(badstringFromDatabase); //ISO-2022-JP
        var recovered6 = System.Text.Encoding.GetEncoding(50222).GetBytes(badstringFromDatabase); //ISO-2022-JP
        var recovered7 = System.Text.Encoding.GetEncoding(65001).GetBytes(badstringFromDatabase); //UTF-8
        var recovered8 = System.Text.Encoding.GetEncoding(1200).GetBytes(badstringFromDatabase); //UTF-16
        var recovered9 = System.Text.Encoding.GetEncoding(12000).GetBytes(badstringFromDatabase); //UTF-32
        var recovered10 = System.Text.Encoding.GetEncoding(12001).GetBytes(badstringFromDatabase); //UTF-32BE
        var recovered11 = System.Text.Encoding.GetEncoding(65000).GetBytes(badstringFromDatabase); //UTF-7
        Console.WriteLine("Shift JIS: " + System.Text.Encoding.GetEncoding(932).GetString(recovered1)); //Shift JIS
        Console.WriteLine("EUC: " + System.Text.Encoding.GetEncoding(932).GetString(recovered2)); //EUC
        Console.WriteLine("EUC: " + System.Text.Encoding.GetEncoding(932).GetString(recovered3)); //EUC
        Console.WriteLine("ISO-2022-JP: " + System.Text.Encoding.GetEncoding(932).GetString(recovered4)); //ISO-2022-JP
        Console.WriteLine("ISO-2022-JP: " + System.Text.Encoding.GetEncoding(932).GetString(recovered5)); //ISO-2022-JP
        Console.WriteLine("ISO-2022-JP: " + System.Text.Encoding.GetEncoding(932).GetString(recovered6)); //ISO-2022-JP
        Console.WriteLine("UTF-8: " + System.Text.Encoding.GetEncoding(932).GetString(recovered7)); //UTF-8
        Console.WriteLine("UTF-16: " + System.Text.Encoding.GetEncoding(932).GetString(recovered8)); //UTF-16
        Console.WriteLine("UTF-32: " + System.Text.Encoding.GetEncoding(932).GetString(recovered9)); //UTF-32
        Console.WriteLine("UTF-32BE: " + System.Text.Encoding.GetEncoding(932).GetString(recovered10)); //UTF-32BE
        Console.WriteLine("UTF-7: " + System.Text.Encoding.GetEncoding(932).GetString(recovered11)); //UTF-7


            */
            Console.ReadLine();


        }

        public static void ConvertCodePage(int codePage, byte[] recovered)
        {
            Console.WriteLine("To " + codePage + " : " + Encoding.GetEncoding(codePage).GetString(recovered));
        }
    }
}
