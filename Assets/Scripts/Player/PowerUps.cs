using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private float attackUpBonus;
    [SerializeField] private float attackUpDuration;

    public IEnumerator AttackUp()
    {
        playerCombat.attackDamage += attackUpBonus;

        yield return new WaitForSeconds(attackUpDuration);

        playerCombat.attackDamage -= attackUpBonus;
    }
}
