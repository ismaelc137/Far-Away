using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Terminal : Interactable
{
    [SerializeField]
    private GameObject Trapdoor;
    private bool TrapdoorOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact()
    {
       TrapdoorOpen = !TrapdoorOpen;
       Trapdoor.GetComponent<Animator>().SetBool("isOpent", TrapdoorOpen);
    }
}
