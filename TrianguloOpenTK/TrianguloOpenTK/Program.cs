using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace TrianguloOpenTK
{
    
    public class Game : GameWindow
    {
        int VertexBufferObject;
        int VertexArrayObject;
        int Handle;



        float[] vertices = {
         -0.5f,-0.5f,0.0f,
         0.5f,-0.5f,0.0f,
         0.0f,0.5f,0.0f
        };

        public Game(int ancho, int alto, string titulo) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (ancho, alto), Title = titulo }) { }





        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, @"
                #version 330 core
                layout (location=0) in vec3 aPosition;
                
                void main()
                {
                    gl_Position = vec4(aPosition,1.0);
                }
            ");
            GL.CompileShader(vertexShader);

      


            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, @"
                #version 330 core
                out vec4 FragColor;
                
                void main()
                {
                    FragColor = vec4(1.0f,0.5f,0.2f,1.0f);
                }
            ");
            GL.CompileShader(FragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);




        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(Handle);
            GL.BindVertexArray(VertexArrayObject);

            GL.DrawArrays(PrimitiveType.Triangles,0,3);

            Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
                Close();

        }
    }

   

    class Program
    {

        static void Main(string[] args)
        {
            using (Game game = new Game(1200, 1200, "Triangulo - Progamacion Grafica"))
            {
                game.Run();
            }
        }
    }
}
