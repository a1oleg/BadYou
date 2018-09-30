using HtmlAgilityPack;
using System;
using System.Linq;

namespace BadYou
{
    class Program
    {
        static void Main(string[] args)
        {
            Repo.DBconnect();

            string path = "https://badoo.com/profile/";

            //long start = 650347988;            
            int count = 3000;
            //0650343383 заполнено
            //0581386198 не заполнено
            //0352259359 я

            string start = Repo.GetLast();

            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = null;
            for (int i = 0; i < count; i++)
            {                
                string number = start + i.ToString();

                try
                {
                    doc = hw.Load(path + number);
                }
                catch (Exception ex)
                {
                    Repo.UpdateProp("Last", "FirstName", (start + i).ToString());
                    break;
                }                

                string[] title = doc.DocumentNode.SelectSingleNode("/html/head/title").InnerText.Split("|");

                if (title.Count() == 3 && title[0].Split(",")[1] != " Мужчина")
                {
                    Person p = new Person(doc.DocumentNode, number);

                    if (p.City == " Санкт-Петербург" && p.ImgLinks.Any() && p.Age < 40/*&& p.AboutMe*/)
                    {
                        Repo.MergePerson(p);
                    }
                }
                if (i == count)
                    Repo.UpdateProp("Last", "FirstName", (start + count).ToString());
            }            
        }
    }
}


//if (title[0] == "Badoo")
//{
//    Repo.MergeVertex(number, "not_found");
//}

//else if (title[0] == "Регистрация на Badoo")
//{
//    Repo.MergeVertex(number, "closed");
//}

//else if (title[0].Split(",")[1] == " Мужчина")
//{
//    Repo.MergeVertex(number, "man");
//}     