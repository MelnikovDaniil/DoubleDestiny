using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject UICoinPrefab;
    public float xForce;
    public float yForce;
    public float spawnRate;
    public RectTransform moneySpace;
    public Animator moveToCoinIcon;
    public Text moneyText;
    public float movingSeconds;
    public int moneyCount;

    public AudioClip moneyCollectSound;

    private const float CoinMovingTime = 2f;
    private const float MaxRangeCoinExecutingTime = 0.15f;
    private List<MoveToModel> movingCoins;
    private float moneySizeCoof;

    // Start is called before the first frame update
    void Start()
    {
        movingCoins = new List<MoveToModel>();
        moneySizeCoof = Screen.height / 720.00f;
        //InvokeRepeating("SpawnCoins", 0, spawnRate);
    }

    private void Update()
    {
        foreach (var uiCoin in movingCoins)
        {
            uiCoin.rectTransform.transform.position = Vector2.Lerp(uiCoin.startPosition,
                moveToCoinIcon.transform.position,
                uiCoin.progress);
            uiCoin.progress += Time.deltaTime / movingSeconds;
            if (uiCoin.progress >= 1)
            {
                moveToCoinIcon.Play("Money");
                SoundManager.PlaySoundUI(moneyCollectSound);
                Destroy(uiCoin.rectTransform.gameObject);
                moneyCount++;
                moneyText.text = moneyCount.ToString();
            }
        }

        movingCoins.RemoveAll(x => x.progress >= 1);
    }

    public void SpawnCoins(EnemyScript enemy)
    {
        for (int i = 0; i < enemy.coinReward; i++)
        {
            var coin = Instantiate(coinPrefab, enemy.transform.position, Quaternion.identity, transform);
            var executeTime = Random.Range(CoinMovingTime - MaxRangeCoinExecutingTime, CoinMovingTime + MaxRangeCoinExecutingTime);
            StartCoroutine(ExecuteAfterTime(coin, executeTime));
            var forceVector = new Vector2(Random.Range(-10.0f, 10.0f) * xForce, Random.Range(5f, 10.0f) * yForce);
            coin.GetComponent<Rigidbody2D>().AddRelativeForce(forceVector);
        }
    }

    IEnumerator ExecuteAfterTime(GameObject coin, float time)
    {
        yield return new WaitForSeconds(time);
        MoveToMoneyStorage(coin);
        Destroy(coin);
        // Code to execute after the delay
    }

    public void MoveToMoneyStorage(GameObject coin)
    {
        var uiCoin = Instantiate(UICoinPrefab, moneySpace);
        var viewportPosition = Camera.main.WorldToViewportPoint(coin.transform.position);
        var uiCoinRect = uiCoin.GetComponent<RectTransform>();
        uiCoinRect.anchorMin = viewportPosition;
        uiCoinRect.anchorMax = viewportPosition;
        movingCoins.Add(new MoveToModel()
        {
            startPosition = uiCoinRect.transform.position,
            rectTransform = uiCoinRect,
        });
        uiCoinRect.sizeDelta *= moneySizeCoof;
    }

    private class MoveToModel
    {
        public Vector2 startPosition;
        public RectTransform rectTransform;
        public float progress;
    }
}
