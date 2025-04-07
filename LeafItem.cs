using System;

namespace TextAnalyser;

public class LeafItem<T>
{
   public T Value { get; set; }
   public LeafItem<T>? Left { get; set; }
   public LeafItem<T>? Right { get; set; }

   public LeafItem(T value, LeafItem<T>? left, LeafItem<T>? right)
   {
      Value = value;
      Left = left;
      Right = right;
   }
}
