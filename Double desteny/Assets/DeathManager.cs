using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public GameObject DeathPanel;
    public CameraManager cameraManager;
    public GameObject characters;

    public void EndOfGame()
    {
        cameraManager.SetTarget(characters, 3.5f, 1, Vector3.zero);
        Time.timeScale = 0.5f;
        //GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        Invoke("ActivateDeathPanel", 1);
    }

    private void ActivateDeathPanel()
    {
        Time.timeScale = 1;
        DeathPanel.SetActive(true);
    }
}
