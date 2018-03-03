using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace binaryTree
{


    public class Program
    {
        const string source = @"215
                   192 124
                   117 269 442
                   218 836 347 235
                   320 805 522 417 345
                   229 601 728 835 133 124
                   248 202 277 433 207 263 257
                   359 464 504 528 516 716 871 182
                   461 441 426 656 863 560 380 171 923
                   381 348 573 533 448 632 387 176 975 449
                   223 711 445 645 245 543 931 532 937 541 444
                   330 131 333 928 376 733 017 778 839 168 197 197
                   131 171 522 137 217 224 291 413 528 520 227 229 928
                   223 626 034 683 839 052 627 310 713 999 629 817 410 121
                   924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

        static void Main(string[] args)
        {

            var tree_source = GetTreeFromString(source);

            
            Console.WriteLine("== STARTING TREE ==");
            Console.WriteLine();
            Console.WriteLine(source);
            Console.WriteLine();
            var paths = GetPaths(tree_source);

#if DEBUG
            Console.WriteLine("== VALID PATHS ==");

            foreach (var p in paths)
            {
                Console.WriteLine(Program.PathValuesToSting(Program.PathValues(tree_source, p)));            
            }
            Console.WriteLine();
#endif
            Console.WriteLine("== RESULT ==");

            var maxPath = getMaxPath(tree_source,paths);

            Console.WriteLine("Max Path :{0}", PathValuesToSting(PathValues(tree_source, maxPath.Item2)));
            Console.WriteLine("Max Value :{0}", maxPath.Item1);
            
            Console.ReadLine();


        }

        public static int[][] GetTreeFromString(string source)
        {
            var lines = source.Split('\n');

            var tree_source = lines.Select(line => line.Split(' ').Where(x => x.Length > 0).Select(x => int.Parse(x)).ToArray()).ToArray();

            return tree_source;
        }

        public static bool IsEven(int val)
        {
            return val % 2 == 0;
        }

        public static string PathValuesToSting(int[] path)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");            
            sb.Append(string.Join("->", path));
            
            sb.Append("]");

            return sb.ToString();
        }

        public static int[] PathValues(int[][] tree, Dot[] path)
        {
            int[] res = new int[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                res[i] = getValue(tree, path[i]);
            }
            return res;
        }

        public static int getValue(int[][] tree, Dot p)
        {
            return tree[p.vertical][p.horizontal];
        }

        public static Tuple<Dot,Dot> GetChindren(int[][] tree, Dot p)
        {
            var p1 = Dot.Make(p.vertical + 1, p.horizontal);
            var p2 = Dot.Make(p.vertical + 1, p.horizontal + 1);
            return new Tuple<Dot, Dot>( p1, p2 );
        }

        public static int GetPathSum(int[][] tree, Dot[] path)
        {
            return path.Select(x => getValue(tree, x)).Sum();
        }

        public static bool isBottom(int[][] tree, Dot p)
        {
            if (p.vertical == tree.Length - 1)
                return true;
            return false;
        }

        public static List<Dot[]> GetPathsRecursive(int[][] tree, Dot[] path, Dot root, bool isOdd)
        {
            List<Dot[]> res = new List<Dot[]>(0);

            var pathlist = path.ToList();
            pathlist.Add(root);

            if (isBottom(tree, root))
            {
                res.Add(pathlist.ToArray());
                return res;
            }

            
            var kids = GetChindren(tree, root);

            if (isOdd)
            {
                if (IsEven(getValue(tree, kids.Item1)))                
                    res.AddRange(GetPathsRecursive(tree, pathlist.ToArray(), kids.Item1, false));
                if (IsEven(getValue(tree, kids.Item2)))
                    res.AddRange(GetPathsRecursive(tree, pathlist.ToArray(), kids.Item2, false));
                

            } else
            {
                if(!IsEven(getValue(tree,kids.Item1)))
                    res.AddRange(GetPathsRecursive(tree, pathlist.ToArray(), kids.Item1, true));
                if (!IsEven(getValue(tree, kids.Item2)))
                    res.AddRange(GetPathsRecursive(tree, pathlist.ToArray(), kids.Item2, true));
            }

            

            


            return res;
        }
        

        public static Dot[][] GetPaths(int[][] tree)
        {
            var start = new Dot() { horizontal = 0, vertical = 0 };
            var emprypath = new Dot[] { };
            var paths = GetPathsRecursive(tree, emprypath, start, !IsEven(getValue(tree, start))).ToArray();
            return paths;
        }

        public static Tuple<int, Dot[]> getMaxPath(int[][] tree,IEnumerable<Dot[]> paths)
        {            
            int currentMaxVal = GetPathSum(tree, paths.First());
            Dot[] currentMaxPath = paths.First();
            foreach (Dot[] path in paths.Skip(1))
            {
                int sum = GetPathSum(tree, path);

                if (sum > currentMaxVal)
                {
                    currentMaxPath = path;
                    currentMaxVal = sum;
                }
            }
            return new Tuple<int, Dot[]>(currentMaxVal, currentMaxPath);
        }

    }

    public class Dot
    {
        public int vertical;
        public int horizontal;

        public static Dot Make(int v, int h)
        {
            return new Dot() { vertical = v, horizontal = h };
        }
    }


}