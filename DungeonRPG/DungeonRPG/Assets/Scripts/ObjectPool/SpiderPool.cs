using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPool : ObjectPool<Spider>
{
    public override void Initialize()
    {
        base.Initialize();

        initialBufferSize = 40;
        if (initialBufferSize < 1) initialBufferSize = 1;

        prefabObject = Resources.Load<GameObject>("Prefabs/Monsters/Spider").GetComponent<Spider>();
        pooledObjects = new Queue<Spider>(initialBufferSize);

        for (int i = 0; i < initialBufferSize; i++)
        {
            Spider temp = GameObject.Instantiate(prefabObject);
            temp.Initialize();
            pooledObjects.Enqueue(temp);
        }
    }

    public override Spider New()
    {
        Spider enemyUnit = null;
        if (pooledObjects.Count > 0)
        {
            enemyUnit = pooledObjects.Dequeue();
        }
        else
        {
            enemyUnit = GameObject.Instantiate(prefabObject);
            enemyUnit.Initialize();
        }

        enemyUnit.Activate();
        return enemyUnit;
    }
}
