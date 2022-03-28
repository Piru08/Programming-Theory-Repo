using UnityEngine;

public class DamageAmplifier : MonoBehaviour
{
   public enum AmlifierType
    {
        PLUS_CLICK_DAMAGE,
        CLICK_CRIT,
        PASSIVE_DAMAGE
    }

    public AmlifierType Type { get; private set; }

    public int Priority { get; private set; }
    public bool IsPassive { get; private set; }
    public float Value => InitValue + IncreasePerLevel * Mathf.Clamp(Level- 1, 0, int.MaxValue);
    public float InitValue { get; private set; }
    public float IncreasePerLevel { get; private set; }
    public int Level { get; private set; }
    public int Price => InitPrice + IncreasePricePerLevel * Level;
    public int InitPrice { get; private set; }
    public int IncreasePricePerLevel { get; private set; }
    public int Chance { get; private set; }

    public DamageAmplifier (AmlifierType type, int priority, bool isPassive,
        float value, float increase, int initPrice, int increasePrice, int chanse = 100)
    {
        Type = type;
        Priority = priority;
        IsPassive = isPassive;
        InitValue = value;
        IncreasePerLevel = increase;
        InitPrice = initPrice;
        IncreasePricePerLevel = increasePrice;
        Chance = chanse;

        Level = PlayerPrefs.GetInt("DA_" + Type.ToString(), 0);
    }

    public float CalculateDamage(float initDamage)
    {
        if (Level == 0)
            return initDamage;

        switch (Type)
        {
            case AmlifierType.PLUS_CLICK_DAMAGE:
            case AmlifierType.PASSIVE_DAMAGE:

                return initDamage + Value;

            case AmlifierType.CLICK_CRIT:

                if (Random.Range(0, 100) < Chance)
                    return initDamage * Value;
                else
                    return initDamage;
            default:
                return initDamage;
        }
    }

    public void LevelUp()
    {
        Level++;
        PlayerPrefs.SetInt("DA_" + Type.ToString(), Level);
    }
}
