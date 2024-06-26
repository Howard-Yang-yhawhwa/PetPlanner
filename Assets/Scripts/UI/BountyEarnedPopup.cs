using Ricimi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BountyEarnedPopup : MonoBehaviour
{
    [Header("=== VFX Settings ===")]
    [SerializeField] GameObject UICoinsPrefab;
    [SerializeField] GameObject CoinsContainer;
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform EndPoint;
    [SerializeField] float coinStartSize = 120f;
    [SerializeField] float coinEndSize = 80f;
    [SerializeField] float SpawnRadius = 5f;
    [SerializeField] float lerpDuration;
    [SerializeField] AnimationCurve lerpCurve;
    [SerializeField] Animator coinBadAnimator;

    [Header("=== UI Elements ===")]
    [SerializeField] TMP_Text amountText;
    [SerializeField] TMP_Text notificationText;
    [SerializeField] Button triggerButton;
    [SerializeField] Button closeButton;
    [SerializeField] Popup popupManager;
    [SerializeField] GameObject[] HideObjects;
    [SerializeField] GameObject[] ShowObjects;

    [Header("=== Debug Settings ===")]
    [SerializeField] bool debugMode;
    [SerializeField] int debugAmount;


    private void Start()
    {
        if (debugMode)
        {
            InitAndOpen(debugAmount);
        }
    }

    int amount;
    public void InitAndOpen(int amount)
    {
        foreach (GameObject showObject in ShowObjects)
        {
            showObject.SetActive(false);
        }

        foreach(GameObject hideObject in HideObjects)
        {
            hideObject.SetActive(true); 
        }

        StopAllCoroutines();

        this.amount = amount;
        amountText.text = "+" + amount.ToString();
        StartCoroutine(TitleAnimation());

        triggerButton.onClick.AddListener(() => SpawnCoins());
        closeButton.onClick.AddListener(() => Close());

        popupManager.Open();
    }

    public void Close()
    {
        popupManager.Close();
    }

    IEnumerator TitleAnimation()
    {
        string targetText = "Bounty Earned!";
        string currentText = "";

        for (int i = 0; i < targetText.Length; i++)
        {
            
            notificationText.text = $"{currentText}<color=#FFFFFF>{targetText[i]}</color>";
            currentText += targetText[i] ;
            yield return new WaitForSeconds(0.08f);
        }

        notificationText.text = currentText;
    }

    public void SpawnCoins()
    {
        StartCoroutine(MainAniamtionRoutine());
    }

    IEnumerator MainAniamtionRoutine()
    {
        coinBadAnimator.SetTrigger("Play");

        foreach (GameObject hideObject in HideObjects)
        {
            hideObject.SetActive(false);
        }

        int amountRemaining = amount;
        int coinCount = Mathf.Min(100, Mathf.CeilToInt(amount / 50f));
        List<GameObject> coins = new List<GameObject>();

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPos = Random.insideUnitCircle * SpawnRadius;
            spawnPos += StartPoint.position;

            GameObject coin = ObjectPooler.SpawnObject(UICoinsPrefab, spawnPos, Quaternion.identity, CoinsContainer.transform);
            coin.transform.localScale = new Vector3(1, 1, 1);
            coins.Add(coin);

            StartCoroutine(LerpCoin(coin));

            int deductAmount = Mathf.CeilToInt((float)amount / coinCount);
            amountRemaining = amountRemaining - deductAmount;
            Debug.Log($"Amount Remaining: {amountRemaining}");

            // Add credits to Player Account
            int addAmount = amountRemaining >= 0 ? Mathf.CeilToInt((float)amount / coinCount) : amountRemaining + deductAmount;
            Player.Coins += addAmount;

            // Update Visuals
            amountText.text = "+" + Mathf.Max(0, amountRemaining).ToString();
            yield return new WaitForSeconds(0.02f);
        }

        coinBadAnimator.SetBool("Done", true);

        foreach (GameObject showObject in ShowObjects)
        {
            showObject.SetActive(true);
        }
    }

    IEnumerator LerpCoin(GameObject coin)
    {
        float elapsedTime = 0;
        Vector3 startPos = coin.transform.position;
        Vector3 endPos = EndPoint.position;

        while (elapsedTime < lerpDuration)
        {
            coin.transform.position = Vector3.Lerp(startPos, endPos, lerpCurve.Evaluate(elapsedTime / lerpDuration));
            ((RectTransform)coin.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(coinStartSize, coinEndSize, lerpCurve.Evaluate(elapsedTime / lerpDuration)));
            ((RectTransform)coin.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(coinStartSize, coinEndSize, lerpCurve.Evaluate(elapsedTime / lerpDuration)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        coin.transform.position = endPos;

        ObjectPooler.ReleaseObject(coin);
    }
}
