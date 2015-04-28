using System;
using System.IO;
using Cocos2D;
using Microsoft.Xna.Framework.Graphics;

namespace Spine
{
    public class SkeletonRenderer : CCNodeRGBA, ICCBlendProtocol
    {

        private static readonly int[] QuadTriangles = { 0, 1, 2, 2, 3, 0 };

        private bool _ownsSkeletonData;
        private Atlas _atlas;
        private PolygonBatch _batch;
        private float[] _worldVertices;

        public Skeleton skeleton;
        public Bone rootBone;
        public float timeScale;
        public bool debugSlots;
        public bool debugBones;
        public bool premultipliedAlpha;

        public override bool IsOpacityModifyRGB
        {
            get
            {
                return premultipliedAlpha;
            }
        }

        private void Initialize()
        {
            debugSlots = false;
            debugBones = false;
            timeScale = 1;

            _worldVertices = new float[1000]; // Max number of vertices per mesh.

            _batch = PolygonBatch.CreateWithCapacity(2000); // Max number of vertices and triangles per batch.

            //blendFunc.src = GL_ONE;
            //blendFunc.dst = GL_ONE_MINUS_SRC_ALPHA;
            BlendFunc = new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ONE_MINUS_SRC_ALPHA);

            SetOpacityModifyRGB(true);

            //SetShaderProgram(CCShaderCache::sharedShaderCache()->programForKey(kCCShader_PositionTextureColor));
            ScheduleUpdate();
        }

        protected SkeletonRenderer()
        {
            Initialize();
        }

        public SkeletonRenderer(SkeletonData skeletonData, bool ownsSkeletonData = false)
        {
            Initialize();
            SetSkeletonData(skeletonData, ownsSkeletonData);
        }

        public SkeletonRenderer(string skeletonDataFile, Atlas atlas, float scale = 0)
        {
            Initialize();

            SkeletonJson json = new SkeletonJson(atlas);
            json.Scale = scale;
            SkeletonData skeletonData = json.ReadSkeletonData(skeletonDataFile);
            //CCAssert(skeletonData, json->error ? json->error : "Error reading skeleton data.");
            //spSkeletonJson_dispose(json);

            SetSkeletonData(skeletonData, true);
        }

        public SkeletonRenderer(string skeletonDataFile, string atlasFile, float scale = 0)
        {
            Initialize();

            using (StreamReader reader = new StreamReader(CCFileUtils.GetFileStream(atlasFile)))
            {
                _atlas = new Atlas(reader, Path.GetDirectoryName(atlasFile), new CCTexture2DLoader());
            }
            //CCAssert(atlas, "Error reading atlas file.");

            SkeletonJson json = new SkeletonJson(_atlas);
            json.Scale = scale;

            using (StreamReader reader = new StreamReader(CCFileUtils.GetFileStream(skeletonDataFile)))
            {
                SkeletonData skeletonData = json.ReadSkeletonData(reader);
                SetSkeletonData(skeletonData, true);
            }

            //CCAssert(skeletonData, json->error ? json->error : "Error reading skeleton data.");
            //spSkeletonJson_dispose(json);

        }

        protected void SetSkeletonData(SkeletonData skeletonData, bool ownsSkeletonData)
        {
            skeleton = new Skeleton(skeletonData);
            rootBone = skeleton.Bones[0];
            this._ownsSkeletonData = ownsSkeletonData;
        }

        protected virtual CCTexture2D getTexture(RegionAttachment attachment)
        {
            return (CCTexture2D)((AtlasRegion)attachment.RendererObject).page.rendererObject;
        }

        protected virtual CCTexture2D getTexture(MeshAttachment attachment)
        {
            return (CCTexture2D)((AtlasRegion)attachment.RendererObject).page.rendererObject;
        }

        protected virtual CCTexture2D getTexture(SkinnedMeshAttachment attachment)
        {
            return (CCTexture2D)((AtlasRegion)attachment.RendererObject).page.rendererObject;
        }

        public static SkeletonRenderer CreateWithData(SkeletonData skeletonData, bool ownsSkeletonData = false)
        {
            SkeletonRenderer node = new SkeletonRenderer(skeletonData, ownsSkeletonData);
            return node;
        }

        public static SkeletonRenderer CreateWithFile(string skeletonDataFile, Atlas atlas, float scale = 0)
        {
            SkeletonRenderer node = new SkeletonRenderer(skeletonDataFile, atlas, scale);
            return node;
        }

        public static SkeletonRenderer CreateWithFile(string skeletonDataFile, string atlasFile, float scale = 0)
        {
            SkeletonRenderer node = new SkeletonRenderer(skeletonDataFile, atlasFile, scale);
            return node;
        }

        public override void Update(float deltaTime)
        {
            skeleton.Update(deltaTime * timeScale);
        }

