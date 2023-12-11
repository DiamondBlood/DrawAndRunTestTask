using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimationEvent : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void Disable() => _gameObject.SetActive(false);
    public void Enable() => _gameObject.SetActive(true);
}
