namespace _2021maj;

class Forgalom {
    // A tarolt valtozok soronkent
    public string tipus;
    public DateTime datum;
    public int kategoria;
    public int osszeg;

    // Forgalom osztaly egy peldanyat hogyan hozza letre
    public Forgalom(string sor) {
        tipus = sor.Split(';')[0];
        datum = DateTime.Parse(sor.Split(';')[1]); // Datumot be kell parsolni, itt kivetelesen nem Convert.valami
        kategoria = Convert.ToInt32(sor.Split(';')[2]); 
        osszeg = Convert.ToInt32(sor.Split(';')[3]);
    }

    // osztalynak megadunk egy metodust, amit kesobb ugyanugy lekerhetunk. Ez visszaadja a forgalom evszakat
    public string evszak() {
        if (datum.Month <= 2 || datum.Month == 12) return "tel";
        if (datum.Month >= 3 || datum.Month <= 5) return "tavasz";
        if (datum.Month >= 6 || datum.Month <= 8) return "nyar";
        if (datum.Month >= 9 || datum.Month <= 11) return "osz";
        return "";
    }
}
class Program
{
    static void Main(string[] args)
    {
        //  --------------------------------------  1. feladat
        System.Console.WriteLine("1. feladat");

        List<Forgalom> forgalmak;
        try {
            forgalmak = File.ReadAllLines(@".\forgalom.csv")
                                           .Select(e => new Forgalom(e))
                                           .ToList();
        } catch {
            Console.WriteLine("Fajl beolvasasi hiba!");  
            return; // Main-bol valo visszalepes befejezi a programot, mivel adat nelkul nem tudunk tovabb haladni
        }

        Console.WriteLine($"\t{forgalmak.Count()} bejegyzes betoltve.");

        // kesobbiekre nezve egyszerubb kulon kimeneteni forgalom iranya alapjan ket kulon listat
        List<Forgalom> csakbeszerzesek = forgalmak.Where(e => e.tipus=="B").ToList();
        List<Forgalom> csakeladasok = forgalmak.Where(e => e.tipus=="E").ToList();

        // -------------------- 2. feladat
        System.Console.WriteLine("2. feladat");

        int beszerzett_osszeg = csakbeszerzesek.Sum(e=>e.osszeg); // Forgalmak osszege (megadjuk, hogy a classon belul mit osszegezzen)
        int eladott_osszeg = csakeladasok.Sum(e=>e.osszeg);

        System.Console.WriteLine($"\tEves teljes eladas: {eladott_osszeg} Ft erteku aru.");
        System.Console.WriteLine($"\tEves teljes beszerzes: {beszerzett_osszeg} Ft erteku aru.");
        System.Console.WriteLine($"\tRaktaron van meg: {beszerzett_osszeg-eladott_osszeg} Ft erteku aru."); // egyszeru kivonas

        // 3. feladat
        System.Console.WriteLine("3. feladat");
        string legnagyobbBeszerzesEvszak = csakbeszerzesek
                                                         .GroupBy(e=>e.evszak()) // evszak alapjan kulon listakba szedes List<List<Forgalom>>
                                                         .MaxBy(evszak=>evszak.Sum(f=>f.osszeg)) // evszakok kozul a legnagyobb az alapjan, hogy az evszakon beluli forgalmak ertekenek osszege
                                                         .First().evszak(); // az evszakon belul tetszoleges (itt az elso) kivalasztasa, annak evszakanak megnezese
        Console.WriteLine($"\tA legnagyobb beszerzes evszaka a(z) {legnagyobbBeszerzesEvszak} volt.");

        // --------------------4. feladat
        System.Console.WriteLine("4. feladat");

        System.Console.WriteLine("\tValasszon kategoriat:");
        System.Console.WriteLine("\t1. Noi ruhak");
        System.Console.WriteLine("\t2. Ferfi ruhak");
        System.Console.WriteLine("\t3. Gyermekruhak");
        int beolvasott = 0;
        while(true) { // vegtelensegig ismeteljuk
            char beolvasottKarakter = Console.ReadKey(true).KeyChar; // karakter beolvasas parancssorba kijelzes nelkul
            if ('1' <= beolvasottKarakter && beolvasottKarakter <= '3') { // itt meg karakterekkel kell vizsgalni az intervallumot
                beolvasott = Convert.ToInt32(beolvasottKarakter.ToString()); // a ToString sajnos kell, mert ha kozvetlenul karaktert alakitasz int-e, mast csinal, mint amit szeretnel.
                break; // kilepes a vegtelen ciklusbol
            }
        }
                                                            // elso 10 nap akcios    // 15000 alatti   // kivalasztott kategoria
        Forgalom legkorabbiAkciosOlcso = csakeladasok.FirstOrDefault(e => e.datum.Day <= 10 && e.osszeg < 15000 && e.kategoria == beolvasott); // FirstOrDefault ugyan az mint a First, de ha nincs elem, ami megfelel, null-t ad vissza
        if (legkorabbiAkciosOlcso != null) {
                                        // datumot szovegge alakitjuk MMMM (hosszu honap nev) formatumba                               // uj listat csinalok, aminek elemei a kategoriak nevei, es ebbol valasztom ki a megfelelo nevet a kategoria szama alapjan
        System.Console.WriteLine($"Mar {legkorabbiAkciosOlcso.datum.ToString("MMMM")} akcios napjaiban elofordult 15000 Ft alatti eladas {new List<String>{"noi", "ferfi", "gyermek"}[beolvasott-1]} ruhakbol");
        } else {
        System.Console.WriteLine("Nincs a megadott kategoriaban 15000Ft alatti akcios eladas"); // feladat mintaja el van baszva, ferfi kategoriaban gecire nincs se februarba se semmikor akcios eladas 15k alatt...
        }

        //-------------------- 5. feladat
        System.Console.WriteLine("5. feladat");

        List<Forgalom> havontaElsoEladas= forgalmak.Where(e=> e.tipus == "E")
                                                   .GroupBy(e=>e.datum.Month) // ez csinal egy List<List<Forgalom>>-t, honapok alapjan kulon listakra szedi
                                                   .Select(e=>e.First()) // Mindegyik honap listajabol elso erdekel minket
                                                   .ToList();
        try {
            File.WriteAllLines(@"elsok.csv",
                havontaElsoEladas.Select(e=> $"{e.datum.ToString("MMMM")};{e.osszeg}") // szovegge alakitjuk a megadott formatumban
            );
            System.Console.WriteLine("\tA fajl kiirasa sikeres volt.");
            }
        catch {
            System.Console.WriteLine("\tFajl irasi hiba!");
        }
    }
}
