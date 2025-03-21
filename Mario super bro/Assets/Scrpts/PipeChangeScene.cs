using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeChangeScene : MonoBehaviour
{
    public int nextWorld = 1;
    public int nextStage = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DelayedSceneChange());
        }
    }

    private IEnumerator DelayedSceneChange()
    {
        // Wait for 4 seconds
        yield return new WaitForSeconds(3f);

        // After waiting, change the scene
        ChangeScene();
    }

    public void ChangeScene()
    {
        GameManager.Instance.LoadLevel(nextWorld, nextStage);
    }

}
