using UnityEngine;

public static class SpawnPointInstaller
{
    public static bool TrySetPosition(out Vector3 spawnPosition, float maxSpawnDistanceX, float maxSpawnDistanceZ, float targetRadius, LayerMask interferencesMask)
    {
        float spawnPozitionX = Random.Range(maxSpawnDistanceX, -maxSpawnDistanceX);
        float spawnPozitionZ = Random.Range(maxSpawnDistanceZ, -maxSpawnDistanceZ);

        spawnPosition = new Vector3(spawnPozitionX, 0, spawnPozitionZ);

        return CheckPositionToFree(spawnPosition, targetRadius, interferencesMask);
    }

    private static bool CheckPositionToFree(Vector3 spawnPosition, float targetRadius, LayerMask interferencesMask)
    {
        bool isPositionOccupied = Physics.CheckSphere(spawnPosition, targetRadius, interferencesMask);

        return isPositionOccupied;
    }
}