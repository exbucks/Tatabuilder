using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TataBuilder
{
    [Serializable]
    public class TSequence
    {
        public int repeat { get; set; }

        [NonSerialized]
        private TAnimation _animation;
        public TAnimation animation { get { return _animation; } set { _animation = value; } }

        private List<TAction> actions;

        [NonSerialized]
        private int run_repeated;
        [NonSerialized]
        private int run_currentAction;

        public TSequence()
        {
            this.repeat = 1;
            this.animation = null;
            this.actions = new List<TAction>();

            this.run_repeated = 0;
            this.run_currentAction = -1;
        }

        public TSequence clone()
        {
            TSequence sequence = new TSequence();
            sequence.animation = this.animation;
            sequence.repeat = this.repeat;
            this.actions.ForEach((item) => {
                TAction newAction = item.clone();
                newAction.sequence = sequence;
                sequence.actions.Add(newAction);
            });

            return sequence;
        }

        public void fixRelationship()
        {
            actions.ForEach((action) => {
                action.sequence = this;
            });
        }

        // for loading
        public bool parseXml(XElement xml)
        {
            // check validation
            if (xml == null || xml.Name != "Sequence")
                return false;

            try {
                // repeat property
                repeat = int.Parse(xml.Element("Repeat").Value);

                // actions list
                XElement xmlActions = xml.Element("Actions");
                if (xmlActions == null)
                    return false;
                IEnumerable<XElement> xmlActionList = xmlActions.Elements();

                foreach (XElement xmlAction in xmlActionList) {
                    // action class from action tag name
                    string actionClassName = GetType().Namespace + ".T" + xmlAction.Name.ToString();

                    // create action instance with this as sequence of new action
                    TAction action = (TAction)Activator.CreateInstance(Type.GetType(actionClassName));
                    action.sequence = this;
                    if (!action.parseXml(xmlAction))
                        return false;
                    actions.Add(action);
                }
                
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // get xml for saving
        public XElement toXml()
        {
            return
                new XElement("Sequence",
                    new XElement("Repeat", repeat),
                    new XElement("Actions",
                        from action in actions
                        select action.toXml()
                    )
                );
        }

        public int numberOfActions()
        {
            return actions.Count;
        }

        public void addAction(TAction action)
        {
            action.sequence = this;
            actions.Add(action);
        }

        public void insertAction(int actionIndex, TAction action)
        {
            action.sequence = this;
            actions.Insert(actionIndex, action);
        }

        public void deleteAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count)
                actions.RemoveAt(actionIndex);
        }

        public float totalDuration()
        {
            float duration = 0;
            for (int i = 0; i < actions.Count; i++)
                duration += actions[i].duration;

            return duration;
        }

        public bool isInstantAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count)
                return actions[actionIndex].isInstant;
            return false;
        }

        public long durationOfAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count) 
                return actions[actionIndex].duration;
            return 0;
        }

        public TAction actionAtIndex(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count)
                return actions[actionIndex];
            return null;
        }


        public Color startingColorOfAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count)
                return actions[actionIndex].startingColor;
            return Color.White;
        }

        public Color endingColorOfAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count)
                return actions[actionIndex].endingColor;
            return Color.LightGray;
        }

        public Bitmap iconOfAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count)
                return actions[actionIndex].icon;
            return null;
        }

        public Bitmap draggingIconOfAction(int actionIndex)
        {
            if (actionIndex >= 0 && actionIndex < actions.Count)
                return actions[actionIndex].iconWithFrame();
            return null;
        }

        public bool isUsingImage(string img)
        {
            foreach (TAction action in actions) {
                if (action.isUsingImage(img))
                    return true;
            }

            return false;
        }

        public bool isUsingSound(string snd)
        {
            foreach (TAction action in actions) {
                if (action.isUsingSound(snd))
                    return true;
            }

            return false;
        }

        #region Launch Methods

        public void start()
        {
            run_repeated = 0;
            run_currentAction = -1;
        }

        // Execute Sequence for every frame
        // if sequence is progressing then return true
        // if sequence is done and finished then return false
        public bool step(FrmEmulator emulator, long time)
        {
            if ((repeat == -1 || run_repeated < repeat) && actions.Count > 0) {
                if (run_currentAction == -1)
                    changeCurrentAction(0, time);

                bool actionFinished = false;
                int startingAction = run_currentAction;
                while (true) {
                    // execute action
                    actionFinished = actions[run_currentAction].step(emulator, time);

                    // if action finished, next action
                    if (actionFinished) {
                        int nextAction = run_currentAction + 1;
                        bool needNext = false;

                        if (nextAction < actions.Count) {
                            needNext = true;
                        } else {
                            if (repeat == -1) {
                                nextAction = 0;
                                needNext = true;
                            } else if (run_repeated + 1 < repeat) {
                                run_repeated++;
                                nextAction = 0;
                                needNext = true;
                            } else {
                                run_repeated++;
                                needNext = false;
                            }
                        }

                        // go next action
                        if (needNext) {
                            changeCurrentAction(nextAction, time);

                            // check endless loop
                            if (startingAction == nextAction)
                                return true;
                        } else {
                            return false;
                        }
                    } else {
                        return true;
                    }
                }
            }

            return false;
        }

        public void changeCurrentAction(int actionindex, long time)
        {
            actions[actionindex].reset(time);
            run_currentAction = actionindex;
        }

        #endregion
    }
}
