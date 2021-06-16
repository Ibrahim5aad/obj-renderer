using System.Numerics;

namespace ObjRenderer
{
  /// <summary>
  /// Class Object.
  /// </summary>
  public class Object
  {

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Object"/> class.
    /// </summary>
    /// <param name="vertices">The vertices.</param>
    /// <param name="textureVertices">The texture vertices.</param>
    /// <param name="normals">The normals.</param>
    /// <param name="indices">The indices.</param>
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
     
    #endregion

    #region Properties

    /// <summary>
    /// Gets the vertices.
    /// </summary>
    /// <value>The vertices.</value>
    public Vector3[] Vertices { get; }

    /// <summary>
    /// Gets the texture vertices.
    /// </summary>
    /// <value>The texture vertices.</value>
    public Vector2[] TextureVertices { get; }

    /// <summary>
    /// Gets the normals.
    /// </summary>
    /// <value>The normals.</value>
    public Vector3[] Normals { get; }

    /// <summary>
    /// Gets the indices.
    /// </summary>
    /// <value>The indices.</value>
    public int[] Indices { get; }

    #endregion

  }
}
