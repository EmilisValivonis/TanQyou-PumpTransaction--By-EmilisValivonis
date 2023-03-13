using HtmlAgilityPack;
using Timer = System.Timers.Timer;
using System.Timers;

internal class Program
{
    public static Dictionary<string, double> fuelPrices = new Dictionary<string, double>(){
        { "95", 0 },
        { "98", 0 },
        { "Diesel", 0 }
    };

    public static Dictionary<string, string> fuelIds = new Dictionary<string, string>(){
        { "95", "2" },
        { "98", "4203" },
        { "Diesel", "1" }
    };

    private static void Main(string[] args)
    {
        GetFuelPrices();

        Timer aTimer = new System.Timers.Timer(60 * 60 * 1000); //one hour in milliseconds
        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        aTimer.Start();

        Console.WriteLine("Press any key to get pump transaction...");
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        while (keyInfo.Key != ConsoleKey.Escape)
        {
            Random rand = new Random();

            KeyValuePair<string, double> fuel = fuelPrices.ElementAt(rand.Next(0, fuelPrices.Count));

            int amount = rand.Next(5, 65);

            Console.WriteLine("Price: " + Math.Round((fuel.Value * amount), 2) + " Eur");
            Console.WriteLine("Amount filled: " + amount.ToString() + " L");
            Console.WriteLine("Current price per liter: " + fuel.Value.ToString() + " Eur");
            Console.WriteLine("Timestamp " + DateTime.Now.ToString());
            Console.WriteLine("Fuel ID: " + fuelIds[fuel.Key]);
            Console.WriteLine("Fuel: " + fuel.Key);

            keyInfo = Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Press any key to get pump transaction...");
        }
    }

    static void GetFuelPrices()
    {
        HtmlAgilityPack.HtmlWeb website = new HtmlAgilityPack.HtmlWeb();
        HtmlAgilityPack.HtmlDocument document = website.Load("https://carbu.com//belgie/maximumprijs");

        Dictionary<string, string> fuelTableRowNumber = new Dictionary<string, string>(){
            { "95", "1" },
            { "98", "2" },
            { "Diesel", "4" }
        };

        foreach (KeyValuePair<string, string> fuel in fuelTableRowNumber)
        {
            HtmlNode node = document.DocumentNode.SelectSingleNode($" //*[@id=\"news\"]/div/div/table[1]/tbody/tr[{fuel.Value}]/td[2]");
            double price = double.Parse(node.InnerText.Remove(7).Replace(",", "."));
            fuelPrices[fuel.Key] = price;
        }
    }

    static void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        GetFuelPrices();
    }
}