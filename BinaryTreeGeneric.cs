using System;
using System.Xml;

namespace TextAnalyser
{
   public class BinaryTreeGeneric<T> where T : IComparable<T>
   {
      private LeafItem<T>? root;
      public int size { get; private set; }

      public BinaryTreeGeneric()
      {
         root = null;
         size = 0;
      }

      // Returns the value at the nth node (using level-order indexing).
      public T Get(int n)
      {
         if (Math.Abs(n) >= size)
         {
            Console.Error.WriteLine("ERROR: n must not exceed tree size.");
            Environment.Exit(1);
         }
         if (n < 0)
         {
            n = size + n;
         }
         LeafItem<T> node = GetNodeAtIndex(n);
         return node.Value;
      }

      // Sets the value at the nth node.
      public void Set(int n, T value)
      {
         if (n >= size)
         {
            Console.Error.WriteLine("ERROR: n exceeded tree size.");
            return;
         }
         LeafItem<T> node = GetNodeAtIndex(n);
         node.Value = value;
      }

      // Private helper: traverses the tree in level order to return the node at index n.
      private LeafItem<T> GetNodeAtIndex(int n)
      {
         if (n >= size)
         {
            Console.Error.WriteLine("ERROR: n exceeded tree size.");
            Environment.Exit(1);
         }

         Queue<LeafItem<T>> queue = new Queue<LeafItem<T>>();
         queue.Enqueue(root);
         int counter = 0;

         while (queue.Count > 0)
         {
            LeafItem<T> current = queue.Dequeue();
            if (counter == n)
            {
               return current;
            }
            counter++;
            if (current.Left != null)
               queue.Enqueue(current.Left);
            if (current.Right != null)
               queue.Enqueue(current.Right);
         }
         Console.Error.WriteLine("ERROR: No node at index {0} found.", n);
         Environment.Exit(1);
         return null; // Unreachable.
      }

      // Appends a new node with the given value at the "end" (next available spot in level order).
      public void Append(T value)
      {
         LeafItem<T> newNode = new LeafItem<T>(value, null, null);
         if (size == 0)
            root = newNode; // Create the root node.
         else
         {
            LeafItem<T> currentNode = root;
            while (true)
            {
               int valDif = newNode.Value.CompareTo(currentNode.Value);
               if (valDif < 0) // This means that newNode value is less than currentNode
               {
                  // New node is less so move left
                  if (currentNode.Left is null) { currentNode.Left = newNode; break; } // Add node if left pointer is empty
                  else currentNode = currentNode.Left; // If node has left child - move to look at that child
               }
               else
               {
                  // New node is greater so move right
                  if (currentNode.Right is null) { currentNode.Right = newNode; break; }
                  else currentNode = currentNode.Right;
               }
            }
         }
         size++;
      }

      // Removes and returns the value from the last node (highest index in level order).
      public T Pop()
      {
         if (size == 0)
         {
            Console.Error.WriteLine("ERROR: Cannot pop from an empty tree.");
            Environment.Exit(1);
         }

         T returnValue;
         if (size == 1)
         {
            returnValue = root.Value;
            root = null;
         }
         else
         {
            int lastIndex = size - 1;
            LeafItem<T> lastNode = GetNodeAtIndex(lastIndex);
            returnValue = lastNode.Value;

            // Find the parent of the last node.
            int parentIndex = (lastIndex - 1) / 2;
            LeafItem<T> parent = GetNodeAtIndex(parentIndex);

            // Disconnect the last node.
            if (parent.Left == lastNode)
            {
               parent.Left = null;
            }
            else if (parent.Right == lastNode)
            {
               parent.Right = null;
            }
         }
         size--;
         return returnValue;
      }

      // Removes the node at index n. To maintain a complete tree,
      // the target node is replaced with the last node's value, and then the last node is removed.
      public void Remove(int n)
      {
         if (Math.Abs(n) >= size)
         {
            Console.Error.WriteLine("ERROR: n must not exceed tree size.");
            return;
         }
         if (n < 0)
         {
            n = size + n;
         }
         if (n == size - 1)
         {
            // Removing the last node is equivalent to Pop.
            Pop();
            return;
         }

         // Replace the target node's value with the last node's value.
         LeafItem<T> target = GetNodeAtIndex(n);
         LeafItem<T> lastNode = GetNodeAtIndex(size - 1);
         target.Value = lastNode.Value;

         // Remove the last node.
         if (size == 1)
         {
            root = null;
         }
         else
         {
            int lastIndex = size - 1;
            int parentIndex = (lastIndex - 1) / 2;
            LeafItem<T> parent = GetNodeAtIndex(parentIndex);
            if (parent.Left == lastNode)
            {
               parent.Left = null;
            }
            else if (parent.Right == lastNode)
            {
               parent.Right = null;
            }
         }
         size--;
      }

      // Prints the tree's values in level-order.
      public void PrintTree()
      {
         Console.Write("[");
         Queue<LeafItem<T>> queue = new Queue<LeafItem<T>>();
         if (root != null)
         {
            queue.Enqueue(root);
         }
         int counter = 0;
         while (queue.Count > 0)
         {
            LeafItem<T> current = queue.Dequeue();
            Console.Write("{0}", current.Value);
            counter++;
            if (counter < size)
               Console.Write(", ");

            if (current.Left != null)
               queue.Enqueue(current.Left);
            if (current.Right != null)
               queue.Enqueue(current.Right);
         }
         Console.WriteLine("]");
      }

      // Determines whether a value is present in the tree.
      public bool InTree(T toFind)
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

      // Finds the index of a value in the tree (using level-order indexing).
      // Returns -1 if not found.
      public int Find(T toFind)
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
}