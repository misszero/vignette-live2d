using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Graphics.Cubism.Renderer
{
    public class CubismShaderManager
    {
        private IShader maskDrawingShader;
        private IShader unmaskedMeshDrawShader;
        private IShader maskedMeshDrawShader;
        private IShader unmaskedPremultAlphaMeshDrawShader;
        private IShader maskedPremultAlphaMeshDrawShader;

        public CubismShaderManager(ShaderManager shaderManager)
        {
            maskDrawingShader = shaderManager.Load("SetupMaskVertex", "SetupMaskFragment");
            unmaskedMeshDrawShader = shaderManager.Load("UnmaskedVertex", "UnmaskedFragment");
            maskedMeshDrawShader = shaderManager.Load("MaskedVertex", "MaskedFragment");
            unmaskedPremultAlphaMeshDrawShader = shaderManager.Load("UnmaskedVertex", "UnmaskedPremultipliedAlphaFragment");
            maskedPremultAlphaMeshDrawShader = shaderManager.Load("MaskedVertex", "MaskedPremultipliedAlphaFragment");
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