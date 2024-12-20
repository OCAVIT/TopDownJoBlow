using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject[] hpTypes;
    public float health = 100;
    // Start is called before the first frame update
    public void GetDamage(float damage)
    {
        health -= damage;
        hpTypes[0].SetActive(false);
        hpTypes[1].SetActive(false);
        hpTypes[2].SetActive(false);
        if (health > 50) hpTypes[0].SetActive(true);
        if (health <= 50 && health > 20) hpTypes[1].SetActive(true);
        if (health <= 20) hpTypes[2].SetActive(true);
        if(health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
