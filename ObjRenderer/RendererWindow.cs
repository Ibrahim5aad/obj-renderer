using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ObjRenderer
{
  public class RendererWindow : GameWindow
  {
    public RendererWindow() : base(new GameWindowSettings(), new NativeWindowSettings())
    {
    }
     
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
    }
  }
}
