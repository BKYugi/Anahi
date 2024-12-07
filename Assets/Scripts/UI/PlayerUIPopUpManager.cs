using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace FT
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {

        [Header("YOU DIED Pop Up")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup; // ALLOWS US TO SET THE ALPHA TO FADE IN FADE OUT

        public void SendYouDiedPopUp()
        {
            // ACTIVATE POST PROCESSING EFFECTS
            
            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;
            // STRETCH OUT THE POP UP 
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 20f));
            // FADE IN
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
            // WAIT, THEN FADE OUT THE POP UP
            StartCoroutine(WaitTheFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if(duration > 0f)
            {
                text.characterSpacing = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null; 
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if(duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 1;

            yield return null;
        }

        private IEnumerator WaitTheFadeOutPopUpOverTime(CanvasGroup canvas,float duration, float delay)
        {
            if (duration > 0)
            {
                while(delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 0;

            yield return null;
        }


    }
}
