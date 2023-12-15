tehty muutoksia IENumerator StartSpawning methodiin

Alkuperäinen method:

````csharp
IEnumerator SpawnObjects()
{
    while (true)
    {
        GameObject objectToSpawn;

        do
        {
            objectToSpawn = enemyToSpawn[Random.Range(0, enemyToSpawn.Length)];
        } while (objectToSpawn == lastSpawnedEnemy);

        lastSpawnedEnemy = objectToSpawn;

        float nextY;
        do
        {
            nextY = Random.Range(minY, maxY);
        }
        while (Mathf.Abs(nextY - lastY) < minDeltaY);

        Vector2 spawnPosition = new Vector2(mainCamera.transform.position.x +
            mainCamera.orthographicSize * mainCamera.aspect + spawnOffsetX, nextY);

        GameObject spawnedObject = GetObjectFromPool(objectToSpawn);
        if (spawnedObject == null) continue;

        spawnedObject.transform.position = spawnPosition;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.SetActive(true);

        EnemyMover mover = spawnedObject.GetComponent<EnemyMover>();
        if (mover)
        {
            mover.SetSpeed(parallaxBackground.CameraSpeed);
        }

        lastY = nextY;

        currentSpawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
        yield return new WaitForSeconds(currentSpawnInterval);
    }
}
```

Tehdyt muutokset:

IENumerator method

````csharp
IEnumerator SpawnObjects()
{
    while (true)
    {
        GameObject objectToSpawn;

        do
        {
            objectToSpawn = enemyToSpawn[Random.Range(0, enemyToSpawn.Length)];
        } while (objectToSpawn == lastSpawnedEnemy);

        lastSpawnedEnemy = objectToSpawn;

        float nextY;
        do
        {
            nextY = Random.Range(minY, maxY);
        }
        while (Mathf.Abs(nextY - lastY) < minDeltaY);

        Vector2 spawnPosition = new Vector2(mainCamera.transform.position.x +
            mainCamera.orthographicSize * mainCamera.aspect + spawnOffsetX, nextY);

        GameObject spawnedObject = GetObjectFromPool(objectToSpawn);
        if (spawnedObject == null) continue;

        spawnedObject.transform.position = spawnPosition;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.SetActive(true);
        EnemyMover mover = spawnedObject.GetComponent<EnemyMover>();
        if (mover)
        {
            mover.SetCustomSpeedFactor(parallaxBackground.CameraSpeed * 0.5f); // Voit käyttää mitä tahansa sopivaa tekijää tässä
        }

        lastY = nextY;

        currentSpawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
        yield return new WaitForSeconds(currentSpawnInterval);
    }
}
```