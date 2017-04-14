using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public abstract class TLayer
    {
        [NonSerialized]
        private TDocument _document;
        public TDocument document { get { return _document; } set { _document = value; } }

        public string name { get; set; }

        [NonSerialized]
        private TLayer _parent;
        public TLayer parent { get { return _parent; } set { _parent = value; } }

        public bool locked { get; set; }

        public Color backgroundColor { get; set; }

        public float alpha { get; set; }

        public List<TLayer> childs { get; set; }

        public List<TAnimation> animations { get; set; }

        protected List<string> events;
        protected List<string> states;

        [NonSerialized]
        public string run_state;
        [NonSerialized]
        public bool run_enabled;

        public TLayer(TDocument doc)
        {
            document = doc;
            name = "";
            parent = null;
            locked = false;
            backgroundColor = Color.Transparent;
            alpha = 1;
            childs = new List<TLayer>();
            animations = new List<TAnimation>();

            events = new List<string>();
            states = new List<string>();

            run_state = Program.DEFAULT_STATE_DEFAULT;
            run_enabled = true;
        }

        public TLayer(TDocument doc, TLayer parentLayer, string layerName)
        {
            document = doc;
            name = layerName;
            parent = parentLayer;
            locked = false;
            backgroundColor = Color.Transparent;
            alpha = 1;
            childs = new List<TLayer>();
            animations = new List<TAnimation>();
            run_state = Program.DEFAULT_STATE_DEFAULT;
            run_enabled = true;

            events = new List<string>();
            states = new List<string>();
        }

        public virtual TLayer clone()
        {
            TLayer layer = (TLayer)Activator.CreateInstance(this.GetType(), new Object[] { document });
            this.clone(layer);

            return layer;
        }

        protected virtual void clone(TLayer target)
        {
            target.document = this.document;
            target.parent = this.parent;
            target.name = this.name;
            target.locked = this.locked;
            target.backgroundColor = this.backgroundColor;
            target.alpha = this.alpha;

            this.childs.ForEach((item) => {
                TLayer newItem = item.clone();
                newItem.parent = target;
                target.childs.Add(newItem);
            });

            this.animations.ForEach((item) => {
                TAnimation newAnimation = item.clone();
                newAnimation.layer = target;
                target.animations.Add(newAnimation);
            });

            this.events.ForEach((item) => {
                target.events.Add(item);
            });

            this.states.ForEach((item) => {
                target.states.Add(item);
            });
        }

        public virtual void fixRelationship()
        {
            this.childs.ForEach((child) => {
                child.parent = this;
                child.fixRelationship();
            });

            this.animations.ForEach((animation) => {
                animation.layer = this;
                animation.fixRelationship();
            });
        }

        public virtual bool parseXml(XElement xml, TLayer parentLayer)
        {
            if (xml == null)
                return false;

            try {
                name = xml.Element("Name").Value;
                parent = parentLayer;
                locked = TUtil.parseBoolXElement(xml.Element("Locked"), false);
                backgroundColor = Color.FromArgb(TUtil.parseIntXElement(xml.Element("BackgroundColor"), 0));
                alpha = float.Parse(xml.Element("Alpha").Value);

                XElement xmlEvents = xml.Element("Events");
                IEnumerable<XElement> xmlEventList = xmlEvents.Elements("Event");
                foreach (XElement xmlEvent in xmlEventList)
                    events.Add(xmlEvent.Value);

                XElement xmlStates = xml.Element("States");
                IEnumerable<XElement> xmlStateList = xmlStates.Elements("State");
                foreach (XElement xmlState in xmlStateList)
                    states.Add(xmlState.Value);

                XElement xmlAnimations = xml.Element("Animations");
                IEnumerable<XElement> xmlAnimationList = xmlAnimations.Elements("Animation");
                foreach (XElement xmlAnimation in xmlAnimationList) {
                    TAnimation animation = new TAnimation(this);
                    if (!animation.parseXml(xmlAnimation))
                        return false;
                    animations.Add(animation);
                }

                XElement xmlChilds = xml.Element("Childs");
                IEnumerable<XElement> xmlChildList = xmlChilds.Elements();
                foreach (XElement xmlChild in xmlChildList) {
                    TLayer layer = (TLayer)Activator.CreateInstance(Type.GetType(GetType().Namespace + ".T" + xmlChild.Name.ToString()), new Object[] {document});
                    if (!layer.parseXml(xmlChild, this))
                        return false;
                    childs.Add(layer);
                }

                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public virtual XElement toXml()
        {
            return
                new XElement("Layer",
                    new XElement("Name", name),
                    new XElement("Locked", locked),
                    new XElement("BackgroundColor", backgroundColor.ToArgb()),
                    new XElement("Alpha", alpha),
                    new XElement("Events",
                        from eventu in events
                        select new XElement("Event", eventu)
                    ),
                    new XElement("States",
                        from state in states
                        select new XElement("State", state)
                    ),
                    new XElement("Animations",
                        from animation in animations
                        select animation.toXml()
                    ),
                    new XElement("Childs",
                        from child in childs
                        select child.toXml()
                    )
                );
        }

        // get new layer name suffix
        public int newLayerNameSuffix(string prefix)
        {
            int k = 1;
            Regex rgx = new Regex("^" + prefix + "(\\d+)$");
            MatchCollection matches = rgx.Matches(this.name);
            if (matches.Count > 0) {
                int no = int.Parse(matches[0].Groups[1].Value);
                if (no >= k)
                    k = no + 1;
            }

            for (int i = 0; i < childs.Count; i++) {
                int no = childs[i].newLayerNameSuffix(prefix);
                if (no > k)
                    k = no;
            }

            return k;

        }

        // get new layer name
        public string newLayerName(string prefix)
        {
            int suffix = this.newLayerNameSuffix(prefix);
            return prefix + suffix;
        }

        // draw actor with graphics
        public abstract void draw(Graphics g);

        // get child list by zindex
        public List<TActor> sortedChilds()
        {
            List<TActor> list = new List<TActor>();
            for (int i = 0; i < childs.Count; i++) {
                if (childs[i] is TActor)
                    list.Add((TActor)childs[i]);
            }

            // sort by zindex
            list.Sort();

            return list;
        }

        // get all childs recursivly
        public List<TLayer> getAllChilds()
        {
            List<TLayer> list = new List<TLayer>();
            foreach (TLayer layer in childs) {
                list.Add(layer);
                list.AddRange(layer.getAllChilds());
            }

            return list;
        }

        public virtual TLayer findLayer(string targetName)
        {
            if (this.name == targetName)
                return this;

            foreach (TLayer layer in childs) {
                TLayer ret = layer.findLayer(targetName);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        public TScene ownerScene()
        {
            if (this is TScene)
                return (TScene)this;
            else if (this.parent != null)
                return this.parent.ownerScene();
            else
                return null;
        }

        // matrix mutliplied the matrix from top scene to this
        public abstract Matrix matrixFromScreen();

        public float alphaFromScreen()
        {
            if (parent != null) {
                return parent.alphaFromScreen() * this.alpha;
            }

            if (this.alpha < 0)
                return 0;
            else if (this.alpha > 1)
                return 1;
            else
                return this.alpha;
        }

        // convert screen coordinate to logical coordinate of layer
        public PointF screenToLogical(PointF point)
        {
            Matrix m = this.matrixFromScreen();
            PointF[] aPos = { point };
            if (m.IsInvertible) {
                m.Invert();
                m.TransformPoints(aPos);
                return aPos[0];
            } else {
                return PointF.Empty;
            }
        }

        // convert screen vector to logical vector of layer
        public PointF screenVectorToLogical(PointF point)
        {
            Matrix m = this.matrixFromScreen();
            PointF[] aPos = { point };
            if (m.IsInvertible) {
                m.Invert();
                m.TransformVectors(aPos);
                return aPos[0];
            } else {
                return PointF.Empty;
            }
        }

        // convert logical coordinate to screen coordinate
        public PointF logicalToScreen(PointF point)
        {
            Matrix m = this.matrixFromScreen();
            PointF[] aPos = { point };
            m.TransformPoints(aPos);
            return aPos[0];
        }

        // convert logical vector of layer to screen vector
        public PointF logicalVectorToScreen(PointF point)
        {
            Matrix m = this.matrixFromScreen();
            PointF[] aPos = { point };
            m.TransformVectors(aPos);
            return aPos[0];
        }

        public abstract RectangleF bound();

        public PointF[] boundOnScreen()
        {
            Matrix m = this.matrixFromScreen();
            RectangleF b = this.bound();
            PointF[] aPos = { new PointF(b.Left, b.Top), new PointF(b.Left, b.Bottom), new PointF(b.Right, b.Bottom), new PointF(b.Right, b.Top) };
            m.TransformPoints(aPos);
            return aPos;
        }

        public RectangleF boundStraightOnScreen()
        {
            PointF[] aPos = this.boundOnScreen();
            if (aPos.Length == 0)
                return RectangleF.Empty;

            RectangleF bound = new RectangleF(aPos[0], SizeF.Empty);
            for (int i = 1; i < aPos.Length; i++) {
                if (bound.Left > aPos[i].X)
                    bound.X = aPos[i].X;
                if (bound.Right < aPos[i].X)
                    bound.Width = aPos[i].X - bound.Left;
                if (bound.Top > aPos[i].Y)
                    bound.Y = aPos[i].Y;
                if (bound.Bottom < aPos[i].Y)
                    bound.Height = aPos[i].Y - bound.Top;
            }
            return bound;
        }

        public abstract TActor actorAtPosition(Matrix m, PointF screenPos, bool withinInteraction);

        public virtual bool isUsingImage(string img)
        {
            // check self animation
            foreach (TAnimation animation in animations) {
                if (animation.isUsingImage(img))
                    return true;
            }

            // check children
            foreach (TLayer child in childs) {
                if (child.isUsingImage(img))
                    return true;
            }

            return false;
        }

        public virtual bool isUsingSound(string snd)
        {
            // check self animation
            foreach (TAnimation animation in animations) {
                if (animation.isUsingSound(snd))
                    return true;
            }

            // check children
            foreach (TLayer child in childs) {
                if (child.isUsingSound(snd))
                    return true;
            }

            return false;
        }

        //==================================== Animation =================================================//
        public abstract string[] getDefaultEvents();

        public string[] getEvents()
        {
            return getDefaultEvents().Concat(events).ToArray();
        }

        public bool addEvent(string eventu)
        {
            if (getEvents().Contains(eventu))
                return false;

            events.Add(eventu);
            return true;
        }

        public bool isDefaultEvent(string eventu)
        {
            return getDefaultEvents().Contains(eventu);
        }

        public void renameEvent(string eventu, string newEventu)
        {
            int i = events.IndexOf(eventu);
            if (i != -1)
                events[i] = newEventu;
        }

        public bool deleteEvent(string eventu)
        {
            if (isDefaultEvent(eventu))
                return false;
            if (!events.Contains(eventu))
                return false;

            events.Remove(eventu);
            return true;
        }

        public abstract string[] getDefaultStates();

        public string[] getStates()
        {
            return getDefaultStates().Concat(states).ToArray();
        }

        public bool addState(string state)
        {
            if (getStates().Contains(state))
                return false;

            states.Add(state);
            return true;
        }

        public bool isDefaultState(string state)
        {
            return getDefaultStates().Contains(state);
        }

        public void renameState(string state, string newState)
        {
            int i = states.IndexOf(state);
            if (i != -1)
                states[i] = newState;
        }

        public bool deleteState(string state)
        {
            if (isDefaultState(state))
                return false;
            if (!states.Contains(state))
                return false;

            states.Remove(state);
            return true;
        }

        //==================================== Execute =================================================//

        public virtual void fireEvent(string eventu, bool recursive)
        {
            if (run_enabled) {
                // animation of self
                for (int i = 0; i < animations.Count; i++) {
                    TAnimation animation = animations[i];
                    if (animation.eventu == eventu && animation.state == run_state && animation.run_executing == false)
                        animation.start();
                }

                // animation of child
                if (recursive) {
                    for (int i = 0; i < childs.Count; i++) {
                        childs[i].fireEvent(eventu, recursive);
                    }
                }
            }
        }

        public virtual void startAnimation(string eventu)
        {
            if (run_enabled) {
                foreach (TAnimation animation in animations) {
                    if (animation.eventu == eventu && animation.state == run_state && animation.run_executing == true)
                        animation.start();
                }
            }
        }

        public virtual void stopAnimation(string eventu, string state)
        {
            if (run_enabled) {
                foreach (TAnimation animation in animations) {
                    if (animation.eventu == eventu && animation.state == state && animation.run_executing == true)
                        animation.stop();
                }
            }
        }

        public virtual void step(FrmEmulator emulator, long time)
        {
            // step of self
            for (int i = 0; i < animations.Count; i++) {
                TAnimation animation = animations[i];
                if (animation.run_executing) {
                    animation.step(emulator, time);
                }
            }

            // step of child
            for (int i = 0; i < childs.Count; i++) {
                childs[i].step(emulator, time);
            }
        }
    }
}
