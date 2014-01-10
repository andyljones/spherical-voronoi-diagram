using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;

namespace Generator
{
    public class FakeSkiplist<T> : IEnumerable<T>
    {
        public Func<T, T, T, bool> InOrder;

        public INode<T> head; 

        public INode<T> FetchNode(T key)
        {
            if (head == null)
            {
                return null;
            }

            if (InOrder(head.Key, key, head.Right.Key))
            {
                return head;
            }
            var node = head.Right;
            while (node != head)
            {
                if (InOrder(node.Key, key, node.Right.Key))
                {
                    return node;
                }
                node = node.Right;
            }

            return null;
        }

        public INode<T> Insert(T key)
        {
            if (head == null)
            {
                head = new Node<T> { Key = key };
                head.ConnectTo(head);
                return head;
            }

            var node = FetchNode(key);
            var newNode = new Node<T> {Key = key};

            newNode.ConnectTo(node.Right);
            node.ConnectTo(newNode);

            return newNode;
        }

        public INode<T> Remove(T key)
        {
            if (Equals(head.Key, key))
            {
                var oldHead = head;
                oldHead.Left.ConnectTo(oldHead.Right);           
                Debug.WriteLine("Key " + key);
                Debug.WriteLine("Arc removed " + head.Key);
                head = oldHead.Right;
                return oldHead;
            }

            var node = head.Right;
            while (node != head)
            {
                if (Equals(node.Key, key))
                {
                    Debug.WriteLine("Key " + key);
                    Debug.WriteLine("Arc removed " + node.Key);
                    node.Left.ConnectTo(node.Right);
                    return node;
                }
                node = node.Right;
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (head == null)
            {
                yield break;
            }

            var node = head;
            yield return node.Key;
            node = node.Right;
            while (node != head)
            {
                yield return node.Key;
                node = node.Right;
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
