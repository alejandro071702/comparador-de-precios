using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comparador_de_precios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool doingQuery = false;
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (doingQuery) return;
            doingQuery = true;
            query(textBox1.Text);
            NameList.Items.Clear();
            PriceList.Items.Clear();
            SellerList.Items.Clear();
        }

        string bodySEO = "";
        string pageBody = "";
        async void query(string thing)
        {
            bodySEO = "";
            pageBody = "";
            await WebRequestSEO(@"https://google.com/search?q=", thing, "%20", "&sclient=products-cc&udm=28");
            //showProducts(bodySEO);
            //listBox1.Items.Clear();
            await WebRequestSEO(@"https://google.com/search?q=", thing, "%20", "&sclient=products-cc&udm=28&start=20");
            //showProducts(bodySEO);
            await WebRequestSEO(@"https://google.com/search?q=", "Mercado Libre" + thing, "%20", "&sclient=products-cc&udm=28&shoprs=CAEYAioLbWVtb3JpYSByYW0yEQgCEg1NZXJjYWRvIExpYnJlWLexIGAB");
            showProducts(bodySEO, "Mercado Libre");
            await WebRequestSEO(@"https://google.com/search?q=", "mercado libre" + thing, "%20", "&sclient=products-cc&udm=28&&shoprs=CAEYAioLbWVtb3JpYSByYW0yEQgCEg1NZXJjYWRvIExpYnJlWLexIGABstart=20");
            showProducts(bodySEO, "Mercado Libre");
            await WebRequestSEO(@"https://google.com/search?q=", "mercado libre" + thing, "%20", "&sclient=products-cc&udm=28&&shoprs=CAEYAioLbWVtb3JpYSByYW0yEQgCEg1NZXJjYWRvIExpYnJlWLexIGABstart=40");
            showProducts(bodySEO, "Mercado Libre");
            //await WebRequestSEO(@"https://google.com/search?q=", thing + "Mercado Libre", "%20", "&sclient=products-cc&udm=28&start=40");
            //showProducts(bodySEO, "Mercado Libre");
            //await WebRequestSEO(@"https://google.com/search?q=", thing + "Mercado Libre", "%20", "&sclient=products-cc&udm=28&start=60");
            //showProducts(bodySEO, "Mercado Libre");
            //showMercadoLibre(bodySEO);
            await WebRequestSEO(@"https://google.com/search?q=", "cyberpuerta" + thing, "%20", "&sclient=products-cc&udm=28&shoprs=CAEYCioXY3liZXJwdWVydGEgbWVtb3JpYSByYW0yBwgKEgNTw61Yt7EgYAI");
            showProducts(bodySEO, "Cyberpuerta");
            await WebRequestSEO(@"https://google.com/search?q=", "cyberpuerta" + thing, "%20", "&sclient=products-cc&udm=28&start=20&shoprs=CAEYCioXY3liZXJwdWVydGEgbWVtb3JpYSByYW0yBwgKEgNTw61Yt7EgYAI");
            showProducts(bodySEO, "Cyberpuerta");
            //await WebRequestSEO(@"https://google.com/search?q=", "Cyberpuerta" + thing, "%20", "&sclient=products-cc&udm=28&start=40");
            //showProducts(bodySEO, "Cyberpuerta");
            await WebRequestSEO(@"https://google.com/search?q=", thing + "Walmart", "%20", "&sclient=products-cc&udm=28&&sxsrf=ADLYWILdKSu0bgpFFriB6TnSqeAa5NW5iQ%3A1730825593696&ei=eU0qZ4OkKp7KkPIP8b_J4Aw&ved=0ahUKEwjDxLjl08WJAxUeJUQIHfFfEswQ4dUDCBA&uact=5&oq=walmart+memoria+ram&gs_lp=Ehlnd3Mtd2l6LW1vZGVsZXNzLXNob3BwaW5nIhN3YWxtYXJ0IG1lbW9yaWEgcmFtSJeSAVC3cljIkAFwBngBkAEAmAHHA6AB0A-qAQkxLjguMi4wLjG4AQPIAQD4AQGYAgKgAg7CAgoQABiwAxjWBBhHmAMAiAYBkAYIkgcBMqAHmhs");
            showProducts(bodySEO, "Walmart");
            //showMercadoLibre(bodySEO);

            if (PriceList.Items.Count > 0)
            {

                string price = PriceList.Items[0].ToString().Replace(",", "");
                price = price.Substring(4);
                double bestPrice = double.Parse(price);
                int bestIndex = 0;
                for (int i = 1; i < PriceList.Items.Count; i++)
                {
                    price = PriceList.Items[i].ToString().Replace(",", "");
                    price = price.Substring(4);
                    double dPrice = double.Parse(price);
                    if (bestPrice > dPrice)
                    {
                        bestIndex = i;
                        bestPrice = dPrice;
                    }
                }

                listBox1.Items.Add(NameList.Items[bestIndex]);
                listBox1.Items.Add(PriceList.Items[bestIndex]);
                listBox1.Items.Add(SellerList.Items[bestIndex]);
            } else
            {
                MessageBox.Show("Producto no encontrado");
            }
            doingQuery = false;
        }

        static readonly HttpClient client = new HttpClient();

        async Task WebRequestSEO(string seo, string query, string space = "%20", string flags = "")
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            string url = seo + query.Replace(" ", space) + flags;
            Console.WriteLine(url);
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                using (StreamWriter outputFile = new StreamWriter("Google.html"))
                {
                    outputFile.WriteLine(responseBody);
                }
                bodySEO = responseBody;
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
                //MessageBox.Show("Page not found", "Alert");
                return;
            }
            Console.WriteLine("Finish" + url);
        }

        async Task WebRequest(string url)
        {
            pageBody = "";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            Console.WriteLine(url);
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                pageBody = responseBody;
            }
            catch (HttpRequestException e)
            {
                //Console.WriteLine("\nException Caught!");
                //Console.WriteLine("Message :{0} ", e.Message);
                //MessageBox.Show("Page not found", "Alert");
                pageBody = "";
                return;
            }
            Console.WriteLine("Finish" + url);
        }

        void showProducts(string responseBody)
        {
            NameList.Items.Clear();
            PriceList.Items.Clear();
            SellerList.Items.Clear();

            for (int startIndex = responseBody.IndexOf(@"xcR77""><d");
                startIndex != -1;
                startIndex = responseBody.IndexOf("xcR77", startIndex + 1))
            {
                //Product Name
                int startName = responseBody.IndexOf("rgHvZc", startIndex) + "rgHvZc".Length + 3;
                int endName = responseBody.IndexOf("</", startName);
                startName = responseBody.LastIndexOf(">", endName) + 1;

                string name = responseBody.Substring(startName, endName - startName);

                if (name.IndexOf("Google") != -1) continue;

                //Cost
                int startPrice = responseBody.IndexOf("HRLxBb", startIndex);
                int endPrice = responseBody.IndexOf("</", startPrice);
                startPrice = responseBody.LastIndexOf(">", endPrice) + 1;

                string price = responseBody.Substring(startPrice, endPrice - startPrice);

                if (price.IndexOf("mensuales") != price.IndexOf("impuestos")) continue;

                //Seller
                int startSeller = responseBody.IndexOf("> ", endPrice) + 2;
                int endSeller = responseBody.IndexOf("<", startSeller);

                string seller = responseBody.Substring(startSeller, endSeller - startSeller);

                if (seller.IndexOf("Mercado") != -1 ||
                    seller.IndexOf("Cyber") != -1 ||
                    seller.IndexOf("Walmaer") != -1)
                {
                    continue;
                }




                Console.WriteLine("{0}\r\n{1}\r\n{2}", seller, name, price);

                NameList.Items.Add(name);
                PriceList.Items.Add(price);
                SellerList.Items.Add(seller);
            }
        }

        void showProducts(string responseBody, string sellerValidator)
        {
            //NameList.Items.Clear();
            //PriceList.Items.Clear();
            //SellerList.Items.Clear();

            for (int startIndex = responseBody.IndexOf(@"xcR77""><d");
                startIndex != -1;
                startIndex = responseBody.IndexOf("xcR77", startIndex + 1))
            {
                //Product Name
                int startName = responseBody.IndexOf("rgHvZc", startIndex) + "rgHvZc".Length + 3;
                int endName = responseBody.IndexOf("</", startName);
                startName = responseBody.LastIndexOf(">", endName) + 1;

                string name = responseBody.Substring(startName, endName - startName);

                if (name.IndexOf("Google") != -1) continue;

                //Cost
                int startPrice = responseBody.IndexOf("HRLxBb", startIndex);
                int endPrice = responseBody.IndexOf("</", startPrice);
                startPrice = responseBody.LastIndexOf(">", endPrice) + 1;

                string price = responseBody.Substring(startPrice, endPrice - startPrice);

                if (price.IndexOf("mensuales") != price.IndexOf("impuestos")) continue;

                //Seller
                int startSeller = responseBody.IndexOf("> ", endPrice) + 2;
                int endSeller = responseBody.IndexOf("<", startSeller);

                string seller = responseBody.Substring(startSeller, endSeller - startSeller);

                if (seller.IndexOf(sellerValidator) == -1) continue;

                Console.WriteLine("{0}\r\n{1}\r\n{2}", seller, name, price);

                NameList.Items.Add(name);
                PriceList.Items.Add(price);
                SellerList.Items.Add(seller);
            }
        }
        async void showMercadoLibre(string responseBody)
        {
            int c = 0;
            MessageBox.Show(responseBody);
            //MessageBox.Show("Bero");

            textBox2.Text = responseBody;

            for (int start = responseBody.IndexOf(@"mercadolibre.com.mx"); start != -1; start = responseBody.IndexOf(@"mercadolibre.com.mx", start + 1))
            {
                int startURL = responseBody.LastIndexOf("https://", start);
                //if (startURL + 10 < start) continue;
                int endURL = responseBody.IndexOf(@"%3", start);

                string url = responseBody.Substring(startURL, endURL - startURL);

                await WebRequest(url);
                //MessageBox.Show(url);

                if (pageBody == "")
                {
                    MessageBox.Show("NOT FOUND");
                    continue;
                }

                //Name
                int nameStart = pageBody.IndexOf("ui-pdp-title");
                int nameEnd = pageBody.IndexOf("</", nameStart);
                nameStart = pageBody.LastIndexOf(">", nameEnd);

                string name = pageBody.Substring(nameStart, nameEnd - nameStart);

                //Price
                int priceStart = pageBody.IndexOf("itemprop=\"price");
                if (priceStart == -1) continue;
                int priceEnd = pageBody.IndexOf("\">", priceStart);
                priceStart = pageBody.LastIndexOf("\"", priceEnd);

                string price = pageBody.Substring(priceStart, priceEnd - priceStart);

                //MessageBox.Show("HEre");
                //MessageBox.Show(name + "\r\n" + price + "\r\n");

                NameList.Items.Add(name);
                PriceList.Items.Add(price);
                SellerList.Items.Add(" en Mercado Libre");
            }
            //MessageBox.Show("Tjer");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
