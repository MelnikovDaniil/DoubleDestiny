using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPresentation : MonoBehaviour
{
    public virtual string Name { get; }
    public GameObject PresentationPanel { get; set; }
    public CameraManager CameraManager { get; set; }
    public CharactersScript characters { get; set; }

    public AudioClip presentationSound;

    public virtual void BeginPresentBoss()
    {
        if (PresentationPanel != null)
        {
            Time.timeScale = 0.1f;
            SoundManager.PlaySoundUI("whip");
            SoundManager.PlaySoundUI(presentationSound);
            PresentationPanel.SetActive(true);
        }
    }
    public virtual void EndPresentBoss()
    {
        if (PresentationPanel != null)
        {
            Time.timeScale = 1f;
            PresentationPanel.SetActive(false);
        }
    }

    public void CameraGoBack()
    {
        CameraManager.SetCameraBasePosition();
    }
}
