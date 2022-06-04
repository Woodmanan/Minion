using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GCSLogo : MonoBehaviour
{
    [Header("Your team info goes here!")]
    public Sprite secondaryLogo;
    public string secondaryName;

    [Header("Logo Pieces Controls")]
    public RectTransform logoContainer;
    public float initialDelay = 1f;
    public float startSpeed = 100f;
    public float timeBetweenSnaps = .5f;
    public float wiggleTime = .3f;
    public float wiggleAmount = 5;

    public float snapTime = 1f;

    public float holdDelay = 1f;

    [Header("Main Logo Text Controls")]
    public float TextFadeTime = .5f;
    [SerializeField]
    TextMeshProUGUI textBox;

    [Header("Secondarry Content Noise")]
    public AudioClip noise;

    [Header("Secondary Logo Controls")]
    public float waitBeforeShowing;
    public Image secondaryLogoImage;
    public float percentOfSpace = 40f;
    public float secondaryLogoTimeToShow = 1f;

    [Header("Secondary Logo Text Controls")]
    public RectTransform teamLogoContainer;
    public float secondaryTextFadeTime = .5f;
    [SerializeField]
    TextMeshProUGUI secondaryTextBox;

    [Header("Meta controls")]
    public bool matchSplashScreenColor = true;
    [SerializeField] KeyCode keyToSkip;

    List<LetterAnimator> letters;

    // Start is called before the first frame update
    void Start()
    {
        letters = GameObject.FindObjectsOfType<LetterAnimator>().OrderBy(x => x.transform.parent.name).ToList();
        StartCoroutine(RunAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToSkip))
        {
            MoveToNext();
        }
    }

    IEnumerator RunAnim()
    {
        Color col = textBox.color;

        col.a = 0;
        textBox.color = col;
        secondaryTextBox.color = col;
        secondaryLogoImage.color = col;
        teamLogoContainer.anchorMin = new Vector2(1, teamLogoContainer.anchorMin.y);


        foreach (LetterAnimator letter in letters)
        {
            Vector2 newVelocity = Random.Range(-startSpeed, startSpeed) * Vector2.up + Random.Range(-startSpeed, startSpeed) * Vector2.right;
            letter.GetComponent<Rigidbody2D>().velocity = newVelocity;
            //yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(initialDelay);

        foreach (LetterAnimator letter in letters)
        {
            StartCoroutine(WiggleAndSnap(letter));
            yield return new WaitForSeconds(timeBetweenSnaps);
        }

        yield return new WaitForSeconds(wiggleTime + snapTime - timeBetweenSnaps);

        for (float t = 0; t < TextFadeTime; t += Time.deltaTime)
        {
            col.a = t / TextFadeTime;
            textBox.color = col;
            yield return null;
        }

        col.a = 1;
        textBox.color = col;

        if (secondaryLogo != null || secondaryName.Length > 0)
        {
            yield return new WaitForSeconds(waitBeforeShowing);
        }

        if (secondaryLogo != null)
        {
            Color white = Color.white;
            white.a = 0;
            //Run logo anim
            secondaryLogoImage.sprite = secondaryLogo;
            secondaryLogoImage.GetComponent<AspectRatioFitter>().aspectRatio = secondaryLogo.rect.width / secondaryLogo.rect.height;
            for (float t = 0; t < secondaryLogoTimeToShow; t += Time.deltaTime)
            {
                float p = t / secondaryLogoTimeToShow;
                logoContainer.anchorMax = new Vector2(1.0f - (percentOfSpace / 100f) * p, logoContainer.anchorMax.y);
                teamLogoContainer.anchorMin = new Vector2(1.0f - (percentOfSpace / 100f) * p, teamLogoContainer.anchorMin.y);
                white.a = p;
                secondaryLogoImage.color = white;
                yield return null;
            }
        }

        if (secondaryLogo != null || secondaryName.Length > 0)
        {
            GetComponent<AudioSource>().PlayOneShot(noise);
        }

            if (secondaryName.Length > 0)
        {
            secondaryTextBox.text = secondaryName.ToLower();
            col.a = 0;
            for (float t = 0; t < secondaryTextFadeTime; t += Time.deltaTime)
            {
                col.a = t / secondaryTextFadeTime;
                secondaryTextBox.color = col;
                yield return null;
            }

            col.a = 1;
            secondaryTextBox.color = col;
        }

        yield return new WaitForSeconds(holdDelay);

        MoveToNext();
    }

    IEnumerator WiggleAndSnap(LetterAnimator letter)
    {
        RectTransform rect = letter.transform as RectTransform;
        Rigidbody2D rigid = letter.rigid;
        Vector2 velocity = rigid.velocity;
        float rot = rect.rotation.z;

        rigid.angularVelocity = 0;
        letter.DisableCollider();

        for (float t = 0; t < wiggleTime; t+= Time.deltaTime)
        {
            rigid.velocity = Vector2.Lerp(velocity, Vector2.zero, t / wiggleTime);
            rigid.angularVelocity = Mathf.Sin(t * 40) * wiggleAmount;
            yield return null;
        }

        Vector2 pos = rect.anchoredPosition;
        Quaternion rotation = rect.rotation;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;

        letter.PlayNoise();

        for (float t = 0; t < snapTime; t += Time.deltaTime)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, Vector2.zero, t / snapTime);
            rect.rotation = Quaternion.Slerp(rect.rotation, Quaternion.identity, t / snapTime);
            yield return null;
        }

        rect.anchoredPosition = Vector2.zero;
        rect.rotation = Quaternion.identity;
        //letter.PlayNoise();
    }

    public void MoveToNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
    void EditorOnlyUpdates()
    {
        #if UNITY_EDITOR

        if (matchSplashScreenColor && Camera.main.backgroundColor != PlayerSettings.SplashScreen.backgroundColor)
        {
            Debug.Log("The camera background was set to something different than the splash scene color. Correcting that now.");
            Camera.main.backgroundColor = PlayerSettings.SplashScreen.backgroundColor;
        }
        

        #endif
    }

    private void OnValidate()
    {
        EditorOnlyUpdates();
    }

}
