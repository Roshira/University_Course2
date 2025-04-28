using System;
using System.Collections.Generic;

namespace ComplexBTree
{
    // Клас для комплексного числа з цілочисельними компонентами
    public struct ComplexInt : IComparable<ComplexInt>
    {
        public int Real { get; }
        public int Imaginary { get; }

        public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);

        public ComplexInt(int real, int imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public int CompareTo(ComplexInt other)
        {
            // Спочатку порівнюємо за модулями
            double magDiff = this.Magnitude - other.Magnitude;
            if (Math.Abs(magDiff) > 0.0001) // Уникаємо проблем з плаваючою точкою
            {
                return magDiff > 0 ? 1 : -1;
            }

            // Якщо модулі рівні, порівнюємо за дійсною частиною
            return this.Real.CompareTo(other.Real);
        }

        public override string ToString()
        {
            return $"{Real}{(Imaginary >= 0 ? "+" : "")}{Imaginary}i";
        }
    }

    // Клас вузла B-дерева
    public class BTreeNode
    {
        public List<ComplexInt> Keys { get; set; }
        public List<BTreeNode> Children { get; set; }
        public bool IsLeaf { get; set; }

        public BTreeNode(bool isLeaf)
        {
            Keys = new List<ComplexInt>();
            Children = new List<BTreeNode>();
            IsLeaf = isLeaf;
        }
    }

    // Клас B-дерева для комплексних чисел
    public class BTree
    {
        private BTreeNode root;
        private int degree;

        public BTree(int degree)
        {
            this.degree = degree;
            root = new BTreeNode(true);
        }

        // Пошук ключа
        public bool Search(ComplexInt key)
        {
            return SearchKey(root, key) != null;
        }

        private BTreeNode SearchKey(BTreeNode node, ComplexInt key)
        {
            int i = 0;
            while (i < node.Keys.Count && key.CompareTo(node.Keys[i]) > 0)
            {
                i++;
            }

            if (i < node.Keys.Count && key.CompareTo(node.Keys[i]) == 0)
            {
                return node;
            }

            if (node.IsLeaf)
            {
                return null;
            }

            return SearchKey(node.Children[i], key);
        }

        // Вставка ключа
        public void Insert(ComplexInt key)
        {
            BTreeNode r = root;
            if (r.Keys.Count == 2 * degree - 1)
            {
                BTreeNode s = new BTreeNode(false);
                root = s;
                s.Children.Add(r);
                SplitChild(s, 0);
                InsertNonFull(s, key);
            }
            else
            {
                InsertNonFull(r, key);
            }
        }

        private void InsertNonFull(BTreeNode node, ComplexInt key)
        {
            int i = node.Keys.Count - 1;

            if (node.IsLeaf)
            {
                while (i >= 0 && key.CompareTo(node.Keys[i]) < 0)
                {
                    i--;
                }
                node.Keys.Insert(i + 1, key);
            }
            else
            {
                while (i >= 0 && key.CompareTo(node.Keys[i]) < 0)
                {
                    i--;
                }
                i++;

                if (node.Children[i].Keys.Count == 2 * degree - 1)
                {
                    SplitChild(node, i);
                    if (key.CompareTo(node.Keys[i]) > 0)
                    {
                        i++;
                    }
                }
                InsertNonFull(node.Children[i], key);
            }
        }

        private void SplitChild(BTreeNode parentNode, int childIndex)
        {
            BTreeNode child = parentNode.Children[childIndex];
            BTreeNode newChild = new BTreeNode(child.IsLeaf);

            parentNode.Keys.Insert(childIndex, child.Keys[degree - 1]);

            newChild.Keys.AddRange(child.Keys.GetRange(degree, degree - 1));
            child.Keys.RemoveRange(degree - 1, degree);

            if (!child.IsLeaf)
            {
                newChild.Children.AddRange(child.Children.GetRange(degree, degree));
                child.Children.RemoveRange(degree, degree);
            }

            parentNode.Children.Insert(childIndex + 1, newChild);
        }

        // Вивід дерева
        public void Print()
        {
            PrintTree(root, "");
        }

        private void PrintTree(BTreeNode node, string indent)
        {
            Console.Write(indent);
            Console.WriteLine(string.Join(" ", node.Keys));

            if (!node.IsLeaf)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    PrintTree(node.Children[i], indent + "  ");
                }
            }
        }
    }

    // Головний клас програми з інтерфейсом користувача
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("B-Tree for Complex Numbers Implementation");
            Console.Write("Enter the degree of B-Tree (minimum 2): ");
            int degree = int.Parse(Console.ReadLine());

            if (degree < 2)
            {
                Console.WriteLine("Degree must be at least 2. Setting degree to 2.");
                degree = 2;
            }

            BTree tree = new BTree(degree);

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Insert complex number");
                Console.WriteLine("2. Search complex number");
                Console.WriteLine("3. Print tree");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter real part: ");
                            int real = int.Parse(Console.ReadLine());
                            Console.Write("Enter imaginary part: ");
                            int imaginary = int.Parse(Console.ReadLine());
                            ComplexInt complex = new ComplexInt(real, imaginary);
                            tree.Insert(complex);
                            Console.WriteLine($"Complex number {complex} inserted successfully.");
                            break;
                        case 2:
                            Console.Write("Enter real part to search: ");
                            int sReal = int.Parse(Console.ReadLine());
                            Console.Write("Enter imaginary part to search: ");
                            int sImaginary = int.Parse(Console.ReadLine());
                            ComplexInt sComplex = new ComplexInt(sReal, sImaginary);
                            bool found = tree.Search(sComplex);
                            Console.WriteLine(found ? $"Complex number {sComplex} found in the tree." : $"Complex number {sComplex} not found in the tree.");
                            break;
                        case 3:
                            Console.WriteLine("B-Tree structure:");
                            tree.Print();
                            break;
                        case 4:
                            Console.WriteLine("Exiting program...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}