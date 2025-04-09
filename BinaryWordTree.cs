using System;
using System.Xml;

namespace TextAnalyser
{
   public class BinaryWordTree
   {
      public WordLeaf? root;
      public int size { get; private set; }

      public BinaryWordTree()
      {
         root = null;
         size = 0;
      }

      public void Print() => InOrderPrint(root);


      // Appends a new node with the given value in alphabetical order.
      public void Append(WordInfo value, int lineNum)
      {
         WordLeaf newNode = new WordLeaf(value, null, null);
         if (root is null)
            root = newNode; // Create the root node.
         else
         {
            WordLeaf currentNode = root;
            while (true)
            {
               int valDif = newNode.Value.CompareTo(currentNode.Value);

               if (valDif < 0) // This means that newNode value is less than currentNode
               {
                  // New node is less so move left
                  if (currentNode.Left is null)
                  {
                     currentNode.Left = newNode; // Add node if left pointer is empty
                     size++;
                     break;
                  }
                  else currentNode = currentNode.Left; // If node has left child - move to look at that child
               }
               else if (valDif > 0)
               {
                  // New node is greater so move right
                  if (currentNode.Right is null)
                  {
                     currentNode.Right = newNode;
                     size++;
                     break;
                  }
                  else currentNode = currentNode.Right;
               }
               else
               {
                  currentNode.Value.occurrences++;
                  currentNode.Value.lineLocations.Append(lineNum);
                  break;
               }
            }
         }
         size++;
      }

      void InOrderPrint(WordLeaf? node)
      {
         if (node is null) return;

         InOrderPrint(node.Left);
         Console.Write("({0}), ", node.Value);
         InOrderPrint(node.Right);
      }


      void InOrderString(WordLeaf? node, ref string outputString)
      {
         if (node is null) return;

         InOrderString(node.Left, ref outputString);
         outputString += "\n" + node.Value;
         InOrderString(node.Right, ref outputString);
      }

      public void WriteToFile(string fileName)
      {
         string text = "word,count";
         InOrderString(root, ref text);
         File.WriteAllText(fileName, text);
         Console.WriteLine("All unique words and their number of occurrences written to file: '{0}'", fileName);
      }

      public void PrintLongestWord()
      {
         string longestWord = "";
         GetLongestWord(root, ref longestWord);
         Console.WriteLine("The longest word in the text is {0} at {1} characters.", longestWord, longestWord.Length);
      }


      void GetLongestWord(WordLeaf? node, ref string longestWord)
      {
         if (node is null) return;

         GetLongestWord(node.Left, ref longestWord);
         if (node.Value.text.Length > longestWord.Length)
            longestWord = node.Value.text;
         GetLongestWord(node.Right, ref longestWord);
      }

      public void PrintMostCommonWord()
      {
         string mostUsedWord = "";
         int mostUsedCount = 0;
         GetMostUsedWord(root, ref mostUsedCount, ref mostUsedWord);
         Console.WriteLine("The most frequently occurring word in the text is '{0}' at {1} occurrences.", mostUsedWord, mostUsedCount);
      }

      void GetMostUsedWord(WordLeaf? node, ref int highestOccur, ref string mostUsedWord)
      {
         if (node is null) return;

         GetMostUsedWord(node.Left, ref highestOccur, ref mostUsedWord);
         if (node.Value.occurrences > highestOccur)
         {
            mostUsedWord = node.Value.text;
            highestOccur = node.Value.occurrences;
         }
         GetMostUsedWord(node.Right, ref highestOccur, ref mostUsedWord);
      }

      public WordInfo? Search(WordLeaf? root, string toFind)
      {
         if (root == null)
         {
            return null;
         }

         int orderDif = toFind.CompareTo(root.Value.text);

         if (orderDif == 0)
         {

            return root.Value;
         }
         else if (orderDif < 0) { return Search(root.Left, toFind); }  // Item is less than root so look left
         else { return Search(root.Right, toFind); }  // Item is more than root so look right
      }

      void GetWordCount(WordLeaf? node, string toFind, ref int count)
      {
         if (node is null) return;

         GetWordCount(node.Left, toFind, ref count);
         if (node.Value.text.CompareTo(toFind) == 0)
            count = node.Value.occurrences;
         GetWordCount(node.Right, toFind, ref count);
      }
   }
}