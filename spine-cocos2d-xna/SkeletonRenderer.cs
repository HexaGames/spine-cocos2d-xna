using Cocos2D;

namespace Spine
{
    public class SkeletonRenderer: CCNodeRGBA, ICCBlendProtocol
    {
        private bool ownsSkeletonData;
        private Atlas atlas;
        private PolygonBatch batch;
        private float worldVertices;
        
        
	public Skeleton skeleton;
	public Bone rootBone;
public	float timeScale;
public	bool debugSlots;
public	bool debugBones;
public	bool premultipliedAlpha;

        private void initialize()
        {
        }

        protected SkeletonRenderer()
        {
            
        }

        protected void SetSkeletonData(SkeletonData skeletonData, bool ownsSkeletonData)
        {
            
        }

        protected virtual CCTexture2D getTexture(RegionAttachment attachment)
        {
            
        }

        protected virtual CCTexture2D getTexture(MeshAttachment attachment)
        {
            
        }

        protected virtual CCTexture2D getTexture(SkinnedMeshAttachment attachment)
        {
            
        }

        public static SkeletonRenderer createWithData(SkeletonData skeletonData, bool ownsSkeletonData = false)
        {
            
        }

        public static SkeletonRenderer createWithFile(string skeletonDataFile, Atlas atlas, float scale = 0)
        {
            
        }

        public static SkeletonRenderer createWithFile(string skeletonDataFile, string atlasFile, float scale = 0)
        {
            
        }

        public SkeletonRenderer(SkeletonData skeletonData, bool ownsSkeletonData = false)
        {
            
        }

        public SkeletonRenderer(string skeletonDataFile, Atlas atlas, float scale = 0)
        {
            
        }

        public SkeletonRenderer(string skeletonDataFile, string atlasFile, float scale = 0)
        {
            
        }

       public  virtual void Update(float deltaTime)
        {
            
        }

        public virtual void Draw()
        {
            
        }

        public virtual CCRect BoundingBox()
        {
            
        }

	// --- Convenience methods for common Skeleton_* functions.
        public void UpdateWorldTransform()
        {
            
        }

        public void SetToSetupPose()
        {
            
        }

        public void SetBonesToSetupPose()
        {
            
        }

        public void SetSlotsToSetupPose()
        {
            
        }

	/* Returns 0 if the bone was not found. */
        public Bone FindBone(string boneName)
        {
            
        }

	/* Returns 0 if the slot was not found. */
        public Slot findSlot(string slotName)
        {
            
        }
	
	/* Sets the skin used to look up attachments not found in the SkeletonData defaultSkin. Attachments from the new skin are
	 * attached if the corresponding attachment from the old skin was attached. If there was no old skin, each slot's setup mode
	 * attachment is attached from the new skin. Returns false if the skin was not found.
	 * @param skin May be 0.*/
        public bool SetSkin(string skinName)
        {
            
        }

	/* Returns 0 if the slot or attachment was not found. */
        public Attachment getAttachment(string slotName, string attachmentName)
        {
            
        }

	/* Returns false if the slot or attachment was not found. */
        public bool SetAttachment(string slotName, string attachmentName)
        {
            
        }

	// --- BlendProtocol
	public CC_PROPERTY(cocos2d::ccBlendFunc, blendFunc, BlendFunc);
	public virtual void setOpacityModifyRGB (bool value);
	public virtual bool isOpacityModifyRGB ();



        public CCBlendFunc BlendFunc
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}