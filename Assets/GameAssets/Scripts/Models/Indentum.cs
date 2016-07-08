using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.Expando;
using System.Linq;

[System.Serializable]
public class TreeNode : IEnumerable<TreeNode>{
    
    public readonly Dictionary<string, TreeNode> children = new Dictionary<string, TreeNode>();
    public readonly string key;
    public readonly int    level;
    public TreeNode Parent { get; private set; }
	public string leaf;
	public bool isUnlocked = false;

	System.Random rnd = new System.Random ();

    public TreeNode(string key, int level) {
        this.key = key;
        this.level = level;
    }


    public TreeNode GetChild(string key) {
        return this.children[key];
    }

	public List<string> GetAllKeys(){
		return children.Keys.ToList ();
	}

	public string GetRandomKey(){
		return GetAllKeys ().OrderBy (a => rnd.Next ()).ToList ().First ();
	}

	public string GetRandomKeyWithCond(Func <string, bool> Cond){
		return GetAllKeys ().Where(Cond).OrderBy(a => rnd.Next()).ToList().First ();
	}

	public IEnumerator<TreeNode> GetEnumerator(){
		return this.children.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return this.GetEnumerator();
	}

	public void Add(TreeNode item) {
        if (item.Parent != null) {
            item.Parent.children.Remove(item.key);
        }

        item.Parent = this;
        this.children.Add(item.key, item);
    }

	public void AddLeaf(string leaf){
		this.leaf = leaf;
	}

}

public class Indentum {
	
	static int indentWidth = 3;
	static bool isIndentWidthSet = false; 

	static public void ReadLines(string path, Action<string> lineHandler){

		TextAsset textData = (TextAsset)Resources.Load (path, typeof(TextAsset));
		System.IO.StringReader file = new System.IO.StringReader (textData.text);

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
			return (len % indentWidth == 0) ? (len / indentWidth) : 0;
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
			curr.GetChild (strings [1]).AddLeaf (strings [2]);
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

