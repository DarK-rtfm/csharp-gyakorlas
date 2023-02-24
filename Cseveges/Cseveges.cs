using System;
namespace Cseveges;
class Beszelgetes
{
    static string format = "yy.MM.dd-HH:mm:ss";
    public DateTime kezdet;
    public DateTime veg;
    public String kezdemenyezo;
    public String fogado;
    public Beszelgetes(string sor)
    {
        string[] oszlopok = sor.Split(';');
        kezdet = DateTime.ParseExact(oszlopok[0], format, System.Globalization.CultureInfo.InvariantCulture);
        veg = DateTime.ParseExact(oszlopok[1], format, System.Globalization.CultureInfo.InvariantCulture);
        kezdemenyezo = oszlopok[2];
        fogado = oszlopok[3];
    }
}