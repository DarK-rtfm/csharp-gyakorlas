using System;
namespace Godrok;
class Program
{
    static void Main(string[] args)
    {
        List<int> melysegek;
        melysegek = File.ReadAllLines(@"melyseg.txt").Select(e => int.Parse(e)).ToList();

        System.Console.WriteLine("1. feladat");
        System.Console.WriteLine($"A fajl adatainak szama: {melysegek.Count()}");

        System.Console.WriteLine("2. feladat");
        System.Console.Write("Adjon meg egy tavolsagerteket! ");
        int te = int.Parse(Console.ReadLine() ?? "0") - 1;
        System.Console.WriteLine($"Ezen a helyen a felszin {melysegek[te]} melyen van.");

        double arany = melysegek.Count(e => e == 0) / (double)melysegek.Count();
        arany = Math.Round(arany * 10000) / (double)100;
        System.Console.WriteLine("3. feladat");
        System.Console.WriteLine($"Az erintetlen terulet aranya {arany}%");

        List<List<(int m, int i)>> godrok = new List<List<(int, int)>>(); // draga isten, miert??
        foreach ((int m, int i) in melysegek.Zip(Enumerable.Range(0, melysegek.Count())))
        {
            if (m == 0) godrok.Add(new List<(int m, int i)>());
            else godrok.Last().Add((m, i));
        }
        godrok = godrok.Where(e => e.Count() > 0).ToList();

        File.WriteAllLines(@"godrok.txt", godrok.Select(e => e.Select(s => s.m.ToString()).Aggregate((a, b) => a + " " + b)));

        System.Console.WriteLine("5. feladat");
        System.Console.WriteLine($"A godrok szama: {godrok.Count()}");

        List<(int m, int i)> kertgodor = godrok.First(e => e.Select(e => e.i).Contains(te));
        System.Console.WriteLine($"6. feladat");
        System.Console.WriteLine("a)");
        System.Console.WriteLine($"A godor kezdete: {kertgodor.First().i + 1}, a godor vege: {kertgodor.Last().i + 1}");
        System.Console.WriteLine("b)");
        if (
            kertgodor.Zip(kertgodor.Skip(1))
            .SkipWhile(e => e.First.m <= e.Second.m)
            .SkipWhile(e => e.First.m >= e.Second.m).Count() == 0
        ) System.Console.WriteLine("Folyamatosan melyul");
        else System.Console.WriteLine("Nem melyul folyamatosan");
        System.Console.WriteLine("c)");
        System.Console.WriteLine($"A legnagyobb melysege {kertgodor.Max(e => e.m)} meter");
        System.Console.WriteLine("d)");
        System.Console.WriteLine($"A terfogata {kertgodor.Sum(e => e.m) * 10} m^3");
        System.Console.WriteLine("e)");
        System.Console.WriteLine($"A vizmennyiseg {kertgodor.Sum(e => e.m - 1) * 10} m^3");

    }
}
