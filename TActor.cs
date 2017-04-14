using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public abstract class TActor : TLayer, IComparable<TActor>
    {
        protected PointF Anchor;
        public PointF anchor { get { return Anchor; } set { Anchor = value; refreshMatrix(); } }

        protected PointF Position;
        public PointF position { get { return Position; } set { Position = value; refreshMatrix(); } }

        protected SizeF Scale;
        public SizeF scale { get { return Scale; } set { Scale = value; refreshMatrix(); } }

        protected SizeF Skew;
        public SizeF skew { get { return Skew; } set { Skew = value; refreshMatrix(); } }

        // rotation angle in degree
        protected float Rotation;
        public float rotation { get { return Rotation; } set { Rotation = value; refreshMatrix(); } }

        public int zIndex { get; set; }

        public bool draggable { get; set; }

        public bool acceleratorSensibility { get; set; }

        public bool autoInteractionBound { get; set; }

        protected RectangleF InteractionBound;
        public RectangleF interactionBound
        {
            get
            {
                if (autoInteractionBound)
                    return this.bound();
                else
                    return InteractionBound;
            }

            set
            {
                autoInteractionBound = false;
                InteractionBound = value;
            }
        }

        public bool puzzle { get; set; }

        protected RectangleF PuzzleArea;
        public RectangleF puzzleArea
        {
            get
            {
                return PuzzleArea;
            }

            set
            {
                puzzle = true;
                PuzzleArea = value;
            }
        }

        [NonSerialized]
        private Matrix _matrix;
        public Matrix matrix { get { return _matrix; } set { _matrix = value; } }

        [NonSerialized]
        private TActor _backupActor;
        public TActor backupActor { get { return _backupActor; } set { _backupActor = value; } }

        [NonSerialized]
        public float run_xVelocity;
        [NonSerialized]
        public float run_yVelocity;

        public TActor(TDocument doc) : base(doc)
        {
            Anchor = new PointF(0.5F, 0.5F);
            Position = new PointF();
            Scale = new SizeF(1, 1);
            Skew = new SizeF(0, 0);
            Rotation = 0;
            zIndex = 0;
            draggable = false;
            acceleratorSensibility = false;
            autoInteractionBound = true;
            InteractionBound = new RectangleF();
            puzzle = false;
            PuzzleArea = new RectangleF(0, 0, Program.BOOK_WIDTH, Program.BOOK_HEIGHT);
            matrix = new Matrix();
            _backupActor = null;

            run_xVelocity = 0;
            run_yVelocity = 0;
        }

        public TActor(TDocument doc, float x, float y, TLayer parent, string actorName) : base(doc, parent, actorName)
        {
            Anchor = new PointF(0.5F, 0.5F);
            Position = new PointF(x, y);
            Scale = new SizeF(1, 1);
            Skew = new SizeF(0, 0);
            Rotation = 0;
            zIndex = 0;
            draggable = false;
            acceleratorSensibility = false;
            autoInteractionBound = true;
            InteractionBound = new RectangleF();
            puzzle = false;
            PuzzleArea = new RectangleF(0, 0, Program.BOOK_WIDTH, Program.BOOK_HEIGHT);
            matrix = new Matrix();
            _backupActor = null;

            run_xVelocity = 0;
            run_yVelocity = 0;
        }

        protected override void clone(TLayer target)
        {
            base.clone(target);

            TActor targetLayer = (TActor)target;
            targetLayer.anchor = this.anchor;
            targetLayer.position = this.position;
            targetLayer.scale = this.scale;
            targetLayer.skew = this.skew;
            targetLayer.rotation = this.rotation;
            targetLayer.zIndex = this.zIndex;
            targetLayer.draggable = this.draggable;
            targetLayer.acceleratorSensibility = this.acceleratorSensibility;
            targetLayer.autoInteractionBound = this.autoInteractionBound;
            targetLayer.InteractionBound = this.InteractionBound;
            targetLayer.matrix = matrix.Clone();

            targetLayer.puzzle = this.puzzle;
            targetLayer.PuzzleArea = this.PuzzleArea;
        }

        public override void fixRelationship()
        {
            base.fixRelationship();

            matrix = new Matrix();
            refreshMatrix();
        }

        public override bool parseXml(XElement xml, TLayer parentLayer)
        {
            if (xml == null)
                return false;

            if (!base.parseXml(xml, parentLayer))
                return false;

            try {
                Anchor.X = float.Parse(xml.Element("AnchorX").Value);
                Anchor.Y = float.Parse(xml.Element("AnchorY").Value);
                Position.X = float.Parse(xml.Element("PositionX").Value);
                Position.Y = float.Parse(xml.Element("PositionY").Value);
                Scale.Width = float.Parse(xml.Element("ScaleWidth").Value);
                Scale.Height = float.Parse(xml.Element("ScaleHeight").Value);
                Skew.Width = float.Parse(xml.Element("SkewWidth").Value);
                Skew.Height = float.Parse(xml.Element("SkewHeight").Value);
                Rotation = float.Parse(xml.Element("Rotation").Value);
                refreshMatrix();

                zIndex = int.Parse(xml.Element("ZIndex").Value);
                draggable = bool.Parse(xml.Element("Draggable").Value);
                acceleratorSensibility = bool.Parse(xml.Element("AcceleratorSensibility").Value);
                autoInteractionBound = bool.Parse(xml.Element("AutoInteractionBound").Value);
                InteractionBound = new RectangleF(  float.Parse(xml.Element("InteractionBoundX").Value),
                                                    float.Parse(xml.Element("InteractionBoundY").Value),
                                                    float.Parse(xml.Element("InteractionBoundWidth").Value),
                                                    float.Parse(xml.Element("InteractionBoundHeight").Value));

                puzzle = bool.Parse(xml.Element("Puzzle").Value);
                PuzzleArea = new RectangleF(TUtil.parseFloatXElement(xml.Element("PuzzleAreaX"), 0),
                                            TUtil.parseFloatXElement(xml.Element("PuzzleAreaY"), 0),
                                            TUtil.parseFloatXElement(xml.Element("PuzzleAreaWidth"), Program.BOOK_WIDTH),
                                            TUtil.parseFloatXElement(xml.Element("PuzzleAreaHeight"), Program.BOOK_HEIGHT));

                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "Actor";
            xml.Add(
                new XElement("AnchorX", anchor.X),
                new XElement("AnchorY", anchor.Y),
                new XElement("PositionX", position.X),
                new XElement("PositionY", position.Y),
                new XElement("ScaleWidth", scale.Width),
                new XElement("ScaleHeight", scale.Height),
                new XElement("SkewWidth", skew.Width),
                new XElement("SkewHeight", skew.Height),
                new XElement("Rotation", rotation),
                new XElement("ZIndex", zIndex),
                new XElement("Draggable", draggable),
                new XElement("AcceleratorSensibility", acceleratorSensibility),
                new XElement("AutoInteractionBound", autoInteractionBound),
                new XElement("InteractionBoundX", InteractionBound.X),
                new XElement("InteractionBoundY", InteractionBound.Y),
                new XElement("InteractionBoundWidth", InteractionBound.Width),
                new XElement("InteractionBoundHeight", InteractionBound.Height),
                new XElement("Puzzle", puzzle),
                new XElement("PuzzleAreaX", PuzzleArea.X),
                new XElement("PuzzleAreaY", PuzzleArea.Y),
                new XElement("PuzzleAreaWidth", PuzzleArea.Width),
                new XElement("PuzzleAreaHeight", PuzzleArea.Height)
            );

            return xml;
        }

        public void createBackup()
        {
            _backupActor = this.clone() as TActor;
        }

        public void deleteBackup()
        {
            _backupActor = null;
        }

        public abstract void refreshMatrix();

        // matrix mutliplied the matrix from top scene to this
        public override Matrix matrixFromScreen()
        {
            if (parent != null) {
                Matrix m = parent.matrixFromScreen();
                m.Multiply(matrix);
                return m;
            }

            return matrix.Clone();
        }

        // Default comparer for Part type. 
        public int CompareTo(TActor comparePart)
        {
            // A null value means that this object is greater. 
            if (comparePart == null)
                return 1;
            else
                return this.zIndex.CompareTo(comparePart.zIndex);
        }

        public override TActor actorAtPosition(Matrix m, PointF screenPos, bool withinInteraction)
        {
            if (this.alpha <= 0.001)
                return null;

            // total applied matrix
            Matrix m2 = m.Clone();
            m2.Multiply(matrix);

            // recursive find
            List<TActor> items = this.sortedChilds();
            for (int i = items.Count - 1; i >= 0; i--) {
                TActor ret = items[i].actorAtPosition(m2, screenPos, withinInteraction);
                if (ret != null)
                    return ret;
            }

            // local coordinates
            if (m2.IsInvertible) {
                m2.Invert();
                PointF[] aPos = { screenPos };
                m2.TransformPoints(aPos);

                RectangleF b = withinInteraction ? this.interactionBound : this.bound();
                if (b.Contains(aPos[0]))
                    return this;
            }

            return null;
        }

        public float rotationOnScreen()
        {
            if (this.parent != null && this.parent is TActor)
                return ((TActor)this.parent).rotationOnScreen() + this.rotation;
            else
                return this.rotation;
        }

        public PointF[] interactionBoundOnScreen()
        {
            Matrix m = this.matrixFromScreen();
            RectangleF b = this.interactionBound;
            PointF[] aPos = { new PointF(b.Left, b.Top), new PointF(b.Left, b.Bottom), new PointF(b.Right, b.Bottom), new PointF(b.Right, b.Top) };
            m.TransformPoints(aPos);
            return aPos;
        }

        public PointF[] puzzleAreaOnScreen() 
        {
            Matrix m = this.ownerScene().matrixFromScreen();
            RectangleF b = this.puzzleArea;
            PointF[] aPos = { new PointF(b.Left, b.Top), new PointF(b.Left, b.Bottom), new PointF(b.Right, b.Bottom), new PointF(b.Right, b.Top) };
            m.TransformPoints(aPos);
            return aPos;
        }

        //==================================== Animation =================================================//

        public override string[] getDefaultEvents()
        {
            return new string[] { 
                Program.DEFAULT_EVENT_TOUCH, 
                Program.DEFAULT_EVENT_ENTER, 
                Program.DEFAULT_EVENT_DRAGGING, 
                Program.DEFAULT_EVENT_DROP,
                Program.DEFAULT_EVENT_PUZZLE_SUCCESS,
                Program.DEFAULT_EVENT_PUZZLE_FAIL,
            };
        }

        public override string[] getDefaultStates()
        {
            return new string[] { Program.DEFAULT_STATE_DEFAULT };
        }

        public bool isMoving()
        {
            foreach (TAnimation animation in animations) {
                int sequenceCount = animation.numberOfSequences();
                if (animation.run_executing) {
                    for (int i = 0; i < sequenceCount; i++) {
                        TSequence sequence = animation.sequenceAtIndex(i);
                        int actionCount = sequence.numberOfActions();
                        for (int j = 0; j < actionCount; j++) {
                            TAction action = sequence.actionAtIndex(j);
                            if (action is TActionIntervalMove) {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public override void step(FrmEmulator emulator, long time)
        {
            // process for accelerator sensibility actor
            if (this.acceleratorSensibility && !isMoving()) {
                // elapsed time
                float elapsed = emulator.accelerationWatch.ElapsedMilliseconds / 1000.0f;

                // emulate acceleration
                PointF acceleration = new PointF(0f, 0.98f);
                run_xVelocity += acceleration.X * elapsed;
                run_yVelocity += acceleration.Y * elapsed;

                float xDelta = elapsed * run_xVelocity * 500;
                float yDelta = elapsed * run_yVelocity * 500;

                PointF point = this.parent.logicalToScreen(this.position);
                point.X += xDelta;
                point.Y += yDelta;

                // try assign location with new position
                this.position = this.parent.screenToLogical(point);
        
                // check collision of new bound with boundaries
                PointF d = collisionWithBoundaries(this.boundStraightOnScreen());
        
                // correct based on collision
                point.X += d.X;
                point.Y += d.Y;
        
                this.position = this.parent.screenToLogical(point);
            }

            base.step(emulator, time);
        }

        private PointF collisionWithBoundaries(RectangleF frame) 
        {
            PointF lt = this.ownerScene().logicalToScreen(new PointF(0, 0));
            PointF rb = this.ownerScene().logicalToScreen(new PointF(Program.BOOK_WIDTH, Program.BOOK_HEIGHT));

            float dx = 0, dy = 0;
    
            if (frame.Left < lt.X) {
                dx = lt.X - frame.Left;
                run_xVelocity = -(run_xVelocity / 2.0f);
            }
    
            if (frame.Top < lt.Y) {
                dy = lt.Y - frame.Top;
                run_yVelocity = -(run_yVelocity / 2.0f);
            }
    
            if (frame.Right > rb.X) {
                dx = rb.X - frame.Right;
                run_xVelocity = -(run_xVelocity / 2.0f);
            }
    
            if (frame.Bottom > rb.Y) {
                dy = rb.Y - frame.Bottom;
                run_yVelocity = -(run_yVelocity / 2.0f);
            }

            return new PointF(dx, dy);
        }

    }
}
