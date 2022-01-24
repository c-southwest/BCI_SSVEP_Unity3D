using System.Collections;
using System.Collections.Generic;
using System;

public class QuadTree
{
    public string data;
    public QuadTree one;
    public QuadTree two;
    public QuadTree three;
    public QuadTree four;
    public QuadTree parent;

    public QuadTree()
    {

    }
    public QuadTree(string ch)
    {
        data = ch;
    }

    public static void Build(QuadTree node, Queue<string> queue, int depth, int target)
    {
        if (queue.Count == 0)
        {
            return;
        }

        if (depth < target)
        {
            node.one = new QuadTree() { parent = node };
            Build(node.one, queue, depth + 1, target);
            node.two = new QuadTree() { parent = node };
            Build(node.two, queue, depth + 1, target);
            node.three = new QuadTree() { parent = node };
            Build(node.three, queue, depth + 1, target);
            node.four = new QuadTree() { parent = node };
            Build(node.four, queue, depth + 1, target);

        }
        else
        {
            try
            {
                node.one = new QuadTree(queue.Dequeue());
                node.two = new QuadTree(queue.Dequeue());
                node.three = new QuadTree(queue.Dequeue());
                node.four = new QuadTree(queue.Dequeue());
            }
            catch
            {
                return;
            }
        }
    }

    public static void PrintTree(QuadTree node)
    {
        if (node is null) return;
        if (node.one is null)
        {
            Console.Write(node.data + " ");
        }
        else
        {
            PrintTree(node.one);
            PrintTree(node.two);
            PrintTree(node.three);
            PrintTree(node.four);

        }
    }

    public static void GetTree(QuadTree node, List<string> list)
    {
        if (node is null) return;
        if (node.one is null)
        {
            list.Add(node.data);
        }
        else
        {
            GetTree(node.one, list);
            GetTree(node.two, list);
            GetTree(node.three, list);
            GetTree(node.four, list);
        }
    }
}
