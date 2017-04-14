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
    public class TActionIntervalAnimate : TActionInterval
    {

        // milliseconds
        public override long duration
        {
            get
            {
                if (frames == null)
                    return 0;

                long t = 0;
                for (int i = 0; i < frames.Count; i++)
                    t += frames[i].duration;

                return t;
            }

            set
            {
                if (frames != null && frames.Count > 0) {
                    long t = 0;
                    for (int i = 0; i < frames.Count; i++)
                        t += frames[i].duration;

                    if (t > 0) {
                        float r = (float)value / t;
                        t = 0;
                        for (int i = 0; i < frames.Count - 1; i++) {
                            frames[i].duration = (long)(frames[i].duration * r);
                            t += frames[i].duration;
                        }

                        frames[frames.Count - 1].duration = value - t;
                    } else {
                        t = 0;
                        long dt = (long)(value / frames.Count);
                        
                        for (int i = 0; i < frames.Count - 1; i++) {
                            frames[i].duration = dt;
                            t += dt;
                        }

                        frames[frames.Count - 1].duration = value - t;
                    }
                }
            }
        }

        public List<TAnimateFrame> frames { get; set; }

        [NonSerialized]
        private int run_currentFrame;

        public TActionIntervalAnimate()
        {
            name = "Animate";
            startingColor = Color.FromArgb(255, 255, 255);
            endingColor = Color.FromArgb(234, 234, 234);
            icon = Properties.Resources.icon_action_animate;

            frames = new List<TAnimateFrame>();
        }

        protected override void clone(TAction target)
        {
            base.clone(target);

            TActionIntervalAnimate targetAction = (TActionIntervalAnimate)target;
            targetAction.frames.AddRange(this.frames);
        }

        public override bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "ActionIntervalAnimate")
                return false;

            if (!base.parseXml(xml))
                return false;

            try {
                XElement xmlFrames = xml.Element("Frames");
                IEnumerable<XElement> xmlFrameList = xmlFrames.Elements();
                foreach (XElement xmlFrame in xmlFrameList) {
                    string image = TUtil.parseStringXElement(xmlFrame.Element("Image"), "");
                    long duration = TUtil.parseLongXElement(xmlFrame.Element("Duration"), -1);
                    if (string.IsNullOrEmpty(image) || duration == -1)
                        return false;

                    frames.Add(new TAnimateFrame{ image = image, duration = duration });
                }
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override XElement toXml()
        {
            XElement xml = base.toXml();
            xml.Name = "ActionIntervalAnimate";
            xml.Add(
                new XElement("Frames",
                    from frame in frames
                    select 
                        new XElement("Frame", 
                            new XElement("Image", frame.image),
                            new XElement("Duration", frame.duration)
                        )
                )
            );

            return xml;
        }

        public override bool isUsingImage(string img)
        {
            foreach (TAnimateFrame frame in frames) {
                if (frame.image.Equals(img))
                    return true;
            }
            return false;
        }

        #region Launch Methods

        public override void reset(long time)
        {
            base.reset(time);

            run_currentFrame = -1;
        }

        // execute action for every frame
        // if action is finished, return true;
        public override bool step(FrmEmulator emulator, long time)
        {
            TLayer layer = sequence.animation.layer;
            if (frames.Count > 0 && layer is TImageActor) {
                float elapsed = time - run_startTime;
                if (elapsed > duration)
                    elapsed = duration;

                long t = 0;
                int index = 0;
                while (t < elapsed && index < frames.Count)
                    t += frames[index++].duration;

                if (index > 0)
                    index--;

                if (index != run_currentFrame) {
                    run_currentFrame = index;
                    string image = frames[index].image;

                    TImageActor target = (TImageActor)layer;
                    TLibraryManager libraryManager = target.document.libraryManager;
                    int libImageIndex = libraryManager.imageIndex(image);
                    if (libImageIndex != -1) {
                        target.loadImage(Image.FromFile(libraryManager.imageFilePath(libImageIndex)));
                    }
                }

            }

            return base.step(emulator, time);
        }

        #endregion
    }

    [Serializable]
    public class TAnimateFrame
    {
        public string image { get; set; }
        public long duration { get; set; }
    }
}
