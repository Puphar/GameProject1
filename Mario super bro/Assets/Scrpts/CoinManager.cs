using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public CoinData coinData;
    public TextMeshProUGUI coinText;

    void Update()
    {
        coinText.text = "x"+ coinData.coinValue;
    }
}
