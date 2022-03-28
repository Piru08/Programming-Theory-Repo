using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Clicker : MonoBehaviour
{
    public static Clicker Instance;

    public float Money
    {
        get => PlayerPrefs.GetFloat("Money", 0);
        private set => PlayerPrefs.SetFloat("Money", value);
    }

    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private List<AmplifirePref> amplifirePrefs;
    private List<DamageAmplifier> amplifiers;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        amplifiers = new List<DamageAmplifier>()
        {
            new DamageAmplifier(DamageAmplifier.AmlifierType.PLUS_CLICK_DAMAGE, 0, false, 2, 1.5f, 100, 75),
            new DamageAmplifier(DamageAmplifier.AmlifierType.CLICK_CRIT, 100, false, 2, 2f, 200, 100, 25),
            new DamageAmplifier(DamageAmplifier.AmlifierType.PASSIVE_DAMAGE, 0, true, 2, 1.25f, 125, 50),
        };

        for (int i = 0; i < amplifirePrefs.Count; i++)
            amplifirePrefs[i].SetData(amplifiers[i]);

        CalculateOfflineIncume();

        StartCoroutine(PassiveDamageDealer());
        UpdateUI();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastPlayedTime", DateTime.UtcNow.ToString());
    }
    private void CalculateOfflineIncume()
    {
        string lastPlayedTimeString = PlayerPrefs.GetString("LastPlayedTime", null);

        if (lastPlayedTimeString == null)
            return;

        var lastPlayedTime = DateTime.Parse(lastPlayedTimeString);
        int timeSpanRestriction = 24 * 60 * 60;
        double secondsSpan = (DateTime.UtcNow - lastPlayedTime).TotalSeconds;

        if (secondsSpan > timeSpanRestriction)
            secondsSpan = timeSpanRestriction;

        float totalDamage = (float)secondsSpan * GetPassiveDamage();
        DamageTarget(totalDamage);
        Debug.Log($"Time span: {secondsSpan} Income: {totalDamage}");
    }
    private IEnumerator PassiveDamageDealer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            float damage = GetPassiveDamage();
            if (damage == 0)
                continue;
            DamageTarget(damage);
            EffectController.Instance.CreatePassiveEffect((int)damage);

        }
    }
    public void Click()
    {
        float damage = GetDamage();
        DamageTarget(damage);
        EffectController.Instance.CreateClickEffect((int)damage);
    }

    private void DamageTarget(float damage)
    {
        AddMoney(damage);
    }

    private float GetDamage()
    {
        float damage = 1;

        var sortedAmplifiers = amplifiers.FindAll(x => !x.IsPassive);
        sortedAmplifiers.Sort((x, y) => x.Priority.CompareTo(y.Priority));

        foreach (var amplifier in sortedAmplifiers)
            damage = amplifier.CalculateDamage(damage);

        return damage;
    }

    private float GetPassiveDamage()
    {
        float damage = 0;

        var sortedAmplifiers = amplifiers.FindAll(x => x.IsPassive);
        sortedAmplifiers.Sort((x, y) => x.Priority.CompareTo(y.Priority));

        foreach (var amplifier in sortedAmplifiers)
            damage = amplifier.CalculateDamage(damage);

        return damage;
    }
    public void UpdateUI()
    {
        money.text = "$" + (int)Money;
    }

    public void AddMoney(float value)
    {
        Money += value;
        UpdateUI();
        foreach (var pref in amplifirePrefs)
            pref.UpdateUI();
    }
}
