using System;
using System.Collections.Generic;
using System.Text;

namespace SortingLargerThanRAM
{
    public class LoserTree<T> where T : IComparable<T>
    {
        public class Node<T>
        {
            internal Node<T> LeftChild;
            internal Node<T> RightChild;
            internal Node<T> Parent;
            public int OriginIndex; 
            public T Value;

            public Node(T value, int originIndex)
            {
                Value = value;
                OriginIndex = originIndex;
            }

            public Node(T value, int originIndex, Node<T> leftChild, Node<T> rightChild)
            {
                Value = value;
                OriginIndex = originIndex;
                LeftChild = leftChild;
                RightChild = rightChild;
            }
        }

        Node<T> Head;
        public Node<T> Winner { get; private set; }
        /// <summary>
        /// Initializes the Loser Tree given some elements
        /// </summary>
        /// <param name="elements">The values to enter into the tree</param>
        public LoserTree(Queue<Node<T>> elements)
        {
            Head = BuildTree(elements);
        }

        Node<T> BuildTree(Queue<Node<T>> elements)
        {
            for(int i = 0;i<elements.Count;i++)
            {
                if(i != elements.Count - 1) 
                {
                    var winnerElement = elements.Dequeue();
                    var loserElement = elements.Dequeue();
                    if (winnerElement.Value.CompareTo(loserElement.Value) > 0)
                    {
                        var temp = winnerElement;
                        winnerElement = loserElement;
                        loserElement = temp;
                    }
                    var parent = new Node<T>(loserElement.Value, loserElement.OriginIndex, winnerElement, loserElement);
                    winnerElement.Parent = parent;
                    loserElement.Parent = parent;
                    elements.Enqueue(parent);
                }
            }

            if (elements.Count == 1)
            {
                return elements.Dequeue();
            }
            else
            {
                return BuildTree(elements);
            }
        }

        public void ReplayGames(Node<T> oldNode, Node<T> newNode)
        {
            var winner = newNode;
            if (newNode.Value.CompareTo(oldNode.Value) > 0) //swaps the node's value if the previous loser wins over newNode
            {
                winner = oldNode;
                oldNode.Value = newNode.Value;
                oldNode.OriginIndex = newNode.OriginIndex;
            }

            if (oldNode != Head)
            {
                ReplayGames(oldNode.Parent,winner);
            }
        }
    }
}
