using UnityEngine;
using UnityEngine.SceneManagement;

public class SequenceManager
{
    public void ChangeScene(string changeSceneName)
    {
        SceneManager.LoadScene(changeSceneName);
    }
}
