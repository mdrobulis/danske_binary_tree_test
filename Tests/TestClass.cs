using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using binaryTree;
namespace Tests
{
    [TestFixture]
    public class TestClass
    {

        const string source = 
@"215
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

        const string test_source =
@"1
8 9
1 5 9
4 5 2 3";

        [Test]
        public void TestParsing()
        {
            var tree = Program.GetTreeFromString(test_source);
            Assert.AreEqual(tree.Length, 4);            
        }
        [Test]
        public void TestPathLenght()
        {
            var tree = Program.GetTreeFromString(test_source);
            var paths = Program.GetPaths(tree);

            foreach(var p in paths)
            {
                Console.WriteLine(Program.PathValuesToSting(Program.PathValues(tree, p)));

                Assert.AreEqual(4, p.Length);

            }
            Assert.Greater(paths.Count(), 1);

        }
        [Test]
        public void TestPathOddEven()
        {
            var tree = Program.GetTreeFromString(test_source);
            var paths = Program.GetPaths(tree);
                        
            foreach (var p in paths)
            {
                var even = Program.IsEven(Program.getValue(tree, p[0]));

                for (int i = 1; i < p.Length; i++)
                {
                    if (even)
                        Assert.True(!Program.IsEven(Program.getValue(tree, p[i])));
                    else
                        Assert.True(Program.IsEven(Program.getValue(tree, p[i])));
                    even = Program.IsEven(Program.getValue(tree, p[i]));
                }
            }
        }


    }
}
