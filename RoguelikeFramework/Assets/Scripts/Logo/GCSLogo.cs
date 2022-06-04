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
    [Tooltip("Your team logo")]
    public Sprite secondaryLogo;
    [Tooltip("Your team name, possibly with something extra like 'Fall 2018'")]
    public string secondaryName;

    [Header("Meta controls")]
    [Tooltip("Automagically makes the background match the Unity splash, to make it more seamless")]
    public bool matchSplashScreenColor = true;
    [Tooltip("Which key should you press to skip to the next menu instantly")]
    [SerializeField] KeyCode keyToSkip;

    [Header("Logo Pieces Controls")]
    public RectTransform logoContainer;
    [Tooltip("How long the pieces bounce before they start snapping")]
    public float initialDelay = 1f;
    [Tooltip("Initial speed of the pieces")]
    public float startSpeed = 100f;
    [Tooltip("Delay between individual pieces landing")]
    public float timeBetweenSnaps = .5f;
    [Tooltip("How long each piece wiggles before snapping")]
    public float wiggleTime = .3f;
    [Tooltip("Amount of wiggling (units are degrees)")]
    public float wiggleAmount = 5;

    [Tooltip("How long it takes a piece to animate into place, after wiggling")]
    public float snapTime = 1f;

    [Tooltip("How long the whole image will hold before moving to the main menu")]
    public float holdDelay = 1f;

    [Header("Main Logo Text Controls")]
    [Tooltip("How long the main text takes to fade in")]
    public float TextFadeTime = .5f;
    [SerializeField]
    TextMeshProUGUI textBox;
    [SerializeField]
    [Tooltip("The color of all the text boxes")]
    Color textColor;

    [Header("Secondarry Content Noise")]
    [Tooltip("Audio clip that plays after team logo animates in")]
    public AudioClip noise;

    [Header("Secondary Logo Controls")]
    [Tooltip("How long to wait after the main logo is finished, before we show your team logo")]
    public float waitBeforeShowing;
    public Image secondaryLogoImage;
    [Tooltip("What percent of the space the GCS logo filled to take")]
    [Range(0, 100)]
    public float percentOfSpace = 40f;
    [Tooltip("How long it takes to animate your logo in.")]
    public float secondaryLogoTimeToShow = 1f;

    [Header("Secondary Logo Text Controls")]
    public RectTransform teamLogoContainer;
    [Tooltip("How long it takes your team name text to fade in")]
    public float secondaryTextFadeTime = .5f;
    [SerializeField]
    TextMeshProUGUI secondaryTextBox;

    List<LetterAnimator> letters;

    // Start is called before the first frame update
    void Start()
    {
        //Auto-Identify the letters objects - their names are formatted as "#: name" to make this sort work.
        letters = GameObject.FindObjectsOfType<LetterAnimator>().OrderBy(x => x.transform.parent.name).ToList();
        StartCoroutine(RunAnim());
    }

    // Update is called once per frame
    void Update()
    {
        //Skip
        if (Input.GetKeyDown(keyToSkip))
        {
            MoveToNext();
        }
    }

    IEnumerator RunAnim()
    {
        //Hide all color-changing objects
        textColor.a = 0;
        textBox.color = textColor;
        secondaryTextBox.color = textColor;
        secondaryLogoImage.color = textColor;

        //Shrink the team-specific logo, so that it can be expanded later
        teamLogoContainer.anchorMin = new Vector2(1, teamLogoContainer.anchorMin.y);

        //Grab each letter and throw them randomly.
        //This could probably be improved by starting them in a specific formation, and throwing them away from the center of the screen
        foreach (LetterAnimator letter in letters)
        {
            Vector2 newVelocity = Random.Range(-startSpeed, startSpeed) * Vector2.up + Random.Range(-startSpeed, startSpeed) * Vector2.right;
            letter.GetComponent<Rigidbody2D>().velocity = newVelocity;
        }

        //Give them time to bounce around
        yield return new WaitForSeconds(initialDelay);

        //Start re-attaching the letters to their default positions
        foreach (LetterAnimator letter in letters)
        {
            StartCoroutine(WiggleAndSnap(letter));
            yield return new WaitForSeconds(timeBetweenSnaps);
        }

        //Wait until exactly after the last one has snapped.
        yield return new WaitForSeconds(wiggleTime + snapTime - timeBetweenSnaps);

        //Fade in the 'game creation society' textbox
        for (float t = 0; t < TextFadeTime; t += Time.deltaTime)
        {
            textColor.a = t / TextFadeTime;
            textBox.color = textColor;
            yield return null;
        }

        textColor.a = 1;
        textBox.color = textColor;


        //If we're going to show something else, give the main logo a second to be clean.
        if (secondaryLogo != null || secondaryName.Length > 0)
        {
            yield return new WaitForSeconds(waitBeforeShowing);
        }

        //Run the team-specific logo animation
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

        //Play the last noise either after the image, or before the next round of text.
        if (secondaryLogo != null || secondaryName.Length > 0)
        {
            GetComponent<AudioSource>().PlayOneShot(noise);
        }

        //If we have text, run the secondary text animation
        if (secondaryName.Length > 0)
        {
            secondaryTextBox.text = secondaryName.ToLower();
            textColor.a = 0;
            for (float t = 0; t < secondaryTextFadeTime; t += Time.deltaTime)
            {
                textColor.a = t / secondaryTextFadeTime;
                secondaryTextBox.color = textColor;
                yield return null;
            }

            textColor.a = 1;
            secondaryTextBox.color = textColor;
        }

        //After it's all finished, hold for a second to show off the complete logo
        yield return new WaitForSeconds(holdDelay);

        //Head to next scene
        MoveToNext();
    }

    //Coroutine that makes each letter wiggle, then snap to it's correct position
    IEnumerator WiggleAndSnap(LetterAnimator letter)
    {
        RectTransform rect = letter.transform as RectTransform;
        Rigidbody2D rigid = letter.rigid;
        Vector2 velocity = rigid.velocity;
        float rot = rect.rotation.z;

        //Stop spinning, and stop interacting with other objects
        rigid.angularVelocity = 0;
        letter.DisableCollider();

        //Wiggle for a small bit
        for (float t = 0; t < wiggleTime; t+= Time.deltaTime)
        {
            rigid.velocity = Vector2.Lerp(velocity, Vector2.zero, t / wiggleTime);
            //t * 40 is a magic value (sorry) - it just needs to be large enough that sin oscillates visibly.
            rigid.angularVelocity = Mathf.Sin(t * 40) * wiggleAmount;
            yield return null;
        }

        Vector2 pos = rect.anchoredPosition;
        Quaternion rotation = rect.rotation;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;

        //Play the noise early - something weird is happening here that I don't understand, but this makes it work nicely
        letter.PlayNoise();

        //Lerp the letter back to it's original spot
        //Both of these lerps use a trick where we pass in the value itself as the first argument to the lerp -
        //This makes them animate super fast, then glide in slower as they approach 0. Gives it that punchy feel
        //where it slaps in nicely.
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

    //This used to do more things, but now it just makes sure that background matches if you want that.
    //If you later need more editor-only code, this is a great spot to put it. It will get called everytime
    //you edit the object, and won't compile into the build.
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
