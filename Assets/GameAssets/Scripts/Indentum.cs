using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TreeNode {
    
    public readonly Dictionary<string, TreeNode> children = new Dictionary<string, TreeNode>();
    public readonly string key;
    public readonly int    level;
    public TreeNode Parent { get; private set; }

    public TreeNode(string key, int level) {
        this.key = key;
        this.level = level;
    }

    public TreeNode GetChild(string key) {
        return this.children[key];
    }

    public void Add(TreeNode item) {
        if (item.Parent != null) {
            item.Parent.children.Remove(item.key);
        }

        item.Parent = this;
        this.children.Add(item.key, item);
    }
}

public class Indentum {
	
	static int indentWidth = 3;
	static bool isIndentWidthSet = false; 

	static public void ReadLines(string path, Action<string> lineHandler){
		
		System.IO.StreamReader file = new System.IO.StreamReader (path);

		string line;
		while ((line = file.ReadLine()) != null){
			lineHandler (line);
		}
		file.Close ();

	}

	static public TreeNode ParseTree(string path) {

		TreeNode root = new TreeNode("Root", -1);
		TreeNode curr = root;

		Indentum.ReadLines ( path, delegate(string line) {
			if (line.Length == 0){
				return;
			}
			
			int indentLevel = GetIndentLevel(line);
			if(curr.level >= indentLevel)
				curr = BackTrack(curr, curr.level - indentLevel + 1);
			
			curr = ParseLine(curr, line);

		});

		return root;
	}

	static int GetIndentLevel(string line){
		String s = System.Text.RegularExpressions.Regex.Split (line, @"\S+")[0];
		int len = s.Length;

		if (len == 0){
			return 0;
		} else if(isIndentWidthSet){
			if (len % indentWidth == 0) {
				return len / indentWidth;
			} else {
				return 0;
			}
		} else {
			indentWidth = len;
			isIndentWidthSet = true;
			return 1;
		}
		
	}

	static TreeNode ParseLine(TreeNode curr, string line){
		String[] strings = System.Text.RegularExpressions.Regex.Split (line, @"\s+");

		if (strings.Length == 3){
			curr.Add(new TreeNode(strings[1], curr.level + 1));
			curr.GetChild(strings[1]).Add(new TreeNode(strings[2], curr.level + 1));
			return curr.GetChild(strings[1]);

		} else {
			curr.Add(new TreeNode(strings[strings.Length-1], curr.level + 1));
			return curr.GetChild(strings[strings.Length-1]);

		} 
	}

	static TreeNode BackTrack(TreeNode d, int levelDifference) {
		TreeNode cur = d;
		for (int i = levelDifference; i > 0; i--){
			cur = cur.Parent;
		}
		return cur;
	}
}

