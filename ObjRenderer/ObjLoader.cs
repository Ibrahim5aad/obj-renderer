using System;
using System.Collections.Generic;
using System.IO;
using OpenGL;

namespace ObjRenderer
{
    class ObjLoader
    {
        public static Object Loader(string filePath)
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
                    {
                        vertices.Add(new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3])));
                    }

                    else if (line[0] == "vt")
                    {
                        textureVertices.Add(new Vector2(float.Parse(line[1]), float.Parse(line[2])));
                    }

                    else if (line[0] == "vn")
                    {
                        normals.Add(new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3])));
                    }

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
                foreach(Vector3 vec in vertices)
                {
                    verticesArray[vertexPointer++] = vec;
                }

                for(int i = 0; i < Indices.Count; i++)
                {
                    indicesArray[i] = Indices[i];
                }
            }

            return new Object(verticesArray, textureArray, normalsArray, indicesArray);
        }

        private static void processVertices(string[] vertexData,
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

    }

   
}
