
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Color = UnityEngine.Color;

public class bossclickscript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Image obscure1;
    public Image obscure2;
    public TextMeshProUGUI text;
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI thankstext;
    public GameObject textPrefab;
    public GameObject cow;
    public Sprite sprite;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Canvas canvas;
    public TextMeshProUGUI streakText;
    private Animator animator;
    public AudioSource audioSource;
    public AudioSource mooaudiosource;
    public AudioSource music;
    public AudioSource gottem;
    public AudioSource trumpet;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public AudioClip clip4;
    public AudioClip clip5;
    public AudioClip moo;


    private const int MAX_HP_1 = 2;//1000
    private const int MAX_HP_2 = 30;//1500
    private const int MAX_HP_3 = 50;
    private const int MAX_HP_4 = 80;
    private const int MAX_HP_5 = 150;

    private const string INSTRUCTION_1 = "It is a lonely bison, he is just standing there. \n\n <color=red> TAME HIM!</color> ";
    private const string INSTRUCTION_2 = "He is tougher than we thought! \n \n Bison gains: \n\n <color=red> extra HP </color>";
    private const string INSTRUCTION_3 = "HE BECAME AROUSED! \n\n Bison gains: \n\n <color=red>slow HP regen \n even more HP</color>";
    private const string INSTRUCTION_4 = "ALMOST THERE! KEEP 'PETTING' HIM!!! \n\n Bison gains: \n\n <color=red> slow HP regen \n even more HP \n rapid HP regen when streak breaks</color>";
    private const string INSTRUCTION_5 = "HE IS GONNA BUST!  \n\n Bison gains:\n\n <color=red> <color=yellow> fast</color> HP regen \n <color=yellow> extreme </color>  HP \n <color=yellow> full</color> HP regen when streak breaks</color>";

    private float timer = 0;
    private float interval = 0.3f;
    private int streak = 0;
    private Image image;

    private bool coroutineGoing = false;
    private SpriteRenderer cowRenderer;

    List<AudioClip> soundArray = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        thankstext.enabled = false;
        SetMaxHealth(MAX_HP_1);
        SetHealth(MAX_HP_1);
        text.text = "100%";
        cowRenderer = cow.GetComponent<SpriteRenderer>();
        animator = cow.GetComponent<Animator>();
        instructions.text = INSTRUCTION_1;
        image = canvas.GetComponent<Image>();
        image.color = GiveColorFromHex("#1E633A");
        soundArray.Add(clip1);
        soundArray.Add(clip2);
        soundArray.Add(clip3);
        soundArray.Add(clip4);
        soundArray.Add(clip5);
        StartCoroutine(DOPICE());
    }

    private IEnumerator DOPICE()
    {
        float elapsedTime = 0;
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            float alphaValue = Mathf.Clamp01(1 - (elapsedTime / 3f));
            Color tempColor = obscure1.color;
            tempColor.a = alphaValue;
            obscure1.color = tempColor;

            yield return null;
        }
        Color finalColor2 = obscure1.color;
        finalColor2.a = 0f;
        obscure1.color = finalColor2;
    }

    private UnityEngine.Color GiveColorFromHex(string colour)
    {
        ColorUtility.TryParseHtmlString(colour, out UnityEngine.Color parsedColor);
        return parsedColor;
    }

    void Update()
    {
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    if (!finish)
                    {
                        TakeDamage(1);
                    }
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPoint);
                    ShowMilkedMoney("BAM!", localPoint);
                    audioSource.PlayOneShot(soundArray[Random.Range(0, soundArray.Count)]);
                    mooaudiosource.PlayOneShot(moo);
                    animator.Play("BAMAnimation", 0, 0f);
                    timer = 0;
                    ModifyStreak(streak + 1);
                    if (slider.value <= 0)
                    {
                        LevelUpCow();
                    }
                }
            }
        }
        if (StreakBrokenForLong() && streakHealing)
        {
            if (slider.value != slider.maxValue && !coroutineGoing)
            {
                ShowHeal((int)(slider.maxValue / streakHealingAmountDivider));
            }
        }
    }

    private void ModifyStreak(int tmpStreak)
    {
        streak = tmpStreak;
        streakText.text = streak.ToString();
    }


    private bool StreakBrokenForLong()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;
            ModifyStreak(0);

            return true;
        }
        return false;
    }

    private void ShowMilkedMoney(string points, Vector2 spriteCanvasPosition,
    string colour = "#FFFFFF", string huh = "")
    {
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), spriteCanvasPosition, Camera.main, out Vector2 localPoint);

        GameObject newObject = Instantiate(textPrefab, canvas.transform);

        // Get the RectTransform of the new object and set its position
        RectTransform rt = newObject.GetComponent<RectTransform>();
        rt.anchoredPosition = spriteCanvasPosition;
        TextMeshProUGUI newText = newObject.GetComponentInChildren<TextMeshProUGUI>();

        newText.fontSize = 50;
        ColorUtility.TryParseHtmlString(colour, out UnityEngine.Color parsedColor);
        newText.color = parsedColor;
        newText.text = $"{huh}{points}";
        newText.CrossFadeAlpha(0.0f, 0.8f, true);
        newText.raycastTarget = false;
        Destroy(newObject, 0.8f);
    }

    private int slowlyHealAmount = 0;
    private bool streakHealing = false;
    private int streakHealingAmountDivider = 10;
    private bool finish = false;
    public void LevelUpCow()
    {
        switch (slider.maxValue)
        {
            case MAX_HP_1:
                StartCoroutine(FillSlider(MAX_HP_2, GiveColorFromHex("#1A4414")));
                cowRenderer.sprite = sprite;
                instructions.text = INSTRUCTION_2;
                break;
            case MAX_HP_2:
                StartCoroutine(FillSlider(MAX_HP_3, GiveColorFromHex("#634C05")));
                instructions.text = INSTRUCTION_3;
                cowRenderer.sprite = sprite2;
                slowlyHealAmount = 3;
                StartCoroutine(SlowlyHeal());
                break;
            case MAX_HP_3:
                StartCoroutine(FillSlider(MAX_HP_4, GiveColorFromHex("#793E05")));
                instructions.text = INSTRUCTION_4;
                cowRenderer.sprite = sprite3;
                streakHealing = true;
                streakHealingAmountDivider = 5;
                break;
            case MAX_HP_4:
                StartCoroutine(FillSlider(MAX_HP_5, GiveColorFromHex("#712723")));
                instructions.text = INSTRUCTION_5;
                cowRenderer.sprite = sprite4;
                slowlyHealAmount = 7;
                streakHealingAmountDivider = 1;
                break;
            case MAX_HP_5:
                slider.value = 1;
                finish = true;
                streakHealing = false;
                StopAllCoroutines();
                StartCoroutine(FadeInThanks());
                music.volume = music.volume / 3;
                trumpet.Play();
                gottem.Play();
                break;
        }
    }

    private IEnumerator FadeInThanks()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;

            float alphaValue = Mathf.Clamp01(elapsedTime / 3f);

            Color tempColor = obscure1.color;
            tempColor.a = alphaValue;
            obscure1.color = tempColor;
            yield return null;
        }

        Color finalColor = obscure1.color;
        finalColor.a = 1f;
        obscure1.color = finalColor;
        obscure2.color = finalColor;
        thankstext.enabled = true;
        elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;
            float alphaValue = Mathf.Clamp01(1 - (elapsedTime / 3f));
            Color tempColor = obscure2.color;
            tempColor.a = alphaValue;
            obscure2.color = tempColor;

            yield return null;
        }
        Color finalColor2 = obscure2.color;
        finalColor2.a = 0f;
        obscure2.color = finalColor2;
    }

    IEnumerator SmoothTransition(UnityEngine.Color color)
    {
        float elapsedTime = 0f;
        float fillDuration = 30f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            image.color = UnityEngine.Color.Lerp(image.color, color, elapsedTime / fillDuration);
            yield return null;
        }
        image.color = color;
    }

    IEnumerator SlowlyHeal()
    {
        while (true)
        {
            if (slider.maxValue != slider.value && !coroutineGoing)
            {
                ShowHeal(slowlyHealAmount);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void ShowHeal(int heal)
    {
        HealHealth(heal);
        ShowMilkedMoney(heal.ToString(), MoveItABit(Helpers.GetObjectPositionRelativeToCanvas(new Vector2())), ColorToHexString(gradient.Evaluate(slider.normalizedValue)), "+");
    }

    public string ColorToHexString(UnityEngine.Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255f);
        int g = Mathf.RoundToInt(color.g * 255f);
        int b = Mathf.RoundToInt(color.b * 255f);
        int a = Mathf.RoundToInt(color.a * 255f);

        return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
    }

    private Vector2 MoveItABit(Vector2 position)
    {
        UnityEngine.Random.seed = System.DateTime.Now.Millisecond;

        float randomY = UnityEngine.Random.Range(5f, 25f);
        float randomX = UnityEngine.Random.Range(10f, 100f);
        float makeNegativeOrNotY = UnityEngine.Random.Range(0f, 1f) < 0.5 ? -1 : 1;
        float makeNegativeOrNotX = UnityEngine.Random.Range(0f, 1f) < 0.5 ? -1 : 1;

        float moveY = randomY * makeNegativeOrNotY;
        float moveX = randomX * makeNegativeOrNotX;

        return position - new Vector2(+35f + (moveX), -140f + (moveY));
    }

    private IEnumerator FillSlider(int maxValue, Color color)
    {
        coroutineGoing = true;
        float fillDuration = 3f;
        float elapsedTime = 0f;
        SetMaxHealth(maxValue);

        slider.value = 1;
        float startValue = slider.value;
        Color original = image.color;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            SetHealth(Mathf.Lerp(startValue, maxValue, elapsedTime / fillDuration));
            SetTextValue();
            image.color = UnityEngine.Color.Lerp(original, color, elapsedTime / fillDuration);
            yield return null;
        }
        slider.value = maxValue;
        image.color = color;
        coroutineGoing = false;
    }

    private void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        fill.color = gradient.Evaluate(1F);
    }

    private void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        SetTextValue();

    }

    private void SetTextValue()
    {
        text.text = $"{(int)((slider.value / slider.maxValue) * 100)}%";
    }

    private void TakeDamage(int damage)
    {
        slider.value -= damage;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        SetTextValue();
    }

    private void HealHealth(int heal)
    {
        slider.value += heal;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        SetTextValue();
    }
}
