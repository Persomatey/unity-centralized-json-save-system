using System;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

namespace HunterGoodin.SaveSystem
{
	public class SaveTester : MonoBehaviour
	{
		[Header("JSON Stuff")]
		[SerializeField] private TextMeshProUGUI curSaveStringTMP;
		[SerializeField] private TextMeshProUGUI lastSaveStringTMP;
		[SerializeField] private TextMeshProUGUI persistentDataPathTMP;

		[Header("Field Stuff")]
		[SerializeField] private TMP_InputField myStringField;
		[SerializeField] private Toggle myBoolToggle;
		[SerializeField] private TMP_InputField myFloatSlider;
		[SerializeField] private TMP_InputField myIntField;
		[SerializeField] private TMP_InputField myNestedStringField;
		[SerializeField] private Toggle myNestedBoolField;
		[SerializeField] private TextMeshProUGUI intArrTMP;

		public void Update()
		{
			curSaveStringTMP.text = SaveSystem.Instance.CurrentSaveVarToString();
			lastSaveStringTMP.text = SaveSystem.Instance.LastSaveToString();
			persistentDataPathTMP.text = SaveSystem.Instance.PersistentDataPathToString();
		}

		public void UpdateBool()
		{
			SaveSystem.Instance.SaveDataVar.myBool = myBoolToggle.isOn;
		}

		public void UpdateString()
		{
			SaveSystem.Instance.SaveDataVar.myString = myStringField.text;
		}

		public void UpdateFloat()
		{
			SaveSystem.Instance.SaveDataVar.myFloat = float.Parse(myFloatSlider.text);
		}

		public void UpdateInt()
		{
			SaveSystem.Instance.SaveDataVar.myInt = int.Parse(myIntField.text);
		}

		public void UpdateNestedString()
		{
			SaveSystem.Instance.SaveDataVar.user.nestedString = myNestedStringField.text;
		}

		public void UpdateNestedBool()
		{
			SaveSystem.Instance.SaveDataVar.user.nestedBool = myNestedBoolField.isOn;
		}

		public void TryAddHighScore()
		{
			if (SaveSystem.Instance.SaveDataVar.myIntArr.Length < 3)
			{
				SaveSystem.Instance.SaveDataVar.myIntArr = new int[3];
			}

			int lowestIndex = 0;

			int gottenInt = int.Parse(myIntField.text);

			for (int i = 1; i < SaveSystem.Instance.SaveDataVar.myIntArr.Length; i++)
			{
				if (SaveSystem.Instance.SaveDataVar.myIntArr[i] < SaveSystem.Instance.SaveDataVar.myIntArr[lowestIndex])
				{
					lowestIndex = i;
				}
			}

			if (gottenInt <= SaveSystem.Instance.SaveDataVar.myIntArr[lowestIndex])
			{
				return;
			}

			SaveSystem.Instance.SaveDataVar.myIntArr[lowestIndex] = gottenInt;

			Array.Sort(SaveSystem.Instance.SaveDataVar.myIntArr);
			Array.Reverse(SaveSystem.Instance.SaveDataVar.myIntArr);

			string str = $"1. {SaveSystem.Instance.SaveDataVar.myIntArr[0]}\n2. {SaveSystem.Instance.SaveDataVar.myIntArr[1]}\n3. {SaveSystem.Instance.SaveDataVar.myIntArr[2]}";

			intArrTMP.text = str;
		}

		public void Save()
		{
			SaveSystem.Instance.Save();
		}

		public void CustomPathOverride()
		{
			SaveSystem.Instance.UpdatePersistantDataPathToCustomPathFile();
		}

		public void WipeData()
		{
			SaveSystem.Instance.WipeDataToModel();
		}

		public void RevertData()
		{
			SaveSystem.Instance.RevertCurDataToLastSave();
		}
	}
}