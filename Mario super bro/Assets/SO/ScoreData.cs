using UnityEngine;

[CreateAssetMenu(fileName = "New Score Data", menuName = "Score Data")]
public class ScoreData : ScriptableObject
{
    [SerializeField]
    private float _scoreValue;

    public float scoreValue
    {
        get { return _scoreValue; }
        set { _scoreValue = value; }
    }
}
