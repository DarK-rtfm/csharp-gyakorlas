namespace _2018;

class Kor {
    public int jatekos;
    public List<string> dobasok;
    public Kor(string sor) {
        jatekos = Convert.ToInt32(sor.Split(';')[0]);
        dobasok = new List<string>{
                sor.Split(';')[1],
                sor.Split(';')[2],
                sor.Split(';')[3]
        };
    }

}
class Program
{
    static void Main(string[] args)
    {
        IEnumerable<Kor> korok;
        try {
            // A beolvasott fajl minden sorabol lekepzunk egy "Kor"-t
            korok = from sor in File.ReadAllLines(@"dobasok.txt")
                    select new Kor(sor);
            // File.ReadAllLines(...).select(sor => new Kor(sor))
        } catch {
            System.Console.WriteLine("Filebeolvasasi hiba");
            return;
        }

// ----------------------------------------------------------------------
        System.Console.WriteLine("2. feladat");
        System.Console.WriteLine($"Korok szama: {korok.Count()}");

// ----------------------------------------------------------------------
        System.Console.WriteLine("3. feladat");
        // A "Korok" listabol kivalasztjuk (select) azokat a koroket (where) amire igaz, hogy a dobasok kozul a harmadik (0tol szamozva 2) == "D25"
        IEnumerable<Kor> harmadikDobasraBS = from kor in korok
                                             where kor.dobasok[2] == "D25"
                                             select kor;
        // korok.where(kor => kor.dobasok[2]=="D25")
        System.Console.WriteLine($"3. dobasra Bullseye: {harmadikDobasraBS.Count()}");
// ----------------------------------------------------------------------
        System.Console.WriteLine("4. feladat");
        System.Console.Write("Adja meg a szektor erteket! Szektor= ");
        string szektor = System.Console.ReadLine() ?? "";
        int elsoJatekosSzektorban = (
                                        from kor in korok // Azon korokbol
                                        where kor.jatekos == 1 // amikbe az elso jatekos jatszik
                                        select  // kivalasztjuk
                                            (from dobas in kor.dobasok // a koron beluli dobasoknak
                                            where dobas == szektor // amik a szektorban landoltak
                                            select dobas).Count() // a darabszamat
                                    ).Sum(); // es az igy kapott listat (aminek minden eleme az, hogy az adott korben hany dobasa volt a szektorban) osszegezzuk.
        int masodikJatekosSzektorban = (
                                        from kor in korok
                                        where kor.jatekos == 2
                                        select 
                                            (from dobas in kor.dobasok
                                            where dobas == szektor
                                            select dobas).Count()
                                    ).Sum();
        System.Console.WriteLine($"Az 1. jatekos a(z) {szektor} szektoros dobasainak szama: {elsoJatekosSzektorban}");
        System.Console.WriteLine($"Az 2. jatekos a(z) {szektor} szektoros dobasainak szama: {masodikJatekosSzektorban}");
// ----------------------------------------------------------------------
        System.Console.WriteLine("5. feladat");
        int elso180 =
        (
                from kor in korok // A korokbol azon koroket
                where kor.jatekos==1 && kor.dobasok.All(e=>e=="T20") // Amelyekben a dobasok kozul mindegyikre (all) igaz, hogy adott 'e' elemre  e == "T20"
                                                                    // es az elso jatekos kore volt
                select kor // szimplan a feltetel a lenyeg, tehat magat a koroket adjuk vissza
        ).Count(); // A lista elemeinek (azok, amik 180 pontot ertek el) darabszama kell

        int masodik180 =
        (
                from kor in korok
                where kor.jatekos==2 && kor.dobasok.All(e=>e=="T20") 
                select kor
        ).Count();
        System.Console.WriteLine($"Az 1. jatekos {elso180} db 180-ast dobott.");
        System.Console.WriteLine($"A 2. jatekos {masodik180} db 180-ast dobott.");



// ----------------------------------------------------------------------
// Ha mindez lefutott, varjunk egy billentyuleutest, mielott a program bezarodik, hogy a paraszt el tudja olvasni az eredmenyeket
        System.Console.ReadKey();
    }
}
