using GuiLabs.Undo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataBuilder
{
    class THistoryActions
    {
    }

    #region ChangeDocumentBGMAction

    class ChangeDocumentBGMAction : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private string oldBGM;
        private string newBGM;

        public ChangeDocumentBGMAction(TDocument doc, FrmMainContainer mainForm, string oldBGM, string newBGM)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldBGM = oldBGM;
            this.newBGM = newBGM;
        }

        protected override void ExecuteCore()
        {
            document.backgroundMusic = newBGM;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.backgroundMusic = oldBGM;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentBGMVolumeAction

    class ChangeDocumentBGMVolumeAction : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private int oldBGMVolume;
        private int newBGMVolume;

        public ChangeDocumentBGMVolumeAction(TDocument doc, FrmMainContainer mainForm, int oldBGMVolume, int newBGMVolume)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldBGMVolume = oldBGMVolume;
            this.newBGMVolume = newBGMVolume;
        }

        protected override void ExecuteCore()
        {
            document.backgroundMusicVolume = newBGMVolume;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.backgroundMusicVolume = oldBGMVolume;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentNVBDelayAction

    class ChangeDocumentNVBDelayAction : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private int oldNVBDelay;
        private int newNVBDelay;

        public ChangeDocumentNVBDelayAction(TDocument doc, FrmMainContainer mainForm, int oldNVBDelay, int newNVBDelay)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldNVBDelay = oldNVBDelay;
            this.newNVBDelay = newNVBDelay;
        }

        protected override void ExecuteCore()
        {
            document.navigationButtonDelayTime = newNVBDelay;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.navigationButtonDelayTime = oldNVBDelay;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentNVBLeftRenderAction

    class ChangeDocumentNVBLeftRenderAction : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private bool oldRenderButton;
        private bool newRenderButton;

        public ChangeDocumentNVBLeftRenderAction(TDocument doc, FrmMainContainer mainForm, bool oldRenderButton, bool newRenderButton)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldRenderButton = oldRenderButton;
            this.newRenderButton = newRenderButton;
        }

        protected override void ExecuteCore()
        {
            document.navigationLeftButtonRender = newRenderButton;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.navigationLeftButtonRender = oldRenderButton;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentNVBRightRenderAction

    class ChangeDocumentNVBRightRenderAction : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private bool oldRenderButton;
        private bool newRenderButton;

        public ChangeDocumentNVBRightRenderAction(TDocument doc, FrmMainContainer mainForm, bool oldRenderButton, bool newRenderButton)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldRenderButton = oldRenderButton;
            this.newRenderButton = newRenderButton;
        }

        protected override void ExecuteCore()
        {
            document.navigationRightButtonRender = newRenderButton;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.navigationRightButtonRender = oldRenderButton;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentPrevSceneButton

    class ChangeDocumentPrevSceneButton : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private string oldButton;
        private string newButton;

        public ChangeDocumentPrevSceneButton(TDocument doc, FrmMainContainer mainForm, string oldButton, string newButton)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldButton = oldButton;
            this.newButton = newButton;
        }

        protected override void ExecuteCore()
        {
            document.prevSceneButton = newButton;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.prevSceneButton = oldButton;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentNextSceneButton

    class ChangeDocumentNextSceneButton : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private string oldButton;
        private string newButton;

        public ChangeDocumentNextSceneButton(TDocument doc, FrmMainContainer mainForm, string oldButton, string newButton)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldButton = oldButton;
            this.newButton = newButton;
        }

        protected override void ExecuteCore()
        {
            document.nextSceneButton = newButton;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.nextSceneButton = oldButton;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentAvatarDefault

    class ChangeDocumentAvatarDefault : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private string oldImage;
        private string newImage;

        public ChangeDocumentAvatarDefault(TDocument doc, FrmMainContainer mainForm, string oldImage, string newImage)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldImage = oldImage;
            this.newImage = newImage;
        }

        protected override void ExecuteCore()
        {
            document.avatarDefault = newImage;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.avatarDefault = oldImage;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentAvatarFrame

    class ChangeDocumentAvatarFrame : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private string oldImage;
        private string newImage;

        public ChangeDocumentAvatarFrame(TDocument doc, FrmMainContainer mainForm, string oldImage, string newImage)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldImage = oldImage;
            this.newImage = newImage;
        }

        protected override void ExecuteCore()
        {
            document.avatarFrame = newImage;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.avatarFrame = oldImage;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region ChangeDocumentAvatarMask

    class ChangeDocumentAvatarMask : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private string oldImage;
        private string newImage;

        public ChangeDocumentAvatarMask(TDocument doc, FrmMainContainer mainForm, string oldImage, string newImage)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldImage = oldImage;
            this.newImage = newImage;
        }

        protected override void ExecuteCore()
        {
            document.avatarMask = newImage;
            mainForm.updateToolbarDocumentSettings();
        }

        protected override void UnExecuteCore()
        {
            document.avatarMask = oldImage;
            mainForm.updateToolbarDocumentSettings();
        }
    }

    #endregion

    #region AddSceneAction

    public class AddSceneAction : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private TScene scene;

        public AddSceneAction(TDocument doc, FrmMainContainer mainForm, TScene scene)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.scene = scene;
        }

        protected override void ExecuteCore()
        {
            // add new scene to active document
            document.sceneManager.addScene(scene);

            // update scenes panel
            mainForm.getScenesList().Items.Add("");
        }

        protected override void UnExecuteCore()
        {
            int index = document.sceneManager.sceneCount() - 1;

            // remove last scene
            document.sceneManager.deleteScene(index);

            int newCurrentIndex = -1;
            if (document.sceneManager.currentSceneIndex >= document.sceneManager.sceneCount())
                newCurrentIndex = document.sceneManager.sceneCount() - 1;
            else if (document.sceneManager.currentSceneIndex == index)
                newCurrentIndex = index;

            // update scenes panel
            mainForm.getScenesList().Items.RemoveAt(index);

            if (newCurrentIndex != -1)
                mainForm.getScenesList().SelectedIndex = newCurrentIndex;
        }
    }

    #endregion

    #region DeleteSceneAction

    public class DeleteSceneAction : AbstractAction
    {
        private TDocument document;
        private FrmMainContainer mainForm;
        private int sceneIndex;
        private TScene scene;

        public DeleteSceneAction(TDocument doc, FrmMainContainer mainForm, int sceneIndex, TScene scene)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.sceneIndex = sceneIndex;
            this.scene = scene;
        }

        protected override void ExecuteCore()
        {
            // delete scene from document
            document.sceneManager.deleteScene(sceneIndex);

            // current scene idnex;
            int newCurrentIndex = -1;
            if (document.sceneManager.currentSceneIndex >= document.sceneManager.sceneCount())
                newCurrentIndex = document.sceneManager.sceneCount() - 1;
            else if (document.sceneManager.currentSceneIndex == sceneIndex)
                newCurrentIndex = sceneIndex;

            // update scenes panel
            mainForm.getScenesList().Items.RemoveAt(sceneIndex);
            if (newCurrentIndex != -1)
                mainForm.getScenesList().SelectedIndex = newCurrentIndex;
        }

        protected override void UnExecuteCore()
        {
            // insert scene at original index
            document.sceneManager.insertScene(scene, sceneIndex);

            // update scenes panel
            mainForm.getScenesList().Items.Insert(sceneIndex, "");
        }
    }

    #endregion

    #region ReorderSceneAction

    public class ReorderSceneAction : AbstractAction
    {
        TDocument document;
        FrmMainContainer mainForm;
        int oldIndex, newIndex;

        public ReorderSceneAction(TDocument doc, FrmMainContainer mainForm, int oldIndex, int newIndex)
        {
            this.document = doc;
            this.mainForm = mainForm;
            this.oldIndex = oldIndex;
            this.newIndex = newIndex;
        }

        protected override void ExecuteCore()
        {
            TScene scene = document.sceneManager.scene(oldIndex);
            document.sceneManager.deleteScene(oldIndex);

            if (oldIndex > newIndex) {
                document.sceneManager.insertScene(scene, newIndex);
                mainForm.getScenesList().SelectedIndex = newIndex;
            } else {
                document.sceneManager.insertScene(scene, newIndex - 1);
                mainForm.getScenesList().SelectedIndex = newIndex - 1;
            }
        }

        protected override void UnExecuteCore()
        {
            if (oldIndex > newIndex) {
                TScene scene = document.sceneManager.scene(newIndex);
                document.sceneManager.deleteScene(newIndex);
                document.sceneManager.insertScene(scene, oldIndex);

                mainForm.getScenesList().SelectedIndex = oldIndex;
            } else {
                TScene scene = document.sceneManager.scene(newIndex - 1);
                document.sceneManager.deleteScene(newIndex - 1);
                document.sceneManager.insertScene(scene, oldIndex);

                mainForm.getScenesList().SelectedIndex = oldIndex;
            }
        }
    }

    #endregion

    #region ModifySceneAction

    class ModifySceneAction : AbstractAction
    {
        class SceneData
        {
            // scene properties
            public string name;
            public Color backgroundColor;
            public bool touchIndication;
            public bool prevButtonVisible;
            public bool nextButtonVisible;
            public string backgroundMusic;
            public int backgroundMusicVolume;
        }

        private TDocument document;
        private TScene scene;
        private SceneData original;
        private SceneData final;

        public ModifySceneAction(TDocument doc, TScene scene)
        {
            this.document = doc;
            this.scene = scene;
            this.original = new SceneData();
            this.final = new SceneData();

            setOriginalData(scene);
        }

        public void setOriginalData(TScene scene)
        {
            original.name = scene.name;
            original.backgroundColor = scene.backgroundColor;
            original.touchIndication = scene.touchIndication;
            original.prevButtonVisible = scene.prevButtonVisible;
            original.nextButtonVisible = scene.nextButtonVisible;
            original.backgroundMusic = scene.backgroundMusic;
            original.backgroundMusicVolume = scene.backgroundMusicVolume;
        }

        public void setFinalData(TScene scene)
        {
            final.name = scene.name;
            final.backgroundColor = scene.backgroundColor;
            final.touchIndication = scene.touchIndication;
            final.prevButtonVisible = scene.prevButtonVisible;
            final.nextButtonVisible = scene.nextButtonVisible;
            final.backgroundMusic = scene.backgroundMusic;
            final.backgroundMusicVolume = scene.backgroundMusicVolume;
        }

        protected override void ExecuteCore()
        {
            scene.name = final.name;
            scene.backgroundColor = final.backgroundColor;
            scene.touchIndication = final.touchIndication;
            scene.prevButtonVisible = final.prevButtonVisible;
            scene.nextButtonVisible = final.nextButtonVisible;
            scene.backgroundMusic = final.backgroundMusic;
            scene.backgroundMusicVolume = final.backgroundMusicVolume;

            // update scene thumbnail
            document.sceneManager.updateThumbnail(scene);
        }

        protected override void UnExecuteCore()
        {
            scene.name = original.name;
            scene.backgroundColor = original.backgroundColor;
            scene.touchIndication = original.touchIndication;
            scene.prevButtonVisible = original.prevButtonVisible;
            scene.nextButtonVisible = original.nextButtonVisible;
            scene.backgroundMusic = original.backgroundMusic;
            scene.backgroundMusicVolume = original.backgroundMusicVolume;

            // update scene thumbnail
            document.sceneManager.updateThumbnail(scene);
        }
    }

    #endregion

    #region AddActorAction

    public class AddActorAction : AbstractAction
    {
        private TDocument document;
        private TActor actor;

        public AddActorAction(TDocument doc, TActor actor)
        {
            this.document = doc;
            this.actor = actor;
        }

        protected override void ExecuteCore()
        {
            actor.parent.childs.Add(actor);
            TScene scene = actor.ownerScene();
            document.sceneManager.updateThumbnail(scene);
        }

        protected override void UnExecuteCore()
        {
            // clear selection
            document.selectedItems.Remove(actor);

            actor.parent.childs.Remove(actor);
            TScene scene = actor.ownerScene();
            document.sceneManager.updateThumbnail(scene);
        }
    }

    #endregion

    #region DeleteActorAction

    class DeleteActorsAction : AbstractAction
    {
        class ActorData
        {
            public TActor actor { get; set; }
            public TLayer parent { get; set; }
            public int index { get; set; }
        }

        private TDocument document;
        private List<TActor> actorList;
        private List<ActorData> actorDatas;

        public DeleteActorsAction(TDocument doc, List<TActor> selectedItems)
        {
            this.document = doc;
            this.actorList = new List<TActor>(selectedItems);
            this.actorDatas = new List<ActorData>();
        }

        protected override void ExecuteCore()
        {
            actorDatas.Clear();
            document.selectedItems.Clear();

            actorList.ForEach((actor) => {
                if (actor.parent != null) {
                    actorDatas.Add(new ActorData { actor = actor, parent = actor.parent, index = actor.parent.childs.IndexOf(actor) });
                    actor.parent.childs.Remove(actor);
                }
            });

            if (actorList.Count > 0) {
                TActor actor = actorList[0];
                TScene scene = actor.ownerScene();
                document.sceneManager.updateThumbnail(scene);
            }
        }

        protected override void UnExecuteCore()
        {
            for (int i = actorDatas.Count - 1; i >= 0; i--) {
                ActorData actorData = actorDatas[i];
                actorData.parent.childs.Insert(actorData.index, actorData.actor);
            }

            if (actorList.Count > 0) {
                TActor actor = actorList[0];
                TScene scene = actor.ownerScene();
                document.sceneManager.updateThumbnail(scene);
            }
        }
    }

    #endregion

    #region ModifyActorAction

    class ModifyActorAction : AbstractAction
    {
        class ActorData
        {
            // common actor properties
            public string name;
            public bool draggable;
            public bool acceleratorSensibility;
            public PointF anchor;
            public PointF position;
            public SizeF scale;
            public SizeF skew;
            public float rotation;
            public Color backgroundColor;
            public float alpha;
            public int zIndex;
            public bool autoInteractionBound;
            public RectangleF interactionBound;

            // puzzle properties
            public bool puzzle;
            public RectangleF puzzleArea;

            // text actor properties
            public string text;
            public Font font;
            public Color color;
            public SizeF boxSize;
        }

        private TDocument document;
        private TActor actor;
        private ActorData original;
        private ActorData final;

        public ModifyActorAction(TDocument doc, TActor actor)
        {
            this.document = doc;
            this.actor = actor;
            this.original = new ActorData();
            this.final = new ActorData();

            setOriginalData(actor);
        }

        public void setOriginalData(TActor actor)
        {
            original.name = actor.name;
            original.draggable = actor.draggable;
            original.acceleratorSensibility = actor.acceleratorSensibility;
            original.anchor = actor.anchor;
            original.position = actor.position;
            original.scale = actor.scale;
            original.skew = actor.skew;
            original.rotation = actor.rotation;
            original.backgroundColor = actor.backgroundColor;
            original.alpha = actor.alpha;
            original.zIndex = actor.zIndex;
            original.interactionBound = actor.interactionBound;
            original.autoInteractionBound = actor.autoInteractionBound;

            original.puzzleArea = actor.puzzleArea;
            original.puzzle = actor.puzzle;

            if (actor is TTextActor) {
                TTextActor textActor = actor as TTextActor;
                original.text = textActor.text;
                original.font = textActor.font;
                original.color = textActor.color;
                original.boxSize = textActor.boxSize;
            }
        }

        public void setFinalData(TActor actor)
        {
            final.name = actor.name;
            final.draggable = actor.draggable;
            final.acceleratorSensibility = actor.acceleratorSensibility;
            final.anchor = actor.anchor;
            final.position = actor.position;
            final.scale = actor.scale;
            final.skew = actor.skew;
            final.rotation = actor.rotation;
            final.backgroundColor = actor.backgroundColor;
            final.alpha = actor.alpha;
            final.zIndex = actor.zIndex;
            final.interactionBound = actor.interactionBound;
            final.autoInteractionBound = actor.autoInteractionBound;

            final.puzzleArea = actor.puzzleArea;
            final.puzzle = actor.puzzle;

            if (actor is TTextActor) {
                TTextActor textActor = actor as TTextActor;
                final.text = textActor.text;
                final.font = textActor.font;
                final.color = textActor.color;
                final.boxSize = textActor.boxSize;
            }
        }

        public bool isModified()
        {
            bool modified =
                original.name != final.name ||
                original.draggable != final.draggable ||
                original.acceleratorSensibility != final.acceleratorSensibility ||
                original.anchor != final.anchor ||
                original.position != final.position ||
                original.scale != final.scale ||
                original.skew != final.skew ||
                original.rotation != final.rotation ||
                original.backgroundColor != final.backgroundColor ||
                original.alpha != final.alpha ||
                original.zIndex != final.zIndex ||
                original.autoInteractionBound != final.autoInteractionBound ||
                original.interactionBound != final.interactionBound ||
                original.puzzle != final.puzzle ||
                original.puzzleArea != final.puzzleArea;

            if (actor is TTextActor && modified == false) {
                modified |=
                    original.text != final.text ||
                    original.font != final.font ||
                    original.color != final.color ||
                    original.boxSize != final.boxSize;
            }

            return modified;
        }

        protected override void ExecuteCore()
        {
            actor.name = final.name;
            actor.draggable = final.draggable;
            actor.acceleratorSensibility = final.acceleratorSensibility;
            actor.anchor = final.anchor;
            actor.position = final.position;
            actor.scale = final.scale;
            actor.skew = final.skew;
            actor.rotation = final.rotation;
            actor.backgroundColor = final.backgroundColor;
            actor.alpha = final.alpha;
            actor.zIndex = final.zIndex;

            actor.interactionBound = final.interactionBound;
            actor.autoInteractionBound = final.autoInteractionBound; // must assign autoInteractionBound after assin interaction bound, because interactionBound setting will change autoInteractionBound property

            actor.puzzleArea = final.puzzleArea;
            actor.puzzle = final.puzzle;

            if (actor is TTextActor) {
                TTextActor textActor = actor as TTextActor;
                textActor.text = final.text;
                textActor.font = final.font;
                textActor.color = final.color;
                textActor.boxSize = final.boxSize;
            }

            TScene scene = actor.ownerScene();
            document.sceneManager.updateThumbnail(scene);
        }

        protected override void UnExecuteCore()
        {
            actor.name = original.name;
            actor.draggable = original.draggable;
            actor.acceleratorSensibility = original.acceleratorSensibility;
            actor.anchor = original.anchor;
            actor.position = original.position;
            actor.scale = original.scale;
            actor.skew = original.skew;
            actor.rotation = original.rotation;
            actor.backgroundColor = original.backgroundColor;
            actor.alpha = original.alpha;
            actor.zIndex = original.zIndex;

            actor.interactionBound = original.interactionBound;
            actor.autoInteractionBound = original.autoInteractionBound; // must assign autoInteractionBound after assin interaction bound, because interactionBound setting will change autoInteractionBound property

            actor.puzzleArea = original.puzzleArea;
            actor.puzzle = original.puzzle;

            if (actor is TTextActor) {
                TTextActor textActor = actor as TTextActor;
                textActor.text = original.text;
                textActor.font = original.font;
                textActor.color = original.color;
                textActor.boxSize = original.boxSize;
            }

            TScene scene = actor.ownerScene();
            document.sceneManager.updateThumbnail(scene);
        }
    }

    #endregion

    #region TransferActorAction

    class TransferActorAction : AbstractAction
    {
        class ActorMatrixData
        {
            // actor matrix properties
            public PointF position;
            public SizeF scale;
            public SizeF skew;
            public float rotation;
        }

        private TDocument document;
        private TActor actor;
        private ActorMatrixData oldData;
        private ActorMatrixData newData;
        private TLayer oldParent;
        private TLayer newParent;
        private int oldIndex;

        public TransferActorAction(TDocument doc, TActor actor, TLayer parent)
        {
            this.document = doc;
            this.actor = actor;
            this.oldData = new ActorMatrixData();
            this.newData = new ActorMatrixData();
            this.oldParent = actor.parent;
            this.newParent = parent;

            this.oldData.position = actor.position;
            this.oldData.scale = actor.scale;
            this.oldData.skew = actor.skew;
            this.oldData.rotation = actor.rotation;

            // actor's position based on new parent
            PointF pt = actor.parent.logicalToScreen(actor.position);
            pt = parent.screenToLogical(pt);

            // actor's rotation based on new parent
            float angle = actor.rotationOnScreen();
            if (parent is TActor)
                angle -= ((TActor)parent).rotationOnScreen();
            TUtil.normalizeDegreeAngle(angle);

            // for scale
            RectangleF bound = actor.bound();
            PointF s = actor.logicalVectorToScreen(new PointF(bound.Width, bound.Height));
            SizeF scale = new SizeF(1, 1);

            Matrix m2 = new Matrix();
            m2.Translate(pt.X, pt.Y);
            m2.Rotate((float)(angle * 180 / Math.PI));
            m2.Translate(-actor.anchor.X * actor.bound().Width, -actor.anchor.Y * actor.bound().Height);

            Matrix m = parent.matrixFromScreen();
            m.Multiply(m2);
            if (m.IsInvertible) {
                PointF[] aPos = { s };
                m.Invert();
                m.TransformVectors(aPos);
                s = aPos[0];
                scale = new SizeF(s.X / bound.Width, s.Y / bound.Height);
            }

            this.newData.position = pt;
            this.newData.scale = scale;
            this.newData.skew = actor.skew;
            this.newData.rotation = angle;

            oldIndex = actor.parent.childs.IndexOf(actor);
        }

        protected override void ExecuteCore()
        {
            actor.parent.childs.Remove(actor);
            newParent.childs.Add(actor);
            actor.parent = newParent;

            actor.position = newData.position;
            actor.scale = newData.scale;
            actor.skew = newData.skew;
            actor.rotation = newData.rotation;
        }

        protected override void UnExecuteCore()
        {
            actor.parent.childs.Remove(actor);
            oldParent.childs.Insert(oldIndex, actor);
            actor.parent = oldParent;

            actor.position = oldData.position;
            actor.scale = oldData.scale;
            actor.skew = oldData.skew;
            actor.rotation = oldData.rotation;
        }
    }

    #endregion

    #region ChangeImageActorAction

    public class ChangeImageActorAction : AbstractAction
    {
        private TDocument document;
        private TImageActor actor;
        private string oldImage;
        private string newImage;

        public ChangeImageActorAction(TDocument doc, TImageActor actor, string image)
        {
            this.document = doc;
            this.actor = actor;
            this.oldImage = actor.image;
            this.newImage = image;
        }

        protected override void ExecuteCore()
        {
            actor.image = newImage;
            actor.loadImage();

            TScene scene = actor.ownerScene();
            document.sceneManager.updateThumbnail(scene);
        }

        protected override void UnExecuteCore()
        {
            actor.image = oldImage;
            actor.loadImage();

            TScene scene = actor.ownerScene();
            document.sceneManager.updateThumbnail(scene);
        }
    }

    #endregion

    #region AddAnimationAction

    public class AddAnimationAction : AbstractAction
    {
        FrmMainContainer mainForm;
        TLayer layer;
        TAnimation animation;

        public AddAnimationAction(FrmMainContainer mainForm, TLayer layer, TAnimation animation)
        {
            this.mainForm = mainForm;
            this.layer = layer;
            this.animation = animation;
        }

        protected override void ExecuteCore()
        {
            layer.animations.Add(animation);
            mainForm.addAnimationListItem(animation);
        }

        protected override void UnExecuteCore()
        {
            layer.animations.Remove(animation);
            mainForm.removeAnimationListItem(-1);
            mainForm.updateToolbarAnimationProperties();
        }
    }

    #endregion

    #region DeleteAnimationAction

    public class DeleteAnimationAction : AbstractAction
    {
        FrmMainContainer mainForm;
        TLayer layer;
        int index;
        TAnimation animation;

        public DeleteAnimationAction(FrmMainContainer mainForm, TLayer layer, int index)
        {
            this.mainForm = mainForm;
            this.layer = layer;
            this.index = index;
            this.animation = layer.animations[index];
        }

        protected override void ExecuteCore()
        {
            layer.animations.RemoveAt(index);
            mainForm.removeAnimationListItem(index);
            mainForm.updateToolbarAnimationProperties();
        }

        protected override void UnExecuteCore()
        {
            layer.animations.Insert(index, animation);
            mainForm.insertAnimationListItem(index, animation);
        }
    }

    #endregion

    #region ChangeAnimationAction

    public class ChangeAnimationAction : AbstractAction
    {
        FrmMainContainer mainForm;
        TLayer layer;
        int index;
        TAnimation oldAnimation;
        TAnimation newAnimation;

        public ChangeAnimationAction(FrmMainContainer mainForm, TLayer layer, TAnimation oldAnimation, TAnimation newAnimation)
        {
            this.mainForm = mainForm;
            this.layer = layer;
            this.oldAnimation = oldAnimation;
            this.newAnimation = newAnimation;
            this.index = layer.animations.IndexOf(oldAnimation);
        }

        protected override void ExecuteCore()
        {
            layer.animations[index] = newAnimation;
            mainForm.updateAnimationListItem(index, newAnimation);
        }

        protected override void UnExecuteCore()
        {
            layer.animations[index] = oldAnimation;
            mainForm.updateAnimationListItem(index, oldAnimation);
        }
    }

    #endregion

    #region ChangeAnimationEventAction

    public class ChangeAnimationEventAction : AbstractAction
    {
        FrmMainContainer mainForm;
        int index;
        TAnimation animation;
        string oldEvent, newEvent;

        public ChangeAnimationEventAction(FrmMainContainer mainForm, int index, TAnimation animation, string newEvent)
        {
            this.mainForm = mainForm;
            this.index = index;
            this.animation = animation;
            this.oldEvent = animation.eventu;
            this.newEvent = newEvent;
        }

        protected override void ExecuteCore()
        {
            animation.eventu = newEvent;
            mainForm.updateAnimationListItemImage(index, animation);
        }

        protected override void UnExecuteCore()
        {
            animation.eventu = oldEvent;
            mainForm.updateAnimationListItemImage(index, animation);
        }
    }

    #endregion

    #region ChangeAnimationStateAction

    public class ChangeAnimationStateAction : AbstractAction
    {
        FrmMainContainer mainForm;
        int index;
        TAnimation animation;
        string oldState, newState;

        public ChangeAnimationStateAction(FrmMainContainer mainForm, int index, TAnimation animation, string newState)
        {
            this.mainForm = mainForm;
            this.index = index;
            this.animation = animation;
            this.oldState = animation.state;
            this.newState = newState;
        }

        protected override void ExecuteCore()
        {
            animation.state = newState;
            mainForm.updateAnimationListItemImage(index, animation);
        }

        protected override void UnExecuteCore()
        {
            animation.state = oldState;
            mainForm.updateAnimationListItemImage(index, animation);
        }
    }

    #endregion
}
