using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;

    [Header("UI Manager Reference")]
    public UIManager uiManager; 

    [Header("Field of View Settings")]
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int defense;
    [SerializeField] private int power;
    [SerializeField] private int level;
    [SerializeField] private int xp;
    [SerializeField] private int xpToNextLevel;

    public int MaxHitPoints
    {
        get => maxHitPoints;
        set => maxHitPoints = value;
    }

    public int HitPoints
    {
        get => hitPoints;
        set => hitPoints = value;
    }

    public int Defense
    {
        get => defense;
        set => defense = value;
    }

    public int Power
    {
        get => power;
        set => power = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int XP
    {
        get => xp;
        set => xp = value;
    }

    public int XPToNextLevel
    {
        get => xpToNextLevel;
        set => xpToNextLevel = value;
    }

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();
        if (IsPlayer())
        {
            UpdateUI();
        }
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);
        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (IsPlayer())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }

    private void Die()
    {
        if (IsPlayer())
        {
            uiManager?.AddMessage("You died!", Color.red);
        }
        else
        {
            uiManager?.AddMessage($"{name} is dead!", Color.green);
            GameManager.Get.RemoveEnemy(this);
        }

        GameObject remains = GameManager.Get.CreateActor("Dead", transform.position);
        remains.name = $"Remains of {name}";

        Destroy(gameObject);
    }

    public void DoDamage(int damage, Actor attacker)
    {
        hitPoints = Mathf.Max(hitPoints - damage, 0);
        if (IsPlayer())
        {
            uiManager?.UpdateHealth(hitPoints, maxHitPoints);
        }

        if (hitPoints == 0)
        {
            Die();
            attacker?.GainXP(XP);
        }
    }

    public void Heal(int amount)
    {
        int healedHP = Mathf.Min(maxHitPoints - hitPoints, amount);
        hitPoints += healedHP;
        if (IsPlayer())
        {
            uiManager?.UpdateHealth(hitPoints, maxHitPoints);
            uiManager?.AddMessage($"You were healed for {healedHP} HP!", Color.green);
        }
    }

    public void GainXP(int amount)
    {
        xp += amount;
        while (xp >= xpToNextLevel)
        {
            xp -= xpToNextLevel;
            LevelUp();
        }
        uiManager?.UpdateXP(xp, xpToNextLevel);
    }

    private void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * 1.5f);
        maxHitPoints += 10;
        defense += 2;
        power += 2;

        uiManager?.UpdateLevel(level);
        uiManager?.AddMessage($"Congratulations! You've reached level {level}!", Color.yellow);
        uiManager?.UpdateXP(xp, xpToNextLevel);
    }

    private bool IsPlayer() => GetComponent<Player>() != null;

    private void UpdateUI()
    {
        uiManager?.UpdateHealth(hitPoints, maxHitPoints);
        uiManager?.UpdateXP(xp, xpToNextLevel);
        uiManager?.UpdateLevel(level);
    }
}
