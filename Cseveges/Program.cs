namespace Cseveges;

class Program
{
    static void Main(string[] args)
    {
        List<Beszelgetes> beszelgetesek = new List<Beszelgetes>();
        bool firstline = false;
        foreach (string line in System.IO.File.ReadLines(@"csevegesek.txt"))
        {
            if (!firstline)
            {
                firstline = true; continue;
            }
            beszelgetesek.Add(new Beszelgetes(line));
        }
        List<string> tagok = new List<string>();
        foreach (string line in System.IO.File.ReadLines(@"tagok.txt"))
        {
            tagok.Add(line);
        }


        System.Console.WriteLine($"4. Feladat: Tagok szama: {tagok.Count()}fo - Beszelgetesek: {beszelgetesek.Count()}db");
        Beszelgetes leghosszabb = beszelgetesek.MaxBy(e => (e.veg - e.kezdet).TotalSeconds);
        System.Console.WriteLine("5. feladat: Leghosszabb beszelgetes adatai");
        System.Console.WriteLine($"\tkezdemenyezo: {leghosszabb.kezdemenyezo}");
        System.Console.WriteLine($"\tfogado:       {leghosszabb.fogado}");
        System.Console.WriteLine($"\tkezdete:      {leghosszabb.kezdet.ToString("yy.MM.dd-HH:mm:ss")}");
        System.Console.WriteLine($"\tvege:         {leghosszabb.veg.ToString("yy.MM.dd-HH:mm:ss")}");
        System.Console.WriteLine($"\thossz:        {(leghosszabb.veg - leghosszabb.kezdet).TotalSeconds}mp");

        System.Console.Write("6. feladat: Adja meg egy tag nevet: ");
        string nev = Console.ReadLine();
        string idk = beszelgetesek.Where(e => e.fogado == nev || e.kezdemenyezo == nev).Select(e => e.veg - e.kezdet).Aggregate(TimeSpan.FromTicks(0), (a, b) => a + b).ToString(@"hh\:mm\:ss");
        System.Console.WriteLine($"\tA beszelgetesek osszes idege: {idk}");

        System.Console.WriteLine("7. feladat: Nem beszelgettek senkivel");
        foreach (string senkivel in tagok.Where(e => !beszelgetesek.Select(b => b.fogado).Contains(e) && !beszelgetesek.Select(b => b.kezdemenyezo).Contains(e)))
        {
            System.Console.WriteLine($"\t{senkivel}");
        }

        flatten(ref beszelgetesek);
        beszelgetesek.OrderBy(e => e.kezdet);
        List<(DateTime k, DateTime v)> csendek = beszelgetesek.Zip(beszelgetesek.TakeLast(beszelgetesek.Count() - 1)).Select(e => (e.First.veg, e.Second.kezdet)).ToList();
        (DateTime k, DateTime v) leghosszabbcsend = csendek.MaxBy(e => e.v - e.k);
        System.Console.WriteLine("8. feladat: Leghosszabb csendes idoszak 15h-tol");
        System.Console.WriteLine($"\tKezdete: {leghosszabbcsend.k.ToString("yy.MM.dd-HH:mm:ss")}");
        System.Console.WriteLine($"\tVege: {leghosszabbcsend.v.ToString("yy.MM.dd-HH:mm:ss")}");
        System.Console.WriteLine($"\tHossza: {(leghosszabbcsend.v - leghosszabbcsend.k).ToString(@"hh\:mm\:ss")}");
    }

    static void flatten(ref List<Beszelgetes> orig)
    {
    label:
        orig = orig.Where(e => e.kezdet > DateTime.MinValue).ToList();
        for (int i = 0; i < orig.Count(); i++)
        {
            for (int j = 0; j < orig.Count(); j++)
            {
                if (i == j) continue;
                if (orig[i].kezdet < orig[j].kezdet && orig[j].veg < orig[i].veg)
                {
                    orig[j].kezdet = DateTime.MinValue; orig[j].veg = DateTime.MinValue;
                    goto label;
                }
                else
                if (orig[i].kezdet < orig[j].kezdet && orig[j].kezdet < orig[i].veg)
                {
                    orig[i].veg = orig[j].veg;
                    orig[j].kezdet = DateTime.MinValue; orig[j].veg = DateTime.MinValue;
                    goto label;
                }
            }
        }
        orig = orig.Where(e => e.kezdet > DateTime.MinValue).ToList();
    }
}
