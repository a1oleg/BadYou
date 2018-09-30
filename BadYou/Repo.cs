using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace BadYou
{
    public static class Repo
    {
        public static GraphClient db;

        public static void DBconnect()
        {
            if (db == null)
            {
                db = new GraphClient(new Uri("http://localhost:11002/db/data"), "neo4j", "ptybn1");
                //db = new GraphClient(new Uri("https://hobby-cjifkhhaanncgbkeedghmepl.dbs.graphenedb.com:24780/db/data/"), "a1oleg", "b.JgAr7iM4iqi3.PtrraWURkTLBhkB0");
                db.Connect();
            }
        }

        public static void MergePerson(Person person)
        {
            Person p = (Person)person.Clone();
            db.Cypher
            .Merge("(vertex:Person{Name:{name}})")
                    .OnCreate()
                    .Set("vertex = {p}")
                    .WithParams(new
                    {
                        name = p.Name,
                        p
                    })
                    .ExecuteWithoutResults();

            if(person.Girl)
            {
                MergeAndJoin(person.Name, person.City, "City");
                MergeAndJoin(person.Name, person.Country, "Country");

                foreach(string pill in person.Pills)
                    MergeAndJoin(person.Name, pill, "Pill");

                foreach (string lang in person.Languages)
                    MergeAndJoin(person.Name, lang, "Lang");
            }   
        }

        private static void MergeAndJoin(string source, string target, string tag)
        {
            MergeVertex(target, tag);
            MergeRelationship(source, target, tag);
        }

        private static void MergeVertex(string name, string tag)
        {
            db.Cypher
            .Merge($"(t:{tag}{{Name:{{name}}}})")
            .WithParam("name", name)
            .ExecuteWithoutResults();
            //AddLabel(name, tag);
        }

        //private static void AddLabel(string name, string tag)
        //{
        //    db.Cypher
        //    .Match("(vertex)")
        //    .Where((Person vertex) => vertex.Name == name)
        //    .Set($"vertex:{tag}")
        //    .ExecuteWithoutResults();
        //}

        internal static void MergeRelationship(string first, string second, string relname)
        {
            db.Cypher
            .Match("(a)", "(b)")
            .Where((Person a) => a.Name == first)
            .AndWhere((Person b) => b.Name == second)
            .Merge($"(a)-[r:{relname}]->(b)")
            .ExecuteWithoutResults();
        }
    }
}
