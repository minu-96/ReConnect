using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
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

    public void HideAsset(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ShowAsset(GameObject obj)
    {
        obj.SetActive(true);
    }
}
