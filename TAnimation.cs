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
    public class TAnimation : ITimeLineDataSource
    {
        public string eventu { get; set; }
        public string state { get; set; }

        [NonSerialized]
        private TLayer _layer;
        public TLayer layer { get { return _layer; } set { _layer = value; } }

        private List<TSequence> sequences;

        // for launch
        [NonSerialized]
        public bool run_executing;

        public TAnimation(TLayer layer)
        {
            eventu = Program.DEFAULT_EVENT_UNDEFINED;
            state = Program.DEFAULT_STATE_DEFAULT;
            this.layer = layer;

            sequences = new List<TSequence>();

            // for launch
            run_executing = false;
        }

        #region TAnimation Methods

        public TAnimation clone()
        {
            TAnimation anim = new TAnimation(this.layer);
            anim.eventu = eventu;
            anim.state = state;
            sequences.ForEach((item) => {
                TSequence newSequence = item.clone();
                newSequence.animation = anim;
                anim.sequences.Add(newSequence);
            });

            return anim;
        }

        public static TAnimation newAnimation(TLayer layer, TAction action)
        {
            TAnimation animation = new TAnimation(layer);

            TSequence sequence = new TSequence();
            animation.addSequence(sequence);

            action.sequence = sequence;
            sequence.addAction(action);

            return animation;
        }

        public void fixRelationship()
        {
            sequences.ForEach((sequence) => {
                sequence.animation = this;
                sequence.fixRelationship();
            });
        }

        public bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "Animation")
                return false;

            try {
                eventu = xml.Element("Event").Value;
                state = xml.Element("State").Value;

                XElement xmlSequences = xml.Element("Sequences");
                if (xmlSequences == null)
                    return false;
                IEnumerable<XElement> xmlSequenceList = xmlSequences.Elements();
                foreach (XElement xmlSequence in xmlSequenceList) {
                    TSequence sequence = new TSequence();
                    sequence.animation = this;
                    if (!sequence.parseXml(xmlSequence))
                        return false;
                    sequences.Add(sequence);
                }

                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public XElement toXml()
        {
            return
                new XElement("Animation",
                    new XElement("Event", eventu),
                    new XElement("State", state),
                    new XElement("Sequences",
                        from sequence in sequences
                        select sequence.toXml()
                    )
                );
        }

        public int numberOfSequences()
        {
            return sequences.Count;
        }

        public TSequence sequenceAtIndex(int index)
        {
            if (index >= 0 && index < sequences.Count)
                return sequences[index];
            return null;
        }

        public TSequence addSequence()
        {
            TSequence sequence = new TSequence();
            sequence.animation = this;
            sequences.Add(sequence);

            return sequence;
        }

        public void addSequence(TSequence sequence)
        {
            sequence.animation = this;
            sequences.Add(sequence);
        }

        public void removeSequence(int index)
        {
            if (index >= 0 && index < sequences.Count)
                sequences.RemoveAt(index);
        }

        public int numberOfActionsInSequence(int sequenceIndex)
        {
            if (sequenceIndex >= 0 && sequenceIndex < sequences.Count) {
                return sequences[sequenceIndex].numberOfActions();
            }

            return 0;
        }

        public TAction actionAtIndex(int sequenceIndex, int actionIndex)
        {
            if (sequenceIndex >= 0 && sequenceIndex < sequences.Count) {
                TSequence sequence = sequences[sequenceIndex];
                return sequence.actionAtIndex(actionIndex);
            }

            return null;
        }

        public void insertAction(int sequenceIndex, int actionIndex, TAction action)
        {
            if (sequenceIndex >= 0 && sequenceIndex < sequences.Count) {
                sequences[sequenceIndex].insertAction(actionIndex, action);
            }
        }

        public void deleteAction(int sequenceIndex, int actionIndex)
        {
            if (sequenceIndex >= 0 && sequenceIndex < sequences.Count) {
                sequences[sequenceIndex].deleteAction(actionIndex);
            }
        }

        public bool isUsingImage(string img)
        {
            foreach (TSequence sequence in sequences) {
                if (sequence.isUsingImage(img))
                    return true;
            }

            return false;
        }

        public bool isUsingSound(string snd)
        {
            foreach (TSequence sequence in sequences) {
                if (sequence.isUsingSound(snd))
                    return true;
            }

            return false;
        }

        #endregion

        #region Launch Methods

        public void start()
        {
            run_executing = true;
            for (int i = 0; i < sequences.Count; i++) {
                sequences[i].start();
            }
        }

        public void stop()
        {
            run_executing = false;
        }

        public void step(FrmEmulator emulator, long time)
        {
            bool progressing = false;
            for (int i = 0; i < sequences.Count; i++) {
                progressing |= sequences[i].step(emulator, time);
            }

            if (!progressing)
                this.stop();
        }

        #endregion

        #region ITimeLineDataSource Interface Methods

        public int numberOfRows()
        {
            return numberOfSequences();
        }

        public float totalDuration()
        {
            float ret = 0;
            for (int i = 0; i < sequences.Count; i++) {
                float duration = sequences[i].totalDuration();
                if (duration > ret)
                    ret = duration;
            }

            return ret;
        }

        public int numberOfItemsInRow(int rowIndex)
        {
            return numberOfActionsInSequence(rowIndex);
        }

        public bool isInstantItem(int rowIndex, int itemIndex)
        {
            if (rowIndex >= 0 && rowIndex < sequences.Count) {
                TSequence sequence = sequences[rowIndex];
                return sequence.isInstantAction(itemIndex);
            }

            return false;
        }

        public float durationOfItem(int rowIndex, int itemIndex)
        {
            if (rowIndex >= 0 && rowIndex < sequences.Count) {
                TSequence sequence = sequences[rowIndex];
                return (float)sequence.durationOfAction(itemIndex) / 1000;
            }

            return 0;
        }

        public Color startingColorOfItem(int rowIndex, int itemIndex)
        {
            if (rowIndex >= 0 && rowIndex < sequences.Count) {
                TSequence sequence = sequences[rowIndex];
                return sequence.startingColorOfAction(itemIndex);
            }

            return Color.White;
        }

        public Color endingColorOfItem(int rowIndex, int itemIndex)
        {
            if (rowIndex >= 0 && rowIndex < sequences.Count) {
                TSequence sequence = sequences[rowIndex];
                return sequence.endingColorOfAction(itemIndex);
            }

            return Color.LightGray;
        }

        public Bitmap iconOfItem(int rowIndex, int itemIndex)
        {
            if (rowIndex >= 0 && rowIndex < sequences.Count) {
                TSequence sequence = sequences[rowIndex];
                return sequence.iconOfAction(itemIndex);
            }

            return null;
        }

        public Bitmap draggingIconOfItem(int rowIndex, int itemIndex)
        {
            if (rowIndex >= 0 && rowIndex < sequences.Count) {
                TSequence sequence = sequences[rowIndex];
                return sequence.draggingIconOfAction(itemIndex);
            }

            return null;
        }

        #endregion
    }
}
