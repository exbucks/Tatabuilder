using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TataBuilder
{
    public class TLibraryManager
    {
        TDocument Document;

        List<string> ImageFiles;
        ImageList LargeImageListThumbnails;
        ImageList SmallImageListThumbnails;
        ImageList ImageToolbarThumbnails;

        public static int LARGE_IMAGE_LIST_THUMBNAIL_WIDTH = 85;
        public static int LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT = 65;
        public static int SMALL_IMAGE_LIST_THUMBNAIL_WIDTH = 45;
        public static int SMALL_IMAGE_LIST_THUMBNAIL_HEIGHT = 35;

        List<string> SoundFiles;

        public TLibraryManager(TDocument doc)
        {
            Document = doc;

            ImageFiles = new List<string>();

            LargeImageListThumbnails = new ImageList();
            LargeImageListThumbnails.ColorDepth = ColorDepth.Depth32Bit;
            LargeImageListThumbnails.ImageSize = new Size(LARGE_IMAGE_LIST_THUMBNAIL_WIDTH, LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT);

            SmallImageListThumbnails = new ImageList();
            SmallImageListThumbnails.ColorDepth = ColorDepth.Depth32Bit;
            SmallImageListThumbnails.ImageSize = new Size(SMALL_IMAGE_LIST_THUMBNAIL_WIDTH, SMALL_IMAGE_LIST_THUMBNAIL_HEIGHT);

            ImageToolbarThumbnails = new ImageList();
            ImageToolbarThumbnails.ColorDepth = ColorDepth.Depth32Bit;
            ImageToolbarThumbnails.ImageSize = new Size(32, 32);

            SoundFiles = new List<string>();
        }

        public bool parseXml(XElement xml)
        {
            if (xml == null || xml.Name != "Libraries")
                return false;

            XElement xmlImages = xml.Element("Images");
            if (xmlImages == null)
                return false;
            IEnumerable<XElement>xmlImageList = xmlImages.Elements("Image");
            foreach (XElement xmlImage in xmlImageList) 
                addImage(xmlImage.Value);

            XElement xmlSounds = xml.Element("Sounds");
            if (xmlSounds == null)
                return false;
            IEnumerable<XElement> xmlSoundList = xmlSounds.Elements("Sound");
            foreach (XElement xmlSound in xmlSoundList)
                addSound(xmlSound.Value);

            return true;
        }

        public XElement toXml()
        {
            return 
                new XElement("Libraries",
                    new XElement("Images",
                        from img in ImageFiles
                        select new XElement("Image", img)
                    ),
                    new XElement("Sounds",
                        from snd in SoundFiles
                        select new XElement("Sound", snd)
                    )
                );
        }

        public ImageList largeImageListThumbnails() {
            return LargeImageListThumbnails;
        }

        public ImageList smallImageListThumbnails()
        {
            return SmallImageListThumbnails;
        }

        public bool addImage(string fileName) {
            if (imageIndex(fileName) == -1) {
                try {
                    float w1, h1, w2, h2;
                    Image loadedImage = Image.FromFile(Document.getImagesDirectoryPath() + "\\" + fileName);

                    if (loadedImage.Width > loadedImage.Height) {
                        w1 = LARGE_IMAGE_LIST_THUMBNAIL_WIDTH * 0.9F;
                        h1 = w1 * loadedImage.Height / loadedImage.Width;
                        w2 = SMALL_IMAGE_LIST_THUMBNAIL_WIDTH * 0.9F;
                        h2 = w2 * loadedImage.Height / loadedImage.Width;
                    } else {
                        h1 = LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT * 0.9F;
                        w1 = h1 * loadedImage.Width / loadedImage.Height;
                        h2 = SMALL_IMAGE_LIST_THUMBNAIL_HEIGHT * 0.9F;
                        w2 = h2 * loadedImage.Width / loadedImage.Height;
                    }

                    Bitmap thumbnail1 = new Bitmap(LARGE_IMAGE_LIST_THUMBNAIL_WIDTH, LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT);
                    Graphics g1 = Graphics.FromImage(thumbnail1);
                    g1.DrawRectangle(new Pen(Color.FromArgb(171, 171, 171)), 0, 0, LARGE_IMAGE_LIST_THUMBNAIL_WIDTH - 1, LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT - 1);
                    g1.DrawImage(loadedImage, (LARGE_IMAGE_LIST_THUMBNAIL_WIDTH - w1) / 2, (LARGE_IMAGE_LIST_THUMBNAIL_HEIGHT - h1) / 2, w1, h1);
                    g1.Dispose();
                    LargeImageListThumbnails.Images.Add(thumbnail1);

                    Bitmap thumbnail2 = new Bitmap(SMALL_IMAGE_LIST_THUMBNAIL_WIDTH, SMALL_IMAGE_LIST_THUMBNAIL_HEIGHT);
                    Graphics g2 = Graphics.FromImage(thumbnail2);
                    g2.DrawRectangle(new Pen(Color.FromArgb(171, 171, 171)), 0, 0, SMALL_IMAGE_LIST_THUMBNAIL_WIDTH - 1, SMALL_IMAGE_LIST_THUMBNAIL_HEIGHT - 1);
                    g2.DrawImage(loadedImage, (SMALL_IMAGE_LIST_THUMBNAIL_WIDTH - w2) / 2, (SMALL_IMAGE_LIST_THUMBNAIL_HEIGHT - h2) / 2, w2, h2);
                    g2.Dispose();
                    SmallImageListThumbnails.Images.Add(thumbnail2);

                    float w, h;
                    if (loadedImage.Width > loadedImage.Height) {
                        w = 32;
                        h = w * loadedImage.Height / loadedImage.Width;
                    } else {
                        h = 32;
                        w = h * loadedImage.Width / loadedImage.Height;
                    }

                    Bitmap thumbnail = new Bitmap(32, 32);
                    Graphics g = Graphics.FromImage(thumbnail);
                    g.DrawImage(loadedImage, (32 - w) / 2, (32 - h) / 2, w, h);
                    g.Dispose();

                    ImageToolbarThumbnails.Images.Add(thumbnail);

                    ImageFiles.Add(fileName);

                } catch (Exception) {
                    return false;
                }

                return true;
            }

            return false;
        }

        public void removeImage(int index)
        {
            if (index >= 0 && index < ImageFiles.Count) {
                LargeImageListThumbnails.Images.RemoveAt(index);
                SmallImageListThumbnails.Images.RemoveAt(index);
                ImageToolbarThumbnails.Images.RemoveAt(index);
                ImageFiles.RemoveAt(index);
            }
        }

        public int imageCount() 
        {
            return ImageFiles.Count;
        }

        public string imageFileName(int index)
        {
            if (index >= 0 && index < ImageFiles.Count)
                return ImageFiles[index];
            else
                return "";
        }

        public string imageFilePath(int index) 
        {
            if (index >= 0 && index < ImageFiles.Count)
                return Document.getImagesDirectoryPath() + "\\" + ImageFiles[index];
            else
                return "";
        }

        public Image imageForToolbarAtIndex(int index)
        {
            if (index >= 0 && index < ImageToolbarThumbnails.Images.Count)
                return ImageToolbarThumbnails.Images[index];
            else
                return null;
        }

        public int imageIndex(string filename)
        {
            for (int i = 0; i < ImageFiles.Count; i++) {
                if (ImageFiles[i] == filename)
                    return i;
            }

            return -1;
        }

        public bool addSound(string fileName)
        {
            if (soundIndex(fileName) == -1) {
                try {
                    SoundFiles.Add(fileName);
                } catch (Exception) {
                    return false;
                }

                return true;
            }

            return false;
        }

        public void removeSound(string fileName)
        {
            SoundFiles.Remove(fileName);
        }

        public int soundCount()
        {
            return SoundFiles.Count;
        }

        public string soundFileName(int index)
        {
            if (index >= 0 && index < SoundFiles.Count)
                return SoundFiles[index];
            else
                return "";
        }

        public string soundFilePath(int index)
        {
            if (index >= 0 && index < SoundFiles.Count)
                return Document.getSoundsDirectoryPath() + "\\" + SoundFiles[index];
            else
                return "";
        }

        public int soundIndex(string fileName)
        {
            for (int i = 0; i < SoundFiles.Count; i++) {
                if (SoundFiles[i] == fileName)
                    return i;
            }

            return -1;
        }
    }
}