        public override void Draw()
        {
            //CC_NODE_DRAW_SETUP();
            //ccGLBindVAO(0);

            CCColor3B nodeColor = Color;
            skeleton.R = nodeColor.R / (float)255;
            skeleton.G = nodeColor.G / (float)255;
            skeleton.B = nodeColor.B / (float)255;
            skeleton.A = DisplayedOpacity / (float)255;

            int blendMode = -1;
            CCColor4B color;
            float[] uvs = null;
            int verticesCount = 0;
            int[] triangles = null;
            int trianglesCount = 0;
            float r = 0, g = 0, b = 0, a = 0;
            for (int i = 0, n = skeleton.Slots.Count; i < n; i++)
            {
                Slot slot = skeleton.DrawOrder[i];
                if (slot.Attachment == null)
                    continue;
                CCTexture2D texture = null;
                if (slot.Attachment is RegionAttachment)
                {
                    RegionAttachment attachment = (RegionAttachment)slot.Attachment;
                    attachment.ComputeWorldVertices(slot.Bone, _worldVertices);
                    texture = getTexture(attachment);
                    uvs = attachment.UVs;
                    verticesCount = 8;
                    triangles = QuadTriangles;
                    trianglesCount = 6;
                    r = attachment.R;
                    g = attachment.G;
                    b = attachment.B;
                    a = attachment.A;
                }
                else if (slot.Attachment is MeshAttachment)
                {
                    MeshAttachment attachment = (MeshAttachment)slot.Attachment;
                    attachment.ComputeWorldVertices(slot, _worldVertices);
                    texture = getTexture(attachment);
                    uvs = attachment.UVs;
                    verticesCount = attachment.Vertices.Length;
                    triangles = attachment.Triangles;
                    trianglesCount = attachment.Triangles.Length;
                    r = attachment.R;
                    g = attachment.G;
                    b = attachment.B;
                    a = attachment.A;
                }
                else if (slot.Attachment is SkinnedMeshAttachment)
                {
                    SkinnedMeshAttachment attachment = (SkinnedMeshAttachment)slot.Attachment;
                    attachment.ComputeWorldVertices(slot, _worldVertices);
                    texture = getTexture(attachment);
                    uvs = attachment.UVs;
                    verticesCount = attachment.UVs.Length;
                    triangles = attachment.Triangles;
                    trianglesCount = attachment.Triangles.Length;
                    r = attachment.R;
                    g = attachment.G;
                    b = attachment.B;
                    a = attachment.A;
                }
                if (texture != null)
                {
                    if ((int)slot.Data.BlendMode != blendMode)
                    {
                        _batch.Flush();
                        blendMode = (int)slot.Data.BlendMode;
                        switch (slot.Data.BlendMode)
                        {
                            //UNDONE BlendMode
                            case BlendMode.additive:
                                //CCGLBlendFunc(premultipliedAlpha ? GL_ONE : GL_SRC_ALPHA, GL_ONE);
                                break;
                            case BlendMode.multiply:
                                //ccGLBlendFunc(GL_DST_COLOR, GL_ONE_MINUS_SRC_ALPHA);
                                break;
                            case BlendMode.screen:
                                //ccGLBlendFunc(GL_ONE, GL_ONE_MINUS_SRC_COLOR);
                                break;
                            default:
                                //ccGLBlendFunc(blendFunc.src, blendFunc.dst);
                                break;
                        }
                    }
                    color.A = (byte)(skeleton.A * slot.A * a * 255);
                    float multiplier = premultipliedAlpha ? color.A : 255;
                    color.R = (byte)(skeleton.R * slot.R * r * multiplier);
                    color.G = (byte)(skeleton.G * slot.G * g * multiplier);
                    color.B = (byte)(skeleton.B * slot.B * b * multiplier);
                    _batch.Add(texture, _worldVertices, uvs, verticesCount, triangles, trianglesCount, color);
                }
            }
            _batch.Flush();

            //if (debugSlots) {
            //    // Slots.
            //    ccDrawColor4B(0, 0, 255, 255);
            //    glLineWidth(1);
            //    CCPoint points[4];
            //    for (int i = 0, n = skeleton->slotsCount; i < n; i++) {
            //        spSlot* slot = skeleton->drawOrder[i];
            //        if (!slot->attachment || slot->attachment->type != SP_ATTACHMENT_REGION) continue;
            //        spRegionAttachment* attachment = (spRegionAttachment*)slot->attachment;
            //        spRegionAttachment_computeWorldVertices(attachment, slot->bone, worldVertices);
            //        points[0] = ccp(worldVertices[0], worldVertices[1]);
            //        points[1] = ccp(worldVertices[2], worldVertices[3]);
            //        points[2] = ccp(worldVertices[4], worldVertices[5]);
            //        points[3] = ccp(worldVertices[6], worldVertices[7]);
            //        ccDrawPoly(points, 4, true);
            //    }
            //}
            //if (debugBones) {
            //    // Bone lengths.
            //    glLineWidth(2);
            //    ccDrawColor4B(255, 0, 0, 255);
            //    for (int i = 0, n = skeleton->bonesCount; i < n; i++) {
            //        spBone *bone = skeleton->bones[i];
            //        float x = bone->data->length * bone->m00 + bone->worldX;
            //        float y = bone->data->length * bone->m10 + bone->worldY;
            //        ccDrawLine(ccp(bone->worldX, bone->worldY), ccp(x, y));
            //    }
            //    // Bone origins.
            //    ccPointSize(4);
            //    ccDrawColor4B(0, 0, 255, 255); // Root bone is blue.
            //    for (int i = 0, n = skeleton->bonesCount; i < n; i++) {
            //        spBone *bone = skeleton->bones[i];
            //        ccDrawPoint(ccp(bone->worldX, bone->worldY));
            //        if (i == 0) ccDrawColor4B(0, 255, 0, 255);
            //    }
            //}
        }

