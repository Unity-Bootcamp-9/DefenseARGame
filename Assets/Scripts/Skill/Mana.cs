using System;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    private const int MaxMana = 10;
    private const int MinMana = 0;
    public int CurrentMana { get; private set; } = MaxMana;
    private const float regenRate = 0.5f;
    private float _elapsedTime;

    public event Action ManaChanged;

    public void FindListener()
    {
        Skill[] skills = FindObjectsOfType<Skill>();

        foreach (Skill skill in skills)
        {
            skill.Mana = this;
            ManaChanged += skill.ChangeColor;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();

            int index = i;

            ManaChanged += () =>
            {
                image.color = CurrentMana > index ? Color.cyan : Color.gray;
            };
        }
        
        ManaChanged?.Invoke();
    }

    /// <summary>
    /// 보유 마나를 더하거나 뺌
    /// </summary>
    /// <param name="value">더하거나 뺄 값</param>
    public void UpdateMana(int value)
    {
        CurrentMana += value;

        if (CurrentMana >= MaxMana) CurrentMana = MaxMana;
        else if (CurrentMana <= MinMana) CurrentMana = MinMana;

        ManaChanged?.Invoke();
    }

    private void Update()
    {
        if (CurrentMana >= MaxMana)
        {
            _elapsedTime = Time.time;
        }

        if (Time.time - _elapsedTime >= regenRate)
        {
            UpdateMana(1);
            _elapsedTime = Time.time;
        }
    }
}
