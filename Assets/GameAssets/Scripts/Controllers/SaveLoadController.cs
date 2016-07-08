using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadController {

	static Data data = GameObject.Find("Data").GetComponent<PersistentData>().data;

	public static void Save(){
		data = GameObject.Find("Data").GetComponent<PersistentData>().data;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, data);
		file.Close();
	}

	public static Data Load() {
		Data data;

		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			data = (Data)bf.Deserialize(file);
			file.Close();
			return data;
		} else {
			return null;
		}
	}
}