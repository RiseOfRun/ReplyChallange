using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;

namespace ReplyChallange
{
    abstract class Unit
    {
        public bool OnWork = false;
        public int Name;
        public string Company;
        public int Bonus;
        public int[] Coords = new int[2];
        public int ComputeTP(Unit neighbour)
        {
            int WP = 0, BP = 0, SU =0, S=0;
            
            foreach (var skill in neighbour.Skills)
            {
                if (Skills.Exists(x =>x == skill))
                {
                    S++;
                }
                else
                {
                    SU++;
                }
            }
            SU += Skills.Capacity;    
            
            WP = SU * S;
            if (Company == neighbour.Company)
            {
                BP = Bonus * neighbour.Bonus;
            }

            return WP + BP;
        }
        public List<string> Skills = new List<string>();
    }

    class Developer : Unit
    {
        private int n;
        public Developer(string info)
        {
            string[] lines = info.Split(' ');
            Company = lines[0];
            Bonus = int.Parse(lines[1]);
            n = int.Parse(lines[2]);
            for (int i = 0; i < n; i++)
            {
                Skills.Add(lines[3+i]);
            }
        }
    }

    class ProjectManager : Unit
    {
        public ProjectManager(string info)
        {
            string[] lines = info.Split(' ');
            Company = lines[0];
            Bonus = int.Parse(lines[1]);
        }
    }
    
    internal class Program
    {
        static List<Unit> units= new List<Unit>();
        static List<Unit> TakeBestTeam()
        {
            int n = units.Capacity;
            int Max = 0;
            List<Unit> BestPair =new List<Unit>();
            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    int currentScore = units[i].ComputeTP(units[j]);
                    if (currentScore>=Max)
                    {
                        BestPair = new List<Unit>{units[i], units[j]};
                    }
                }
            }

            return BestPair;
        }
        //ToDo Вернуть 2 пары координат 
        public static void Main(string[] args)
        {
            int width, height, unitsCount, currentLine;
            //{i,j}
            string[] lines = File.ReadAllLines("f_glitch.txt");
            width = int.Parse(lines[0].Split(' ')[0]);
            height = int.Parse(lines[0].Split(' ')[1]);
            KeyValuePair<char, bool>[,] map = new KeyValuePair<char, bool>[height, width];

            for (int i = 0; i < height; i++)
            {
                int j = 0;
                foreach (var node in lines[i + 1])
                {
                    map[i, j] = new KeyValuePair<char, bool>(node, false);
                    j++;
                }
            }

            unitsCount = int.Parse(lines[height + 1]);
            currentLine = height + 2;
            for (int i = 0; i < unitsCount; i++)
            {
                units.Add(new Developer(lines[i + currentLine]));
            }

            currentLine = currentLine + unitsCount;
            unitsCount = int.Parse(lines[currentLine]);
            currentLine++;
            for (int i = 0; i < unitsCount; i++)
            {
                units.Add(new ProjectManager(lines[currentLine + i]));
            }

            unitsCount = units.Count;
        }
    }
}