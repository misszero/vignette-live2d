using System;
using System.Collections.Generic;
using System.IO;
using CubismFramework;
using MathNet.Numerics.LinearAlgebra.Single;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.OpenGL;
using osuTK.Graphics.OpenGL;

namespace osu.Framework.Live2D.Cubism
{
    public class CubismRenderer : ICubismRenderer, IDisposable
    {
        private const int clipping_mask_size = 256;
        private List<CubismClippingMask> ClippingMasks = new List<CubismClippingMask>();
        private List<CubismTexture> Textures = new List<CubismTexture>();

        private bool isDisposed = false;

        ~CubismRenderer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public override ICubismClippingMask CreateClippingMask()
        {
            var mask = new CubismClippingMask(clipping_mask_size, clipping_mask_size);
            ClippingMasks.Add(mask);
            return mask;
        }

        public override ICubismTexture CreateTexture(byte[] textureBytes)
        {
            var upload = new TextureUpload(new MemoryStream(textureBytes));
            var texture = new Texture(upload.Width, upload.Height);
            texture.SetData(upload);

            var cubismTexture = new CubismTexture(texture);
            Textures.Add(cubismTexture);

            return cubismTexture;
        }

        public override void DisposeClippingMask(ICubismClippingMask clippingMask)
        {
            var mask = (CubismClippingMask)clippingMask;
            mask.Dispose();
            ClippingMasks.Remove(mask);
        }

        public override void DisposeTexture(ICubismTexture texture)
        {
            var tex = (CubismTexture)texture;
            tex.Dispose();
            Textures.Remove(tex);
        }

        public override void DrawMask(ICubismTexture texture, float[] vertex_buffer, float[] uv_buffer, short[] index_buffer, ICubismClippingMask clipping_mask, Matrix clipping_matrix, bool use_culling)
        {
            
        }

        public override void DrawMesh(ICubismTexture texture, float[] vertex_buffer, float[] uv_buffer, short[] index_buffer, ICubismClippingMask clipping_mask, Matrix clipping_matrix, BlendModeType blend_mode, bool use_culling, double opacity)
        {

        }

        public override void StartDrawingModel(float[] model_color, Matrix mvp_matrix)
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            isDisposed = true;
        }
    }
}