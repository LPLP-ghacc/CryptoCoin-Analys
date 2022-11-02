using CryptoCoinAnalys;
using Newtonsoft.Json;
using System.Net;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;

        //get the coins sorted
        var coins = JsonExtender.OpenJson().OrderByDescending(x => x.changePercent24Hr);

        //create new tableprinter for cool outputting
        TablePrinter? tablePrinter = new TablePrinter("symbol", "Coin name", "Price $", "Changes", "Rank");

        foreach (var coin in coins)
        {
            tablePrinter.AddRow(coin.symbol, coin.name, "$ " + coin.priceUsd, coin.changePercent24Hr + " %", coin.rank);
        }

        tablePrinter.Print();

        Console.ReadLine();
    }

    public static class JsonExtender
    {
        public static List<Coin> OpenJson()
        {
            //send request
            HttpWebRequest? httpRequest = (HttpWebRequest)WebRequest.Create("http://api.coincap.io/v2/assets");

            //take response
            HttpWebResponse? httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var json = streamReader.ReadToEnd();

                //deserialize as dynamic
                dynamic? dyn = JsonConvert.DeserializeObject(json);

                var coin = new List<Coin>();

                foreach (var item in dyn.data)
                {
                    //create new coin
                    coin.Add(new Coin()
                    {
                        id = item.id,
                        rank = item.rank,
                        symbol = item.symbol,
                        name = item.name,
                        //supply = item.supply,
                        //maxSupply = item.maxSupply,
                        marketCapUsd = item.marketCapUsd,
                        volumeUsd24Hr = item.volumeUsd24Hr,
                        priceUsd = item.priceUsd,
                        changePercent24Hr = item.changePercent24Hr,
                        //vwap24Hr = item.vwap24Hr
                    });
                }
                return coin;
            }
        }
    }

    public class Coin
    {
        public string? id { get; set; }
        public string? rank { get; set; }
        public string? symbol { get; set; }
        public string? name { get; set; }
        public float? supply { get; set; }
        public float? maxSupply { get; set; }
        public float? marketCapUsd { get; set; }
        public float? volumeUsd24Hr { get; set; }
        public float? priceUsd { get; set; }
        public float? changePercent24Hr { get; set; }
        public float? vwap24Hr { get; set; }
    }

    //pipah-pipaah?! 
}
