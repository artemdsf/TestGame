using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public void LoadNextScene()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;

		if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
		{
			nextSceneIndex = 0;
		}

		SceneManager.LoadScene(nextSceneIndex);
	}
}
