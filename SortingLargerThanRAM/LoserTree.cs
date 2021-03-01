using System;
using System.Collections.Generic;
using System.Text;

namespace SortingLargerThanRAM
{
    public class LoserTree<T> where T : IComparable<T>
    {
        public class Node
        {
            internal Node LeftChild;
            internal Node RightChild;
            internal Node Parent;
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

            internal Node(T value, int originIndex, Node leftChild, Node rightChild)
            {
                Value = value;
                OriginIndex = originIndex;
                LeftChild = leftChild;
                RightChild = rightChild;
                IsEmpty = false;
            }

            internal Node(T value, int originIndex, Node leftChild, Node rightChild, Node parent, bool isEmpty)
            {
                Value = value;
                OriginIndex = originIndex;
                LeftChild = leftChild;
                RightChild = rightChild;
                IsEmpty = isEmpty;
                Parent = parent;
            }
        }

        Node Head;
        public Node Winner { get; private set; }
        /// <summary>
        /// Initializes the Loser Tree given some elements
        /// </summary>
        /// <param name="elements">The values to enter into the tree</param>
        public LoserTree(Queue<Node> elements)
        {
            Head = BuildTree(elements);
        }

        private Node BuildTree(Queue<Node> elements)
        {
            if (elements.Count == 0) throw new Exception("Don't add no elements to the tree");
            var newElements = new Queue<(Node current, Node winner)>();
            Node currentWinner = null;
            for (int i = 0; i < elements.Count;)
            {
                if (i != elements.Count - 1)
                {
                    var winnerElement = elements.Dequeue();
                    var loserElement = elements.Dequeue();
                    if (winnerElement.Value.CompareTo(loserElement.Value) > 0)
                    {
                        var temp = winnerElement;
                        winnerElement = loserElement;
                        loserElement = temp;
                    }

                    var parent = new Node(loserElement.Value, loserElement.OriginIndex, winnerElement, loserElement);
                    winnerElement.Parent = parent;
                    loserElement.Parent = parent;
                    newElements.Enqueue((parent, winnerElement));

                    if (currentWinner == null || winnerElement.Value.CompareTo(currentWinner.Value) < 0)
                    {
                        currentWinner = winnerElement;
                    }
                }
                else
                {
                    var oddElement = elements.Dequeue();
                    newElements.Enqueue((oddElement, oddElement));
                }
            }

            if (elements.Count == 1)
            {
                Winner = elements.Dequeue();
                return Winner;
            }
            else
            {
                Winner = currentWinner;
                return BuildTree(newElements);
            }
        }

        private Node BuildTree(Queue<(Node current,Node winner)> elements)
        {
            if (elements.Count == 0) throw new Exception("Don't add no elements to the tree");
            Node currentWinner = null;
            for(int i = 0;i<elements.Count;i++)
            {
                if(i != elements.Count - 1) 
                {
                    var winnerElement = elements.Dequeue();
                    var loserElement = elements.Dequeue();
                    if (winnerElement.winner.Value.CompareTo(loserElement.winner.Value) > 0)
                    {
                        var temp = winnerElement;
                        winnerElement = loserElement;
                        loserElement = temp;
                    }

                    var parent = new Node(loserElement.winner.Value, loserElement.winner.OriginIndex, winnerElement.current, loserElement.current);
                    winnerElement.current.Parent = parent;
                    loserElement.current.Parent = parent;
                    elements.Enqueue((parent,winnerElement.winner));
                }
            }

            if (elements.Count == 1)
            {
                return elements.Dequeue().current;
            }
            else
            {
                return BuildTree(elements);
            }
        }

        public void RefreshTree(Node winnerNode, Node newNode)
        {
            winnerNode.OriginIndex = newNode.OriginIndex;
            winnerNode.Value = newNode.Value;
            winnerNode.IsEmpty = newNode.IsEmpty;

            int newWinnerOrigin = ReplayGames(winnerNode.Parent, winnerNode);

            Winner = FindNewWinner(newWinnerOrigin);
        }

        private Node FindNewWinner(int winnerOrigin)
        {
            var stack = new Stack<Node>();
            var currentNode = Head;

            while (currentNode != null || stack.Count != 0)
            {
                while(currentNode != null)
                {
                    stack.Push(currentNode);
                    currentNode = currentNode.LeftChild;
                }

                currentNode = stack.Pop();
                if(currentNode.OriginIndex == winnerOrigin && currentNode.LeftChild == null && currentNode.RightChild == null)
                {
                    return currentNode;
                }

                currentNode = currentNode.RightChild;
            }
            throw new Exception("Origin Index does not exist");
        }

        private int ReplayGames(Node oldNode, Node newNode)
        {
            Node winner = null;
            Node loser = null;

            if (!oldNode.IsEmpty && (newNode.IsEmpty || oldNode.Value.CompareTo(newNode.Value) < 0)) //swaps the node's value if the previous loser wins over newNode
            {
                winner = oldNode;
                loser = newNode;
            }
            else
            {
                winner = newNode;
                loser = oldNode;
            }

            int winnerOrigin;
            if (oldNode != Head)
            {
                winnerOrigin = ReplayGames(oldNode.Parent,winner);
            }
            else
            {
                winnerOrigin = winner.OriginIndex;
            }

            oldNode.Value = loser.Value;  //sets old parent's value and origin to the current loser given the newNode's replacement
            oldNode.OriginIndex = loser.OriginIndex;
            oldNode.IsEmpty = loser.IsEmpty;

            return winnerOrigin;
        }
    }
}
