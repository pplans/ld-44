using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
	public SceneAsset[] levels;

	public void LoadLevelX(int level)
	{
		SceneManager.LoadScene(levels[level].name, LoadSceneMode.Single);
	}
}
