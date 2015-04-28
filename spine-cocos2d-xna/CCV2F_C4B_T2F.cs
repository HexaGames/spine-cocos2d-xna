using Cocos2D;
using Microsoft.Xna.Framework.Graphics;

namespace Spine
{
    /// <summary>
    /// a Point with a vertex point, a tex coord point and a color 4B
    /// </summary>
    public struct CCV2F_C4B_T2F : IVertexType
    {
        /// <summary>
        /// vertices (2F)
        /// </summary>
        public CCVertex2F Vertices;			// 8 bytes

        /// <summary>
        /// colors (4B)
        /// </summary>
        public CCColor4B Colors;				// 4 bytes

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public CCTex2F TexCoords;			// 8 byts

        public static readonly VertexDeclaration VertexDeclaration;

        static CCV2F_C4B_T2F()
        {
            var elements = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }
}