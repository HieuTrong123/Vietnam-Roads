using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        FindObjectOfType<AudioManager>().PlaySound("ButtonClick");
        StartCoroutine(LoadScene());
    }
    public void QuitGame(){
        Application.Quit();
    }
    private IEnumerator LoadScene(){
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Level");
    }
}
