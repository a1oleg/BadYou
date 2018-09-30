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

            db.Cypher
                    .Merge("(vertex:Person{Name:{name}})")
                    .OnCreate()
                    .Set("vertex = {person}")
                    .WithParams(new
                    {
                        name = person.Name,
                        person
                    })
                    .ExecuteWithoutResults();
        }

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
