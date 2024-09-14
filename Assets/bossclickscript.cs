using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class bossclickscript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI text;
    public TextMeshProUGUI instructions;
    public GameObject textPrefab;
    public GameObject cow;
    public Sprite sprite;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Canvas canvas;
    public TextMeshProUGUI streakText;
    private Animator animator;


    private const int MAX_HP_1 = 10;//1000
    private const int MAX_HP_2 = 20;//1500
    private const int MAX_HP_3 = 12;//3000
    private const int MAX_HP_4 = 1400;//6000
    private const int MAX_HP_5 = 30;//15000

    private const string INSTRUCTION_1 = "It is a lonely bison, he is just standing there. \n\n <color=red> TAME HIM!</color> ";
    private const string INSTRUCTION_2 = "He is tougher than we thought! \n \n Bison gains: \n\n <color=red> extra HP </color>";
    private const string INSTRUCTION_3 = "HE BECAME AROUSED! \n\n Bison gains: \n\n <color=red>slow HP regen \n even more HP</color>";
    private const string INSTRUCTION_4 = "ALMOST THERE! KEEP 'PETTING' HIM!!! \n\n Bison gains: \n\n <color=red> slow HP regen \n even more HP \n rapid HP regen when streak breaks</color>";
    private const string INSTRUCTION_5 = "HE IS GONNA BUST!  \n\n Bison gains:\n\n <color=red> <color=yellow> fast</color> HP regen \n <color=yellow> extreme </color>  HP \n <color=yellow> full</color> HP regen when streak breaks</color>";

    private float timer = 0;
    private float interval = 0.3f;
    private int streak = 0;

    private bool coroutineGoing = false;
    private SpriteRenderer cowRenderer;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(MAX_HP_1);
        SetHealth(MAX_HP_1);
        text.text = "100%";
        cowRenderer = cow.GetComponent<SpriteRenderer>();
        animator = cow.GetComponent<Animator>();
        instructions.text = INSTRUCTION_1;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && !coroutineGoing)
        {
            //for (int i = 0; i < Input.touchCount; i++)
            //{
            //Touch touch = Input.GetTouch(i);
            //if (touch.phase == TouchPhase.Began)
            //{

            TakeDamage(1);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPoint);
            ShowMilkedMoney("BAM!", localPoint);
            animator.Play("BAMAnimation", 0, 0f);
            timer = 0;
            ModifyStreak(streak + 1);
            if (slider.value <= 0)
            {
                LevelUpCow();
            }
            //}
            //}
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
        ColorUtility.TryParseHtmlString(colour, out Color parsedColor);
        newText.color = parsedColor;
        newText.text = $"{huh}{points}";
        newText.CrossFadeAlpha(0.0f, 0.8f, true);
        newText.raycastTarget = false;
        Destroy(newObject, 0.8f);
    }

    private int slowlyHealAmount = 0;
    private bool streakHealing = false;
    private int streakHealingAmountDivider = 10;
    public void LevelUpCow()
    {
        switch (slider.maxValue)
        {
            case MAX_HP_1:
                StartCoroutine(FillSlider(MAX_HP_2));
                cowRenderer.sprite = sprite;
                instructions.text = INSTRUCTION_2;
                break;
            case MAX_HP_2:
                StartCoroutine(FillSlider(MAX_HP_3));
                instructions.text = INSTRUCTION_3;
                cowRenderer.sprite = sprite2;
                slowlyHealAmount = 1; //TODO:  dat tady 3
                StartCoroutine(SlowlyHeal());
                break;
            case MAX_HP_3:
                StartCoroutine(FillSlider(MAX_HP_4));
                instructions.text = INSTRUCTION_4;
                cowRenderer.sprite = sprite3;
                streakHealing = true;
                streakHealingAmountDivider = 10;
                break;
            case MAX_HP_4:
                StartCoroutine(FillSlider(MAX_HP_5));
                instructions.text = INSTRUCTION_5;
                cowRenderer.sprite = sprite4;
                slowlyHealAmount = 3; //TODO:  dat tady 5?
                streakHealingAmountDivider = 1;
                break;
            case MAX_HP_5:
                break;
        }
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

    public string ColorToHexString(Color color)
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

    private IEnumerator FillSlider(int maxValue)
    {
        coroutineGoing = true;
        float fillDuration = 3f;
        float elapsedTime = 0f;
        SetMaxHealth(maxValue);

        slider.value = 1;
        float startValue = slider.value;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            SetHealth(Mathf.Lerp(startValue, maxValue, elapsedTime / fillDuration));
            SetTextValue();
            yield return null;
        }
        slider.value = maxValue;
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
