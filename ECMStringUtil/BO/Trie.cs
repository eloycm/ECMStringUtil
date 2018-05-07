using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMStringUtil.BO
{
    public class Trie
    {
        public Char CharValue { get; set; }
        public Trie Parent { get; set; }
        public Trie LastReadNode { get; set; }
        public string LastSearch { get; set; }
        public Dictionary<char, Trie> Children { get; set; }
        public bool IsCompleteWord { get; set; }

        public Trie()
        {
            Children = new Dictionary<char, Trie>();
        }
        public Trie(Trie parent)
        {
            Children = new Dictionary<char, Trie>();
            Parent = parent;
        }

        public Trie AddChild(char c)
        {
            if (Children.ContainsKey(c))
                return Children[c];

            var child = new Trie(this);
            child.CharValue = c;
            Children.Add(c, child);

            return child;
        }
        public void AddString(string s)
        {
            var current = this;

            foreach (var c in s)
            {
                current = current.AddChild(c);

            }
            LastReadNode = current;
            current.IsCompleteWord = true;
        }
        public void AddStrings(IEnumerable<string> ss)
        {
            foreach (var s in ss)
            {
                AddString(s);
            }
        }
        public bool Exist(string s)
        {
            var current = this;
            foreach (var c in s)
            {
                if (!current.Children.ContainsKey(c))
                {
                    LastReadNode = current;
                    LastSearch = s;
                    return false;
                }

                current = current.Children[c];
            }
            LastReadNode = current;
            LastSearch = s;
            return current.IsCompleteWord || current.Children.Count == 0;
        }
        public bool Contains(string s)
        {
            var current = this;
            foreach (var c in s)
            {
                if (!current.Children.ContainsKey(c))
                    return false;
                current = current.Children[c];
            }
            LastReadNode = current;
            LastSearch = s;
            return true;
        }
        public bool CheckChar(char c)
        {
            if (!LastReadNode.Children.ContainsKey(c))
                return false;
            LastReadNode = LastReadNode.Children[c];

            return true;
        }
        public List<string> WordsDown()
        {
            foreach (var c in Children)
            {
                if (c.Value.Children.Count == 0)
                    return new List<string>();


            }
            return null;
        }
        public string GetCurrentWord(Trie t)
        {

            var rs = "";
            while (t.Parent != null)
            {
                rs = t.CharValue + rs;
                t = t.Parent;
            }
            return rs;
        }
        public string GetCurrentWord()
        {
            return GetCurrentWord(LastReadNode);
        }
        public List<Trie> WordsPositions(Trie t)
        {
            var rs = new List<Trie>();
            foreach (var c in t.Children)
            {
                if (c.Value.IsCompleteWord || c.Value.Children.Count == 0)
                {
                    rs.Add(c.Value);
                    if (c.Value.Children.Count > 0)
                    {
                        var t3 = WordsPositions(c.Value);
                        rs.AddRange(t3);
                    }
                    continue;
                }
                var t2 = WordsPositions(c.Value);
                rs.AddRange(t2);
            }
            return rs;
        }
        public List<Trie> WordsPositions()
        {
            return WordsPositions(LastReadNode);
        }
        public List<string> GetDescendantWords(Trie t)
        {
            var pos = WordsPositions(t);
            var rs = new List<string>();

            foreach (var c in pos)
            {
                var s = GetCurrentWord(c);
                rs.Add(s);
            }

            return rs;

        }
        public List<string> GetDescendantWords()
        {
            return GetDescendantWords(LastReadNode);
        }
    }
}
