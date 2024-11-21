using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneGenerator : MonoBehaviour
{
    public GameObject ground;
    public GameObject zonePrefab;
    public int zoneLevel;

    const float WorldBlockSize = 10.0f;
    const float ZoneY = 0.0002f;

    public static ZoneGenerator Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void CreateZoneAtZeroCoordinates()
    {
        var zone = Instantiate(zonePrefab, Vector3.zero, Quaternion.identity);
        zone.transform.SetParent(ground.transform);
        zone.GetComponent<Zone>().SetPurchased(true);
    }

    void Start()
    {
        CreateZoneAtZeroCoordinates();

        var initialPlatforms = GeneratePlatforms(ground);
        
        foreach (var platform in initialPlatforms)
        {
            platform.GetComponent<Zone>().SetPurchased(true);
        
            var platforms = GeneratePlatforms(platform);
        
            foreach (var nestedPlatform in platforms)
            {
                nestedPlatform.GetComponent<Zone>().SetPurchased(false);
            }
        }
    }

    List<GameObject> GeneratePlatforms(GameObject platform)
    {
        var initialPlatformList = new List<GameObject>();

        var initialCornersPlatforms = GeneratePlatformsInCorners(platform);
        var initialSidesPlatforms = GeneratePlatformsOutside(platform);

        initialPlatformList.AddRange(initialCornersPlatforms);
        initialPlatformList.AddRange(initialSidesPlatforms);

        return initialPlatformList;
    }

    public void GeneratePlatformsAroundPlatform(GameObject platform)
    {
        GeneratePlatformsInCorners(platform);
        GeneratePlatformsOutside(platform);

        IncreaseZoneLevel();
    }

    void IncreaseZoneLevel()
    {
        zoneLevel += 1;
    }

    List<GameObject> GeneratePlatformsInCorners(GameObject platform)
    {
        var platformScale = platform.transform.localScale;
        var zoneScale = zonePrefab.transform.localScale;

        var platformSizeX = platformScale.x * WorldBlockSize / 2;
        var platformSizeZ = platformScale.z * WorldBlockSize / 2;

        var halfZoneSizeX = zoneScale.x * WorldBlockSize / 2;
        var halfZoneSizeZ = zoneScale.z * WorldBlockSize / 2;

        Vector3[] offsetVectors =
        {
            new Vector3(-platformSizeX - halfZoneSizeX, ZoneY, platformSizeZ + halfZoneSizeZ), // Верхний левый
            new Vector3(platformSizeX + halfZoneSizeX, ZoneY, platformSizeZ + halfZoneSizeZ), // Верхний правый
            new Vector3(-platformSizeX - halfZoneSizeX, ZoneY, -platformSizeZ - halfZoneSizeZ), // Нижний левый
            new Vector3(platformSizeX + halfZoneSizeX, ZoneY, -platformSizeZ - halfZoneSizeZ) // Нижний правый
        };

        var createdPlatforms = new List<GameObject>();

        foreach (var offset in offsetVectors)
        {
            var position = platform.transform.TransformPoint(offset);

            if (IsPositionOccupied(position, zoneScale))
                continue;

            var zone = Instantiate(zonePrefab, position, Quaternion.identity);
            zone.transform.SetParent(ground.transform);

            createdPlatforms.Add(zone);
        }

        return createdPlatforms;
    }

    List<GameObject> GeneratePlatformsOutside(GameObject platform)
    {
        var platformScale = platform.transform.localScale;
        var zoneScale = zonePrefab.transform.localScale;

        var platformSizeX = platformScale.x * WorldBlockSize / 2;
        var platformSizeZ = platformScale.z * WorldBlockSize / 2;

        var halfZoneSizeX = zoneScale.x * WorldBlockSize / 2;
        var halfZoneSizeZ = zoneScale.z * WorldBlockSize / 2;

        Vector3[] offsetVectors =
        {
            new Vector3(-platformSizeX + halfZoneSizeX, ZoneY, platformSizeZ + halfZoneSizeZ),
            new Vector3(platformSizeX - halfZoneSizeX, ZoneY, platformSizeZ + halfZoneSizeZ),
            new Vector3(platformSizeX + halfZoneSizeX, ZoneY, platformSizeZ - halfZoneSizeZ),
            new Vector3(platformSizeX + halfZoneSizeX, ZoneY, -platformSizeZ + halfZoneSizeZ),
            new Vector3(platformSizeX - halfZoneSizeX, ZoneY, -platformSizeZ - halfZoneSizeZ),
            new Vector3(-platformSizeX + halfZoneSizeX, ZoneY, -platformSizeZ - halfZoneSizeZ),
            new Vector3(-platformSizeX - halfZoneSizeX, ZoneY, -platformSizeZ + halfZoneSizeZ),
            new Vector3(-platformSizeX - halfZoneSizeX, ZoneY, platformSizeZ - halfZoneSizeZ)
        };

        var createdPlatforms = new List<GameObject>();

        foreach (var offset in offsetVectors)
        {
            var position = platform.transform.TransformPoint(offset);

            if (IsPositionOccupied(position, zoneScale))
                continue;

            var zone = Instantiate(zonePrefab, position, Quaternion.identity);
            zone.transform.SetParent(ground.transform);

            createdPlatforms.Add(zone);
        }

        return createdPlatforms;
    }

    static bool IsPositionOccupied(Vector3 position, Vector3 scale)
    {
        var zoneSize = scale * WorldBlockSize / 2;
        var colliders = Physics.OverlapBox(position, zoneSize / 2, Quaternion.identity);

        return colliders.Any(overlappingCollider =>
            overlappingCollider.GetComponent<Zone>() != null);
    }
}
