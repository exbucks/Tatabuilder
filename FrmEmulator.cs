using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    public partial class FrmEmulator : Form
    {
        public enum EmulatorType { Phone, Tablet };

        public EmulatorType type { get; set; }
        public TDocument document { get; set; }
        public TSoundEmulator soundEmulator { get; set; }
        public TScene currentScene { get; set; }

        private Image imgMenuButton = Properties.Resources.emulator_img_menu_button;
        private Image imgPrevButton;
        private Image imgNextButton;

        private TScene nextScene = null;
        private bool bgmOn = true;
        private bool effectOn = true;
        private bool voiceOn = true;
        public bool textOn = true;

        private bool bStepFlag = false;
        private Thread threadStep = null;
        Stopwatch sw = new Stopwatch();
        private static Object locker = new Object();

        bool MousePressed = false;
        PointF MouseDownPos = new PointF();
        TActor MouseDownActor = null;

        enum MenuState { Default, Help };
        MenuState menuState;

        delegate void TransitionDelegate(long time);
        TransitionDelegate transitionDelegate;
        long transitionStartTime;

        List<TAnimation> extraAnimations = new List<TAnimation>();

        public Stopwatch accelerationWatch = new Stopwatch();

        public FrmEmulator()
        {
            InitializeComponent();

            InitializeEmulator();
        }

        public FrmEmulator(EmulatorType type)
        {
            InitializeComponent();

            this.type = type;
            InitializeEmulator();
        }

        private void InitializeEmulator() {
            // borderless form
            BackColor = Color.Magenta;
            TransparencyKey = Color.Magenta;

            soundEmulator = null;
            currentScene = null;

            if (type == EmulatorType.Phone) {
                // phone
                this.BackgroundImage = Properties.Resources.emulator_phone;

                // buttons
                btnExit.BackgroundImage = Properties.Resources.emulator_phone_exit;
                btnRelaunch.BackgroundImage = Properties.Resources.emulator_phone_relaunch;
                btnSnapshoot.BackgroundImage = Properties.Resources.emulator_phone_snapshoot;

                btnExit.Location = new Point(640, 27);
                btnRelaunch.Location = new Point(640, 152);
                btnSnapshoot.Location = new Point(640, 276);

                pnlDisplayBox.Location = new Point(133, 28);
                pnlDisplayBox.Size = new Size(480, 320);
            } else if (type == EmulatorType.Tablet) {
                // form
                this.BackgroundImage = Properties.Resources.emulator_tablet;

                // buttons
                btnExit.BackgroundImage = Properties.Resources.emulator_tablet_exit;
                btnRelaunch.BackgroundImage = Properties.Resources.emulator_tablet_relaunch;
                btnSnapshoot.BackgroundImage = Properties.Resources.emulator_tablet_snapshoot;

                btnExit.Location = new Point(1183, 44);
                btnRelaunch.Location = new Point(1183, 396);
                btnSnapshoot.Location = new Point(1183, 746);

                pnlDisplayBox.Location = new Point(127, 46);
                pnlDisplayBox.Size = new Size(1024, 768);
            }

            // form
            this.Size = this.BackgroundImage.Size;

            // buttons
            btnExit.Size = btnExit.BackgroundImage.Size;
            btnRelaunch.Size = btnRelaunch.BackgroundImage.Size;
            btnSnapshoot.Size = btnSnapshoot.BackgroundImage.Size;
        }

        /*
        Constants in Windows API
        0x84 = WM_NCHITTEST - Mouse Capture Test
        0x1 = HTCLIENT - Application Client Area
        0x2 = HTCAPTION - Application Title Bar

        This function intercepts all the commands sent to the application. 
        It checks to see of the message is a mouse click in the application. 
        It passes the action to the base action by default. It reassigns 
        the action to the title bar if it occured in the client area
        to allow the drag and move behavior.
        */

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg) {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRelaunch_Click(object sender, EventArgs e)
        {
            endDocument();
            startDocument();
        }

        private void btnSnapshoot_Click(object sender, EventArgs e)
        {
            if (currentScene != null) {
                int width = pnlDisplayBox.Size.Width;
                int height = pnlDisplayBox.Size.Height;

                Bitmap bm = new Bitmap(width, height);
                pnlDisplayBox.DrawToBitmap(bm, new Rectangle(0, 0, width, height));

                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + @"_" + currentScene.name + @".png";
                string fullpath = path + @"\" + filename;
                if (!File.Exists(fullpath)) {
                    try {
                        bm.Save(fullpath, ImageFormat.Png);
                        MessageBox.Show(@"Snapshoot was created in desktop with filename " + filename, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        MessageBox.Show(@"It was failed to create the snapshoot.\nError : " + ex.ToString(), Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    MessageBox.Show(@"Already snapshoot exist in desktop with filename " + filename, Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FrmEmulator_Load(object sender, EventArgs e)
        {
            sw.Start();
            startDocument();
        }

        private void FrmEmulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            endDocument();
            sw.Stop();
        }

        private void pnlDisplayBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentScene == null)
                return;

            if (e.Button == MouseButtons.Left && transitionDelegate == null) {
                // matrix
                Matrix mat = matrixOfEmulator();

                // set flag that mouse is pressed and store position
                MousePressed = true;
                MouseDownPos = new PointF(e.X, e.Y);

                // target item
                TActor selectedActor = currentScene.actorAtPosition(mat, e.Location, true);
                if (selectedActor != null) {
                    // fire touch event
                    MouseDownActor = selectedActor;
                    MouseDownActor.fireEvent(Program.DEFAULT_EVENT_TOUCH, false);

                    // fire drag event
                    if (MouseDownActor.draggable || MouseDownActor.puzzle) {
                        MouseDownActor.createBackup();
                    }
                }

                // redraw workspace
                this.pnlDisplayBox.Refresh();
            }
        }

        private void pnlDisplayBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentScene == null)
                return;

            if (e.Button == MouseButtons.Left && MousePressed) {

                // if have selection, move it
                if (MouseDownActor != null && (MouseDownActor.draggable || MouseDownActor.puzzle)) {
                    PointF p = MouseDownActor.parent.logicalToScreen(MouseDownActor.position);
                    p.X += e.X - MouseDownPos.X;
                    p.Y += e.Y - MouseDownPos.Y;

                    // if clicked actor has accelorator sensitibility, set velocity
                    if (MouseDownActor.acceleratorSensibility) {
                        MouseDownActor.run_xVelocity = (e.X - MouseDownPos.X) / 20;
                        MouseDownActor.run_yVelocity = (e.Y - MouseDownPos.Y) / 20;
                    }

                    MouseDownActor.position = MouseDownActor.parent.screenToLogical(p);

                    // fire dragging event
                    MouseDownActor.fireEvent(Program.DEFAULT_EVENT_DRAGGING, false);
                }

                // update mouse position
                MouseDownPos.X = e.X; MouseDownPos.Y = e.Y;

                // redraw workspace
                this.pnlDisplayBox.Refresh();

            }
        }

        private void pnlDisplayBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentScene == null)
                return;

            if (e.Button == MouseButtons.Left && MousePressed) {
                // fire drop event
                if (MouseDownActor != null) {
                    MouseDownActor.fireEvent(Program.DEFAULT_EVENT_DROP, false);

                    // check this actor is puzzle actor
                    if (MouseDownActor.puzzle && !MouseDownActor.isMoving()) {

                        PointF[] bound1 = MouseDownActor.interactionBoundOnScreen();

                        // check if the puzzle actor went the correct puzzle area
                        if (TUtil.isPolygonsIntersect(bound1, MouseDownActor.puzzleAreaOnScreen())) {

                            // turn off the puzzle function after success
                            MouseDownActor.puzzle = false;

                            // fire puzzle success event
                            MouseDownActor.fireEvent(Program.DEFAULT_EVENT_PUZZLE_SUCCESS, false);
                        } else {
                            // if puzzle is failed, actor return to original position
                            TAnimation animation = new TAnimation(MouseDownActor);
                            TSequence sequence = animation.addSequence();
                            sequence.addAction(new TActionIntervalMove() { duration = 300, position = MouseDownActor.backupActor.position });
                            sequence.addAction(new TActionInstantDispatchEvent() { actor = MouseDownActor.name, eventu = Program.DEFAULT_EVENT_PUZZLE_FAIL, recursive = false });

                            animation.start();
                            extraAnimations.Add(animation);
                        }
                    }
                }

                MousePressed = false;
                MouseDownActor = null;

                // redraw workspace
                this.pnlDisplayBox.Refresh();
            }
        }

        private void startDocument()
        {
            // default values
            bgmOn = true;
            effectOn = true;
            voiceOn = true;

            transitionDelegate = null;
            transitionStartTime = 0;

            // sound emulator
            soundEmulator = new TSoundEmulator(document.libraryManager);

            // play document background
            playBGM(document.backgroundMusic, document.backgroundMusicVolume);

            
            // load prev button image
            try {
                int prevSceneButtonIndex = document.libraryManager.imageIndex(document.prevSceneButton);
                if (prevSceneButtonIndex == -1)
                    imgPrevButton = TUtil.resizedImage(Properties.Resources.emulator_img_nav_prev, new Size(Program.NAVBUTTON_WIDTH, Program.NAVBUTTON_HEIGHT), Program.NAVBUTTON_STRETCH);
                else
                    imgPrevButton = TUtil.resizedImage(Image.FromFile(document.libraryManager.imageFilePath(prevSceneButtonIndex)), new Size(Program.NAVBUTTON_WIDTH, Program.NAVBUTTON_HEIGHT), Program.NAVBUTTON_STRETCH);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                imgPrevButton = TUtil.resizedImage(Properties.Resources.emulator_img_nav_prev, new Size(Program.NAVBUTTON_WIDTH, Program.NAVBUTTON_HEIGHT), Program.NAVBUTTON_STRETCH);
            }

            // load next button image
            try {
                int nextSceneButtonIndex = document.libraryManager.imageIndex(document.nextSceneButton);
                if (nextSceneButtonIndex == -1)
                    imgNextButton = TUtil.resizedImage(Properties.Resources.emulator_img_nav_next, new Size(Program.NAVBUTTON_WIDTH, Program.NAVBUTTON_HEIGHT), Program.NAVBUTTON_STRETCH);
                else
                    imgNextButton = TUtil.resizedImage(Image.FromFile(document.libraryManager.imageFilePath(nextSceneButtonIndex)), new Size(Program.NAVBUTTON_WIDTH, Program.NAVBUTTON_HEIGHT), Program.NAVBUTTON_STRETCH);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                imgNextButton = TUtil.resizedImage(Properties.Resources.emulator_img_nav_next, new Size(Program.NAVBUTTON_WIDTH, Program.NAVBUTTON_HEIGHT), Program.NAVBUTTON_STRETCH);
            }
            

            // initial scene
            changeScene(document.currentScene());

            // start timer
            bStepFlag = true;
            threadStep = new Thread(new ThreadStart(step));
            threadStep.SetApartmentState(System.Threading.ApartmentState.STA);
            threadStep.Start();
        }

        private void endDocument()
        {
            if (threadStep != null) {
                // stop thread
                bStepFlag = false;

                // Spin for a while waiting for the started thread to become
                // alive:
                while (!threadStep.IsAlive) ;

                // Put the Main thread to sleep for 1 millisecond to allow oThread
                // to do some work:
                Thread.Sleep(1);

                // Request that oThread be stopped
                threadStep.Abort();

                // Wait until oThread finishes. Join also has overloads
                // that take a millisecond interval or a TimeSpan object.
                threadStep.Join();
            }

            // stop current scene
            finishCurrentScene();

            // stop All Sounds
            soundEmulator.stopAllSounds();
            soundEmulator = null;

            // clear document's running state
            document.run_avatar = null;
        }

        private void finishCurrentScene()
        {
            // free resources for old scene
            stopAllEffects();
            stopAllVoices();

            // free captured mouse
            MousePressed = false;
            MouseDownActor = null;

            // clear extra animations
            extraAnimations.Clear();
        }

        private Matrix matrixOfEmulator()
        {
            // calc matrix
            float zoom = Math.Min((float)pnlDisplayBox.Width / Program.BOOK_WIDTH, (float)pnlDisplayBox.Height / Program.BOOK_HEIGHT);
            Matrix mat = new Matrix();
            mat.Translate(pnlDisplayBox.Width / 2, pnlDisplayBox.Height / 2);
            mat.Scale(zoom, zoom);
            mat.Translate(-0.5F * Program.BOOK_WIDTH, -0.5F * Program.BOOK_HEIGHT);
            return mat;
        }

        private List<TActor> extraActorsOfEmulator(TScene scene)
        {
            // result
            List<TActor> extras = new List<TActor>();

            // back navigation button
            if (scene.prevButtonVisible && imgPrevButton != null) {
                TImageActor btnPrev = new TImageActor(document, imgPrevButton, 0, Program.BOOK_HEIGHT, scene, "[emulator_actor]:prev_button");
                btnPrev.anchor = new PointF(0, 1);
                extras.Add(btnPrev);

                if (document.navigationLeftButtonRender)
                {
                    TAnimation animation = new TAnimation(btnPrev);
                    TSequence sequence = animation.addSequence();
                    sequence.addAction(new TActionIntervalDelay() { duration = document.navigationButtonDelayTime * 1000 });
                    sequence.addAction(new TActionInstantDispatchEvent() { actor = btnPrev.name, eventu = Program.DEFAULT_EVENT_UNDEFINED, recursive = false });
                    animation.eventu = Program.DEFAULT_EVENT_ENTER;
                    animation.start();
                    btnPrev.animations.Add(animation);

                    animation = new TAnimation(btnPrev);
                    sequence = animation.addSequence();
                    sequence.addAction(new TActionIntervalDelay() { duration = 500 });
                    sequence.addAction(new TActionIntervalScale() { duration = 300, scale = new SizeF(1.4f, 1.4f) });
                    sequence.addAction(new TActionIntervalScale() { duration = 300, scale = new SizeF(1f, 1f) });
                    sequence.repeat = 100;
                    animation.eventu = Program.DEFAULT_EVENT_UNDEFINED;
                    btnPrev.animations.Add(animation);
                } else {
                    TAnimation animation = TAnimation.newAnimation(btnPrev, new TActionInstantGoScene() { type = TActionInstantGoScene.ActionType.PREVIOUS });
                    animation.eventu = Program.DEFAULT_EVENT_TOUCH;
                    btnPrev.animations.Add(animation);
                }
            }

            // next navigation button
            if (scene.nextButtonVisible && imgNextButton != null) {
                TImageActor btnNext = new TImageActor(document, imgNextButton, Program.BOOK_WIDTH, Program.BOOK_HEIGHT, scene, "[emulator_actor]:next_button");
                btnNext.anchor = new PointF(1, 1);
                extras.Add(btnNext);

                if (document.navigationRightButtonRender)
                {
                    TAnimation animation = new TAnimation(btnNext);
                    TSequence sequence = animation.addSequence();
                    sequence.addAction(new TActionIntervalDelay() { duration = document.navigationButtonDelayTime * 1000 });
                    sequence.addAction(new TActionInstantDispatchEvent() { actor = btnNext.name, eventu = Program.DEFAULT_EVENT_UNDEFINED, recursive = false });
                    animation.eventu = Program.DEFAULT_EVENT_ENTER;
                    animation.start();
                    btnNext.animations.Add(animation);

                    animation = new TAnimation(btnNext);
                    sequence = animation.addSequence();
                    sequence.addAction(new TActionIntervalDelay() { duration = 500 });
                    sequence.addAction(new TActionIntervalScale() { duration = 300, scale = new SizeF(1.4f, 1.4f) });
                    sequence.addAction(new TActionIntervalScale() { duration = 300, scale = new SizeF(1f, 1f) });
                    sequence.repeat = 100;
                    animation.eventu = Program.DEFAULT_EVENT_UNDEFINED;
                    btnNext.animations.Add(animation);
                } else {
                    TAnimation animation = TAnimation.newAnimation(btnNext, new TActionInstantGoScene() { type = TActionInstantGoScene.ActionType.NEXT });
                    animation.eventu = Program.DEFAULT_EVENT_TOUCH;
                    btnNext.animations.Add(animation);
                }
            }

            // menu button
            {
                TImageActor btnMenu = new TImageActor(document, Properties.Resources.emulator_img_menu_button, Program.BOOK_WIDTH, 0, scene, "[emulator_actor]:menu_button");
                btnMenu.anchor = new PointF(1, 0);
                extras.Add(btnMenu);

                TAnimation animation = TAnimation.newAnimation(btnMenu, new TActionInstantDispatchEvent() { actor = "[emulator_actor]:menu_dialog", eventu = "[emulator_event]:show_dialog", recursive = true });
                animation.eventu = Program.DEFAULT_EVENT_TOUCH;
                btnMenu.animations.Add(animation);
            }

            // menu popup
            {
                // background
                TImageActor dlgMenu = new TImageActor(document, Properties.Resources.emulator_img_menu_dialog_bg, 0, 0, scene, "[emulator_actor]:menu_dialog");
                {
                    dlgMenu.anchor = new PointF(0, 0);
                    dlgMenu.alpha = 0;
                    extras.Add(dlgMenu);

                    // show dialog
                    TAnimation animShow = new TAnimation(dlgMenu);
                    TSequence seqShow = animShow.addSequence();
                    seqShow.addAction(new TActionIntervalFade() { duration = 100, type = TActionIntervalFade.ActionType.IN });
                    animShow.eventu = "[emulator_event]:show_dialog";
                    dlgMenu.animations.Add(animShow);

                    // hide dialog
                    TAnimation animHide = new TAnimation(dlgMenu);
                    TSequence seqHide = animHide.addSequence();
                    seqHide.addAction(new TActionIntervalDelay() { duration = 200 });
                    seqHide.addAction(new TActionIntervalFade() { duration = 100, type = TActionIntervalFade.ActionType.OUT });
                    animHide.eventu = "[emulator_event]:hide_dialog";
                    dlgMenu.animations.Add(animHide);

                    TAnimation animation = TAnimation.newAnimation(dlgMenu, new TActionInstantDispatchEvent() { actor = "[emulator_actor]:menu_dialog", eventu = "[emulator_event]:hide_dialog", recursive = true });
                    animation.eventu = Program.DEFAULT_EVENT_TOUCH;
                    dlgMenu.animations.Add(animation);
                }

                TAnimation animShowBase = TAnimation.newAnimation(null, new TActionIntervalMove() {
                    duration = 300,
                    easingMode = TEasingFunction.EasingMode.Out,
                    easingType = TEasingFunction.EasingType.Bounce
                });
                animShowBase.eventu = "[emulator_event]:show_dialog";

                TAnimation animHideBase = TAnimation.newAnimation(null, new TActionIntervalMove() {
                    duration = 300,
                    easingMode = TEasingFunction.EasingMode.In,
                    easingType = TEasingFunction.EasingType.Back
                });
                animHideBase.eventu = "[emulator_event]:hide_dialog";

                TImageActor btnBGM = new TImageActor(document, Properties.Resources.emulator_img_bgm_off, -100, 290, dlgMenu, "[emulator_actor]:menu_bgm_button");
                {
                    btnBGM.anchor = new PointF(0.5f, 0.5f);
                    dlgMenu.childs.Add(btnBGM);

                    // show animation from base show animation
                    TAnimation animShow = animShowBase.clone();
                    animShow.layer = btnBGM;
                    ((TActionIntervalMove)animShow.actionAtIndex(0, 0)).position = new PointF(170, 290);
                    btnBGM.animations.Add(animShow);

                    // hide animation from base hide animation
                    TAnimation animHide = animHideBase.clone();
                    animHide.layer = btnBGM;
                    ((TActionIntervalMove)animHide.actionAtIndex(0, 0)).position = btnBGM.position;
                    btnBGM.animations.Add(animHide);

                    // initialize button status, when menu is popuped
                    TAnimation animInit = TAnimation.newAnimation(btnBGM, new TActionRuntime(0, delegate(float percent)
                    {
                        if (bgmOn)
                            btnBGM.loadImage(Properties.Resources.emulator_img_bgm_on);
                        else
                            btnBGM.loadImage(Properties.Resources.emulator_img_bgm_off);
                    }));
                    animInit.eventu = "[emulator_event]:show_dialog";
                    btnBGM.animations.Add(animInit);

                    // perform action of button and change status when button is clicked
                    TAnimation animToggle = TAnimation.newAnimation(btnBGM, new TActionRuntime(0, delegate(float percent)
                    {
                        toggleBGM();

                        if (bgmOn)
                            btnBGM.loadImage(Properties.Resources.emulator_img_bgm_on);
                        else
                            btnBGM.loadImage(Properties.Resources.emulator_img_bgm_off);
                    }));
                    animToggle.eventu = Program.DEFAULT_EVENT_TOUCH;
                    btnBGM.animations.Add(animToggle);
                }

                TImageActor btnEffect = new TImageActor(document, Properties.Resources.emulator_img_effect_off, -100, 480, dlgMenu, "[emulator_actor]:menu_effect_button");
                {
                    btnEffect.anchor = new PointF(0.5f, 0.5f);
                    dlgMenu.childs.Add(btnEffect);

                    // show animation from base show animation
                    TAnimation animShow = animShowBase.clone();
                    animShow.layer = btnEffect;
                    ((TActionIntervalMove)animShow.actionAtIndex(0, 0)).position = new PointF(170, 480);
                    btnEffect.animations.Add(animShow);

                    // hide animation from base hide animation
                    TAnimation animHide = animHideBase.clone();
                    animHide.layer = btnEffect;
                    ((TActionIntervalMove)animHide.actionAtIndex(0, 0)).position = btnEffect.position;
                    btnEffect.animations.Add(animHide);

                    // initialize button status, when menu is popuped
                    TAnimation animInit = TAnimation.newAnimation(btnEffect, new TActionRuntime(0, delegate(float percent) {
                        if (effectOn)
                            btnEffect.loadImage(Properties.Resources.emulator_img_effect_on);
                        else
                            btnEffect.loadImage(Properties.Resources.emulator_img_effect_off);
                    }));
                    animInit.eventu = "[emulator_event]:show_dialog";
                    btnEffect.animations.Add(animInit);

                    // perform action of button and change status when button is clicked
                    TAnimation animToggle = TAnimation.newAnimation(btnEffect, new TActionRuntime(0, delegate(float percent) {
                        toggleEffect();

                        if (effectOn)
                            btnEffect.loadImage(Properties.Resources.emulator_img_effect_on);
                        else
                            btnEffect.loadImage(Properties.Resources.emulator_img_effect_off);
                    }));
                    animToggle.eventu = Program.DEFAULT_EVENT_TOUCH;
                    btnEffect.animations.Add(animToggle);
                }

                TImageActor btnVoice = new TImageActor(document, Properties.Resources.emulator_img_voice_off, -100, 600, dlgMenu, "[emulator_actor]:menu_voice_button");
                {
                    btnVoice.anchor = new PointF(0.5f, 0.5f);
                    dlgMenu.childs.Add(btnVoice);

                    // show animation from base show animation
                    TAnimation animShow = animShowBase.clone();
                    animShow.layer = btnVoice;
                    ((TActionIntervalMove)animShow.actionAtIndex(0, 0)).position = new PointF(170, 670);
                    btnVoice.animations.Add(animShow);

                    // hide animation from base hide animation
                    TAnimation animHide = animHideBase.clone();
                    animHide.layer = btnVoice;
                    ((TActionIntervalMove)animHide.actionAtIndex(0, 0)).position = btnVoice.position;
                    btnVoice.animations.Add(animHide);

                    // initialize button status, when menu is popuped
                    TAnimation animInit = TAnimation.newAnimation(btnVoice, new TActionRuntime(0, delegate(float percent) {
                        if (voiceOn)
                            btnVoice.loadImage(Properties.Resources.emulator_img_voice_on);
                        else
                            btnVoice.loadImage(Properties.Resources.emulator_img_voice_off);
                    }));
                    animInit.eventu = "[emulator_event]:show_dialog";
                    btnVoice.animations.Add(animInit);

                    // perform action of button and change status when button is clicked
                    TAnimation animToggle = TAnimation.newAnimation(btnVoice, new TActionRuntime(0, delegate(float percent) {
                        toggleVoice();

                        if (voiceOn)
                            btnVoice.loadImage(Properties.Resources.emulator_img_voice_on);
                        else
                            btnVoice.loadImage(Properties.Resources.emulator_img_voice_off);
                    }));
                    animToggle.eventu = Program.DEFAULT_EVENT_TOUCH;
                    btnVoice.animations.Add(animToggle);
                }

                TImageActor btnText = new TImageActor(document, Properties.Resources.emulator_img_text_off, -100, 100, dlgMenu, "[emulator_actor]:menu_text_button");
                {
                    btnText.anchor = new PointF(0.5f, 0.5f);
                    dlgMenu.childs.Add(btnText);

                    // show animation from base show animation
                    TAnimation animShow = animShowBase.clone();
                    animShow.layer = btnText;
                    ((TActionIntervalMove)animShow.actionAtIndex(0, 0)).position = new PointF(170, 100);
                    btnText.animations.Add(animShow);

                    // hide animation from base hide animation
                    TAnimation animHide = animHideBase.clone();
                    animHide.layer = btnText;
                    ((TActionIntervalMove)animHide.actionAtIndex(0, 0)).position = btnText.position;
                    btnText.animations.Add(animHide);

                    // initialize button status, when menu is popuped
                    TAnimation animInit = TAnimation.newAnimation(btnText, new TActionRuntime(0, delegate(float percent) {
                        if (textOn)
                            btnText.loadImage(Properties.Resources.emulator_img_text_on);
                        else
                            btnText.loadImage(Properties.Resources.emulator_img_text_off);
                    }));
                    animInit.eventu = "[emulator_event]:show_dialog";
                    btnText.animations.Add(animInit);

                    // perform action of button and change status when button is clicked
                    TAnimation animToggle = TAnimation.newAnimation(btnText, new TActionRuntime(0, delegate(float percent) {
                        toggleText();

                        if (textOn)
                            btnText.loadImage(Properties.Resources.emulator_img_text_on);
                        else
                            btnText.loadImage(Properties.Resources.emulator_img_text_off);
                    }));
                    animToggle.eventu = Program.DEFAULT_EVENT_TOUCH;
                    btnText.animations.Add(animToggle);
                }

                /*
                TImageActor btnBack = new TImageActor(document, Properties.Resources.emulator_img_menu_dialog_bg, 0, 0, dlgMenu, "[emulator_actor]:menu_back_button");
                {
                    btnBack.anchor = new PointF(1.0f, 0f);
                    dlgMenu.childs.Add(btnBack);

                    // show animation from base show animation
                    TAnimation animShow = animShowBase.clone();
                    animShow.layer = btnBack;
                    ((TActionIntervalMove)animShow.actionAtIndex(0, 0)).position = new PointF(900, 500);
                    btnBack.animations.Add(animShow);

                    // hide animation from base hide animation
                    TAnimation animHide = animHideBase.clone();
                    animHide.layer = btnBack;
                    ((TActionIntervalMove)animHide.actionAtIndex(0, 0)).position = btnBack.position;
                    btnBack.animations.Add(animHide);

                    TAnimation animation = TAnimation.newAnimation(btnBack, new TActionInstantDispatchEvent() { actor = "[emulator_actor]:menu_dialog", eventu = "[emulator_event]:hide_dialog", recursive = true });
                    animation.eventu = Program.DEFAULT_EVENT_TOUCH;
                    btnBack.animations.Add(animation);
                }
                */
            }

            return extras;
        }

        public void changeScene(TScene scene)
        {
            // stop/free all sounds and effects of current scene
            finishCurrentScene();

            // assign new scene to current scene
            currentScene = (TScene)scene.clone();
            currentScene.run_emulator = this;
            currentScene.run_matrix = matrixOfEmulator();
            currentScene.run_extraActors = extraActorsOfEmulator(currentScene);

            // scene have owner bgm
            if (currentScene.backgroundMusic != "")
                playBGM(currentScene.backgroundMusic, currentScene.backgroundMusicVolume);

            // fire enter event
            currentScene.fireEvent(Program.DEFAULT_EVENT_ENTER, true);
        }

        public void transitScene(TScene scene)
        {
            if (transitionDelegate != null)
                transitionDelegate(transitionStartTime + 100000);

            nextScene = (TScene)scene.clone();
            transitionStartTime = sw.ElapsedMilliseconds;
            transitionDelegate = delegate(long time)
            {
                const int duration = 400; //ms
                int elapsed = (int)(time - transitionStartTime);
                if (elapsed >= duration / 2 && nextScene != null) {
                    changeScene(nextScene);
                    nextScene = null;
                }

                if (elapsed >= duration) {
                    elapsed = duration;
                    transitionDelegate = null;
                }

                if (elapsed < duration / 2)
                    currentScene.alpha = 1 - elapsed / (duration / 2f);
                else
                    currentScene.alpha = elapsed / (duration / 2f) - 1;
            };
        }

        public void gotoPrevScene()
        {
            this.transitScene(document.prevScene(this.currentScene));
            this.pnlDisplayBox.Invalidate();
        }

        public void gotoNextScene()
        {
            this.transitScene(document.nextScene(this.currentScene));
            this.pnlDisplayBox.Invalidate();
        }

        public void gotoCoverScene()
        {
//            this.transitScene(document.coverScene());
//            this.pnlDisplayBox.Invalidate();
        }

        public void gotoSpecificScene(string sceneName)
        {
            TScene scene = document.findScene(sceneName);
            if (scene != null) {
                this.transitScene(scene);
                this.pnlDisplayBox.Invalidate();
            }
        }

        public void makeAvatar()
        {
            FrmAvatarMaker dlg = new FrmAvatarMaker();
            dlg.document = document;

            if (dlg.ShowDialog() == DialogResult.OK && dlg.avatar != null) {
                document.run_avatar = dlg.avatar;
            }
        }

        public void clearAvatar()
        {
            document.run_avatar = null;
        }

        public void playEffect(string filename, int volume, bool loop)
        {
            if (effectOn)
                soundEmulator.playEffect(filename, volume, loop);
        }

        public void stopEffect(string filename)
        {
            if (effectOn)
                soundEmulator.stopEffect(filename);
        }

        public void stopAllEffects()
        {
            if (effectOn)
                soundEmulator.stopAllEffects();
        }

        public void playVoice(string filename, int volume, bool loop)
        {
            if (voiceOn)
                soundEmulator.playVoice(filename, volume, loop);
        }

        public void stopVoice(string filename)
        {
            if (voiceOn)
                soundEmulator.stopVoice(filename);
        }

        public void stopAllVoices()
        {
            if (voiceOn)
                soundEmulator.stopAllVoices();
        }

        public void playBGM()
        {
            if (bgmOn)
                soundEmulator.playBGM();
        }

        public void playBGM(string filename, int volume)
        {
            if (bgmOn)
                soundEmulator.playBGM(filename, volume);
        }

        public void stopBGM()
        {
            if (bgmOn)
                soundEmulator.stopBGM();
        }

        public void stopAllSounds(bool bgm, bool effect, bool voice)
        {
            soundEmulator.stopAllSounds(bgm, effect, voice);
        }

        public void toggleBGM(bool auto = true, bool on = true)
        {
            if ((auto && bgmOn) || (!auto && !on)) {
                soundEmulator.stopBGM();
                bgmOn = false;
            } else {
                soundEmulator.playBGM();
                bgmOn = true;
            }
        }

        public void toggleEffect(bool auto = true, bool on = true)
        {
            if ((auto && effectOn) || (!auto && !on)) {
                soundEmulator.stopAllEffects();
                effectOn = false;
            } else {
                effectOn = true;
            }
        }

        public void toggleVoice(bool auto = true, bool on = true)
        {
            if ((auto && voiceOn) || (!auto && !on)) {
                soundEmulator.stopAllVoices();
                voiceOn = false;
            } else {
                voiceOn = true;
            }
        }

        public void toggleText(bool auto = true, bool on = true)
        {
            if ((auto && textOn) || (!auto && !on)) {
                textOn = false;
            } else {
                textOn = true;
            }
        }

        //========================================= important main methods ================================================//

        private void pnlDisplayBox_Paint(object sender, PaintEventArgs e)
        {
            lock (locker) 
            {
                Graphics g = e.Graphics;

                if (type == EmulatorType.Phone)
                    g.DrawImage(Properties.Resources.emulator_bg_iphone, 0, 0, pnlDisplayBox.Width, pnlDisplayBox.Height);

                if (currentScene != null) {

                    // apply matrix
                    GraphicsState gs = g.Save();
                    g.MultiplyTransform(matrixOfEmulator());
                    g.SetClip(new Rectangle(0, 0, Program.BOOK_WIDTH, Program.BOOK_HEIGHT));

                    // draw scene
                    currentScene.draw(g);

                    // restore graphics
                    g.Restore(gs);
                }
            }
        }

        private void step()
        {
            while (bStepFlag) {
                lock (locker) {
                    // animations of current scene
                    long time = sw.ElapsedMilliseconds;
                    currentScene.step(this, time);

                    // extra animationn
                    foreach (TAnimation animation in extraAnimations) {
                        if (animation.run_executing) {
                            animation.step(this, time);
                        }
                    }

                    // transition effect of emulator
                    if (transitionDelegate != null)
                        transitionDelegate(time);
        
                    // mark the stop watch for acceleration
                    accelerationWatch.Restart();

                    pnlDisplayBox.Invalidate();
                }

                Thread.Sleep(10);
            }
        }

        #region ImageButton Class

        partial class ImageButton
        {
            private Image onImage;
            private Image offImage;
            private int posX;
            private int posY;

            public bool isOn { get; set; }

            public ImageButton(Image offImage, Image onImage, int x, int y)
            {
                this.offImage = offImage;
                this.onImage = onImage;
                this.posX = x;
                this.posY = y;
                isOn = false;
            }

            public void draw(Graphics g, int offsetX, int offsetY)
            {
                Image img = isOn ? onImage : offImage;
                g.DrawImage(img, offsetX + posX, offsetY + posY);
            }

            public bool hitTest(int x, int y, int offsetX, int offsetY)
            {
                Image img = isOn ? onImage : offImage;
                return new Rectangle(offsetX + posX, offsetY + posY, img.Width, img.Height).Contains(x, y);
            }
        }

        #endregion
    }
}
