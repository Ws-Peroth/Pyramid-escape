using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Button button;
    void Start()
    {
        button.onClick.AddListener(OnButton);
    }

    private void OnButton()
    {
        SceneManager.LoadScene((int) Scenes.Game);
    }
}
