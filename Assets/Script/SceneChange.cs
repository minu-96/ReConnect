using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void SceneChange0(string Scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Scene);
    }

    private void Start()
    {
        string name = SceneManager.GetActiveScene().name;
        if(name == "Loding")
        {
            SceneManager.LoadScene("InGame");
        }
    }
}
