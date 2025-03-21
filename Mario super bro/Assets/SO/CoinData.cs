using UnityEngine;

[CreateAssetMenu(fileName = "New Coin Data", menuName = "Coin Data")]
public class CoinData : ScriptableObject
{
    [SerializeField]
    private float _coinValue;

    public float coinValue
    {
        get { return _coinValue; }
        set { _coinValue = value; }
    }
}
