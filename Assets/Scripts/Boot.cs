﻿using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    private void Awake()
    {
        //NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("TeamSelection", LoadSceneMode.Single);
    }
}