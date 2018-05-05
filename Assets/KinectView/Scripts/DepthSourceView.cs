using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Linq; // used for Sum of array
using System;

public enum DepthViewMode
{
    SeparateSourceReaders,
    MultiSourceReader,
}

public class DepthSourceView : MonoBehaviour
{
    public DepthViewMode ViewMode = DepthViewMode.SeparateSourceReaders;
    public Terrain Terrain;

    public GameObject ColorSourceManager;
    public GameObject DepthSourceManager;
    public GameObject MultiSourceManager;
    
    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;
    private Mesh _Mesh;
    private Vector3[] _Vertices;
    private Vector2[] _UV;
    private int[] _Triangles;
    
    // Only works at 4 right now
    private const int _DownsampleSize = 4;
    private const double _DepthScale = 0.1f;
    private const int _Speed = 50;
    
    private MultiSourceManager _MultiManager;
    private ColorSourceManager _ColorManager;
    private DepthSourceManager _DepthManager;

    public float period = 0.0f;
    // float[] depthLookUp = new float[2048];
       

void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;

            // Downsample to lower resolution
            //CreateMesh(frameDesc.Width / _DownsampleSize, frameDesc.Height / _DownsampleSize);
            
       //     for (int i = 0; i < depthLookUp.Length; i++)
      //      {
        //        depthLookUp[i] = rawDepthToMeters(i);
       //     }
         
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }
    }

    void changeHeight(float [,] heightMap)
    {

        // xRes = 513 yRes = 513
        //var xRes = Terrain.terrainData.heightmapWidth;
        //var yRes = Terrain.terrainData.heightmapHeight;

        Terrain.terrainData.SetHeights(0, 0, heightMap);

        // int xBase = 125;
        // int yBase = 125;
        // this returns a rank 2 float array starting at xBase, yBase in heightmap
        // with the specified width and height. Each item in the array ranges from 0.0 to 1.0
        // with 0 being the lowest height and 1 being the highest

    }

    void CreateMesh(int width, int height)
    {
        _Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _Mesh;

        _Vertices = new Vector3[width * height];
        _UV = new Vector2[width * height];
        _Triangles = new int[6 * ((width - 1) * (height - 1))];

        int triangleIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;

                _Vertices[index] = new Vector3(x, -y, 0);
                _UV[index] = new Vector2(((float)x / (float)width), ((float)y / (float)height));

                // Skip the last row/col
                if (x != (width - 1) && y != (height - 1))
                {
                    int topLeft = index;
                    int topRight = topLeft + 1;
                    int bottomLeft = topLeft + width;
                    int bottomRight = bottomLeft + 1;

                    _Triangles[triangleIndex++] = topLeft;
                    _Triangles[triangleIndex++] = topRight;
                    _Triangles[triangleIndex++] = bottomLeft;
                    _Triangles[triangleIndex++] = bottomLeft;
                    _Triangles[triangleIndex++] = topRight;
                    _Triangles[triangleIndex++] = bottomRight;
                }
            }
        }

        _Mesh.vertices = _Vertices;
        _Mesh.uv = _UV;
        _Mesh.triangles = _Triangles;
        _Mesh.RecalculateNormals();
    }
    
    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.TextField(new Rect(Screen.width - 250 , 10, 250, 20), "DepthMode: " + ViewMode.ToString());
        GUI.EndGroup();
    }

    void Update()

    {
        if (_Sensor == null)
        {
            return;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            if(ViewMode == DepthViewMode.MultiSourceReader)
            {
                ViewMode = DepthViewMode.SeparateSourceReaders;
            }
            else
            {
                ViewMode = DepthViewMode.MultiSourceReader;
            }
        }
        
        float yVal = Input.GetAxis("Horizontal");
        float xVal = -Input.GetAxis("Vertical");

        transform.Rotate(
            (xVal * Time.deltaTime * _Speed), 
            (yVal * Time.deltaTime * _Speed), 
            0, 
            Space.Self);
            
        if (ViewMode == DepthViewMode.SeparateSourceReaders)
        {
            if (ColorSourceManager == null)
            {
                return;
            }
            
            _ColorManager = ColorSourceManager.GetComponent<ColorSourceManager>();
            if (_ColorManager == null)
            {
                return;
            }
            
            if (DepthSourceManager == null)
            {
                return;
            }
            
            _DepthManager = DepthSourceManager.GetComponent<DepthSourceManager>();
            if (_DepthManager == null)
            {
                return;
            }
            //   gameObject.GetComponent<Renderer>().material.mainTexture = _ColorManager.GetColorTexture();
            RefreshData(_DepthManager.GetData(),
                _ColorManager.ColorWidth,
                _ColorManager.ColorHeight);
        }
        else
        {
            if (MultiSourceManager == null)
            {
                return;
            }
            
            _MultiManager = MultiSourceManager.GetComponent<MultiSourceManager>();
            if (_MultiManager == null)
            {
                return;
            }
            
            gameObject.GetComponent<Renderer>().material.mainTexture = _MultiManager.GetColorTexture();


            //InvokeRepeating("RefreshData", 0.0f, 3.0f);

            if (Input.GetKeyDown(KeyCode.C))
            {
                //switch all painted in texture 1 to texture 2
                Debug.Log("one to two");
                UpdateTerrainTexture(Terrain.terrainData, 1, 2);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                //switch all painted in texture 2 to texture 1
                Debug.Log("two to one");
                UpdateTerrainTexture(Terrain.terrainData, 2, 1);
            }

            RefreshData(_MultiManager.GetDepthData(),
                        _MultiManager.ColorWidth,
                        _MultiManager.ColorHeight);


        }

    }

    float rawDepthToMeters(int depthValue)
    {
        if (depthValue < 2047)
        {
            return (float)(1.0 / (depthValue * -0.0030711016 + 3.3309495161));
        }
        return (0.0f);
    }


    private void RefreshData(ushort[] depthData, int colorWidth, int colorHeight)
    {

        var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
        float[,] heightMap = new float[512, 512];
        int w = 0;
        int h = 0;
        int nx = Terrain.terrainData.heightmapWidth;
        int ny = Terrain.terrainData.heightmapHeight;
        float[,] heights = Terrain.terrainData.GetHeights(0, 0, nx, ny);
        // Remove marigins of test environment in room 6A by setting max(h)=300 and min(w)=175
        for (h = 80;  h< 360; h+=1) {
            for (w = 180; w < 410; w+=1) {
                if (heights[h + 60, w - 90] != 0 & Math.Abs(heightMap[h + 60, w - 90] - heights[h + 60, w - 90]) / heights[h + 60, w - 90] <= 0.2)
//                    heightMap[h + 60, w - 90] = heights[h + 60, w - 90];
                    heightMap[h + 60, w - 90] = 0;

                else
//                    heightMap[h + 60, w - 90] = (1f - ((depthData[w + (h * 512)] / 1000f)) + heights[h + 60, w - 90]) * 0.65f;
                heightMap[h + 60, w - 90] = 0;
                if (heightMap[h + 60, w -90] > 0.86f)
              {
                    heightMap[h + 60, w -90] = 0;
               }
        //        //heightMap[h, w] = (depthData[w + (h * 512)]/2460 + heights[h, w]) * .2f;
            }
        }

        //Debug.Log(1f - depthData[200 + (200 * 512)] / 2460f);
        //Debug.Log(depthData[200 + (200 * 512)]);

        //Debug.Log(depthLookUp[depthData[200 + (200 * 512)]]); 

        Terrain.terrainData.SetHeights(0, 0, heightMap);

        //ColorSpacePoint[] colorSpace = new ColorSpacePoint[depthData.Length];
        //_Mapper.MapDepthFrameToColorSpace(depthData, colorSpace);

        //for (int y = 0; y < frameDesc.Height; y += _DownsampleSize)
        //{
        //    for (int x = 0; x < frameDesc.Width; x += _DownsampleSize)
        //    {
        //        int indexX = x / _DownsampleSize;
        //        int indexY = y / _DownsampleSize;
        //        int smallIndex = (indexY * (frameDesc.Width / _DownsampleSize)) + indexX;

        //        double avg = GetAvg(depthData, x, y, frameDesc.Width, frameDesc.Height);

        //        avg = avg * _DepthScale;

        //        _Vertices[smallIndex].z = (float)avg;

        //        // Update UV mapping with CDRP
        //        var colorSpacePoint = colorSpace[(y * frameDesc.Width) + x];
        //        _UV[smallIndex] = new Vector2(colorSpacePoint.X / colorWidth, colorSpacePoint.Y / colorHeight);
        //    }
        //}

        //_Mesh.vertices = _Vertices;
        //_Mesh.uv = _UV;
        //_Mesh.triangles = _Triangles;
        //_Mesh.RecalculateNormals();
    }
 

       
    private double GetAvg(ushort[] depthData, int x, int y, int width, int height)
    {
        double sum = 0.0;
        
        for (int y1 = y; y1 < y + 4; y1++)
        {
            for (int x1 = x; x1 < x + 4; x1++)
            {
                int fullIndex = (y1 * width) + x1;
                
                if (depthData[fullIndex] == 0)
                    sum += 4500;
                else
                    sum += depthData[fullIndex];
                
            }
        }

        return sum / 16;
    }

    void OnApplicationQuit()
    {
        if (_Mapper != null)
        {
            _Mapper = null;
        }
        
        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }


    static void UpdateTerrainTexture(TerrainData terrainData, int textureNumberFrom, int textureNumberTo)
    {
        //get current paint mask
        Debug.Log("here");
        float[,,] alphas = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        // make sure every grid on the terrain is modified
        for (int i = 0; i < terrainData.alphamapWidth; i++)
        {
            for (int j = 0; j < terrainData.alphamapHeight; j++)
            {
                //for each point of mask do:
                //paint all from old texture to new texture (saving already painted in new texture)
                alphas[i, j, textureNumberTo] = Mathf.Max(alphas[i, j, textureNumberFrom], alphas[i, j, textureNumberTo]);
                //set old texture mask to zero
                alphas[i, j, textureNumberFrom] = 0f;
            }
        }
        // apply the new alpha
        terrainData.SetAlphamaps(0, 0, alphas);
    }
}
