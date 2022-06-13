using System.Collections.Generic;
using UnityEngine;

public enum PoolCode
{
    // Items
    SlashSkill,
    ThrowSwordSkill,
    PoisonSmoke,
    Mummy,
    Alter,
    EndDoor,
    MaxCount
}

public class PoolManager : Singleton<PoolManager>
{
    #region SetPoolData

    [SerializeField] private GameObject[] prefabCombine;

    private Queue<GameObject>[] _poolCombine = new Queue<GameObject>[(int)PoolCode.MaxCount];

    protected override void Awake()
    {
        // pool 초기화
        _poolCombine = new Queue<GameObject>[(int)PoolCode.MaxCount];

        for (var i = 0; i < _poolCombine.Length; i++)
        {
            _poolCombine[i] = new Queue<GameObject>();
        }
    }

    #endregion

// 오브젝트 파괴
    public void DestroyPrefab(GameObject prefab, PoolCode poolIndex)
    {
        var pool = _poolCombine[(int)poolIndex];
        pool.Enqueue(prefab);
        prefab.SetActive(false);
    }

// 오브젝트 생성
    public GameObject CreatPrefab(PoolCode poolIndex)
    {
        var index = (int) poolIndex;
        var pool = _poolCombine[index];

        if (pool.Count != 0)
        {
            return pool.Dequeue();
        }

        var prefab = Instantiate(prefabCombine[index], transform);
        prefab.SetActive(false);
        return prefab;
    }
}
