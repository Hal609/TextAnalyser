using System;

namespace TextAnalyser;

public class ListItem<T>
{
   public T Value { get; set; }
   public ListItem<T> Next { get; set; }

   public ListItem(T value, ListItem<T> next)
   {
      Value = value;
      Next = next;
   }
}
