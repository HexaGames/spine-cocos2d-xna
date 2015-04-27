using System.Diagnostics;
using Cocos2D;

namespace Spine
{
    public class SkeletonAnimation : SkeletonRenderer
    {
        private SkeletonRenderer super;
        private bool ownsAnimationStateData;

        public AnimationState state;

        //public StartListener startListener;
        //public EndListener endListener;
        //public CompleteListener completeListener;
        //public EventListener eventListener;
        //public void setStartListener (spTrackEntry* entry, StartListener listener);
        //public void setEndListener (spTrackEntry* entry, EndListener listener);
        //public void setCompleteListener (spTrackEntry* entry, CompleteListener listener);
        //public void setEventListener (spTrackEntry* entry, EventListener listener);


        void Initialize()
        {
            ownsAnimationStateData = true;
            state = new AnimationState(new AnimationStateData(skeleton.Data));
            //state.rendererObject = this;
            //state->listener = animationCallback;

            //_spAnimationState* stateInternal = (_spAnimationState*)state;
            //stateInternal->disposeTrackEntry = disposeTrackEntry;
        }

        protected SkeletonAnimation()
        {

        }

        public SkeletonAnimation(SkeletonData skeletonData)
            : base(skeletonData)
        {
            Initialize();
        }

        public SkeletonAnimation(string skeletonDataFile, Atlas atlas, float scale = 0)
            : base(skeletonDataFile, atlas, scale)
        {
            Initialize();
        }

        //public SkeletonAnimation(string skeletonDataFile, string atlasFile, float scale = 0)
        //    : base(skeletonDataFile, atlasFile, scale)
        //{
        //    Initialize();
        //}

        public static SkeletonAnimation CreateWithData(SkeletonData skeletonData)
        {
            SkeletonAnimation node = new SkeletonAnimation(skeletonData);
            return node;
        }

        public new static SkeletonAnimation CreateWithFile(string skeletonDataFile, Atlas atlas, float scale = 0)
        {
            SkeletonAnimation node = new SkeletonAnimation(skeletonDataFile, atlas, scale);
            return node;
        }

        //public static SkeletonAnimation CreateWithFile(string skeletonDataFile, string atlasFile, float scale = 0)
        //{
        //    SkeletonAnimation node = new SkeletonAnimation(skeletonDataFile, atlasFile, scale);
        //    return node;
        //}

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            deltaTime *= timeScale;
            state.Update(deltaTime);
            state.Apply(skeleton);
            skeleton.UpdateWorldTransform();
        }

        public void SetAnimationStateData(AnimationStateData stateData)
        {
            Debug.Assert(stateData != null, "stateData cannot be null.");

            //if (ownsAnimationStateData) spAnimationStateData_dispose(state->data);
            //spAnimationState_dispose(state);

            ownsAnimationStateData = false;
            state = new AnimationState(stateData);
            //state->rendererObject = this;
            //state->listener = animationCallback;
        }

        public void SetMix(string fromAnimation, string toAnimation, float duration)
        {
            state.Data.SetMix(fromAnimation, toAnimation, duration);
        }

        public TrackEntry SetAnimation(int trackIndex, string name, bool loop)
        {
            Animation animation = skeleton.Data.FindAnimation(name);
            if (animation == null)
            {
                CCLog.Log("Spine: Animation not found: %s", name);
                return null;
            }
            return state.SetAnimation(trackIndex, animation, loop);
        }

        public TrackEntry AddAnimation(int trackIndex, string name, bool loop, float delay = 0)
        {
            Animation animation = skeleton.Data.FindAnimation(name);
            if (animation == null)
            {
                CCLog.Log("Spine: Animation not found: %s", name);
                return null;
            }
            return state.AddAnimation(trackIndex, animation, loop, delay);
        }

        public TrackEntry GetCurrent(int trackIndex = 0)
        {
            return state.GetCurrent(trackIndex);
        }

        public void ClearTracks()
        {
            state.ClearTracks();
        }

        public void ClearTrack(int trackIndex = 0)
        {
            state.ClearTrack(trackIndex);
        }

        //public virtual void onAnimationStateEvent (int trackIndex, EventType type, Event event, int loopCount)
        //    {

        //    }
        //public virtual void onTrackEntryEvent (int trackIndex, EventType type, Event event, int loopCount)
        //    {

        //    }

    }
}