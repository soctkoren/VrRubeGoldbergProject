using UnityEngine;
using UnityEngine.SceneManagement;

public class GetActiveScene : MonoBehaviour
{
    private Scene scene;
    public bool levelWon;

    void Start()
    {
       levelWon = false;
    }

    void Update()
    {
        if (levelWon)
        {
            scene = SceneManager.GetActiveScene();
            char last = scene.name[scene.name.Length - 1];
            int levelIdx = (int)System.Char.GetNumericValue(last) + 1;
            Application.LoadLevel("Level" + levelIdx);
            levelWon = false;
        }
    }
}