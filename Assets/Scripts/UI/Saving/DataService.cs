using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton - There should only ever be one DataService and it should persist
/// between scene loads.
/// This class is responsible for loading/saving data.
/// </summary>
public class DataService : MonoBehaviour
{
	private static DataService _instance = null;
	public static DataService Instance
	{
		get
		{
			// If the instance of this class doesn't exist
			if (_instance == null)
			{
				// Check the scene for a Game Object with this class
				_instance = FindObjectOfType<DataService>();

				// If none is found in the scene then create a new Game Object
				// and add this class to it.
				if (_instance == null)
				{
					GameObject go = new GameObject(typeof(DataService).ToString());
					_instance = go.AddComponent<DataService>();
				}
			}

			return _instance;
		}
	}

	// When the scene first runs ensure that there is only one
	// instance of this class. This allows us to add it to any scene and 
	// not conflict with any pre-existing instance from a previous scene.
	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(this);
		}
		else
		{
			DontDestroyOnLoad(gameObject);

			// In Unity 5.4 OnLevelWasLoaded has been deprecated and the action
			// now occurs through this callback.
			SceneManager.sceneLoaded += OnLevelLoad;
		}
	}

	/// <summary>
	/// The currently loaded Save Data.
	/// </summary>
	public SaveData SaveData { get; private set; }

	/// <summary>
	/// Ensure that the player preferences are applied to the new scene.
	/// </summary>
	// In Unity 5.4 OnLevelWasLoaded has been deprecated and the action
	// now occurs through 'SceneManager.sceneLoaded' callback.
	void OnLevelLoad(Scene scene, LoadSceneMode sceneMode)
	{
		if (scene.name == "Test level")
        {
			Load();
			Save();
		}
	}

	public void Save()
    {
		SaveData.enemiesAlive = GetLivingEnemies().ToList();
		GameObject Player = GameObject.Find("Player");
		SaveData.playerPos = Player.transform.position;
		SaveData.playerHealth = Player.GetComponent<PlayerMovement>().hc.GetHealth();
		SaveData.WriteToFile("/save.dat");
    }

	public void SaveAtGroundPos(Vector2 pos)
	{
		SaveData.enemiesAlive = GetLivingEnemies().ToList();
		GameObject Player = GameObject.Find("Player");
		SaveData.playerPos = GetPlayerPosByBottom(pos);
		SaveData.playerHealth = Player.GetComponent<PlayerMovement>().hc.GetHealth();
		SaveData.WriteToFile("/save.dat");
	}

	public Vector2 GetPlayerPosByBottom(Vector2 bottom)
	{
		GameObject Player = GameObject.Find("Player");
		Vector2 s = Player.GetComponent<BoxCollider2D>().size/2;
		return new Vector2(bottom.x, bottom.y + s.y);
	}

	public void Load()
	{
		SaveData tempSaveData = SaveData.ReadFromFile("/save.dat");
		if (tempSaveData == null)
        {
			SaveData = new SaveData();
			return;
        }
		SaveData = tempSaveData;
		HashSet<string> enemies = new HashSet<string>(SaveData.enemiesAlive);
		Transform Player = GameObject.Find("Player").transform;
		Player.position = new Vector3(SaveData.playerPos.x, SaveData.playerPos.y, Player.position.z);
		Player.GetComponent<PlayerMovement>().SetHealth(SaveData.playerHealth);
		Transform Camera = GameObject.Find("Main Camera").transform;
		Camera.position = new Vector3(Player.position.x, Player.position.y, Camera.position.z);
		foreach (Transform t in GameObject.Find("Enemies").transform)
		{
			if (t.gameObject.activeInHierarchy)
			{
				Enemy e = t.GetComponent<Enemy>();
				string name = e.GetUniqueName();
				if (!enemies.Contains(name))
				{
					t.gameObject.SetActive(false);
				}
				e.ReturnToStart();
			}
		}
	}

	public void ClearSave()
    {
		SaveData.ClearSave("/save.dat");
    }

    public HashSet<string> GetLivingEnemies()
    {
		HashSet<string> enemies = new HashSet<string>();
		foreach (Transform t in GameObject.Find("Enemies").transform)
        {
			if (t.gameObject.activeInHierarchy)
            {
				enemies.Add(t.GetComponent<Enemy>().GetUniqueName());
			}
		}
		return enemies;
    }
}

