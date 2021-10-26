using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ObjRenderer
{
     
    /// <summary>
    /// Class ObjLoader.
    /// </summary>
    public class ObjLoader
    {

        #region Fields

        private static ObjLoader _instance;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the loader instance.
        /// </summary>
        /// <value>The loader instance.</value>
        public static ObjLoader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ObjLoader();

                return _instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the specified file given its path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Object.</returns>
        /// <exception cref="System.IO.FileNotFoundException">File does not exist.</exception>
        public Object Load(string filePath)
        {

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textureVertices = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> Indices = new List<int>();
            Vector3[] verticesArray = null;
            Vector2[] textureArray = null;
            Vector3[] normalsArray = null;
            int[] indicesArray = null;
            string l;



            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File does not exist.");
            }

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    l = sr.ReadLine();
                    List<string> line = new List<string>(l.ToLower().Split(' '));

                    if (line.Count == 0)
                        continue;
                    else if (line[0] == "v")
                        vertices.Add(new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3])));
                    else if (line[0] == "vt")
                        textureVertices.Add(new Vector2(float.Parse(line[1]), float.Parse(line[2])));
                    else if (line[0] == "vn")
                        normals.Add(new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3])));
                    else if (line[0] == "f")
                    {
                        textureArray = new Vector2[vertices.Count];
                        normalsArray = new Vector3[vertices.Count];
                        break;
                    }
                }

                l = sr.ReadLine();

                while (l != null)
                {
                    if (l.Split(' ')[0] != "f")
                    {
                        l = sr.ReadLine();
                        continue;
                    }
                    string[] currLine = l.Split(' ');
                    string[] vertex1 = currLine[1].Split('/');
                    string[] vertex2 = currLine[2].Split('/');
                    string[] vertex3 = currLine[3].Split('/');

                    processVertices(vertex1, Indices, textureVertices, normals, textureArray, normalsArray);
                    processVertices(vertex2, Indices, textureVertices, normals, textureArray, normalsArray);
                    processVertices(vertex3, Indices, textureVertices, normals, textureArray, normalsArray);
                    l = sr.ReadLine();
                }

                sr.Close();

                verticesArray = new Vector3[vertices.Count];
                indicesArray = new int[Indices.Count];
                int vertexPointer = 0;
                foreach (Vector3 vec in vertices)
                {
                    verticesArray[vertexPointer++] = vec;
                }

                for (int i = 0; i < Indices.Count; i++)
                {
                    indicesArray[i] = Indices[i];
                }
            }

            return new Object(verticesArray, textureArray, normalsArray, indicesArray);
        }


        /// <summary>
        /// Processes the vertices.
        /// </summary>
        /// <param name="vertexData">The vertex data.</param>
        /// <param name="indices">The indices.</param>
        /// <param name="textures">The textures.</param>
        /// <param name="normals">The normals.</param>
        /// <param name="textureArray">The texture array.</param>
        /// <param name="normalsArray">The normals array.</param>
        private void processVertices(string[] vertexData,
                                       List<int> indices,
                                       List<Vector2> textures,
                                       List<Vector3> normals,
                                       Vector2[] textureArray,
                                       Vector3[] normalsArray)
        {
            int currentVertexPointer = int.Parse(vertexData[0]) - 1;
            indices.Add(currentVertexPointer);
            Vector2 currentTexture = textures[int.Parse(vertexData[1]) - 1];
            textureArray[currentVertexPointer] = currentTexture;
            Vector3 currentNormal = normals[int.Parse(vertexData[2]) - 1];
            normalsArray[currentVertexPointer] = currentNormal;

        }
         
        #endregion

    }


}
