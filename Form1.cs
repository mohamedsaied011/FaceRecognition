using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;
namespace FaceRecognition

{
    public partial class Form1 : Form
    {
        #region variables

        #endregion
 

        private Capture VideoCapture = null;
        private Image<Bgr, Byte> currentFrame = null;
        Mat frame = new Mat();
        private bool FaceDetectionEnabled = false;
        CascadeClassifier facecascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt.xml");
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            VideoCapture = new Capture();
            VideoCapture.ImageGrabbed += processFrame;
            VideoCapture.Start();
        }

        private void processFrame(object sender, EventArgs e)
        { // step 1 video capture
            VideoCapture.Retrieve(frame, 0);
            currentFrame = frame.ToImage<Bgr, Byte>().Resize(picCapture.Width, picCapture.Height, Inter.Cubic);
            // step 2 face detection 
            if (FaceDetectionEnabled)
            {
                // convert from bgr to gray image 
                Mat grayImage = new Mat();
                CvInvoke.CvtColor(currentFrame, grayImage, ColorConversion.Bgr2Gray);
                // Enhance the image
                CvInvoke.EqualizeHist(grayImage, grayImage);
                Rectangle[] faces = facecascadeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
                // if faces detected
                if (faces.Length >0)
                {
                    foreach (var face in faces)
                    {
                        CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Blue).MCvScalar, 2);

                    }
                }
            }
            // render the video capture into the picture box piccapture
            picCapture.Image = currentFrame.Bitmap;

        }

        private void btnDetectFaces_Click(object sender, EventArgs e)
        {
            FaceDetectionEnabled = true;

        }

        private void picCapture_Click(object sender, EventArgs e)
        {

        }
    }
}
