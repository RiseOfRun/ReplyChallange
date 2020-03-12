using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;

namespace ReplyChallange
{
    abstract class Unit
    {
        public bool OnWork = false;
        public int Name;
        public char Profession;
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
            Profession = '_';
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
            Profession = 'M';
            string[] lines = info.Split(' ');
            Company = lines[0];
            Bonus = int.Parse(lines[1]);
        }
    }
    
    internal class Program
    {
        static int width;
        static int height;
        static KeyValuePair<char, bool>[,] map;
        private static int[,] returnPairCoordinates(char unit1, char unit2)
            {
                int[,] qq = new int[2,2];
                KeyValuePair<int, int> newPair;
                for (int i = 0; i < height; i++) {
                    for (int j = 0; j < width; j++)
                    {
                        if ((unit1 == '_' && map[i, j].Key == '_'|| unit1 == 'M' && map[i, j].Key == 'M') && map[i, j].Value != true) //if it is a place for developer or Manager
                        {
                            newPair = SomePairExist(i, j, unit2);
                            if (newPair.Key != i && newPair.Value != j) // if there is some place for other developer
                            {
                                KeyValuePair<char, bool> q = map[i, j];
                                map[i, j] = new KeyValuePair<char, bool>(q.Key, true);
                                return new int[,]{{i,j},{newPair.Key, newPair.Value}};
                            }
                            else
                               if ((unit2 == '_' && map[i, j].Key == '_' || unit2 == 'M' && map[i, j].Key == 'M') &&
                                                         map[i, j].Value != true)
                                                     {
                                                         newPair = SomePairExist(i, j, unit1);
                                                         if (newPair.Key != i && newPair.Value != j) // if there is some place for other developer
                                                         {
                                                             KeyValuePair<char, bool> q = map[i, j];
                                                             map[i, j] = new KeyValuePair<char, bool>(q.Key, true);
                                                             return new int[,]{{i,j},{newPair.Key, newPair.Value}};
                                                         }
                                                     }
                        }
                        

                    }
                }

                return null;
            }
            
            static KeyValuePair<int, int> SomePairExist(int i, int j, char unit2)
            {
                KeyValuePair<int, int> firstCoordinates = new KeyValuePair<int, int>(i,j);
                if (j < width - 1)// if its not last element (width)
                {
                    if (unit2 == '_' && map[i, j + 1].Key == '_' && map[i, j+1].Value != true) //if partner is developer -> add (X, Y+1) in list of developers
                        return new KeyValuePair<int, int>(i,j+1);

                    if (unit2 == 'M' && map[i, j + 1].Key == 'M' && map[i, j+1].Value != true) //if partner is developer -> add (X, Y+1) in list of developers
                        return new KeyValuePair<int, int>(i,j+1);
                }

                if (i < height - 1)             // if its not last element (height)
                {
                    if ( map[i, j].Value != true && unit2 == '_' && map[i + 1, j].Key == '_') // if partner is developer  -> add (X+1, Y) in list of developers
                    {
                        return new KeyValuePair<int, int>(i+1,j);
                    }

                    if (map[i, j].Value != true && unit2 == 'M' && map[i + 1, j].Key == 'M') // if it is D->M, add in list of DMs
                    {
                        return new KeyValuePair<int, int>(i+1,j);
                    }
                }
                return firstCoordinates;
            }
            
        private static bool canSetUp = true;

        static bool SetUpPair(List<Unit> pair)
        {
            int[,] pos = returnPairCoordinates(pair[0].Profession, pair[1].Profession);
            if (pos!=null)
            {
                pair[0].Coords = new int[] {pos[0, 0], pos[0, 1]};
                pair[0].OnWork = true;
                pair[1].Coords = new int[] {pos[1, 0], pos[1, 1]};
                pair[1].OnWork = true;
                return true;
            }
            else
            {
                SetUpSingle(pair[0]);
                SetUpSingle(pair[1]);
                return false;
            }
        }

        static bool SetUpSingle(Unit noob)
        {
            bool settedUp = false;
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].OnWork||units[i].Coords[0]==-1&&units[i].Coords[1]==-1)
                {
                    continue;
                }
                KeyValuePair<int, int> pos = SomePairExist(units[i].Coords[0], units[i].Coords[1], noob.Profession);
                    if (pos.Key==noob.Coords[0]&&pos.Value==noob.Coords[1])
                    {
                        continue;
                    }
                    else
                    {
                        settedUp = true;
                        noob.Coords = new int[] {pos.Key, pos.Value};
                        noob.OnWork = true;
                        return true;
                    }
            }
            noob.Coords = new int[] {-1, -1};
            noob.OnWork = true;
            return false;
        }
        
        static List<Unit> units= new List<Unit>();
        static List<Unit> TakeBestTeam()
        {
            int n = units.Count;
            int Max = 0;
            List<Unit> BestPair =new List<Unit>();
            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    if (units[i].OnWork||units[j].OnWork)
                    {
                        continue;
                    }
                    int currentScore = units[i].ComputeTP(units[j]);
                    if (currentScore>=Max)
                    {
                        BestPair = new List<Unit>{units[i], units[j]};
                    }
                }
            }

            return BestPair;
        }
        
        static void fileRider()
        {
            using (StreamWriter sw = new StreamWriter("output.txt", false, System.Text.Encoding.Default))
            {
                foreach (var unit in units)
                {
                    if(unit.Coords[0]!=-1)
                        sw.WriteLine("%d %d",unit.Coords[1], unit.Coords[0]);
                    else sw.WriteLine("X");
                }

            }
        }
        //ToDo Вернуть 2 пары координат 
        public static void Main(string[] args)
        {
            int unitsCount, currentLine;
            //{i,j}
            string[] lines = File.ReadAllLines("a_solar.txt");
            width = int.Parse(lines[0].Split(' ')[0]);
            height = int.Parse(lines[0].Split(' ')[1]);
            map = new KeyValuePair<char, bool>[height, width];
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
            while (!units.All(x =>x.OnWork))
            {
                List<Unit> BestPair = TakeBestTeam();
                if (BestPair.Count!=0)
                {
                    SetUpPair(BestPair);
                }
                else
                {
                    foreach (var noob in units)
                    {
                        if (!noob.OnWork)
                        {
                            SetUpSingle(noob);
                        }
                    }
                }
            }

            fileRider();
        }
    }
}