using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace HunterGoodin.SaveSystem
{
	public class SaveSystem : MonoBehaviour
	{
		// Singleton Stuff
		public static SaveSystem Instance => instance;
		private static SaveSystem instance;
		public bool initialized { get; private set; }

		// Save System Stuff 
		private string path;
		[SerializeField] private TextAsset model;
		[SerializeField] private TextAsset custom;
		private SaveData saveDatavar = new SaveData();
		public SaveData SaveDataVar => saveDatavar;

		// MonoBehaviour Functions 

		private void Awake()
		{
			// Singleton Stuff 
			if (instance != null)
			{
				Destroy(gameObject);
			}
			else
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}

		private void Start()
		{
			path = Path.Combine(Application.persistentDataPath, "saveData.json");

			if (!File.Exists(path))
			{
				File.WriteAllText(path, model.text);
				Save();
			}

			SaveParser();
		}

		// Save Utilities 

		private void SaveParser()
		{
			Debug.Log($"Looking for save data at '{path}'");

			// Parse 
			if (File.Exists(path))
			{
				Debug.Log($"Save data at path '{path}' exists. Reading save data.");
				string json = File.ReadAllText(path);
				Debug.Log($"Save data text:\n{json}");
				saveDatavar = JsonUtility.FromJson<SaveData>(json);
			}
			else
			{
				Debug.LogError($"Save file not found at path '{path}'!");
				return;
			}

			initialized = true;
		}

		public void Save()
		{
			// Serialize the data 
			string json = JsonUtility.ToJson(saveDatavar, true);
			File.WriteAllText(path, json);

			// Log 
			Debug.Log($"Saving \n{json} \nto '{path}'");

			// We don't NEED to parse again... but just for the sanity check, I'm going to 
			SaveParser();
		}

		// Override Saves 

		public void WipeDataToModel()
		{
			Debug.Log($"Wiping data at '{path}'");
			File.WriteAllText(path, model.text);
			Debug.Log($"Saved the following to '{path}' to model:\n{model.text}");
		}

		public void UpdatePathFileToModelButWithExistingDataOnPersistantDataPath()
		{
			Debug.Log($"Updating data at '{path}'");
			string json = JsonUtility.ToJson(saveDatavar, true);
			File.WriteAllText(path, json);
			Debug.Log($"Saved the following to '{path}':\n{json}");
		}

		public void UpdatePersistantDataPathToCustomPathFile()
		{
			Debug.Log($"Updating data to Custom at '{path}'");
			File.WriteAllText(path, custom.text);
			Debug.Log($"Saved the following to '{path}' to custom:\n{custom.text}");
		}

		public void RevertCurDataToLastSave()
		{
			SaveParser();
		}

		// To Strings 

		public string CurrentSaveVarToString()
		{
			string json = JsonUtility.ToJson(saveDatavar, true);
			Debug.Log(json);
			return json;
		}

		public string LastSaveToString()
		{
			string json = File.ReadAllText(path);
			return json;
		}

		public string PersistentDataPathToString()
		{
			return path;
		}

		// Data Class 

		[System.Serializable]
		public class SaveData
		{
			public User user;
			public bool myBool;
			public string myString;
			public float myFloat;
			public int myInt;
			public int[] myIntArr;

			[System.Serializable]
			public class User
			{
				public string nestedString;
				public bool nestedBool;
			}
		}
	}
}