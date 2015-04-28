using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Cocos2D;
using Microsoft.Xna.Framework.Graphics;
using Spine;


namespace tests
{
    public class AppDelegate : CCApplication
    {
        public AppDelegate(Game game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            s_pSharedApplication = this;
            CCDrawManager.InitializeDisplay(game, graphics, DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft);

#if WINDOWS_PHONE8
            HandleMediaStateAutomatically = false; // Bug in MonoGame - https://github.com/Cocos2DXNA/cocos2d-xna/issues/325
#endif
            game.Window.AllowUserResizing = true;
            graphics.PreferMultiSampling = false;

#if WINDOWS || WINDOWSGL || WINDOWSDX || MACOS
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
#endif
        }

        /// <summary>
        /// Implement for initialize OpenGL instance, set source path, etc...
        /// </summary>
        public override bool InitInstance()
        {
            return base.InitInstance();
        }

        /// <summary>
        ///  Implement CCDirector and CCScene init code here.
        /// </summary>
        /// <returns>
        ///  true  Initialize success, app continue.
        ///  false Initialize failed, app terminate.
        /// </returns>
        public override bool ApplicationDidFinishLaunching()
        {
            //initialize director
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.SetOpenGlView();

            //CCSpriteFontCache.FontScale = 0.6f;
            //CCSpriteFontCache.RegisterFont("arial", 12, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 38, 50, 64);
            //CCSpriteFontCache.RegisterFont("MarkerFelt", 16, 18, 22);
            //CCSpriteFontCache.RegisterFont("MarkerFelt-Thin", 12, 18);
            //CCSpriteFontCache.RegisterFont("Paint Boy", 26);
            //CCSpriteFontCache.RegisterFont("Schwarzwald Regular", 26);
            //CCSpriteFontCache.RegisterFont("Scissor Cuts", 26);
            //CCSpriteFontCache.RegisterFont("A Damn Mess", 26);
            //CCSpriteFontCache.RegisterFont("Abberancy", 26);
            //CCSpriteFontCache.RegisterFont("Abduction", 26);

            // turn on display FPS
            pDirector.DisplayStats = true;
            // set FPS. the default value is 1.0/60 if you don't call this
            pDirector.AnimationInterval = 1.0 / 60;
            CCSize designSize = new CCSize(960, 640);

            if (CCDrawManager.FrameSize.Height > 320)
            {
                CCSize resourceSize = new CCSize(960, 640);
                CCContentManager.SharedContentManager.SearchPaths.Add("hd");
                /*
                CCContentManager.SharedContentManager.SearchPaths.Add("hd/extensions");
                CCContentManager.SharedContentManager.SearchPaths.Add("extensions");
                CCContentManager.SharedContentManager.SearchPaths.Add("hd/animations");
                CCContentManager.SharedContentManager.SearchPaths.Add("animations");
                CCContentManager.SharedContentManager.SearchPaths.Add("hd/TileMaps");
                CCContentManager.SharedContentManager.SearchPaths.Add("TileMaps");
                CCContentManager.SharedContentManager.SearchPaths.Add("hd/ccb");
                CCContentManager.SharedContentManager.SearchPaths.Add("ccb");
                CCContentManager.SharedContentManager.SearchPaths.Add("hd/Images");
                CCContentManager.SharedContentManager.SearchPaths.Add("Particles");
                CCContentManager.SharedContentManager.SearchPaths.Add("Sounds");
                CCContentManager.SharedContentManager.SearchPaths.Add("TileMaps");
                 */
                pDirector.ContentScaleFactor = resourceSize.Height / designSize.Height;
            }

            CCDrawManager.SetDesignResolutionSize(designSize.Width, designSize.Height, CCResolutionPolicy.ShowAll);

            /*
            #if WINDOWS || WINDOWSGL
                        CCDrawManager.SetDesignResolutionSize(1280, 768, CCResolutionPolicy.ExactFit);
            #else
                        CCDrawManager.SetDesignResolutionSize(800, 480, CCResolutionPolicy.ShowAll);
                        //CCDrawManager.SetDesignResolutionSize(480, 320, CCResolutionPolicy.ShowAll);
            #endif
            */

            // create a scene. it's an autorelease object
            CCScene pScene = new CCScene();

            /*           
            CCScene pScene = CCScene.node();
            var pLayer = Box2DView.viewWithEntryID(0);
            pLayer.scale = 10;
            pLayer.anchorPoint = new CCPoint(0, 0);
            pLayer.position = new CCPoint(CCDirector.sharedDirector().getWinSize().width / 2, CCDirector.sharedDirector().getWinSize().height / 4);
            */



            //String name = "spineboy";
            //String name = "goblins-mesh";
            bool binaryData = false;

            String name = "raptor";
            //bool binaryData = true;

            float scale = 1;
            if (name == "spineboy") scale = 0.6f;
            if (name == "raptor") scale = 0.5f;

            var skeletonAnimation = new SkeletonAnimation(name + ".json", name + ".atlas", scale);
            //skeletonAnimation.premultipliedAlpha = true;

            //SkeletonData skeletonData;
            //if (binaryData)
            //{
            //    SkeletonBinary binary = new SkeletonBinary(atlas);
            //    binary.Scale = scale;
            //    //skeletonData = binary.ReadSkeletonData(assetsFolder + name + ".skel");
            //}
            //else
            //{
            //    SkeletonJson json = new SkeletonJson(atlas);
            //    json.Scale = scale;
            //    skeletonData = json.ReadSkeletonData(assetsFolder + name + ".json");
            //}
            //skeleton = new Skeleton(skeletonData);
            if (name == "goblins-mesh")
                skeletonAnimation.SetSkin("goblin");

            //// Define mixing between animations.
            //AnimationStateData stateData = new AnimationStateData(skeleton.Data);
            //state = new AnimationState(stateData);

            //skeletonAnimation.NodeToWorldTransform();
            //skeletonAnimation.SetSlotsToSetupPose();
            //skeletonAnimation.UpdateWorldTransform();

            if (name == "spineboy")
            {
                //stateData.SetMix("run", "jump", 0.2f);
                //stateData.SetMix("jump", "run", 0.4f);

                //// Event handling for all animations.
                //state.Start += Start;
                //state.End += End;
                //state.Complete += Complete;
                //state.Event += Event;

                skeletonAnimation.SetAnimation(0, "test", false);
                TrackEntry entry = skeletonAnimation.AddAnimation(0, "jump", false, 0);
                //entry.End += End; // Event handling for queued animations.
                skeletonAnimation.AddAnimation(0, "run", true, 0);
            }
            else if (name == "raptor")
            {
                skeletonAnimation.SetAnimation(0, "walk", true);
                skeletonAnimation.SetAnimation(1, "empty", false);
                skeletonAnimation.AddAnimation(1, "gungrab", false, 2);
            }
            else
            {
                skeletonAnimation.SetAnimation(0, "walk", true);
            }

            //skeletonAnimation.AnchorPoint = CCPoint.AnchorLowerLeft;
            skeletonAnimation.PositionX = 300;
            skeletonAnimation.PositionY = 200;
            //skeleton.UpdateWorldTransform();

            var headSlot = skeletonAnimation.FindSlot("head");
            pScene.AddChild(skeletonAnimation);

            //CCLayer pLayer = new TestController();
            //pScene.AddChild(pLayer);

            pDirector.RunWithScene(pScene);
            return true;
        }

        /// <summary>
        /// The function be called when the application enter background
        /// </summary>
        public override void ApplicationDidEnterBackground()
        {
            CCDirector.SharedDirector.Pause();

            // if you use SimpleAudioEngine, it must be pause
            // SimpleAudioEngine::sharedEngine()->pauseBackgroundMusic();
        }

        /// <summary>
        /// The function be called when the application enter foreground  
        /// </summary>
        public override void ApplicationWillEnterForeground()
        {
            CCDirector.SharedDirector.Resume();

            // if you use SimpleAudioEngine, it must resume here
            // SimpleAudioEngine::sharedEngine()->resumeBackgroundMusic();
        }
    }
}