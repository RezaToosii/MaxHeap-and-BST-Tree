using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Design
{
    public class Node                                              //Definition Node
    {
        public int Data;
        public Node Father;
        public Node Right;                                          //Right child
        public Node Left;                                           //Left child
        public Node(int value)
        {
            Data = value;
            Left = null;
            Right = null;
            Father = null;
        }
    }
    public class MaxHeap
    {
        public Node root;
        public MaxHeap()                                            //make maxheap no root
        {
            root = null;
        }
        public MaxHeap(int value)                                   //make maxheap with root
        {
            root = new Node (value);
        }
        
        
        public void InsertHeap(int value)
        {
            Node newNode = new Node(value);
            if (root == null)                                       //add root
            {
                root = newNode;
                return;
            }
            InsertHeap(root, newNode);                              //add other node
            GoUp(newNode);                                          //rise in tree until smaller from father
        }
        private void InsertHeap(Node CNode, Node newNode)
        {
            if (CNode.Left == null)                                 //add left root
            {
                CNode.Left = newNode;
                newNode.Father = CNode;
            }
            else if (CNode.Right == null)                           //add right root
            {
                CNode.Right = newNode;
                newNode.Father = CNode;
            }
            else
            {
                Queue<Node> queue = new Queue<Node>();              //move in heap tree level by level
                queue.Enqueue(CNode);

                while (queue.Count > 0)
                {
                    Node node = queue.Dequeue();

                    if (node.Left == null)                          //first add in left
                    {
                        node.Left = newNode;
                        newNode.Father = node;
                        break;
                    }
                    else if (node.Right == null)                    //add in right
                    {
                        node.Right = newNode;
                        newNode.Father = node;
                        break;
                    }

                    queue.Enqueue(node.Left);                       //add left and right in queue
                    queue.Enqueue(node.Right);
                }
            }
        }
        private void GoUp(Node node)                                //Node change with his father in a loop
        {
            while (node.Father != null && node.Father.Data < node.Data)
            {
                int temp = node.Father.Data;
                node.Father.Data = node.Data;
                node.Data = temp;
                node = node.Father;
            }
        }
        
        
        public bool IsHeap()
        {
            return IsHeap(root);
        }
        private bool IsHeap(Node node)
        {
            if (node == null)
                return true;

            bool isLeftChildValid = node.Left == null || (node.Data >= node.Left.Data && IsHeap(node.Left));            //check rulse for left
            bool isRightChildValid = node.Right == null || (node.Data >= node.Right.Data && IsHeap(node.Right));        //check rulse for right

            return isLeftChildValid && isRightChildValid;           //if both left & right is true return true
        }
        
        
        public void FindAndDeleteFromHeap(int value)
        {
            Node nodeToDelete = FindNode(root, value);              //first find node
            if (nodeToDelete != null)
            {
                Node lastNode = GetLastNode();                      //get last node to 
                if (lastNode != nodeToDelete)
                {
                    nodeToDelete.Data = lastNode.Data;              //change last node data with node to delete
                    DeleteNode(lastNode);                           //delete last node
                    GoDown(nodeToDelete);                           //fall to first empty place
                    GoUp(nodeToDelete);                             //rise in tree until smaller from father
                }
                else
                {
                    DeleteNode(lastNode);
                }
            }
            else Console.WriteLine("node with this data not exist.");
        }
        private Node FindNode(Node node, int value)                 //check right & left until find node
        {
            if (node == null)                                       //not found
                return null;

            if (node.Data == value)                                 //founded
                return node;

            Node foundNode = FindNode(node.Left, value);            //go left
            if (foundNode == null)
                foundNode = FindNode(node.Right, value);            //if not found go right

            return foundNode;
        }
        private Node GetLastNode()                                  //find last node with move level by level
        {
            if (root == null)
                return null;

            Node lastNode = null;
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                lastNode = queue.Dequeue();
                if (lastNode.Left != null)
                    queue.Enqueue(lastNode.Left);
                if (lastNode.Right != null)
                    queue.Enqueue(lastNode.Right);
            }

            return lastNode;
        }
        private void DeleteNode(Node node)                          //just remove connection between node & father
        {
            if (node.Father == null)
            {
                root = null;
            }
            else if (node.Father.Left == node)
            {
                node.Father.Left = null;
            }
            else
            {
                node.Father.Right = null;
            }

            node.Father = null;
        }
        private void GoDown(Node node)                              //fall node in tree until bigger from both child
        {
            while (node.Left != null || node.Right != null)
            {
                Node maxChild;
                if (node.Left != null && node.Right != null)        //two child
                {
                    if(node.Left.Data > node.Right.Data)
                    {
                        maxChild = node.Left;
                    }
                    else maxChild = node.Right;
                }
                else                                                //one child
                {
                    if(node.Left != null)
                    {
                        maxChild = node.Left;
                    }
                    else maxChild = node.Right;
                }

                if (node.Data >= maxChild.Data)
                    break;
                /*                                                  //change with connection
                Node temp0 = null;
                temp0.Right = maxChild.Right;
                temp0.Left = maxChild.Left;
                temp0.Father = node.Father;

                if (maxChild == node.Right)
                {
                    maxChild.Right = node;
                    maxChild.Left = node.Left;
                }
                else
                {
                    maxChild.Left = node;
                    maxChild.Right = node.Right;
                }

                node.Left = temp0.Left;
                node.Right = temp0.Right;
                node.Father = maxChild;
                maxChild.Father = temp0.Father;
                */

                int temp = node.Data;                               //change with data
                node.Data = maxChild.Data;
                maxChild.Data = temp;
                node = maxChild;
            }
        }
        
        
        public void PrintHeap()                                     //move in heap level by level and add in queue
        {
            if (root == null)
                return;

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                int levelSize = queue.Count;

                for (int i = 0; i < levelSize; i++)
                {
                    Node node = queue.Dequeue();
                    Console.Write(node.Data + " ");

                    if (node.Left != null)
                        queue.Enqueue(node.Left);

                    if (node.Right != null)
                        queue.Enqueue(node.Right);
                }

                Console.WriteLine();
            }
        }
        
        
        public void PrintSortedHeap()
        {
            if (root == null)
                return;

            Console.Write("Sorted Heap: ");
            List<int> heapValues = new List<int>();
            SortHeap(root, heapValues);                             //save sorted heap in list and print
            foreach (int item in heapValues)
            {
                Console.Write(item+ " - ");
            }
        }
        private void SortHeap(Node node, List<int> values)
        {
            MaxHeap maxHeapCopy = new MaxHeap();
            maxHeapCopy.MergeHeaps(this);
            while (maxHeapCopy.root != null)                        //delete root (max num in heap) and add in list
            {
                values.Add(maxHeapCopy.root.Data);
                maxHeapCopy.DeleteRoot(maxHeapCopy.root);
            }
        }
        private void DeleteRoot(Node node)                          //delete root with 'deleteNode'
        {
            Node lastNode = GetLastNode();
            node.Data = lastNode.Data;                              //change last node data with node to delete
            DeleteNode(lastNode);                                   //delete last node
            GoDown(node);
            GoUp(node);
        }
        
        
        public void MergeHeaps(MaxHeap heap2)
        {
            MergeHeaps(root, heap2.root);
        }
        private void MergeHeaps(Node node1 , Node node2)            //move Inorder (VLR) in heap2 and add in heap
        {
            if (node2 == null) return;
            InsertHeap(node2.Data);
            MergeHeaps(node1, node2.Left);
            MergeHeaps(node1, node2.Right);
        }
        
        
        public void FindKthBiggestNumInHeap(int k)                  //sorted heap and find k-1 in list
        {
            List<int> heapValues = new List<int>();
            SortHeap(root, heapValues);

            if (k >= 1 && k <= heapValues.Count)
                Console.WriteLine("kth big number is : " + heapValues[k - 1]);
            else Console.WriteLine("Out of range.");
        }
        
        
        public BST ConvertToBST()                                   //move in heap level by level and insert in BST
        {
            BST bst = new BST();

            if (root == null)                                       //check empty BST
                return bst;

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                bst.InsertBST(node.Data);

                if (node.Left != null)
                    queue.Enqueue(node.Left);

                if (node.Right != null)
                    queue.Enqueue(node.Right);
            }

            return bst;
        }
    }
    public class BST
    {
        public Node root;

        public void InsertBST(int value)
        {
            root = InsertBST(root, null, value);
        }
        private Node InsertBST(Node node, Node parent, int value)       //finds the right place and create node there
        {
            if (node == null)
            {
                node = new Node(value);
                node.Father = parent;
            }
            else if (value < node.Data)
            {
                node.Left = InsertBST(node.Left, node, value);
            }
            else
            {
                node.Right = InsertBST(node.Right, node, value);
            }
            return node;
        }
        
        
        public bool IsBST()
        {
            return IsBST(root, int.MinValue, int.MaxValue);
        }
        private bool IsBST(Node node, int min, int max)                 //check with valid range
        {
            if (node == null)
                return true;

            if (node.Data < min || node.Data > max)
                return false;                                           //right child must between father and max
                                                                        //left child must between min and father
            return IsBST(node.Left, min, node.Data - 1) && IsBST(node.Right, node.Data + 1, max);
        }
        
        
        public bool FindAndDeleteFromBST(int value)
        {
            return FindAndDeleteFromBST(root, value);
        }
        private bool FindAndDeleteFromBST(Node node, int value)         //find value location
        {
            if (node == null)                                           //if node not exist
                return false;

            if (value < node.Data)                                      //go left
            {
                return FindAndDeleteFromBST(node.Left, value);
            }
            else if (value > node.Data)                                 //go right
            {
                return FindAndDeleteFromBST(node.Right, value);
            }
            else                                                        //founded
            {
                return DeleteInBST(node);
            }
        }
        private bool DeleteInBST(Node node)                             //remove connection between node & father
        {
            if (node.Left == null && node.Right == null)            //node dont have child
            {

                if (node.Father == null)                                    //root
                {
                    root = null;
                }
                else if (node == node.Father.Left)                          //as father left child
                {
                    node.Father.Left = null;
                }
                else                                                        //as father right child
                {
                    node.Father.Right = null;
                }
            }
            else if (node.Left == null)                             //have right child
            {
                if (node.Father == null)                                    //root
                {
                    root = node.Right;
                }
                else if (node == node.Father.Left)                          //as father left child
                {
                    node.Father.Left = node.Right;
                }
                else                                                        //as father right child         
                {
                    node.Father.Right = node.Right;
                }

                node.Right.Father = node.Father;
            }
            else if (node.Right == null)                            //have left child
            {
                if (node.Father == null)                                    //root
                {
                    root = node.Left;
                }
                else if (node == node.Father.Left)                          //as father left child
                {
                    node.Father.Left = node.Left;
                }
                else                                                        //as father right child
                {
                    node.Father.Right = node.Left;
                }

                node.Left.Father = node.Father;
            }
            else                                                    //have both child
            {
                Node successor = BSTMinimum(node.Right);
                node.Data = successor.Data;
                FindAndDeleteFromBST(node.Right, successor.Data);
            }

            return true;
        }
        private Node BSTMinimum(Node node)                              //find Leftmost node in bst
        {
            if (node.Left == null)
                return node;

            return BSTMinimum(node.Left);
        }
        
        
        public void PrintSortedBST()
        {
            Console.Write("sorted BST : ");
            PrintSortedBST(root);
        }
        private void PrintSortedBST(Node node)                          //print BST and uses Inorder (LVR)
        {
            if (node == null)
                return;

            PrintSortedBST(node.Left);
            Console.Write(node.Data + " - ");
            PrintSortedBST(node.Right);
        }
        
        
        public void MergeBST(BST bst)
        {
            MergeBST(root, bst.root);
        }
        private void MergeBST(Node node1, Node node2)                   //insert in BST uses preorder (VLR)
        {
            if (node2 == null) return;
            InsertBST(node1, null, node2.Data);
            MergeBST(node1, node2.Left);
            MergeBST(node1, node2.Right);
        }
        
        
        public void PrintkthBiggestNumInBST(int k)                      //sort bst and find kth big number
        {
            List<int> BSTValues = new List<int>();
            SortBST(root, BSTValues);

            if (k >= 1 && k <= BSTValues.Count)
                Console.WriteLine("kth big number is : " + BSTValues[k - 1]);
            else Console.WriteLine("Out of range.");
        }
        private void SortBST( Node node , List<int> values)             //move (RLV) in tree and save data in list 
        {
            if (node == null)
                return;

            SortBST(node.Right, values);
            values.Add(node.Data);
            SortBST(node.Left, values);
        }
        private Node BSTMaximum(Node node)                              //find Rightmost node in bst
        {
            if (node.Right == null)
                return node;

            return BSTMaximum(node.Right);
        }
        
        
        public MaxHeap ConvertToMaxHeap()
        {
            MaxHeap maxHeap = new MaxHeap();

            if (root == null)
                return maxHeap;

            ConvertToMaxHeap(root, maxHeap);

            return maxHeap;
        }
        private void ConvertToMaxHeap(Node node, MaxHeap maxHeap)        //insert in heap with preorder (VLR)
        {
            if (node == null)
                return;

            maxHeap.InsertHeap(node.Data);
            ConvertToMaxHeap(node.Left, maxHeap);
            ConvertToMaxHeap(node.Right, maxHeap);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int x = 0;
            bool T = false;
            MaxHeap MH1 = new MaxHeap();
            MaxHeap MH2 = new MaxHeap();
            BST B1 = new BST();
            BST B2 = new BST();
            Console.WriteLine("wellcome to this program.\n");
            try
            {
                Console.WriteLine("  1:Insert Heap                      2:Is Heap ");
                Console.WriteLine("  3:Find & Delete from Heap          4:Print Sorted Heap ");
                Console.WriteLine("  5:Merge Heaps                      6:Print kth Biggest Num In Heap ");
                Console.WriteLine("  7:Heap to BST                      8:Insert BST ");
                Console.WriteLine("  9:Is BST                          10:Find & Delete from BST ");
                Console.WriteLine(" 11:Print Sorted BST                12:Merge BST ");
                Console.WriteLine(" 13:Print kth Biggest Num In BST    14:BST to heap ");
                Console.WriteLine(" 15:Quit");

                Console.Write("\nWhich service you want use : ");
                int i = Convert.ToInt32(Console.ReadLine());
                for (; i != 15;)                                               //check user number to not out of range                   
                {
                    if (i > 15 || i < 1)
                    {
                        Console.Write("service number must between 1 and 15.\nPlease enter again : ");
                        i = Convert.ToInt32(Console.ReadLine());
                    }
                    else break;
                }
                for (; i != 15;)
                {
                    switch (i)
                    {

                        case 1:
                            Console.Write("Please enter a number : ");
                            x = Convert.ToInt32(Console.ReadLine());
                            MH1.InsertHeap(x);

                            Console.WriteLine("Done.");
                            break;


                        case 2:
                            T = MH1.IsHeap();
                            if (T == true) Console.WriteLine("Yes that's Maxheap.");
                            else Console.WriteLine("No that's not MaxHeap.");
                            break;


                        case 3:
                            Console.Write("Please enter a number : ");
                            x = Convert.ToInt32(Console.ReadLine());
                            MH1.FindAndDeleteFromHeap(x);

                            Console.WriteLine("Done.");
                            break;


                        case 4:
                            Console.Write("you want print sorted heap (1) or level by level (2) ? ");
                            x = Convert.ToInt32(Console.ReadLine());
                            if (x == 1) MH1.PrintSortedHeap();
                            else MH1.PrintHeap();
                            Console.WriteLine("Done.");
                            break;


                        case 5:
                            if (MH2.root == null) Console.WriteLine("Please first run '14:BST to heap'.");
                            else
                            {
                                MH1.MergeHeaps(MH2);
                                Console.WriteLine("Done.");
                            }
                            break;


                        case 6:
                            Console.Write("Please enter a number : ");
                            x = Convert.ToInt32(Console.ReadLine());
                            MH1.FindKthBiggestNumInHeap(x);

                            Console.WriteLine("Done.");
                            break;


                        case 7:
                            B2 = MH1.ConvertToBST();

                            Console.WriteLine("Done.");
                            break;


                        case 8:
                            Console.Write("Please enter a number : ");
                            x = Convert.ToInt32(Console.ReadLine());
                            B1.InsertBST(x);

                            Console.WriteLine("Done.");
                            break;


                        case 9:
                            T = B1.IsBST();
                            if (T == true) Console.WriteLine("Yes that's BST.");
                            else Console.WriteLine("No that's not BST.");
                            break;


                        case 10:
                            Console.Write("Please enter a number : ");
                            x = Convert.ToInt32(Console.ReadLine());
                            T = B1.FindAndDeleteFromBST(x);
                            if (T == false) Console.WriteLine("This value not exist in BST.");
                            else Console.WriteLine("Done.");
                            break;


                        case 11:
                            B1.PrintSortedBST();

                            Console.WriteLine("Done.");
                            break;


                        case 12:
                            if (B2.root == null) Console.WriteLine("Please first run '7:Heap to BST'.");
                            else
                            {
                                B1.MergeBST(B2);
                                Console.WriteLine("Done.");
                            }
                            break;


                        case 13:
                            Console.Write("Please enter a number : ");
                            x = Convert.ToInt32(Console.ReadLine());
                            B1.PrintkthBiggestNumInBST(x);

                            break;


                        case 14:
                            MH2 = B1.ConvertToMaxHeap();

                            Console.WriteLine("Done.");
                            break;
                    }
                    Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("  1:Insert Heap                      2:Is Heap ");
                    Console.WriteLine("  3:Find & Delete from Heap          4:Print Sorted Heap ");
                    Console.WriteLine("  5:Merge Heaps                      6:Print kth Biggest Num In Heap ");
                    Console.WriteLine("  7:Heap to BST                      8:Insert BST ");
                    Console.WriteLine("  9:Is BST                          10:Find & Delete from BST ");
                    Console.WriteLine(" 11:Print Sorted BST                12:Merge BST ");
                    Console.WriteLine(" 13:Print kth Biggest Num In BST    14:BST to heap ");
                    Console.WriteLine(" 15:Quit");

                    Console.Write("\nWhich service you want use : ");
                    i = Convert.ToInt32(Console.ReadLine());
                    for (; i != 15;)                                               //check user number to not out of range                   
                    {
                        if (i > 15 || i < 1)
                        {
                            Console.Write("service number must between 1 and 15.\nPlease enter again : ");
                            i = Convert.ToInt32(Console.ReadLine());
                        }
                        else break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}