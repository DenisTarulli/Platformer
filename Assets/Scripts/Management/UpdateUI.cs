using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private Image[] hearts;

    public void HealthUpdate(int hp)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hp)
                hearts[i].gameObject.SetActive(true);
            else
                hearts[i].gameObject.SetActive(false);
        }

        //if (hp == 0)
            //GameOver();
    }
}
