using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageStateCutScene : MonoBehaviour
{
    public int nextWorld = 1;
    public int nextStage = 3;

    private void Start()
    {
        StartCoroutine(DelayedSceneChange());
    }

    private IEnumerator DelayedSceneChange()
    {
        // Wait for 4 seconds
        yield return new WaitForSeconds(7f);

        // After waiting, change the scene
        ChangeScene();
    }

    public void ChangeScene()
    {
        GameManager.Instance.LoadLevel(nextWorld, nextStage);
    }
}
