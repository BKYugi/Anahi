using UnityEngine;
using UnityEngine.Rendering;
namespace FT
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public CharacterManager character;
        public PlayerManager player;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [SerializeField] PlayerUIHudManager playerUIHudManager;

        [Header("Stamina")]
        public int endurance = 10;
        public int maxStamina = 0;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 2;
        [SerializeField] float staminaRegenerationAmount = 2;
        public float _currentStamina = 0;

        [Header("Health")]
        public int vitality = 10;
        public float maxHealth = 0;
        public float _currentHealth = 0;

        [Header("Stats")]
        public bool isDead;
        //[SerializeField]
        public float CurrentStamina
        {
            get { return _currentStamina; }
            set {
                _currentStamina = Mathf.Clamp(value, 0, maxStamina);
                ResetStaminaRegenTimer(_currentStamina, value);
                PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(_currentStamina, value);
                //CurrentStamina = value;
            }
        }
        public float CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                if (isDead) return; // Interrompe qualquer atualização de vida se estiver morto.

                int oldValue = Mathf.RoundToInt(_currentHealth); // Valor antigo
                _currentHealth = Mathf.Clamp(value, 0, maxHealth); // Atualiza o valor
                int newValue = Mathf.RoundToInt(_currentHealth); // Novo valor

                CheckHP(oldValue, newValue); // Verifica se houve morte
                PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(oldValue, _currentHealth); // Atualiza a UI imediatamente


                //if(isDead)
                //{
                //    return;
                //}
                ////_currentHealth = Mathf.Clamp(value, 0, maxHealth);
                ////PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(_currentHealth, value);
                ////CurrentHealth = value;
                //int oldValue = Mathf.RoundToInt(_currentHealth); // valor antigo
                //_currentHealth = Mathf.Clamp(value, 0, maxHealth); // atualiza o valor
                //
                //int newValue = Mathf.RoundToInt(_currentHealth); // novo valor
                //CheckHP(oldValue, newValue); // chama CheckHP para verificar se deve processar morte
                //
                //// Atualiza a UI
                //PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(oldValue, _currentHealth);
            }
        }

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            player = GetComponent<PlayerManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            playerUIHudManager = FindObjectOfType<PlayerUIHudManager>();

        }
        protected virtual void Start()
        {
            maxHealth = CalculateHealthBasedOnVitalityLevel(vitality);
            maxStamina = CalculateStaminaBasedOnEnduranceLevel(endurance);

            CurrentHealth = maxHealth;
            CurrentStamina = maxStamina;

        }

        public void CheckHP(int oldValue, int newValue)
        {
            if(CurrentHealth <= 0)
            {
                isDead= true; 
                Debug.Log("Murio");
                StartCoroutine(character.ProcessDeathEvent());
            }

            if(CurrentHealth > maxHealth)
            {
                CurrentHealth = maxHealth;
            }
        }

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            //float maxHealth = 0;

            maxHealth = vitality * 10;

            return Mathf.RoundToInt(maxHealth);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            //float maxStamina = 0;

            maxStamina = endurance * 10;

            return Mathf.RoundToInt(maxStamina);
        }

        public virtual void RegenerateStamina()
        {
            if (character.isSprinting || character.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if(staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if(CurrentStamina < maxStamina)
                {
                    staminaTickTimer += Time.deltaTime;
                    if (staminaTickTimer >= 0.1f)
                    {
                        staminaTickTimer = 0;
                        CurrentStamina += staminaRegenerationAmount;
                    }
                }

                
            }
        }

        public virtual void ResetStaminaRegenTimer(float previusStaminaAmount, float currentStaminaAmount)
        {
            if(currentStaminaAmount < previusStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }

        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth = CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth);
            CurrentHealth = maxHealth;
        }
        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina = CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina);
            CurrentStamina = maxStamina;
        }

        [System.Obsolete]
        private void OnValidate()
        {
            UpdateMaxStats();  // Atualiza os valores máximos
            UpdateUI();        // Atualiza a UI
        }
        private void UpdateMaxStats()
        {
            maxHealth = vitality * 10;  // Exemplo: maxHealth é 10 vezes vitality
            maxStamina = endurance * 10; // Exemplo: maxStamina é 10 vezes endurance
        }

        [System.Obsolete]
        private void UpdateUI()
        {
            if (playerUIHudManager == null)
            {
                playerUIHudManager = FindObjectOfType<PlayerUIHudManager>();
                //Debug.LogWarning("PlayerUIHudManager is not assigned or found in CharacterStatsManager.");
                if (playerUIHudManager == null)
                {
                    Debug.LogWarning("PlayerUIHudManager is not assigned or found in CharacterStatsManager.");
                    return;
                }
                return;
            }
            if (!Application.isPlaying)
            {
                Debug.LogWarning("UpdateUI called in Editor mode. Skipping execution.");
                return;
            }

            if (playerUIHudManager == null)
            {
                Debug.LogError("PlayerUIHudManager not assigned in CharacterStatsManager.");
                return;
            }
            // Atualize a UI com os novos valores de maxHealth e maxStamina
            //FindObjectOfType<PlayerUIHudManager>().UpdateHealthAndStaminaBars(maxHealth, maxStamina);
            playerUIHudManager.UpdateHealthAndStaminaBars(maxHealth, maxStamina);
            PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(_currentHealth, _currentHealth);
            //PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth);
            //playerUIHudManager.UpdateHealthAndStaminaBars(maxHealth, maxStamina);
        }
    }
}
