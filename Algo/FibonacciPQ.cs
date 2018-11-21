namespace DevWeek.Algo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Fibonacci indexed priority queue of generic keys.
    /// This implementation uses a Fibonacci heap.    
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <remarks>
    /// An integer between 0 and N-1 is associated with each key.
    /// This implementation uses a Fibonacci heap along with an array to associate keys with integers in the given range.
    /// <see href="https://algs4.cs.princeton.edu/99misc/IndexFibonacciMinPQ.java.html"/>
    /// </remarks>
    public class FibonacciPQ<TKey> 
        : IEnumerable<TKey>
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Head of the circular root list
        /// </summary>
        private Node head;
        /// <summary>
        /// Minimum Node of the root list
        /// </summary>
        private Node min;
        /// <summary>
        /// Number of keys in the heap
        /// </summary>
        private int size;
        /// <summary>
        /// Maximum number of elements in the heap.
        /// </summary>
        private int n;
        /// <summary>
        /// Used for the consolidate operation
        /// </summary>
        private IDictionary<int, Node> table = new Dictionary<int, Node>();
        /// <summary>
        /// Array of Nodes in the heap.
        /// </summary>
        private Node[] nodes;

        /// <summary>
        /// Represents a Node of a tree.
        /// </summary>
        private class Node
        {
            /// <summary>
            /// Key of this Node
            /// </summary>
            internal TKey key;
            /// <summary>
            /// Siblings of this Node
            /// </summary>
            internal Node prev, next;
            /// <summary>
            /// predecessor/successor of this Node
            /// </summary>
            internal Node child, parent;
            /// <summary>
            /// Index associated with the key
            /// </summary>
            internal int index;
            /// <summary>
            /// Order of the tree rooted by this Node
            /// </summary>
            internal int order;

            /// <summary>
            /// Indicates if this Node already lost a child
            /// </summary>
            // bool mark;

            public override string ToString() => $"{index} {order} {key}";
        }

        public FibonacciPQ(int n)
        {
            this.n = n;
            nodes = new Node[n];
        }

        
        private FibonacciPQ(Node[] nodes)
            : this(nodes.Length)
        {
            foreach (Node node in nodes)
            {
                if (node == null) continue;
                Insert(node.index, node.key);
            }   
        }

        public FibonacciPQ(TKey[] keys)
            : this(keys.Select((k, i) => new Node { index = i, key = /*keys[idx]*/k } ).ToArray()) 
        {
        }


        /// <summary>
        /// Insert a key in the queue.
        /// </summary>
        /// <remarks>Worst case is O(1).</remarks>
        /// <param name="i">an index</param>
        /// <param name="key">a key</param>
        public void Insert(int i, TKey key)
        {
            if (HasIndex(i)) throw new ArgumentException("Specified index is already in the queue", nameof(i));

            Node x = new Node();
            x.key = key;
            x.index = i;
            nodes[i] = x;
            size++;

            head = InternalInsert(x, head);

            if (min == null) min = head;
            else min = (greater(min.key, key)) ? head : min;
        }

        /// <summary>Does the priority queue contains the index i ?</summary>
        /// <param name="i">an index</param>
        /// <returns>true if i is on the priority queue, false if not</returns>
        public bool HasIndex(int i)
        {
            if (i < 0 || i >= n) throw new ArgumentOutOfRangeException(nameof(i));
            return nodes[i] != null;
        }

        /// <summary>
        /// compares two keys
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        private bool greater(TKey key1, TKey key2)
        {
            if (key1 == null) return false;
            if (key2 == null) return true;
            return key1.CompareTo(key2) > 0;
        }

        /// <summary>
        /// Inserts a Node in a circular list containing head, returns a new head.
        /// </summary>
        /// <param name="x">node to insert</param>
        /// <param name="h">existing head</param>
        /// <returns>new head</returns>
        private Node InternalInsert(Node x, Node h)
        {
            if (h == null)
            {
                x.prev = x;
                x.next = x;
            }
            else
            {
                h.prev.next = x;
                x.next = h;
                x.prev = h.prev;
                h.prev = x;
            }
            return x;
        }

        /// <summary>Gets the minimum key currently in the queue.</summary>
        /// <returns>the minimum key currently in the priority queue</returns>
        /// <remarks>Worst case is O(1)</remarks>
        public TKey MinimumKey()
        {
            if (IsEmpty()) throw new InvalidOperationException("Priority queue is empty");
            return min.key;
        }

        /// <summary>Gets the max key currently in the queue.</summary>
        /// <returns>the max key currently in the priority queue</returns>
        /// <remarks>Worst case is O(1)</remarks>
        public TKey MaximumKey()
        {
            if (IsEmpty()) throw new InvalidOperationException("Priority queue is empty");
            return nodes[size-1].key;
        }

        /// <summary>Whether the priority queue is empty</summary>
        /// <returns>true if the priority queue is empty, false if not</returns>
        /// <remarks>Worst case is O(1)</remarks>
        public bool IsEmpty() => size == 0;

        /// <summary>Number of elements currently on the priority queue.</summary>
        /// <returns>the number of elements on the priority queue</returns>
        public int Size() => size;

        private TKey keyOf(int i)
        {
            if (!HasIndex(i)) throw new ArgumentException("Specified index is not in the queue", nameof(i));

            return nodes[i].key;
        }

        /// <summary>indexer</summary>
        /// <param name="i">index</param>
        /// <returns>key</returns>
        /// <remarks>to allow client code to use [] notation.</remarks>
        public TKey this[int i]
        {
            get => keyOf(i);
            set => Insert(i, value);
        }

        /// <summary>
        /// Function for consolidating all trees in the root list
        /// </summary>
        /// <remarks>
        /// Coalesces the roots, thus reshapes the heap, improves performance greatly
        /// </remarks>
        public void Consolidate()
        {
            table.Clear();

            Node x = head, y = null, z = null;
            int maxOrder = 0;
            min = head;
            
            do
            {
                y = x;
                x = x.next;
                z =  table.ContainsKey(y.order) ? table[y.order] : null;
                while (z != null)
                {
                    table.Remove(y.order);
                    if (greater(y.key, z.key))
                    {
                        link(y, z);
                        y = z;
                    }
                    else
                    {
                        link(z, y);
                    }
                    z = table.ContainsKey(y.order) ? table[y.order] : null;
                }

                table[y.order] = y;

                if (y.order > maxOrder) maxOrder = y.order;
            }
            while (x != head);

            head = null;

            foreach (Node n in table.Values)
            {
                min = greater(min.key, n.key) ? n : min;
                head = InternalInsert(n, head);
            }
        }

        /// <summary>
        /// Assuming root1 holds a greater key than root2, root2 becomes the new root.
        /// </summary>
        /// <param name="root1"></param>
        /// <param name="root2"></param>
        private void link(Node root1, Node root2)
        {
            root1.parent = root2;
            root2.child = InternalInsert(root1, root2.child);
            root2.order++;
        }

        public TKey DeleteMinimum()
        {
            throw new NotImplementedException(nameof(DeleteMinimum));
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
