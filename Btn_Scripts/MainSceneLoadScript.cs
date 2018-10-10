using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneLoadScript : MonoBehaviour {
    

    public void LoadMainScene()
    {
            SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
    }

}
