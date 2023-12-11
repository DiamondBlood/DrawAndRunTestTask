using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public static Player Singleton { get; private set; }
    
    [SerializeField] private List<GameObject> _guys;
    [SerializeField] private TMP_Text _gems;
    [SerializeField] private Animator _restartFrame;
    [SerializeField] private SplineFollower _follower;
    [SerializeField] private Transform _playerParent;
    [SerializeField] private Draw _drawer;
    [SerializeField] private Camera _camera;

    public float offset;
    private int _gemsCount;
    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _gemsCount = PlayerPrefs.GetInt("Gems", 0);
    }
    public void GetGem()
    {
        _gemsCount++;
        PlayerPrefs.SetInt("Gems", _gemsCount);
        _gems.text = _gemsCount.ToString();
    }

    public void GameStart()
    {
        foreach (GameObject guy in _guys) { guy.GetComponent<Guy>().SetRunAnimation(); }
    }

    public void LevelPassed() => StartCoroutine(SetWinState());

    IEnumerator SetWinState()
    {
        _camera.transform.DOMove(new Vector3(0f, 4f, 58f), 0.3f);
        _camera.transform.DORotate(new Vector3(30f, 0f, 0f), 0.3f);
        int rows = _guys.Count / 10;
        int left;
        if (rows!=0)
        {
            left = _guys.Count % (rows * 10);
        }
        else { left = _guys.Count % 10; }
        int rowsMax = rows + 1;
        int counter = 0;
        for (int i = 1; i < rowsMax+1; i++)
        {
            if (i!=rowsMax)
            {
                for (int j = 0; j < 10; j++)
                {
                    _guys[counter].transform.DOLocalMove(new Vector3(-0.83f + (j * 0.2f), 0f, 2.1f + (-0.3f * i)), 0.5f);
                    _guys[counter].transform.DORotate(new Vector3(0f, 180f), 0.5f);
                    counter++;
                }
            }
            else
            {
                for (int j = 0; j < left; j++)
                {
                    _guys[counter].transform.DOLocalMove(new Vector3(-0.83f + (j * 0.2f), 0f, 2.1f + (-0.3f * i)), 0.5f);
                    _guys[counter].transform.DORotate(new Vector3(0f, 180f), 0.5f);
                    counter++;
                }
            }
            
        }
        foreach(GameObject guy in _guys)
        {
            guy.GetComponent<Guy>().SetWinAnimation();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void AddGuy(GameObject guy)
    {
        _guys.Add(guy);
        guy.tag = "Player";
        guy.GetComponent<Guy>().enabled = true;
        guy.GetComponent<Guy>().SetRunAnimation();
        guy.GetComponent<Guy>().ChangeMaterial();
        guy.transform.SetParent(_playerParent);
        guy.transform.DOMoveZ(guy.transform.position.z + offset, 0.2f);
        guy.transform.DORotate(new Vector3(-25f, 0), 0.2f);

    }

    public void DestroyGuy(GameObject guy)
    {
        guy.transform.SetParent(null);
        _guys.Remove(guy);
        if (_guys.Count == 0)
        {
            _drawer.enabled = false;
            _restartFrame.SetTrigger("In");
            _follower.enabled = false;
        }
    }

    public void ChangePositions(Vector3[] points)
    {
        int range = points.Length/_guys.Count;
        for (int i = 0; i < _guys.Count; i++)
        { 
            _guys[i].transform.DOMove(new Vector3(points[i * range].x*2.5f, points[i * range].y - points[i*range].y, points[i * range].y * 4  + points[i * range].z-1f), 0.2f);
        }
    }
}
