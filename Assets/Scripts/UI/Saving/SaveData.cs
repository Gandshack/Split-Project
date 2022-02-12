using UnityEngine;
using System.IO; // Required fro reading/writing to files.
using System.Collections.Generic; // Used for Lists

/// <summary>
/// Responsible for:
/// - Maintaining the stats for a player and their progress
/// - Writing this data to a file.
/// - Reading this data from a file.
/// </summary>
public class SaveData
{
	#region Defaults
	#endregion

	// We initialize all of the stats to be default values.
	public List<string> enemiesAlive;
	public Vector2 playerPos;
	public int playerHealth;

	const bool DEBUG_ON = true;

	/// <summary>
	/// Writes the instance of this class to the specified file in JSON format.
	/// </summary>
	/// <param name="filePath">The file name and full path to write to.</param>
	public void WriteToFile(string filePath)
	{
		filePath = Application.persistentDataPath + filePath;
		// Convert the instance ('this') of this class to a JSON string with "pretty print" (nice indenting).
		string json = JsonUtility.ToJson(this, true);

		// Write that JSON string to the specified file.
		File.WriteAllText(filePath, json);

		// Tell us what we just wrote if DEBUG_ON is on.
		if (DEBUG_ON)
			Debug.LogFormat("WriteToFile({0}) -- data:\n{1}", filePath, json);
	}

	/// <summary>
	/// Returns a new SaveData object read from the data in the specified file.
	/// </summary>
	/// <param name="filePath">The file to attempt to read from.</param>
	public static SaveData ReadFromFile(string filePath)
	{
		filePath = Application.persistentDataPath + filePath;
		// If the file doesn't exist then just return the default object.
		if (!File.Exists(filePath))
		{
			Debug.LogErrorFormat("ReadFromFile({0}) -- file not found, returning new object", filePath);
			return null;
		}
		else
		{
			// If the file does exist then read the entire file to a string.
			string contents = File.ReadAllText(filePath);

			// If debug is on then tell us the file we read and its contents.
			if (DEBUG_ON)
				Debug.LogFormat("ReadFromFile({0})\ncontents:\n{1}", filePath, contents);

			// Otherwise we can just use JsonUtility to convert the string to a new SaveData object.
			return JsonUtility.FromJson<SaveData>(contents);
		}
	}

	public static void ClearSave(string filePath)
	{
		File.Delete(Application.persistentDataPath + filePath);
	}

	/// <summary>
	/// A friendly string representation of this object.
	/// </summary>
	public override string ToString()
	{
		return "";
	}
}