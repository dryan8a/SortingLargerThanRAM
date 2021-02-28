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
            public bool IsEmpty;

            public Node(T value, int originIndex)
            {
                Value = value;
                OriginIndex = originIndex;
                IsEmpty = false;
            }

            public Node(T value, int originIndex, bool isEmpty)
            {
                Value = value;
                OriginIndex = originIndex;
                IsEmpty = isEmpty;
            }

            internal Node(T value, int originIndex, Node<T> leftChild, Node<T> rightChild)
            {
                Value = value;
                OriginIndex = originIndex;
                LeftChild = leftChild;
                RightChild = rightChild;
                IsEmpty = false;
            }

            internal Node(T value, int originIndex, Node<T> leftChild, Node<T> rightChild, Node<T> parent, bool isEmpty)
            {
                Value = value;
                OriginIndex = originIndex;
                LeftChild = leftChild;
                RightChild = rightChild;
                IsEmpty = isEmpty;
                Parent = parent;
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
            Head = BuildTree(elements,true);
        }

        private Node<T> BuildTree(Queue<Node<T>> elements, bool isBottomLayer = false)
        {
            if (elements.Count == 0) throw new Exception("Don't add no elements to the tree");
            Node<T> currentWinner = null;
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

                    if(isBottomLayer && (currentWinner == null || winnerElement.Value.CompareTo(currentWinner.Value) < 0))
                    {
                        currentWinner = winnerElement;
                    }
                }
            }

            if (elements.Count == 1)
            {
                return elements.Dequeue();
            }
            else
            {
                if(isBottomLayer)
                {
                    Winner = currentWinner;
                }
                return BuildTree(elements);
            }
        }

        public void RefreshTree(Node<T> winnerNode, Node<T> newNode)
        {
            winnerNode.OriginIndex = newNode.OriginIndex;
            winnerNode.Value = newNode.Value;
            winnerNode.IsEmpty = newNode.IsEmpty;
            var newWinner = ReplayGames(winnerNode.Parent, winnerNode);

            while(newWinner.LeftChild != null)
            {
                if(newWinner.LeftChild.OriginIndex.Equals(newWinner.OriginIndex))
                {
                    newWinner = newWinner.LeftChild;
                }
                else if (newWinner.RightChild.OriginIndex.Equals(newWinner.OriginIndex))
                {
                    newWinner = newWinner.RightChild;
                }
                else
                {
                    throw new Exception("Something broke");
                }
            }

            Winner = newWinner;
        }

        private Node<T> ReplayGames(Node<T> oldNode, Node<T> newNode)
        {
            Node<T> winner = null;
            Node<T> loser = null;

            if (newNode.IsEmpty || oldNode.Value.CompareTo(newNode.Value) < 0) //swaps the node's value if the previous loser wins over newNode
            {
                winner = oldNode;
                loser = newNode;
            }
            else
            {
                winner = newNode;
                loser = oldNode;
            }

            if (oldNode != Head)
            {
                winner = ReplayGames(oldNode.Parent,winner);
            }

            var trueWinner = winner;
            if(winner.LeftChild != null && winner.LeftChild.OriginIndex == winner.OriginIndex)
            {
                trueWinner = winner.LeftChild;
            }
            if (winner.RightChild != null && winner.RightChild.OriginIndex == winner.OriginIndex)
            {
                trueWinner = winner.RightChild;
            }

            oldNode.Value = loser.Value;
            oldNode.OriginIndex = loser.OriginIndex;
            return trueWinner;
        }
    }
}
