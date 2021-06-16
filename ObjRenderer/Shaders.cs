namespace ObjRenderer
{
  /// <summary>
  /// Class Shaders.
  /// </summary>
  static class Shaders
  {

    /// <summary>
    /// The vertex shader
    /// </summary>
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


    /// <summary>
    /// The fragment shader
    /// </summary>
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
}
