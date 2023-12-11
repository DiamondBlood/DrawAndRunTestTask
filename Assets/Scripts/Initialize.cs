using TMPro;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevelText;
    [SerializeField] private TMP_Text _levelPassedText;
    [SerializeField] private TMP_Text _gems;
    private void Start()
    {
        _gems.text = PlayerPrefs.GetInt("Gems",0).ToString();
        _currentLevelText.text = "LEVEL " + PlayerPrefs.GetInt("Level",0).ToString();
        _levelPassedText.text = _currentLevelText.text + " PASSED";
    }
}
