using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Graphics.Cubism
{
    public class CubismShaderManager
    {
        private IShader maskDrawingShader;
        private IShader unmaskedMeshDrawShader;
        private IShader maskedMeshDrawShader;
        private IShader unmaskedPremultAlphaMeshDrawShader;
        private IShader maskedPremultAlphaMeshDrawShader;
        private bool disposed = false;

        public CubismShaderManager(ShaderManager shaderManager)
        {
            maskDrawingShader = shaderManager.Load("SetupMaskVertex", "SetupMaskFragment");
            unmaskedMeshDrawShader = shaderManager.Load("UnmaskedVertex", "UnmaskedFragment");
            maskedMeshDrawShader = shaderManager.Load("MaskedVertex", "MaskedFragment");
            unmaskedPremultAlphaMeshDrawShader = shaderManager.Load("UnmaskedPremultipliedAlphaVertex", "UnmaskedPremultipliedAlphaFragment");
            maskedPremultAlphaMeshDrawShader = shaderManager.Load("MaskedPremultipliedAlphaVertex", "MaskedPremultipliedAlphaFragment");
        }

        public IShader GetDrawMaskShader() => maskDrawingShader;
        public IShader GetDrawMeshShader(bool useClippingMask, bool usePremultipliedAlpha)
        {
            if (!useClippingMask)
                return (!usePremultipliedAlpha) ? unmaskedMeshDrawShader : unmaskedPremultAlphaMeshDrawShader;
            else
                return (!usePremultipliedAlpha) ? maskedMeshDrawShader : maskedPremultAlphaMeshDrawShader;
        }
    }
}