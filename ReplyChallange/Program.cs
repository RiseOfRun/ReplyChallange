using System;
using System.Collections.Generic;
using System.IO;

namespace ReplyChallange
{
    abstract class Unit
    {
        public string Company;
        public int Bonus;
        public int[] coords = new int[2];
        public abstract int ComputeTP(Unit neighbour);
    }

    class Developer : Unit
    {
        List<string> Skills = new List<string>();
        public override int ComputeTP(Unit neighbour)
        {
            throw new System.NotImplementedException();
        }
    }

    class ProjectManager : Unit
    {
        public override int ComputeTP(Unit neighbour)
        {
            throw new System.NotImplementedException();
        }
    }
    
    internal class Program
    {
        
        public static void Main(string[] args)
        {
            int width, height;
            List<Unit> units= new List<Unit>();
            //{i,j}
            string[] lines = File.ReadAllLines("a_solar.txt");
            width = int.Parse(lines[0][0].ToString());
            height = int.Parse(lines[0][2].ToString());
            KeyValuePair<char,bool>[,] map =new KeyValuePair<char, bool>[height, width];

            for (int i = 0; i < height; i++)
            {
                int j = 0;
                foreach (var node in lines[i + 1])
                {
                    map[i,j] = new KeyValuePair<char, bool>(node,false);
                    j++;
                }
            }
        }
    }
}