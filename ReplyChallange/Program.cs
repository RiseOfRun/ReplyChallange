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
        private int n;
        public Developer(string info)
        {
            string[] lines = info.Split(' ');
            Company = lines[0];
            Bonus = int.Parse(lines[1]);
            n = int.Parse(lines[2]);
            for (int i = 0; i < n; i++)
            {
                Skills.Add();
            }
        }

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
        //ToDo Вернуть 2 пары координат 
        public static void Main(string[] args)
        {
            int width, height, userCount, currentLine;
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

            userCount = int.Parse(lines[height + 1]);
            currentLine = height + 2;
            for (int i = 0; i < userCount; i++)
            {
                
                units.Add(new Developer());
            }
            

        }
    }
}