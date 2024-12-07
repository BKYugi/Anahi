using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace FT
{
    public class UI_StatBar : MonoBehaviour
    {

        [SerializeField]private Slider slider;
        private RectTransform rectTransform;
        // VARIABLE TO SCALE BAR SIZE DEPENDING ON STAT ( MORE ENDURANCE MORE STAMINA )
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;
        // SECONDARY BAR BEHIND MAY BAR FOR POLISH EFFECT ( YELLOW BAR THAT SHOWS HOW MUCH AN ACTION TAKES AWAY FROM CURRENT STAMINA)

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>(); // Inicializa rectTransform

        }

        public virtual void SetStat(float newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWithStats)
            {
                // SCALE THE TRANSFORM OF THIS OBJECT
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
            }
        }
    }
}
