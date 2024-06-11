using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject inventory;
    public InventoryUI inventoryUI;
    public Healthbar healthbar;

    public InventoryUI InventoryUI { get { return inventory.GetComponent<InventoryUI>(); } }


    public static UIManager Instance { get; private set; }

    [Header("Documents")]
    public GameObject HealthBar;
    public GameObject Messages;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {

    }
    void Update()
    {

    }
    public void UpdateHealth(int current, int max)
    {
        HealthBar.GetComponent<HealthBar>().SetValues(current, max);
    }

    public void AddMessage(string message, Color color)
    {
        Messages.GetComponent<Messages>().AddMessage(message, color);
    }
    public void UpdateHealthbar(int currentHealth, int maxHealth)
    {
        if (healthbar != null)
        {
            healthbar.SetHealth(currentHealth, maxHealth);
        }
    }

    public void UpdateLevel(int level)
    {
        if (healthbar != null)
        {
            healthbar.SetLevel(level);
        }
    }

    public void UpdateXP(int xp, int xpToNextLevel)
    {
        if (healthbar != null)
        {
            healthbar.SetXP(xp, xpToNextLevel);
        }
    }
}
