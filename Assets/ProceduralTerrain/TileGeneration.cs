using UnityEngine;

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public class TileGeneration : MonoBehaviour
{
    [SerializeField] NoiseMapGeneration noiseMapGeneration;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] float mapScale;
    [SerializeField] float heightMultiplier;
    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] public TerrainType[] terrainTypes;
    [SerializeField] private Wave[] waves;

    private void Start()
    {
        GenerateTile();
    }

    void GenerateTile()
    {
        Vector3[] meshVerticies = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVerticies.Length);
        int tileWidth = tileDepth;

        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;

        float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);

        Material terrainMaterial = GetComponent<Renderer>().material;
        Texture2D tileTexture = BuildTexture(heightMap);
        terrainMaterial.SetTexture("_Texture",tileTexture);
        //this.meshRenderer.material.mainTexture = tileTexture;


        UpdateMeshVerticies(heightMap);
    }

    private void UpdateMeshVerticies(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Vector3[] meshVerticies = this.meshFilter.mesh.vertices;

        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];

                Vector3 vertex = meshVerticies[vertexIndex];
                meshVerticies[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);

                vertexIndex++;
            }
        }

        this.meshFilter.mesh.vertices = meshVerticies;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
        this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }

    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];

                TerrainType terrainType = ChooseTerrainType(height);

                colorMap[colorIndex] = terrainType.color;
            }
        }

        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();

        return tileTexture;
    }

    /*
    private void addColor(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Material terrainMaterial = GetComponent<Renderer>().material;

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];

                TerrainType terrainType = ChooseTerrainType(height);

                colorMap[colorIndex] = terrainType.color;
                terrainMaterial.SetColor("_Color",colorMap[colorIndex]);
            }
        }
    }
    */

    TerrainType ChooseTerrainType(float height)
    {
        foreach(TerrainType terrainType in terrainTypes)
        {
            if(height < terrainType.height)
            {
                return terrainType;
            }
        }
        return terrainTypes[terrainTypes.Length - 1];
    }
}