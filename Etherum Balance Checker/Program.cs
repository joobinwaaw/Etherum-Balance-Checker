using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Newtonsoft.Json;
using Nethereum.Web3;

public class Program
{

    /*/
     * This Code is for Educational Purposes.
    /*/
    
    public static decimal GetEthPrice()
    {

        /*/
         * how many dollars is eth
         * https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=USD
         * You can find out here
         * Replace config.json file with the actual value.
        /*/
        using (StreamReader r = new StreamReader("config.json"))
        {
            string json = r.ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(json);
            return Convert.ToDecimal(data["ethereum_price_usd"]);
        }
    }

    public static decimal GetEthBalance(string address, Nethereum.Web3.Web3 web3)
    {
        BigInteger balanceWei = web3.Eth.GetBalance.SendRequestAsync(address).Result;
        decimal balanceEth = Web3.Convert.FromWei(balanceWei);
        return balanceEth;
    }

    public static void Main(string[] args)
    {
        var web3 = new Web3("https://eth-mainnet.alchemyapi.io/v2/SWnnr6RA00IpzVBqxszTH44af_QbWrN1"); // Replace with your own API Addresss

        var balanceAddresses = new List<string>();
        using (StreamReader file = new StreamReader("eth.txt"))
        {
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string address = line.Trim();

                decimal balanceEth = GetEthBalance(address, web3);

                decimal ethPrice = GetEthPrice();
                decimal balanceDollar = balanceEth * ethPrice;
                if (balanceDollar > 1) // <<< change this part to the value you want to save addresses over how many dollars. >>>
                {
                    balanceAddresses.Add(address);
                }
               
                Console.WriteLine($"Address: {address} | {balanceEth:N18} | {balanceDollar:N2} ETH");
                
                /*/
                 * 
                 * It is recommended to close the console (put it in the comment line) for the process to be fast.
                 * 
                /*/
            }
        }

        

        using (StreamWriter outputFile = new StreamWriter("high_balance.txt"))
        {
            foreach (string address in balanceAddresses)
            {
                outputFile.WriteLine(address);
            }
        }

        Console.ReadKey();  // Console Active
    }
}