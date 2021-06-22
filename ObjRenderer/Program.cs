using OpenTK.Windowing.Desktop;

namespace ObjRenderer
{
  class Program
  {
    static void Main(string[] args)
    {

      using (RendererWindow wnd = new RendererWindow())
      {
        wnd.RenderFrequency = 60.0;
        wnd.Run();
      }



    }
  }
}
