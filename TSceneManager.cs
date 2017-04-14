using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TataBuilder
{
    public class TSceneManager
    {
        public TDocument document { get; set; }
        public int currentSceneIndex { get; set; }

        List<TScene> Scenes;
        ImageList Thumbnails;

        public TSceneManager(TDocument doc)
        {
            document = doc;
            currentSceneIndex = -1;

            Scenes = new List<TScene>();

            Thumbnails = new ImageList();
            Thumbnails.ColorDepth = ColorDepth.Depth32Bit;
            Thumbnails.ImageSize = new Size(Program.SCENE_THUMBNAIL_WIDTH, Program.SCENE_THUMBNAIL_HEIGHT);
        }

        public bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "Scenes")
                return false;

            IEnumerable<XElement> xmlSceneList = xml.Elements();
            foreach (XElement xmlScene in xmlSceneList) {
                TScene scene = new TScene(document);
                if (!scene.parseXml(xmlScene, null))
                    return false;

                addScene(scene);
            }

            return true;
        }

        public XElement toXml()
        {
            return 
                new XElement("Scenes",
                    from scene in Scenes
                    select scene.toXml()
                );
        }

        public Image thumbnailImage(int index)
        {
            if (index >= 0 && index < Scenes.Count)
                return Scenes[index].thumbnailImage();
            return null;
        }

        public string newSceneName()
        {
            int k = 0;
            for (int i = 0; i < Scenes.Count; i++) {
                Regex rgx = new Regex("^Scene_(\\d+)$");
                MatchCollection matches = rgx.Matches(Scenes[i].name);
                if (matches.Count > 0) {
                    int no = int.Parse(matches[0].Groups[1].Value);
                    if (k < no)
                        k = no;
                }

            }

            return "Scene_" + (k + 1);
        }

        public void addScene(TScene scene)
        {
            Scenes.Add(scene);
            Thumbnails.Images.Add(scene.thumbnailImage());

            if (Scenes.Count == 1)
                this.currentSceneIndex = 0;
        }

        public void insertScene(TScene scene, int index)
        {
            for (int i = index; i < Scenes.Count; i++)
                Thumbnails.Images.RemoveAt(index);
            Scenes.Insert(index, scene);
            for (int i = index; i < Scenes.Count; i++)
                Thumbnails.Images.Add(Scenes[i].thumbnailImage());
        }

        public void deleteScene(int index)
        {
            if (index >= 0 && index < Scenes.Count) {
                Scenes.RemoveAt(index);
                Thumbnails.Images.RemoveAt(index);
            }
        }

        public TScene scene(int index) 
        {
            if (index >= 0 && index < Scenes.Count) {
                return Scenes[index];
            }
            return null;
        }

        public int indexOfScene(string sceneName)
        {
            for (int i = 0; i < Scenes.Count; i++) {
                if (Scenes[i].name == sceneName)
                    return i;
            }
            return -1;
        }

        public int sceneCount()
        {
            return Scenes.Count;
        }

        public ImageList thumbnailImageList() 
        {
            return Thumbnails;
        }

        public void updateThumbnail(int index)
        {
            if (index >= 0 && index < Scenes.Count) {
                Thumbnails.Images[index] = Scenes[index].thumbnailImage();
            }
        }

        public void updateThumbnail(TScene scene)
        {
            for (int i = 0; i < Scenes.Count; i++) {
                if (Scenes[i] == scene) {
                    this.updateThumbnail(i);
                    return;
                }
            }
        }
    }
}
