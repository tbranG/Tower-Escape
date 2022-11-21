using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void LoadScene(int index) { SceneManager.LoadScene(index); }

}
