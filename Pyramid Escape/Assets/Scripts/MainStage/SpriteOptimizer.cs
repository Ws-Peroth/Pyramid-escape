using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOptimizer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tile")) other.gameObject.SetActive(true);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Tile")) other.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tile")) other.gameObject.SetActive(false);
    }
}
