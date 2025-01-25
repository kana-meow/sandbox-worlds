using UnityEngine;

public class EntitySpawner : Singleton<EntitySpawner> {

    private void Start() {
        // manually spawn passive animal for testing
        SpawnEntity("base:passive_animal");
        SpawnEntity("base:player", new Vector3(2, 1, 0));
    }

    public void SpawnEntity(string entityID, Vector3 pos = default) {
        //EntityJson entityData;
        //GlobalRegistry.Instance.TryGetEntity(entityID, out entityData);

        // if (entityData != null) {
        //     GameObject newGameObject = new(entityData.DisplayName);
        //    newGameObject.transform.position = pos;
        //     BaseEntity newEntity = newGameObject.AddComponent<BaseEntity>();
        //newEntity.Initialize(entityData);
    }
}