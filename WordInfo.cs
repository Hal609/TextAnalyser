using System;

namespace TextAnalyser;

public class WordInfo : IEquatable<WordInfo>, IComparable<WordInfo>
{
   public string text;
   public int occurrences { get; set; }

   public LinkedList<int> lineLocations;
   public WordInfo(string txt, int occur, int lineNum)
   {
      text = txt;
      occurrences = occur;
      lineLocations = new LinkedList<int> { };
      lineLocations.Append(lineNum);
   }


   public override string ToString() => $"{text},{occurrences}";
   public static bool operator ==(WordInfo c1, WordInfo c2) => c1.text.Equals(c2.text);
   public static bool operator !=(WordInfo c1, WordInfo c2) => !c1.text.Equals(c2.text);

   public void IncrementOccur()
   {
      occurrences++;
   }
   public bool Equals(WordInfo other) => this.text == other.text;
   public int CompareTo(WordInfo other)
   {
      return this.text.CompareTo(other.text);
   }

   public void PrintLineLocations()
   {
      Console.WriteLine("The word '{0}' occurs on lines: {1}", text, lineLocations);
   }

}

