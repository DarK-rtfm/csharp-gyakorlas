namespace otszaz;
class Program
{
    static int ertek(int db)
    {
        List<int> alap = new List<int> { 500, 450, 400 };
        if (db > 3)
        {
            for (int i = 3; i < db; i++)
                alap.Add(400);
        }
        else alap = alap.Take(db).ToList();
        return alap.Sum();
    }

    static List<(int db, string aru)> summarize(List<string> total)
    {
        return total.Distinct().Select(e => (total.Count(f => f == e), e)).ToList();
    }
    static void Main(string[] args)
    {
        List<string> osszkosar = File.ReadAllLines(@"penztar.txt").ToList();
        List<List<string>> kosarak = new List<List<string>>();

        kosarak.Add(new List<string>());
        foreach (var f in osszkosar)
        {
            if (f == "F") kosarak.Add(new List<string>());
            else kosarak.Last().Add(f);
        }
        kosarak = kosarak.Where(f => f.Count() > 0).ToList();

        System.Console.WriteLine("2. feladat");
        System.Console.WriteLine($"A fizetesek szama: {kosarak.Count()}");
        System.Console.WriteLine();
        System.Console.WriteLine("3. feladat");
        System.Console.WriteLine($"Az elso vasarlo {kosarak.First().Count()} darab arucikket vasarolt");
        System.Console.WriteLine();

        System.Console.WriteLine("4. feladat");
        System.Console.Write("Adja meg egy vasarlas sorszamat! ");
        int sorszam = int.Parse(Console.ReadLine() ?? "2");
        System.Console.Write("Adja meg egy arucikk nevet! ");
        string arucikk = Console.ReadLine() ?? "kefe";
        System.Console.Write("Adja meg a vasarolt darabszamot! ");
        int dbszam = int.Parse(Console.ReadLine() ?? "2");
        System.Console.WriteLine();

        System.Console.WriteLine("5. feladat");
        System.Console.WriteLine($"Az elso vasarlas sorszama: {kosarak.IndexOf(kosarak.First(e => e.Contains(arucikk))) + 1}");
        System.Console.WriteLine($"Az utolso vasarlas sorszama: {kosarak.IndexOf(kosarak.Last(e => e.Contains(arucikk))) + 1}");
        System.Console.WriteLine($"{kosarak.Count(e => e.Contains(arucikk))} vasarlas soran vettek belole");

        System.Console.WriteLine("6. feladat");
        System.Console.WriteLine($"{dbszam} darab vetelekor fizetendo: {ertek(dbszam)}");

        System.Console.WriteLine("7. feladat");
        foreach (var item in summarize(kosarak[sorszam - 1]))
        {
            System.Console.WriteLine($"{item.db} {item.aru}");
        }

        File.WriteAllLines(@"osszeg.txt",
            kosarak
            .Select(vasarlas => summarize(vasarlas).Select(f => ertek(f.db)).Sum())
            .Zip(Enumerable.Range(1, kosarak.Count()))
            .Select(x => x.Second + ": " + x.First)
        );
    }
}
