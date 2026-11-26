using System;
class Program
{
    static void Main()
    {
        userInput.CitireNumar();
        int egale = 0;
        if (userInput.b1 == userInput.b2) egale = 1;
        if (egale == 1)
        {
            Console.WriteLine($"Numarul introdus: {userInput.nrTxt} este deja in baza dorita.");
            return;
        }
        bool negativ = SeprareParti.EsteNegativ(userInput.nrTxt);
        string intregTxt = SeprareParti.ParteaIntreaga(userInput.nrTxt);
        string fractieTxt = SeprareParti.ParteaFractionara(userInput.nrTxt);
        long intreg = ConversieBaze.IntregDinB1LaZecimal(intregTxt, userInput.b1);
        double fractie = ConversieBaze.FractieDinB1LaZecimal(fractieTxt, userInput.b1);
        string intregB2 = ConversieBaze.IntregDinZecimalLaB2(intreg, userInput.b2);
        string fractieB2 = ConversieBaze.FractieDinZecimalLaB2(fractie, userInput.b2);
        Console.WriteLine();
        Console.WriteLine("Numarul va fi aproximat la maxim 7 cifre dupa virgula in baza dorita.");
        Console.WriteLine("Rezultat: " + Asamblez.AsamblezNumar(negativ, intregB2, fractieB2));
    }

}

class userInput 
{
    public static string nrTxt;
    public static int b1, b2;
    public static void CitireNumar()
    {
        while (true)
        {
            Console.Write("Introduceti numarul: ");
             nrTxt = Console.ReadLine().Trim().ToUpper();
            Console.WriteLine();
            Console.Write("Introduceti baza numarului: ");
            string b1Txt = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Introduceti baza in care doriti sa convertiti: ");
            string b2Txt = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Verificam numerele introduse...");
              int.TryParse(b1Txt, out b1);
              int.TryParse(b2Txt, out b2);
            if (BazaValida(b1) && BazaValida(b2))
            {
                if (NumarValid(nrTxt, b1))
                {
                    Console.WriteLine("Numarul introdus este valid.");
                    break;
                }
                else
                {
                    Console.WriteLine("Numarul introdus nu este valid.");
                    continue;
                }
            }
            else
            {
                Console.WriteLine("Baza introdusa nu este valida.");
                continue;
            }
        }
    }
    private static bool BazaValida(int b)
    { 
        if (b>= 2 && b <= 16)
            return true;
        else
            return false;   
    }
    public static bool NumarValid(string nrTxt, int b)
    {
        string cifre = "0123456789ABCDEF";
        nrTxt = nrTxt.Trim().ToUpper(); // de ex: "  1a.3F  " -> "1A.3F"
        if (nrTxt.Length == 0) return false;
        int nrPuncte = 0, nrVirgule = 0;
        int nrCifre = 0;
        for (int i = 0; i < nrTxt.Length; i++)
        {
            char c = nrTxt[i];
            if (c == '-')
            {   if (i != 0) return false;
                continue;
            }
            if (c == '.' )
            {
                nrPuncte++;
                continue;
            }
            if(c == ',')
            {
                nrVirgule++;
                continue;
            }
            int valoare = cifre.IndexOf(c);
            if (valoare == -1 || valoare >= b) return false; // sa coresponda cu baza
            nrCifre++;
        }
        if (nrPuncte + nrVirgule != 1 ) return false;
        if (nrCifre == 0) return false;
        return true;





    }
    
}
// clasa SeparareParti: extragerea partii intregi si a partii fractionare dintr-un numar ex: 1,2 :pInr e 1 si pFr e 2
class SeprareParti
{
    public static bool EsteNegativ(string nrTxt)
    {
        return nrTxt[0] == '-';
    }
    public static string ParteaIntreaga(string nrTxt)
    {
        int index = nrTxt.IndexOfAny(new char[] { '.', ',' });
        if (index == -1) return "0";
        int negativ = 0;
        if(nrTxt[0] == '-') negativ = 1;
        if (negativ == 1 ) return nrTxt.Substring(negativ, index - 1);
        return nrTxt.Substring(0, index);

    }
    public static string ParteaFractionara(string nrTxt)
    {
        int index = nrTxt.IndexOfAny(new char[] { '.', ',' });
        if (index == -1)  return "0";
        return nrTxt.Substring(index + 1);
        
    }
}
class Conversie 
{
    const string cifre = "0123456789ABCDEF";
    public static int CharLaValoare(char a)
    {
        int index = cifre.IndexOf(a);
        return index;
    }
    public static char ValoareLaChar(int  a)
    {
        char c = cifre[a];
        return c;
    }
}
class ConversieBaze
{
    const int MaxCifre = 7; // limita pentru partea fractionara in caz ca e periodica
    public static long IntregDinB1LaZecimal(string nrTxt, int b1)
    {
        long rezultat = 0;
        foreach (char c in nrTxt)
        {
            rezultat = rezultat * b1 + Conversie.CharLaValoare(c);
        }
        return rezultat;
    }

    public static string IntregDinZecimalLaB2(long nr, int b2)
    {
        int r;
        string rezultat = "";
        if (nr == 0) return "0";
        while (nr > 0)
        {
            r = (int)(nr % b2);
            rezultat = Conversie.ValoareLaChar(r) + rezultat;
            nr = nr / b2;
        }

        return rezultat;

    }
    public static double FractieDinB1LaZecimal(string nrTxt, int b1)
    {
        double rezultat = 0;
        double putere = 1.0 / b1;
        foreach (char c in nrTxt)
        {
            rezultat = rezultat + Conversie.CharLaValoare(c) * putere;
            putere = putere / b1;
        }
        return rezultat;
    }
    public static string FractieDinZecimalLaB2(double nr, int b2)
    {
        string rezultat = "";
        int nrCifre = 0;

        while (nr > 0 && nrCifre < MaxCifre)
        {
            nr = nr * b2;
            int cifra = (int)nr;
            rezultat = rezultat + Conversie.ValoareLaChar(cifra);
            nr = nr - cifra;
            nrCifre++;
        }

        return rezultat;
    }
}
class Asamblez
{
    public static string AsamblezNumar(bool negativ, string intreg, string fractie)
    {
        if (fractie == "") fractie = "0";
        string rezultat = ""  ;
        if (negativ) rezultat = "-";
        rezultat = rezultat + intreg + "." + fractie;
        return rezultat;
    }
}

    
