using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;	//List<>, Contains(), Remove()

public class GameManager : Singleton<GameObject>
{

	private string _currentLevelName = string.Empty;

	List<AsyncOperation> _loadOperation;

	private void Start(){

		DontDestroyOnLoad (gameObject);
		_loadOperation = new List<AsyncOperation>();
		LoadLevel ("Scene");
	}

	void OnLoadOperationComplete(AsyncOperation ao){

		if (_loadOperation.Contains (ao)) {
			_loadOperation.Remove (ao);
		}

		Debug.Log ("Load Complete");
	}

	void OnUnloadOperationComplete(AsyncOperation ao){
		Debug.Log ("Unload Complete");
	}

	public void LoadLevel(string levelName){

		AsyncOperation ao = SceneManager.LoadSceneAsync (levelName, LoadSceneMode.Additive);
		if (ao == null) {
			Debug.LogError ("[Game Manager] Unable to load: " + levelName);
			return;
		}
		ao.completed += OnLoadOperationComplete;
		_loadOperation.Add (ao);
		_currentLevelName = levelName;
	}

	public void UnloadLevel(string levelName){
		AsyncOperation ao = SceneManager.UnloadSceneAsync (levelName);
		if (ao == null) {
			Debug.LogError ("[Game Manager] Unable to unload: " + levelName);
			return;
		}
		ao.completed += OnUnloadOperationComplete;
	}

}
