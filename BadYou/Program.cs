using HtmlAgilityPack;
using System;
using System.Linq;

namespace BadYou
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "https://badoo.com/profile/";
            string number = "0352259359";
            //0650343383 заполнено
            //0581386198 не заполнено
            //0352259359 я

            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(path + number);

            string[] title = doc.DocumentNode.SelectSingleNode("/html/head/title").InnerText.Split("|");

            if (title[0] != "Регистрация на Badoo")
            {
                Repo.MergePerson(new Person(doc.DocumentNode));
            }
        }
    }
}
