using System;
using Tao.FreeGlut;
using OpenGL;


namespace ObjRenderer
{
    class ObjRenderer
    {
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


        static readonly Object obj = ObjLoader.Loader(@"C:\Users\Saad\Desktop\Desktop\RevitModel.obj");

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

            


            program = new ShaderProgram(VertexShader, FragmentShader);
            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(cameraPosition, Target, worldUp));
            program["light_direction"].SetValue(new Vector3(50,20,50));

            concrete = new Texture(@"C:\ProgramData\Autodesk\Revit\Addins\SimLab\OBJExporter\data\Imported_Textures\2\DWF\SimLabDWFFile\Concrete.Cast-In-Place.Flat.Grey.1.jpeg");

            vertices = new VBO<Vector3>(obj.Vertices);
            indices = new VBO<int>(obj.Indices, BufferTarget.ElementArrayBuffer);
            uv = new VBO<Vector2>(obj.TextureVertices);
            normals = new VBO<Vector3>(obj.Normals);

            watch = System.Diagnostics.Stopwatch.StartNew();

            Glut.glutMainLoop();
        }


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




        private static void OnClose()
        {
            
            vertices.Dispose();
            indices.Dispose();
            uv.Dispose();
            concrete.Dispose();
            program.DisposeChildren = true;
            program.Dispose();
        }

        private static void OnDisplay()
        {

        }

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

            program["model_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(x,y,z)));

            Gl.BindBufferToShaderAttribute(vertices, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(normals, program, "vertexNormal");
            Gl.BindBufferToShaderAttribute(uv, program, "vertexUV");
            Gl.BindBuffer(indices);
            Gl.DrawElements(BeginMode.Triangles, indices.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Glut.glutSwapBuffers();
        }


        #region

        public static string VertexShader = @"
#version 130

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec2 vertexUV;

out vec3 normal;
out vec2 uv;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
    normal = normalize((model_matrix * vec4(vertexNormal, 0)).xyz);
    uv = vertexUV;
    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
}
";

        public static string FragmentShader = @"
#version 130

uniform sampler2D texture;
uniform vec3 light_direction;

in vec2 uv;
in vec3 normal;

out vec4 fragment;

void main(void)
{
    float diffuse = 0.015*max(dot(normal, light_direction), 0);
    float ambient = 0.7;
    float lighting = max(diffuse, ambient);
    vec4 sample = texture2D(texture, uv);

    fragment = vec4(sample.xyz * lighting, sample.a);
}
";
    }

    #endregion
}
