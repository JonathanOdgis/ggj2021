﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnBoarding : MonoBehaviour
{

    [SerializeField]
    int value = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("We got an item!");
            GameManagerOnBoarding.Instance.updateScore(value);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}