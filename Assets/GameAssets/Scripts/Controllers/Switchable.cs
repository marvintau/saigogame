using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Switchable can be considered as an extension to the TreeNode structure.
/// We typically employ a two-level TreeNode structure as item list, and perform
/// select operation over it. We need to check whether the item is valid to
/// choose, and by selecting the item some subsequent operations might be
/// triggered.
/// </summary>
public class Switchable {

	/// <summary>
	/// The data that loaded from text
	/// </summary>
	public TreeNode data;

	string selectedFirstKey;
	string selectedSecondKey;
	TreeNode selectedNode;

	/// <summary>
	/// Loads the item data from well-indented text
	/// </summary>
	/// <param name="dataPath">path of the data to be loaded</param>
	void LoadItemData(string dataPath) {
		data = Indentum.ParseTree (dataPath);
	}

	/// <summary>
	/// Creates the UI components (mostly buttons) for selecting items.
	/// </summary>
	/// <param name="parentUIName">Parent user interface name.</param>
	/// <param name="CreateComponent">Create component.</param>
	public void CreateComponents(GameObject parentUIObject, Func<string, bool> IsApplicable, Action<string, string, Func<string, bool>, GameObject> CreateComponent){

		foreach (KeyValuePair<string, TreeNode> firstLevel in data.children) {
			foreach (KeyValuePair<string, TreeNode> secondLevel in firstLevel.Value.children) {
				CreateComponent (firstLevel.Key, secondLevel.Key, IsApplicable, parentUIObject);
			}
		}
	}

	/// <summary>
	/// randomly select a component from applicable ones and append
	/// to parent UI component.
	/// </summary>
	/// <param name="parentUIName">Parent user interface name.</param>
	/// <param name="IsNotApplicable">Is not applicable.</param>
	public void AppendRandomComponent(GameObject parentUIObject, Func<string, bool> IsApplicable, Action<string, string, Func<string, bool>, GameObject> AppendComponent){


		string typeKey = data.GetRandomKey ();
		string gearKey = data.GetChild (typeKey).GetRandomKeyWithCond (IsApplicable);

		AppendComponent (typeKey, gearKey, IsApplicable, parentUIObject);

		parentUIObject.GetComponent <UIGrid>().Reposition ();
	}

	/// <summary>
	/// Select the TreeNode with key specified with element returned
	/// </summary>
	/// <param name="key">Key.</param>///
	public void SelectItem(string firstKey, string secondKey){
		selectedFirstKey = firstKey;
		selectedSecondKey = secondKey;
		selectedNode = data.GetChild (selectedFirstKey).GetChild (selectedSecondKey);
		Debug.Log (selectedSecondKey);
	}

	public TreeNode GetSelected(){
		return selectedNode;
	}


	public Switchable(string dataPath){
		LoadItemData (dataPath);
	}
}
