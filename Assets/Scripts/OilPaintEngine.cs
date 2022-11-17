using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPaintEngine : MonoBehaviour
{
    private const bool LOG_MASK_CALC_APPLY_TIME = false;

    private Camera Camera;

    private int CanvasColliderID;
    private Renderer CanvasRenderer;
    private float CanvasWidth; // world space
    private float CanvasHeight; // world space

    public int TextureResolution { get; private set; } = 20; // texture space pixels per 1 world space
    private FastTexture2D Texture;
    private int TextureWidth; // texture space
    private int TextureHeight; // texture space

    private IOilPaintSurface OilPaintSurface;
    private IRakelPaintReservoir RakelPaintReservoir;

    public Vector2 RakelNormal { get; private set; } = new Vector2(1, 0);
    public float RakelLength { get; private set; } = 4f; // world space
    public float RakelWidth { get; private set; } = 0.3f; // world space
    public Paint RakelPaint { get; private set; }

    public RakelDrawer RakelDrawer;

    void Awake()
    {
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        CanvasColliderID = GameObject.Find("Canvas").GetComponent<MeshCollider>().GetInstanceID();
        CanvasRenderer = GameObject.Find("Canvas").GetComponent<Renderer>();

        // convert scale attribute to world space
        CanvasWidth = GameObject.Find("Canvas").GetComponent<Transform>().localScale.x * 10;
        CanvasHeight = GameObject.Find("Canvas").GetComponent<Transform>().localScale.y * 10;

        CreateCanvasAndTools();
    }

    void CreateCanvasAndTools()
    {
        TextureWidth = WorldSpaceLengthToTextureSpaceLength(CanvasWidth, TextureResolution);
        TextureHeight = WorldSpaceLengthToTextureSpaceLength(CanvasHeight, TextureResolution);
        Texture = new FastTexture2D(TextureWidth, TextureHeight);
        CanvasRenderer.material.SetTexture("_MainTex", Texture.Texture);

        OilPaintSurface = new OilPaintSurface(Texture);

        CreateRakelDrawer();
    }

    void CreateRakelDrawer()
    {
        RakelPaintReservoir = new RakelPaintReservoir(
            WorldSpaceLengthToTextureSpaceLength(RakelLength, TextureResolution),
            WorldSpaceLengthToTextureSpaceLength(RakelWidth, TextureResolution));

        int length = WorldSpaceLengthToTextureSpaceLength(RakelLength, TextureResolution);
        int width = WorldSpaceLengthToTextureSpaceLength(RakelWidth, TextureResolution);
        IRakel Rakel = new Rakel(length, width, RakelPaintReservoir, new MaskCalculator(), new MaskApplicator());
        Debug.Log("Rakel is " + Rakel.Length + "x" + Rakel.Width + " = " + Rakel.Length * Rakel.Width);
        Rakel.UpdateNormal(RakelNormal, LOG_MASK_CALC_APPLY_TIME);

        RakelDrawer = new RakelDrawer(Rakel);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldSpaceHit = InputUtil.GetMouseHit(Camera, CanvasColliderID);
        if (!worldSpaceHit.Equals(Vector3.negativeInfinity))
        {
            Vector2Int preciseBrushPosition = WorldSpaceCoordinateToTextureSpaceCoordinate(worldSpaceHit);

            if (Input.GetMouseButtonDown(0))
            {
                RakelDrawer.NewStroke();
            }
            RakelDrawer.AddNode(OilPaintSurface, preciseBrushPosition, RakelNormal, LOG_MASK_CALC_APPLY_TIME);
        }
    }

    Vector2Int WorldSpaceCoordinateToTextureSpaceCoordinate(Vector3 worldSpaceCoordinate)
    {
        // "move" canvas to positive coordinates only, so further calculations are simplified
        float absX = worldSpaceCoordinate.x + CanvasWidth / 2; // TODO what about odd numbers?
        float absY = worldSpaceCoordinate.z + CanvasHeight / 2; // TODO what about odd numbers?

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
        CreateRakelDrawer();
    }

    public void UpdateRakelWidth(float worldSpaceWidth)
    {
        RakelWidth = worldSpaceWidth;
        CreateRakelDrawer();
    }

    public void UpdateRakelNormal(Vector2 normal)
    {
        RakelNormal = normal;
    }

    public void UpdateRakelPaint(Paint paint)
    {
        RakelPaint = paint;
        RakelPaintReservoir.Fill(paint);
    }

    public void UpdateTextureResolution(int pixelsPerWorldSpaceUnit)
    {
        TextureResolution = pixelsPerWorldSpaceUnit;
        CreateCanvasAndTools();
    }
}