        public new CCRect BoundingBox()
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            float scaleX = ScaleX, scaleY = ScaleY;
            for (int i = 0; i < skeleton.Slots.Count; ++i)
            {
                Slot slot = skeleton.Slots[i];
                if (slot.Attachment == null)
                    continue;

                int verticesCount;
                if (slot.Attachment is RegionAttachment)
                {
                    RegionAttachment attachment = (RegionAttachment)slot.Attachment;
                    attachment.ComputeWorldVertices(slot.Bone, _worldVertices);
                    verticesCount = 8;
                }
                else if (slot.Attachment is MeshAttachment)
                {
                    MeshAttachment attachment = (MeshAttachment)slot.Attachment;
                    attachment.ComputeWorldVertices(slot, _worldVertices);
                    verticesCount = attachment.Vertices.Length;
                }
                else if (slot.Attachment is SkinnedMeshAttachment)
                {
                    SkinnedMeshAttachment attachment = (SkinnedMeshAttachment)slot.Attachment;
                    attachment.ComputeWorldVertices(slot, _worldVertices);
                    verticesCount = attachment.UVs.Length;
                }
                else
                    continue;

                for (int ii = 0; ii < verticesCount; ii += 2)
                {
                    float x = _worldVertices[ii] * scaleX, y = _worldVertices[ii + 1] * scaleY;
                    minX = Math.Min(minX, x);
                    minY = Math.Min(minY, y);
                    maxX = Math.Max(maxX, x);
                    maxY = Math.Max(maxY, y);
                }
            }
            CCPoint position = Position;
            return new CCRect(position.X + minX, position.Y + minY, maxX - minX, maxY - minY);
        }

        // --- Convenience methods for common Skeleton_* functions.
        public void UpdateWorldTransform()
        {
            skeleton.UpdateWorldTransform();
        }

        public void SetToSetupPose()
        {
            skeleton.SetToSetupPose();
        }

        public void SetBonesToSetupPose()
        {
            skeleton.SetBonesToSetupPose();
        }

        public void SetSlotsToSetupPose()
        {
            skeleton.SetSlotsToSetupPose();
        }

        /* Returns 0 if the bone was not found. */
        public Bone FindBone(string boneName)
        {
            return skeleton.FindBone(boneName);
        }

        /* Returns 0 if the slot was not found. */
        public Slot FindSlot(string slotName)
        {
            return skeleton.FindSlot(slotName);
        }

        /* Sets the skin used to look up attachments not found in the SkeletonData defaultSkin. Attachments from the new skin are
         * attached if the corresponding attachment from the old skin was attached. If there was no old skin, each slot's setup mode
         * attachment is attached from the new skin. Returns false if the skin was not found.
         * @param skin May be 0.*/
        public bool SetSkin(string skinName)
        {
            skeleton.SetSkin(skinName);
            return true;
        }

        /* Returns 0 if the slot or attachment was not found. */
        public Attachment GetAttachment(string slotName, string attachmentName)
        {
            return skeleton.GetAttachment(slotName, attachmentName);
        }

        /* Returns false if the slot or attachment was not found. */
        public bool SetAttachment(string slotName, string attachmentName)
        {
            skeleton.SetAttachment(slotName, attachmentName);
            return true;
        }

        // --- BlendProtocol
        public virtual void SetOpacityModifyRGB(bool value)
        {
            premultipliedAlpha = value;
        }


        //public CC_PROPERTY(cocos2d::ccBlendFunc, blendFunc, BlendFunc);
        public CCBlendFunc BlendFunc { get; set; }
    }
}