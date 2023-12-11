using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevelText;
    [SerializeField] private TMP_Text _levelPassedText;
    public void OnNextClick()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level",1)+1);
        PlayerPrefs.Save();
        _currentLevelText.text = "LEVEL " + PlayerPrefs.GetInt("Level", 0).ToString();
        _levelPassedText.text = _currentLevelText.text + " PASSED";
        SceneManager.LoadScene(0);
    }
    public void Restart() => SceneManager.LoadScene(0);
}
