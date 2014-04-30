using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;

//A ParallelInvoke
namespace TestThread
{
    class Program93a
    {
        static void Main93(string[] args)
        {
            if (!File.Exists("WordLookup.txt"))    // Contains about 150,000 words
                new WebClient().DownloadFile(
                  "http://www.albahari.com/ispell/allwords.txt", "WordLookup.txt");

            var wordLookup = new HashSet<string>(
              File.ReadAllLines("WordLookup.txt"),
              StringComparer.InvariantCultureIgnoreCase);

            var random = new Random();
            string[] wordList = wordLookup.ToArray();

            string[] wordsToTest = Enumerable.Range(0, 1000000)
              .Select(i => wordList[random.Next(0, wordList.Length)])
              .ToArray();

            wordsToTest[12345] = "cat";     // Introduce a couple
            wordsToTest[23456] = "dodg";     // of spelling mistakes.

            var query = wordsToTest.AsParallel()
                    .Select((word, index) => new IndexedWord { Word = word, Index = index })
                    .Where(iword => !wordLookup.Contains(iword.Word))
                    .OrderBy(iword => iword.Index);

            foreach (var item in query)
                Console.WriteLine("{0}:{1}",item.Word,item.Index );
            
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
    struct IndexedWord { public string Word; public int Index; }
}
