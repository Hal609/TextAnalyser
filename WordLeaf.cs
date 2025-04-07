using System;

namespace TextAnalyser;

public class WordLeaf
{
   public WordInfo Value { get; set; }
   public WordLeaf? Left { get; set; }
   public WordLeaf? Right { get; set; }

   public override string ToString() => Value.ToString();

   public WordLeaf(WordInfo value, WordLeaf? left, WordLeaf? right)
   {
      Value = value;
      Left = left;
      Right = right;
   }
}
