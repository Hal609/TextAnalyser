using System;

namespace TextAnalyser;

public class LinkedList<T> where T : IComparable<T>
{
   private ListItem<T> head;

   // The size field is preserved.
   public int size { get; private set; }

   public LinkedList()
   {
      head = null;
      size = 0;
   }

   public T Get(int n)
   {
      if (Math.Abs(n) >= size)
      {
         Console.Error.WriteLine("ERROR: n must not exceed list size.");
         System.Environment.Exit(1);
      }
      if (n < 0)
      {
         n = size + n;
      }

      ListItem<T> current = head;
      int counter = 0;
      while (current != null)
      {
         if (counter == n)
         {
            return current.Value;
         }
         current = current.Next;
         counter++;
      }
      Console.Error.WriteLine("ERROR: No item at {0} found", n);
      System.Environment.Exit(1);
      return default(T);
   }

   public void Set(int n, T value)

   {
      if (n >= size)
      {
         Console.Error.WriteLine("ERROR: N exceeded list size.");
         return;
      }
      ListItem<T> current = head;
      int counter = 0;
      while (current != null)
      {
         if (counter == n)
         {
            current.Value = value;
            break;
         }
         current = current.Next;
         counter++;
      }
   }

   // Private helper method corresponding to _get_list_nth_address in C.
   // It traverses the list and returns the ListItem at the given index.
   private ListItem<T> GetNodeAtIndex(int n)
   {
      if (n >= size)
      {
         Console.Error.WriteLine("ERROR: N exceeded list size.");
         System.Environment.Exit(1);
      }
      ListItem<T> current = head;
      int counter = 0;
      while (current != null)
      {
         if (counter == n)
         {
            return current;
         }
         current = current.Next;
         counter++;
      }
      Console.Error.WriteLine("ERROR: No item at {0} found", n);
      System.Environment.Exit(1);
      return default(ListItem<T>);
   }
   private void SetNext(int n, ListItem<T> nextPtr)
   {
      if (n >= size)
      {
         Console.Error.WriteLine("ERROR: N exceeded list size.");
         return;
      }
      ListItem<T> current = head;
      int counter = 0;
      while (current != null)
      {
         if (counter == n)
         {
            current.Next = nextPtr;
            break;
         }
         current = current.Next;
         counter++;
      }
   }
   public void Append(T value)
   {
      if (size == 0)
      {
         // Create the first node if the list is empty.
         head = new ListItem<T>(value, null);
      }
      else
      {
         // Create a new node.
         ListItem<T> newNode = new ListItem<T>(value, null);
         // Attach it to the last node using SetNext (which traverses the list).
         SetNext(size - 1, newNode);
      }
      size++;
   }

   public T Pop()
   {
      if (size == 0)
      {
         Console.Error.WriteLine("ERROR: Cannot pop from empty list.");
         System.Environment.Exit(1);
      }

      T returnValue;
      if (size == 1)
      {
         // If there is only one element, remove head.
         returnValue = head.Value;
         head = null;
      }
      else
      {
         // Get the second-to-last node.
         ListItem<T> penultimate = GetNodeAtIndex(size - 2);
         ListItem<T> lastNode = penultimate.Next;
         returnValue = lastNode.Value;
         // Disconnect the last node.
         penultimate.Next = null;
      }
      size--;
      return returnValue;
   }

   public void Remove(int n)
   {
      if (Math.Abs(n) >= size)
      {
         Console.Error.WriteLine("ERROR: n must not exceed list size.");
         return;
      }
      if (n < 0)
      {
         n = size + n;
      }
      if (n == 0)
      {
         // Remove the head.
         head = head.Next;
      }
      else
      {
         // Find the (n-1)th node and skip over the nth node.
         ListItem<T> previous = GetNodeAtIndex(n - 1);
         if (previous != null && previous.Next != null)
         {
            previous.Next = previous.Next.Next;
         }
      }
      size--;
   }

   public void PrintList()
   {
      Console.Write("[");
      for (int i = 0; i < size; i++)
      {
         T value = Get(i);
         if (i < size - 1)
            Console.Write("{0}, ", value);
         else
            Console.Write("{0}", value);
      }
      Console.WriteLine("]");
   }

   public bool InList(T toFind)
   {
      for (int i = 0; i < size; i++)
      {
         if (Get(i).CompareTo(toFind) == 0)
         {
            return true;
         }
      }
      return false;
   }

   public int find(T toFind)
   {
      for (int i = 0; i < size; i++)
      {
         if (Get(i).CompareTo(toFind) == 0)
         {
            return i;
         }
      }
      return -1;
   }
}