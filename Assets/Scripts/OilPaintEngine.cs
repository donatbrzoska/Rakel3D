using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPaintEngine : MonoBehaviour
{
    private Camera Camera;

    private int CanvasColliderID;
    private Renderer CanvasRenderer;
    private float CanvasWidth; // world space
    private float CanvasHeight; // world space

    public int TextureResolution { get; private set; } = 100; // texture space pixels per 1 world space
    private OilPaintTexture Texture;
    private int TextureWidth; // texture space
    private int TextureHeight; // texture space

    public Rakel Rakel { get; private set; }
    public Vector2 RakelNormal { get; private set; } = new Vector2(1, 0);
    public Color RakelColor { get; private set; } = new Color(0.3f, 0, 0.7f);
    public float RakelLength { get; private set; } = 4f; // world space
    public float RakelWidth { get; private set; } = 2f; // world space

    void Awake()
    {
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        CanvasColliderID = GameObject.Find("Canvas").GetComponent<MeshCollider>().GetInstanceID();
        CanvasRenderer = GameObject.Find("Canvas").GetComponent<Renderer>();

        // convert scale attribute to world space
        CanvasWidth = GameObject.Find("Canvas").GetComponent<Transform>().localScale.x * 10;
        CanvasHeight = GameObject.Find("Canvas").GetComponent<Transform>().localScale.y * 10;

        CreateTexture();

        //Rakel = new Rakel();
        //Rakel = new BasicRakel(new BasicMaskCalculator(), new BasicMaskApplicator());
        Rakel = new OptimizedRakel(new OptimizedMaskCalculator(), new OptimizedMaskApplicator());
        InitializeRakel();
    }

    void CreateTexture()
    {
        TextureWidth = WorldSpaceLengthToTextureSpaceLength(CanvasWidth, TextureResolution);
        TextureHeight = WorldSpaceLengthToTextureSpaceLength(CanvasHeight, TextureResolution);
        Texture = new OilPaintTexture(TextureWidth, TextureHeight);
        CanvasRenderer.material.SetTexture("_MainTex", Texture.Texture);
    }

    void InitializeRakel()
    {
        Rakel.UpdateLength(WorldSpaceLengthToTextureSpaceLength(RakelLength, TextureResolution));
        Rakel.UpdateWidth(WorldSpaceLengthToTextureSpaceLength(RakelWidth, TextureResolution));
        Debug.Log("Rakel is " + Rakel.Length + "x" + Rakel.Width + " = " + Rakel.Length * Rakel.Width);
        Rakel.UpdateNormal(RakelNormal);
        Rakel.UpdateColor(RakelColor);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldSpaceHit = InputUtil.GetMouseHit(Camera, CanvasColliderID);
        if (!worldSpaceHit.Equals(Vector3.negativeInfinity))
        {
            Vector2Int preciseBrushPosition = ToCanvasTextureSpacePoint(worldSpaceHit);
            Rakel.UpdatePosition(preciseBrushPosition);
            Rakel.ApplyToCanvas(Texture, false);
        }
    }

    Vector2Int ToCanvasTextureSpacePoint(Vector3 worldSpacePoint)
    {
        // "move" canvas to positive coordinates only, so further calculations are simplified
        float absX = worldSpacePoint.x + CanvasWidth / 2; // TODO what about odd numbers?
        float absY = worldSpacePoint.z + CanvasHeight / 2; // TODO what about odd numbers?

        // for the space conversion, a ratio between the two sizes is needed
        float xMultiplier = TextureWidth / CanvasWidth; // Calculate: How much wider is the texture than the world space?
        float yMultiplier = TextureHeight / CanvasHeight;

        int textureX = Mathf.RoundToInt(absX * xMultiplier);
        int textureY = Mathf.RoundToInt(absY * yMultiplier);

        return new Vector2Int(textureX, textureY);
    }

    int WorldSpaceLengthToTextureSpaceLength(float worldSpaceLength, int textureResolution)
    {
        return (int)(textureResolution * worldSpaceLength);
    }

    /*
     MouseInput: Canvas hitpoint in world space
     Canvas:     Convert world space hitpoint --> canvas texture space hitpoint
     Brush:      Do stuff with texture space hitpoint
    */

    public void UpdateRakelLength(float worldSpaceLength)
    {
        RakelLength = worldSpaceLength;
        Rakel.UpdateLength(WorldSpaceLengthToTextureSpaceLength(worldSpaceLength, TextureResolution));
    }

    public void UpdateRakelWidth(float worldSpaceWidth)
    {
        RakelWidth = worldSpaceWidth;
        Rakel.UpdateWidth(WorldSpaceLengthToTextureSpaceLength(worldSpaceWidth, TextureResolution));
    }

    public void UpdateRakelNormal(Vector2 normal)
    {
        Rakel.UpdateNormal(normal);
    }

    public void UpdateTextureResolution(int pixelsPerWorldSpaceUnit)
    {
        TextureResolution = pixelsPerWorldSpaceUnit;
        CreateTexture();
        InitializeRakel(); // Rakel is dependent on TextureResolution
    }
}