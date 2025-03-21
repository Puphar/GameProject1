using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public GameObject popUpScorePrefab;
    public TMP_Text popUptext;

    public ScoreData scoreData;
    public TextMeshProUGUI scoreText;


    private void Start()
    {
        scoreText.text = " " + scoreData.scoreValue;
    }


    public void Hit()
    {
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);

        popUptext.text = "100";
        Instantiate(popUpScorePrefab, transform.position, Quaternion.identity);

        scoreData.scoreValue += 100;
        scoreText.text = " " + scoreData.scoreValue;
        Debug.Log("Add score");
    }
}
