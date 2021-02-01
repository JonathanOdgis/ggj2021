using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyEncounterSpawner : MonoBehaviour
{

    public EnemyEncounterCharacter characterToSpawn;

    EnemyEncounterCharacter character;

    public Customer[] customers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        if (character == null && Vector3.Distance(this.transform.position, Camera.main.transform.position) > 20)
        {
            if (Random.Range(0, 5) >= 2)
            {
                GameObject obj = Instantiate(characterToSpawn.gameObject, this.transform.position, this.transform.rotation);
                character = obj.GetComponent<EnemyEncounterCharacter>();
                character.customer = customers[Random.Range(0, customers.Length)];
            }
        }
        yield return new WaitForSeconds(15f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 2);
    }
}
