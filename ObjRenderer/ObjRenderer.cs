using System;
using System.Numerics;

namespace ObjRenderer
{
  /// <summary>
  /// Class ObjRenderer.
  /// </summary>
  public class ObjRenderer
  {

    #region Fields

    private static int width = 800, height = 500;
    private static ShaderProgram program;
    private static VBO<Vector3> vertices;
    private static VBO<int> indices;
    private static VBO<Vector2> uv;
    private static VBO<Vector3> normals;
    private static Texture concrete;
    private static System.Diagnostics.Stopwatch watch;
    private static float angle;
    public static Vector3 cameraPosition = new Vector3(40, 0, 0);
    public static Vector3 cameraFront = new Vector3(-1, 0, 0);
    public static Vector3 Target = cameraPosition + cameraFront;
    public static Vector3 worldUp = new Vector3(0, 0, 1);
    static float x = 0f;
    static float y = 0f;
    static float z = 0f;
    static readonly Object obj = ObjLoader.Instance.Load(@"C:\");

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjRenderer"/> class.
    /// </summary>
    public ObjRenderer()
    {
      Glut.glutInit();
      Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
      Glut.glutInitWindowSize(width, height);
      Glut.glutCreateWindow("Obj Renderer: by Ibrahim Saad");
      Glut.glutSetCursor(Glut.GLUT_CURSOR_CROSSHAIR);
      Glut.glutKeyboardFunc(processKeys);
      Glut.glutIdleFunc(OnRenderFrame);
      Glut.glutDisplayFunc(OnDisplay);
      Glut.glutCloseFunc(OnClose);

      program = new ShaderProgram(Shaders.VertexShader, Shaders.FragmentShader);
      program.Use();
      program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
      program["view_matrix"].SetValue(Matrix4.LookAt(cameraPosition, Target, worldUp));
      program["light_direction"].SetValue(new Vector3(50, 20, 50));

      concrete = new Texture(@"C:\ProgramData\Autodesk\Revit\Addins\SimLab\OBJExporter\data\Imported_Textures\2\DWF\SimLabDWFFile\Concrete.Cast-In-Place.Flat.Grey.1.jpeg");

      vertices = new VBO<Vector3>(obj.Vertices);
      indices = new VBO<int>(obj.Indices, BufferTarget.ElementArrayBuffer);
      uv = new VBO<Vector2>(obj.TextureVertices);
      normals = new VBO<Vector3>(obj.Normals);

      watch = System.Diagnostics.Stopwatch.StartNew();

      Glut.glutMainLoop();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Processes the keys.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="xx">The xx.</param>
    /// <param name="yy">The yy.</param>
    void processKeys(byte key, int xx, int yy)
    {

      float cameraSpeed = 0.05f;
      if (key == 'w')
      {
        cameraPosition += cameraSpeed * cameraFront;
        x += 0.5f;
      }

      else if (key == 's')
      {
        cameraPosition -= cameraSpeed * cameraFront;
        x -= 0.5f;
      }

      else if (key == 'a')
      {
        cameraPosition -= Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * cameraSpeed;
        y += 0.5f;
      }
      else if (key == 'd')
      {
        cameraPosition += Vector3.Normalize(Vector3.Cross(cameraFront, worldUp)) * cameraSpeed;
        y -= 0.5f;
      }

      else if (key == 'e')
      {
        z += 0.5f;
      }

      else if (key == 'q')
      {
        z -= 0.5f;
      }
    }

    /// <summary>
    /// Called when Close event is raised.
    /// </summary>
    private static void OnClose()
    {

      vertices.Dispose();
      indices.Dispose();
      uv.Dispose();
      concrete.Dispose();
      program.DisposeChildren = true;
      program.Dispose();
    }

    /// <summary>
    /// Called when [display].
    /// </summary>
    private static void OnDisplay()
    {

    }

    /// <summary>
    /// Called when [render frame].
    /// </summary>
    private static void OnRenderFrame()
    {
      watch.Stop();
      float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
      watch.Restart();
      angle += deltaTime;
      Gl.ClearColor(0.3f, 0.4f, 0.9f, 1f);
      Gl.Viewport(0, 0, width, height);
      Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      Gl.UseProgram(program);
      Gl.BindTexture(concrete);

      program["model_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(x, y, z)));

      Gl.BindBufferToShaderAttribute(vertices, program, "vertexPosition");
      Gl.BindBufferToShaderAttribute(normals, program, "vertexNormal");
      Gl.BindBufferToShaderAttribute(uv, program, "vertexUV");
      Gl.BindBuffer(indices);
      Gl.DrawElements(BeginMode.Triangles, indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

      Glut.glutSwapBuffers();
    }


    #endregion
     
  }

}
