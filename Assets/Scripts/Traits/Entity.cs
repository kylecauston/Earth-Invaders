using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public enum SelectionShape { Square, Circle };

    public int maxHealth;
    public int health;
    public Globals.Alignment alignment;
    public Globals.Classification classification;
    public SelectionShape ringShape = SelectionShape.Circle;
    public float ringRadius = 5;
    public float bandWidth = 5;
    public Image healthBar;

    public bool visible = true;

    public CanvasGroup healthDisplay;
    private AgentAI ai;
    private WaitForSeconds fadeDelay = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        ai = GetComponent<AgentAI>();
        healthDisplay = GetComponentInChildren<CanvasGroup>();
        if(healthDisplay)
            healthDisplay.alpha = 0;
    }
   
    public void TakeDamage(int damage, Entity attacker)
    {
        health -= damage;

        healthBar.fillAmount = (float)health / maxHealth;

        if (health <= 0)
            OnDeath();

        StopAllCoroutines();
        StartCoroutine(FadeOut());

        if(ai)
        {
            ai.HandleDamage(attacker);
        }
    }

    public void GetHealed(int gain)
    {
        int last_hp = health;
        health = Mathf.Min(health + gain, maxHealth);

        healthBar.fillAmount = (float)health / maxHealth;

        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    public void OnDeath()
    {
        if (alignment == Globals.Alignment.Neutral)
        {
            TheGameManager.GameManager.instance.SpendResource(-50);
        }
        else if (alignment != TheGameManager.GameManager.instance.playerAlignment)
        {
            TheGameManager.GameManager.instance.SpendResource(-25);
        }
        Destroy(gameObject);
        // TODO: Stop all coroutines
    }

    public void OnMouseEnter()
    {
        InputManager.instance.HoverObject(this);
    }

    public void OnMouseExit()
    {
        InputManager.instance.HoverObject(null);
    }

    public void OnMouseDown()
    {
        InputManager.instance.ClickObject(this);
    }

    private IEnumerator FadeOut()
    {
        for(int i=400; i>0; i--)
        {
            if(i > 255)
            {
                yield return fadeDelay;
            }
            
            healthDisplay.alpha = i / 255.0f;
            yield return fadeDelay;
        }

        yield break;
    }
}
