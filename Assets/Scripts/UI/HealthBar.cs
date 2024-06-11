using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthLabel;
    public Slider healthSlider; // Slider voor de health
    public Text levelText; // Tekst voor level weergave
    public Text xpText; // Tekst voor XP weergave

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        healthBar = root.Q<VisualElement>("HealthBar");
        healthLabel = root.Q<Label>("HealthText");
    }

    
    void Update()
    {

    }
    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        float percent = (float)currentHitPoints / maxHitPoints * 100;
        healthBar.style.width = new Length(percent, LengthUnit.Percent);
        healthLabel.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        healthSlider.value = (float)currentHealth / maxHealth;
    }

    public void SetLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }

    public void SetXP(int xp, int xpToNextLevel)
    {
        if (xpText != null)
        {
            xpText.text = "XP: " + xp + " / " + xpToNextLevel;
        }
    }
}