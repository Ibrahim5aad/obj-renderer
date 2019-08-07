using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace ObjRenderer
{
    class Object
    {
        public readonly Vector3[] Vertices;
        public readonly Vector2[] TextureVertices;
        public readonly Vector3[] Normals;
        public readonly int[] Indices;

        public Object(Vector3[] vertices, 
                        Vector2[] textureVertices, 
                        Vector3[] normals, 
                        int[] indices)
        {
            this.Vertices = vertices;
            this.TextureVertices = textureVertices;
            this.Normals = normals;
            this.Indices = indices;

        }
    }
}
