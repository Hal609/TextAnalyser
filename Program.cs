using System;
using System.Diagnostics;

namespace TextAnalyser;

public class Program
{
   const int MAX_WORD_LEN = 28;

   static BinaryWordTree wordsTree = new BinaryWordTree();

   static bool IsLower(char c)
   {
      return (c >= 97) && (c <= 122);
   }
   static bool IsUpper(char c)
   {
      return (c >= 65) && (c <= 90);
   }
   static bool IsAlphabetic(char c)
   {
      return IsUpper(c) || IsLower(c);
   }

   static char ToLower(char c)
   {
      if (IsUpper(c))
         return (char)(c + 32);
      return c;
   }

   static void PrintTitle()
   {
      Console.WriteLine(@"
 ____  ____  _  _  ____      __    _  _    __    __   _  _  ___  ____  ___ 
(_  _)( ___)( \/ )(_  _)    /__\  ( \( )  /__\  (  ) ( \/ )/ __)(_  _)/ __)
  )(   )__)  )  (   )(     /(__)\  )  (  /(__)\  )(__ \  / \__ \ _)(_ \__ \
 (__) (____)(_/\_) (__)   (__)(__)(_)\_)(__)(__)(____)(__) (___/(____)(___/");
      Console.WriteLine("==========================================================================");
      Console.WriteLine("==========================================================================");
   }

   static string GetPathToDirectory()
   {
      string basePath = "/Users/hal/Documents/MScAI/Spring Term/Algorithms and Data Structures/C#/TextAnalyser/";
      string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
      if (strExeFilePath.Contains("LinkedListNew"))
      {
         int index = strExeFilePath.IndexOf("LinkedListNew");
         basePath = strExeFilePath.Substring(0, index) + "LinkedListNew/";
      }
      return basePath;
   }

   static string? PathMenu()
   {
      int choice = 0;
      string? path;
      while (choice > 51 || choice < 49)
      {
         Console.Write(@"
Please select a text file to analyse:

   1. Sherlock Holmes
   2. Moby Dick
   3. Enter path
   Ctrl-C to exit.

Pick number: ");
         string? stringChoice = Console.ReadLine();
         if (!string.IsNullOrEmpty(stringChoice))
            choice = stringChoice[0];
      }

      string basePath = GetPathToDirectory();

      if (choice == '1') path = basePath + "sherlockHolmes.txt";
      else if (choice == '2') path = basePath + "mobyDick.txt";
      else
      {
         Console.Write("Enter path to txt file: ");
         path = Console.ReadLine();
      }

      return path;
   }

   static string PathSelectValidated()
   {
      string? path;
      while (true)
      {
         path = PathMenu();
         if (File.Exists(path)) break;
         Console.WriteLine("File not found - please try again.");
         Console.Read();
      }
      return path;
   }

   static int AnalysisMenu()
   {
      int choice = 0;
      while (choice > 56 || choice < 49)
      {
         Console.Write(@"
File processed, what would you like to do?

   1. Save unique words list to text file.
   2. Display the number of unique words.
   3. Display all unique words.
   4. View the longest word in the file.
   5. View the most frequently occurring word.
   6. View the line number of a specific word.
   7. View the frequency of a specific word.
   Ctrl-C to exit.

Pick number: ");
         string? stringChoice = Console.ReadLine();
         if (!string.IsNullOrEmpty(stringChoice))
            choice = stringChoice[0];
      }

      return choice;
   }

   static void FindLineInFile(string toFind, string path)
   {
      int lineNumber = 0;
      toFind = toFind.ToLower();

      using (StreamReader sr = new StreamReader(path))
      {
         string? line;
         while ((line = sr.ReadLine()) != null)
         {
            lineNumber++;
            string normalisedLine = line.ToLower();

            if (normalisedLine.Contains(toFind))
            {
               Console.WriteLine("The word '{0}' first occurs at line {1}.", toFind, lineNumber);
               return;
            }
         }
      }

      Console.WriteLine("The word '{0}' does not occur in the text file");
   }

   static string Input(string message = "")
   {
      Console.Write(message);
      return Console.ReadLine();
   }

   static void ShowLineLocations()
   {
      string toFind = Input("Enter a word to count: ");
      WordInfo? res = wordsTree.Search(wordsTree.root, toFind);
      if (res is null) Console.WriteLine("The word '{0}' is not present in the text file.", toFind);
      else res.PrintLineLocations();
   }

   static void CountOccurrences()
   {
      string toFind = Input("Enter a word to count: ");
      WordInfo? res = wordsTree.Search(wordsTree.root, toFind);
      if (res is null) Console.WriteLine("The word '{0}' is not present in the text file.", toFind);
      else Console.WriteLine("The word '{0}' occurs {1} times.", toFind, res.occurrences);
   }

   static int Main(string[] args)
   {
      PrintTitle();

      string path = PathSelectValidated();

      var s = new ConsoleSpinner();
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      using (StreamReader sr = new StreamReader(path))
      {
         Console.WriteLine("\nAnalysing file: {0}", path);

         string curWord = ""; // Empty string to hold the current word as it is built
         int x = 0;           // Counter to tack the number of letters in the word
         int lineNumber = 1;  // Counter for the current line of the text file

         while (sr.Peek() > -1)
         {
            // Update spinner to show code in progressing
            if (stopwatch.ElapsedMilliseconds >= 500) { s.UpdateProgress(); stopwatch.Restart(); }

            // Read a character from the file and set it to lower case
            char c = ToLower((char)sr.Read());

            // If the char is alphabet or an apostrophe within a word then add it to the current word string
            if (IsAlphabetic(c) || ((c == '\'') && curWord.Length > 0))
            {
               curWord += c;
               x = Math.Min(x + 1, MAX_WORD_LEN); // Increase the character number unless it reaches the max length
            }
            else if (c == '\n')
               lineNumber++;
            else if (x > 0 && curWord.Length > 0)
            {
               // Remove any trailing apostrophes
               if (curWord.EndsWith("\'")) curWord = curWord.Remove(curWord.Length - 1);

               WordInfo newWord = new WordInfo(curWord, 1, lineNumber);
               wordsTree.Append(newWord, lineNumber);

               curWord = "";
               x = 0;
            }
         }
      }
      while (true)
      {
         int choice = AnalysisMenu();

         Console.WriteLine();
         if (choice == '1') wordsTree.WriteToFile(path.Substring(0, path.Length - 4) + "_uniqueWords.txt");
         if (choice == '2') Console.WriteLine("File has {0} unique words", wordsTree.size);
         if (choice == '3') wordsTree.Print();
         if (choice == '4') wordsTree.PrintLongestWord();
         if (choice == '5') wordsTree.PrintMostCommonWord();
         if (choice == '6') ShowLineLocations();
         if (choice == '7') CountOccurrences();

         Console.Write("\nPress enter to perform more tasks or ctrl-c to exit.  ");
         Console.Read();
      }
   }
}
