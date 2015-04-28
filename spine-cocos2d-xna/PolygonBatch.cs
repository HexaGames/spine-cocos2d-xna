using System.Diagnostics;
using Cocos2D;
using Microsoft.Xna.Framework.Graphics;

namespace Spine
{
    public class PolygonBatch
    {
        //private CCV2F_C4B_T2F[] _vertices;
        private int[] _triangles; //  TODO  GLushort* triangles;
        private int _verticesCount;
        private int _trianglesCount;

        private CCTexture2D _texture;
        private int _capacity;

        public PolygonBatch() { }

        public static PolygonBatch CreateWithCapacity(int capacity)
        {
            var batch = new PolygonBatch();
            batch.InitWithCapacity(capacity);
            return batch;
        }

        public bool InitWithCapacity(int capacity)
        {
            // 32767 is max index, so 32767 / 3 - (32767 / 3 % 3) = 10920.
            Debug.Assert(capacity <= 10920, "capacity cannot be > 10920");
            Debug.Assert(capacity >= 0, "capacity cannot be < 0");

            this._capacity = capacity;
            _vertices = new CCV2F_C4B_T2F[capacity];
            for (int i = 0; i < capacity; i++)
            {
                _vertices[i] = new CCV2F_C4B_T2F();
            }

            //triangles = MALLOC(GLushort, capacity * 3);
            _triangles = new int[capacity * 3];


            _vertexBuffer = new CCVertexBuffer<CCV2F_C4B_T2F>(capacity, BufferUsage.WriteOnly);
            _indexBuffer = new CCIndexBuffer<ushort>(capacity * 3, BufferUsage.WriteOnly);

            _vertices = _vertexBuffer.Data.Elements;
            _indices = _indexBuffer.Data.Elements;

            return true;
        }

        public void Add(CCTexture2D addTexture, float[] addVertices, float[] uvs, int addVerticesCount, int[] addTriangles, int addTrianglesCount, CCColor4B color)
        {
            if (addTexture != _texture
                || _verticesCount + (addVerticesCount >> 1) > _capacity
                || _trianglesCount + addTrianglesCount > _capacity * 3)
            {
                this.Flush();
                _texture = addTexture;
            }

            _indexBuffer.Count = _trianglesCount + addTrianglesCount;
            for (int i = 0; i < addTrianglesCount; i++, _trianglesCount++)
            {
                _triangles[_trianglesCount] = addTriangles[i] + _verticesCount;
                _indices[_trianglesCount] = (ushort) (addTriangles[i] + _verticesCount);
            }
            _indexBuffer.UpdateBuffer();

            _vertexBuffer.Count = _verticesCount + addVerticesCount;
            for (int i = 0; i < addVerticesCount; i += 2, _verticesCount++)
            {
                var vertex = _vertices[_verticesCount];
                _vertices[_verticesCount].Vertices.X = addVertices[i];
                _vertices[_verticesCount].Vertices.Y = addVertices[i + 1];
                _vertices[_verticesCount].Colors = color;
                _vertices[_verticesCount].TexCoords.U = uvs[i];
                _vertices[_verticesCount].TexCoords.V = uvs[i + 1];
            }
            _vertexBuffer.UpdateBuffer();
        }

        private CCVertexBuffer<CCV2F_C4B_T2F> _vertexBuffer;
        private CCIndexBuffer<ushort> _indexBuffer;
        private CCV2F_C4B_T2F[] _vertices;
        private ushort[] _indices;

        public void Flush()
        {
            if (_verticesCount == 0)
                return;

            //ccGLBindTexture2D(texture->getName());
            CCDrawManager.BindTexture(_texture);

            //glEnableVertexAttribArray(kCCVertexAttrib_Position);
            //glEnableVertexAttribArray(kCCVertexAttrib_Color);
            //glEnableVertexAttribArray(kCCVertexAttrib_TexCoords);

            //glVertexAttribPointer(kCCVertexAttrib_Position, 2, GL_FLOAT, GL_FALSE, sizeof(ccV2F_C4B_T2F), &vertices[0].vertices);
            //glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, sizeof(ccV2F_C4B_T2F), &vertices[0].colors);
            //glVertexAttribPointer(kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, sizeof(ccV2F_C4B_T2F), &vertices[0].texCoords);

            //glDrawElements(GL_TRIANGLES, trianglesCount, GL_UNSIGNED_SHORT, triangles);

            bool save = CCDrawManager.VertexColorEnabled;
            CCDrawManager.VertexColorEnabled = false;
            CCDrawManager.DrawBuffer(_vertexBuffer, _indexBuffer, 0, _trianglesCount / 3);
            CCDrawManager.VertexColorEnabled = save;

            _verticesCount = 0;
            _trianglesCount = 0;
        }
    }
}