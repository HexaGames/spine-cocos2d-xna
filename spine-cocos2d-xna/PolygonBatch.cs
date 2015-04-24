using System.Diagnostics;
using Cocos2D;

namespace Spine
{
    public class PolygonBatch
    {
        //  TODO  GLushort* triangles;
        private int[] triangles;
        private int _capacity;
        private int verticesCount;
        private int trianglesCount;
        private CCV2F_C4B_T2F[] vertices;
        private CCTexture2D texture;

        public PolygonBatch() { }

        public static PolygonBatch CreateWithCapacity(int capacity)
        {
            var batch = new PolygonBatch();
            batch.initWithCapacity(capacity);
            return batch;
        }

        public bool initWithCapacity(int capacity)
        {
            // 32767 is max index, so 32767 / 3 - (32767 / 3 % 3) = 10920.
            Debug.Assert(capacity <= 10920, "capacity cannot be > 10920");
            Debug.Assert(capacity >= 0, "capacity cannot be < 0");

            this._capacity = capacity;
            vertices = new CCV2F_C4B_T2F[capacity];
            //triangles = MALLOC(GLushort, capacity * 3);
            triangles = new int[capacity * 3];
            return true;
        }

        public void add(CCTexture2D addTexture, float[] addVertices, float[] uvs, int addVerticesCount, int[] addTriangles, int addTrianglesCount, CCColor4B color)
        {
            if (addTexture != texture
                || verticesCount + (addVerticesCount >> 1) > _capacity
                || trianglesCount + addTrianglesCount > _capacity * 3)
            {
                this.flush();
                texture = addTexture;
            }

            for (int i = 0; i < addTrianglesCount; ++i, ++trianglesCount)
                triangles[trianglesCount] = addTriangles[i] + verticesCount;

            for (int i = 0; i < addVerticesCount; i += 2, ++verticesCount)
            {
                var vertex = vertices[verticesCount - 1];
                vertex.Vertices.X = addVertices[i];
                vertex.Vertices.Y = addVertices[i + 1];
                vertex.Colors = color;
                vertex.TexCoords.U = uvs[i];
                vertex.TexCoords.V = uvs[i + 1];
            }
        }

        public void flush()
        {
            if (verticesCount == 0)
                return;

            //ccGLBindTexture2D(texture->getName());
            //glEnableVertexAttribArray(kCCVertexAttrib_Position);
            //glEnableVertexAttribArray(kCCVertexAttrib_Color);
            //glEnableVertexAttribArray(kCCVertexAttrib_TexCoords);
            //glVertexAttribPointer(kCCVertexAttrib_Position, 2, GL_FLOAT, GL_FALSE, sizeof(ccV2F_C4B_T2F), &vertices[0].vertices);
            //glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, sizeof(ccV2F_C4B_T2F), &vertices[0].colors);
            //glVertexAttribPointer(kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, sizeof(ccV2F_C4B_T2F), &vertices[0].texCoords);

            //glDrawElements(GL_TRIANGLES, trianglesCount, GL_UNSIGNED_SHORT, triangles);

            verticesCount = 0;
            trianglesCount = 0;
        }
    }
}