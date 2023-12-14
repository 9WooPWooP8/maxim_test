public class BinarySearchTree
{
    public BinarySearchTree(string input)
    {
        foreach (var c in input)
        {
            this.Insert(c);
        }

    }

    Node? root;

    public class Node
    {
        public char val;
        public Node? left, right;

        public Node(char val)
        {
            this.val = val;
        }

        public void Insert(char newVal)
        {
            if (this.val <= newVal)
            {
                if (this.left == null)
                {
                    this.left = new Node(newVal);
                }
                else
                {
                    this.left.Insert(newVal);
                }
            }
            else
            {
                if (this.right == null)
                {
                    this.right = new Node(newVal);
                }
                else
                {
                    this.right.Insert(newVal);
                }
            }
        }
    }

    public void Insert(char val)
    {
        if (root == null)
        {
            root = new Node(val);
            return;
        }

        root.Insert(val);
    }

    public List<char> Traverse()
    {
        var traversalList = new List<char>();

        TraverseTree(traversalList, root);

        return traversalList;
    }

    private void TraverseTree(List<char> traversalList, Node? node = null)
    {
        if (node == null)
        {
            return;
        }

        TraverseTree(traversalList, node.left);
        traversalList.Add(node.val);
        TraverseTree(traversalList, node.right);
        return;
    }
}
