using UnityEngine;

public class LootManager : MonoBehaviour
{
    public GameObject HealthPrefab;
    
    private int _lootAmount;
    #region Private Methods
    private void InitializeLootAmount(bool randomizedLootAmount, int lootAmount)
    {
        if (randomizedLootAmount)
        {
            _lootAmount = Random.Range(LootsConst.MIN_LOOT_AMOUNT, LootsConst.MAX_LOOT_AMOUNT);
        }
        else
        {
            _lootAmount = lootAmount;
        }
    }
    #endregion
    #region Public Methods
    public void Initialisation(bool randomizedLootAmount, int lootAmount)
    {
        InitializeLootAmount(randomizedLootAmount, lootAmount);
    }

    public void Loot()
    {
        for (int i = 0; i < _lootAmount; i++)
        {
            GameObject healthBulet = Instantiate(HealthPrefab, transform.position, Quaternion.identity);
            healthBulet.GetComponent<Bullet>().SetDirection(Vector3.down);
        }
    }
    #endregion
}
