using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace BadYou
{
    public class Person : ICloneable
    {
        public string Name { get; set; }
        
        public string FirstName { get; set; }
        public int Age { get; set; }

        public bool AboutMe { get; set; }

        public string City { get; set; }
        //public bool Girl { get; private set; }

        //public string Country { get; set; }        

        public List<string> Pills { get; set; }        
        public List<string> Languages { get; set; }

        public List<string> ImgLinks { get; set; }



        public Person(HtmlNode nodes, string number)
        {
            Name = number;
            string[] title = nodes.SelectSingleNode("/html/head/title").InnerText.Split("|");
            string[] person = title[0].Split(",");
            string[] location = title[1].Split(",");            
            

            FirstName = person[0];
            Age = Int32.Parse(person[2]);
            City = location[0];
            //Girl = DelSpace(person[1]) == "Женщина";
            //Country = DelSpace(location[1]);

            //var profilesection = doc.DocumentNode.SelectNodes("//span[@class=\"profile-section__txt\"]").ToList();
            string xPath = "/html[1]/body[1]/div[2]/div[1]/main[1]/div[1]/section[1]/div[2]/div[2]/section[1]/div[8]/div[1]/div[2]/div[1]/span[1]";

            AboutMe = nodes.SelectSingleNode(xPath) != null;

            Pills = new List<string>();
            if (nodes.SelectNodes("//span[@class=\"pill__text\"]") != null)
            {
                foreach (var pill in nodes.SelectNodes("//span[@class=\"pill__text\"]"))
                    Pills.Add(pill.InnerText);
            }

            Languages = new List<string>();

            if (nodes.SelectNodes("//h2[@class=\"profile-section__title-text\"]") != null)
            {
                foreach (var lang in nodes.SelectNodes("//h2[@class=\"profile-section__title-text\"]").Last().ChildNodes)
                    Languages.Add(lang.InnerText);
            }               
            

            ImgLinks = new List<string>();
            if (nodes.SelectNodes("//div[@class=\"photo-list__img_\"]") != null)
            {
                foreach (var img in nodes.SelectNodes("//div[@class=\"photo-list__img_\"]"))
                {
                    var x = img.OuterHtml.Split("<img src=\"")[1].Split("\" alt=\"")[0];
                    ImgLinks.Add(x);
                }
            }            
        }

        public Person()
        {
        }

        public static string DelSpace(string line) => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).First();

        public object Clone()
        {
            return new Person()
            {
                Name = this.Name,
                FirstName = this.FirstName,                
                Age = this.Age,
                
                AboutMe = this.AboutMe,
                ImgLinks = this.ImgLinks,                
            };             
        }
    }
}
