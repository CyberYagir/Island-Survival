using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public string _interactText;
    public bool inOver;

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inOver)
                Interact();
        }
    }
    public virtual void Interact()
    {
    }
}


public class WaterDrink : Interactable
{
    [SerializeField] float cooldown;
    float time;

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
    }
    public override void Interact()
    {
        if (time >= cooldown)
        {
            base.Interact();
            GameManager.manager.LocalPlayer.GetComponent<PlayerStats>().water += 20;
            GetComponentInChildren<ParticleSystem>().Play();
            time = 0;
        }      
    }
}
