using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EggRevealShineEffect : MonoBehaviour
{
    [Header("=== Main Settings ===")]
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float minAlpha;
    [SerializeField] float maxAlpha;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float animationDuration;
    [SerializeField] float fadeoutDuration;
    [Header("--- Shine Fill ---")]
    [SerializeField] float minShineFillScale;
    [SerializeField] float maxShineFillScale;
    [SerializeField] float minShineFillAlpha; 
    [SerializeField] float maxShineFillAlpha;

    [Header("=== References ===")]
    [SerializeField] Image shineFillImage;
    [SerializeField] GameObject shineObject;
    [SerializeField] CanvasGroup eggIconCG;
    [SerializeField] Animator eggIconAnimator;
    [SerializeField] Image shineImage;
    [SerializeField] CanvasGroup resultDisplayCG;

    [Header("=== Debug Info ===")]
    [SerializeField] bool debugMode = false;
    [SerializeField] float animationDelay = 1f;

    public bool isAnimationDone = false;

    Animator animator;
    bool animationStarted = false;
    float currentRotationSpeed;

    private void Awake()
    {
        animator = shineObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Reset Animation
        shineObject.transform.localScale = new Vector3(minScale, minScale, 1);
        canvasGroup.alpha = minAlpha;
        eggIconCG.alpha = 1;
        shineImage.color = new Color(shineImage.color.r, shineImage.color.g, shineImage.color.b, minAlpha);
        currentRotationSpeed = minRotationSpeed;
        shineObject.gameObject.transform.SetAsFirstSibling();
        shineObject.gameObject.SetActive(true);
        eggIconCG.gameObject.SetActive(true);
        shineFillImage.transform.localScale = new Vector3(minShineFillScale, minShineFillScale, 1);
        Color shineTemp = shineFillImage.color;
        shineTemp.a = minShineFillAlpha;
        shineFillImage.color = shineTemp;
        resultDisplayCG.gameObject.SetActive(false);
        resultDisplayCG.alpha = 0;

        // Debug Section
        if (debugMode)
        {
            PlayAnimation();
        }
    }

    private void Update()
    {
        // Rotate accourding to the current rotation speed
        if (animationStarted) shineObject.transform.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
    }

    public void PlayAnimation()
    {
        if (animator) animator.enabled = false;

        eggIconAnimator.enabled = false;
        animationStarted = true;
        shineObject.gameObject.transform.SetAsLastSibling();
        StartCoroutine(ShineAnimation());
    }

    IEnumerator ShineAnimation()
    {
        isAnimationDone = false;

        if (debugMode) yield return new WaitForSeconds(animationDelay);

        // Lerp Scale, Alpha (through canvas group), and rotation
        float t = 0;
        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(minScale, maxScale, animationCurve.Evaluate(t / animationDuration));
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, animationCurve.Evaluate(t / animationDuration));
            float eggAlpha = Mathf.Lerp(1, 0, animationCurve.Evaluate(t / animationDuration));
            float shineAlpha = Mathf.Lerp(minAlpha, 1, animationCurve.Evaluate(t / animationDuration));
            float shineFillAlpha = Mathf.Lerp(minShineFillAlpha, maxShineFillAlpha, animationCurve.Evaluate(t / animationDuration));
            currentRotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, animationCurve.Evaluate(t / animationDuration));
            canvasGroup.alpha = alpha;
            eggIconCG.alpha = eggAlpha;
            shineObject.transform.localScale = new Vector3(scale, scale, 1);
            Color shineTemp = shineImage.color;
            shineTemp.a = shineAlpha;
            shineImage.color = shineTemp;

            // Shine Fill
            shineFillImage.transform.localScale = new Vector3(Mathf.Lerp(minShineFillScale, maxShineFillScale, animationCurve.Evaluate(t / animationDuration)), Mathf.Lerp(minShineFillScale, maxShineFillScale, animationCurve.Evaluate(t / animationDuration)), 1);
            shineTemp = shineFillImage.color;
            shineTemp.a = shineFillAlpha;
            shineFillImage.color = shineTemp;

            yield return null;
        }

        // Fade out effect and fade in the result
        resultDisplayCG.gameObject.SetActive(true);
        t = 0;
        while (t < fadeoutDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0, animationCurve.Evaluate(t / fadeoutDuration));
            float resultAlpha = Mathf.Lerp(0, 1, animationCurve.Evaluate(t / fadeoutDuration));
            canvasGroup.alpha = alpha;
            resultDisplayCG.alpha = resultAlpha;

            yield return null;
        }

        // Set Actove to false
        shineObject.gameObject.SetActive(false);
        eggIconCG.gameObject.SetActive(false);

        isAnimationDone = true;
    }
}
