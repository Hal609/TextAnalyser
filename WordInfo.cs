using System;

namespace TextAnalyser;

public class WordInfo : IEquatable<WordInfo>, IComparable<WordInfo>
{
   public WordInfo(string txt, int occur)
   {
      text = txt;
      occurrences = occur;
   }

   public string text;
   public int occurrences { get; set; }

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


}

