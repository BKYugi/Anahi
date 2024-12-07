using UnityEngine;
namespace FT
{
    public class PlayerStatsManager : CharacterStatsManager
    {

        private int _vitality;
        public int Vitality
        {
            get { return _vitality; } set
            {
                if (_vitality != value)
                {
                    int oldVitality = _vitality;
                    _vitality = value;
                    SetNewMaxHealthValue(oldVitality, _vitality);
                }
            }
        }

        private int _endurance;
        public int Endurance
        {
            get { return _endurance; }
            set
            {
                if (_endurance != value)
                {
                    int oldEndurance = _endurance;
                    _endurance = value;
                    SetNewMaxStaminaValue(oldEndurance, _endurance);
                }
            }
        }
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();

            // WHEN MAKE THE CARACTER AND SET THE STATS, DEPENDING THIS WILL BE CALCULATED 
            // TIL THEN, STATS ARE NEVER CALCULATED
            Vitality = vitality;
            Endurance = endurance;
        }

        

    }
}