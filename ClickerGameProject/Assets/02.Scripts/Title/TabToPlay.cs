using System.Collections;
using UnityEngine;
using TMPro;

public class TabToPlay : MonoBehaviour
{
    private float timer;
    [SerializeField] private TextMeshProUGUI text;
    public static bool activated = false;

    private void Awake()
    {
        text.gameObject.SetActive(false);
    }

    private void Exit()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0) && activated == false)
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && activated == false)
# endif
        {
            text.gameObject.SetActive(false);
            activated = true;
        }
    }


    public IEnumerator Cor_BlinkText()
    {
        text.gameObject.SetActive(true);
        while (!activated)
        {
            Exit();

            timer += Time.deltaTime;
            if (timer >= 2.0f)
            {
                //시간 초기화
                text.color = new Color(1f, 1f, 1f, 0 / 1f);
                timer = 0f;
            }
            if(timer >= 1.0f)
            {
                //Fade Out
                text.color = new Color(1f, 1f, 1f, (2f-timer) / 1f);
            }
            else
            {
                //Fade In
                text.color = new Color(1f, 1f, 1f, timer / 1f);
            }
            yield return null;
        }
    }
}
