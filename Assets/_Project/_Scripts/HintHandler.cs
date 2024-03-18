using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintHandler : MonoBehaviour
{
    [Header("_______________________Настройки всплывающих подсказок_______________________")]
    [Space(5)]

    [TextArea(3, 5)]
    [SerializeField] private string[] _textForInfoPanel;
    [Space(10)]

    [Tooltip("Сколько секунд будет видна подсказка")]
    [SerializeField] private float _infoPanelLiveTime = 1;
    [Tooltip("Скорость проявления подсказки")]
    [SerializeField] private float _appearTime = 1;
    [Tooltip("Скорость затухания подсказки")]
    [SerializeField] private float _dissapearTime = 1;

    [Tooltip("Переодичность появления подсказок")]
    [SerializeField] private float _timeBeetwenTips = 10;

    // Переменная отвечающая за индексацию подсказок.
    private int _textIndexer = 0;

    [Space(10)]
    [Header("Все подсказки для режима Особеностей показались")]
    [SerializeField] private bool _tipsForViewIsShown = false;
    [Header("Все подсказки для режима Рабочих параметров показались")]
    [SerializeField] private bool _tipsForDigitalTwinIsShown = false;

    [Space(10)]
    [Header("Компоненты")]
    public Image ImageComponent;
    public TextMeshProUGUI HintText;

    [Space(10)]
    [Header("Цвета текста и панельки")]
    public Color DefaultColor;
    public Color Tranparent;


    [Space(10)]
    [Header("_______________________Debug/Тест_______________________")]
    [Space(5)]
    [SerializeField] private float _tipTimer = 0;
    [SerializeField] private int _tipCounter = 0;

    [HideInInspector] public bool FirstTipGone = false;
    [HideInInspector] public bool TimerisActive = false;





    private void Start()
    {
        AppearInfoPanel(_textForInfoPanel[0], _appearTime);
    }

    private void Update()
    {
        if (AppManager.Instanse.CanCreate)
        {
            switch (AppManager.Instanse.CurrentAppState)
            {
                case (AppState.ViewDescription):
                    if (!_tipsForViewIsShown && TimerisActive)
                    {
                        if (_tipTimer < _timeBeetwenTips)
                        {
                            _tipTimer += Time.deltaTime;
                        }
                        else
                        {
                            if (_textIndexer > 2)
                            {
                                _tipsForViewIsShown = true;
                                return;
                            }
                            AppearInfoPanel(_textForInfoPanel[_textIndexer], _appearTime);
                            _textIndexer++;
                            TimerisActive = false;
                        }
                    }
                    break;

                case (AppState.WorkOptions):
                    if (!_tipsForDigitalTwinIsShown && TimerisActive)
                    {
                        if (_tipTimer < _timeBeetwenTips)
                        {
                            _tipTimer += Time.deltaTime;
                        }
                        else
                        {
                            if (_textIndexer > 4)
                            {
                                _tipsForDigitalTwinIsShown = true;
                                return;
                            }
                            AppearInfoPanel(_textForInfoPanel[_textIndexer], _appearTime);
                            _textIndexer++;
                            TimerisActive = false;
                        }
                    }
                    break;
            }
        }
    }

    public void BeginHintCycle()
    {
        StopAllCoroutines();
        ImageComponent.color = DefaultColor;
        HintText.color = new Color(1, 1, 1, 1);
        HideInfoPanel();
        StartCoroutine(AppearInfoPanelOnTime(_dissapearTime, _textForInfoPanel[1]));
    }

    private void AppearInfoPanel(string newText, float appearTime)
    {
        HintText.text = newText;
        StopAllCoroutines();
        ImageComponent.color = Tranparent;
        HintText.color = new Color(1, 1, 1, 0);
        StartCoroutine(AppearImage(appearTime));

    }

    public void HideInfoPanel()
    {
        StartCoroutine(DissappearImage(_dissapearTime));
    }

    public IEnumerator AppearImage(float appearTime)
    {
        float timeElapsed = 0;
        while (timeElapsed < appearTime)
        {
            float percentage = timeElapsed / appearTime;
            ImageComponent.color = Color.Lerp(Tranparent, DefaultColor, percentage);
            HintText.color = new Color(1, 1, 1, percentage);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        ImageComponent.color = DefaultColor;
        HintText.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(_infoPanelLiveTime);
        if (_tipCounter != 0)
        {
            StartCoroutine(DissappearImage(_dissapearTime));
        }

    }

    public IEnumerator DissappearImage(float dissapearTime)
    {

        float timeElapsed = dissapearTime;
        while (timeElapsed >= 0)
        {
            float percentage = timeElapsed / dissapearTime;
            float realPercentage = 1 - percentage;
            ImageComponent.color = Color.Lerp(DefaultColor, Tranparent, realPercentage);
            HintText.color = new Color(1, 1, 1, percentage);
            timeElapsed -= Time.deltaTime;
            yield return null;
        }
        ImageComponent.color = Tranparent;
        HintText.color = new Color(1, 1, 1, 0);
        if (_tipCounter < 2)
        {
            _tipCounter++;
        }

        if (_tipCounter >= 2 && !FirstTipGone)
        {
            FirstTipGone = true;
        }

        _tipTimer = 0;
        TimerisActive = true;
        StopCoroutine(AppearImage(_appearTime));
    }

    public IEnumerator HideInfoPanelOnTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        HideInfoPanel();
    }

    public IEnumerator AppearInfoPanelOnTime(float timeToWait, string text)
    {
        yield return new WaitForSeconds(timeToWait);
        AppearInfoPanel(text, _appearTime);
    }

}
