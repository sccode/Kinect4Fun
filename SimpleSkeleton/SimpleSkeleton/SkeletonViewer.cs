using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Research.Kinect.Nui;

using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SimpleSkeleton
{
    public partial class SkeletonViewer : Form
    {
        #region Private States
        PlanarImage videoimage;
        //Kinect Runtime
        private Runtime nui;
        //Video params
        private const ImageResolution IMAGE_RESOLUTION = ImageResolution.Resolution640x480;
        private const int IMAGE_WIDTH = 640;
        private const int IMAGE_HEIGHT = 480;
        #endregion Private States

        #region Form
        public SkeletonViewer()
        {
            InitializeComponent();
            InitKinect();
        }

        // The FormClosing event handler
        private void SkeletonViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            nui.Uninitialize();
        }
        #endregion Form

        #region Kinect Init
        private void InitKinect()
        {
            // Detect Kinect
            if (Runtime.Kinects.Count <= 0)
            {
                textStatus.Text = "No Kinect!";
                return;
            }

            //use first Kinect
            nui = Runtime.Kinects[0];
            textStatus.Text = nui.Status.ToString();
            if (nui.Status != KinectStatus.Connected)  return;

            // Initialize to do skeletal tracking
            nui.Initialize(RuntimeOptions.UseSkeletalTracking
                    | RuntimeOptions.UseDepth | RuntimeOptions.UseDepthAndPlayerIndex
                    | RuntimeOptions.UseColor);

            // Open the video stream
            //  return video images from the video camera and skeleton data.
            nui.VideoStream.Open(ImageStreamType.Video, 2, IMAGE_RESOLUTION, ImageType.Color);

            // Add event to receive skeleton data
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(SkeletonFrameReady);
            nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(ImageFrameReady);

            //to experiment, toggle TransformSmooth between true & false
            // parameters used to smooth the skeleton data
            nui.SkeletonEngine.TransformSmooth = true;
            TransformSmoothParameters parameters = new TransformSmoothParameters();
            parameters.Smoothing = 0.7f;
            parameters.Correction = 0.3f;
            parameters.Prediction = 0.4f;
            parameters.JitterRadius = 1.0f;
            parameters.MaxDeviationRadius = 0.5f;
            nui.SkeletonEngine.SmoothParameters = parameters;
        }
        #endregion Kinect Init

        #region Event Handlers
        // The FrameReady event handler for displaying the image.
        void ImageFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            videoimage = e.ImageFrame.Image;
        }

        // The FrameReady event handler for showing the skeleton
        void SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            if (videoimage.Bits == null) return;
            
            SkeletonFrame allSkeletons = e.SkeletonFrame;
            textJoints.Text = "";
            // There could be at most two fully tracked players.
            foreach (SkeletonData data in allSkeletons.Skeletons) {
                // Each SkeletonData contains a collection of Joints but may not actively tracked.
                if (data.TrackingState == SkeletonTrackingState.NotTracked) continue;

                foreach (Joint joint in data.Joints)
                {
                    if (joint.TrackingState == JointTrackingState.NotTracked) continue;

                    textJoints.Text += (int)joint.ID + " " + joint.ID.ToString()
                                        + " (" + joint.TrackingState.ToString() + ")\r\n";
                    // Show joints' positions
                    Vector posJt = joint.Position;
                    textJoints.Text += "    Pos: X: " + joint.Position.X + ", Y: " + joint.Position.Y
                                        + ", Z: " + joint.Position.Z;
                    // Draw Joints
                    //  from 3D to 2D
                    float dX, dY;
                    nui.SkeletonEngine.SkeletonToDepthImage(joint.Position, out dX, out dY);
                    dX = Math.Max(0, Math.Min(dX * IMAGE_WIDTH, IMAGE_WIDTH));
                    dY = Math.Max(0, Math.Min(dY * IMAGE_HEIGHT, IMAGE_HEIGHT));
                    textJoints.Text += " => 2D X: " + (int)dX + ", Y: " + (int)dY + "\r\n";
                    DrawJoint((int)dX, (int)dY);
                    // Get pixel coodinate of the joint in the video bitmap
                    //int x, y;
                    //ImageViewArea iv = new ImageViewArea();
                    //nui.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(
                    //    IMAGE_RESOLUTION,
                    //    iv,
                    //    (int)dX, (int)dY,
                    //    (short)0,
                    //    out x, out y);
                    //textJoints.Text += "\r\nX: " + x + ", Y: " + y;
                    //DrawJoint(x, y);
                }
            }

            Bitmap bmap = PImageToBitmap(videoimage);
            videoViewer.Image = bmap;
        }
        #endregion

        #region Private Methods
        // Convert a PlanarImage to a bitmap
        private Bitmap PImageToBitmap(PlanarImage PImage)
        {
            Bitmap bmap = new Bitmap(
                PImage.Width,
                PImage.Height,
                PixelFormat.Format32bppRgb);
            BitmapData bmapdata = bmap.LockBits(
                new Rectangle(0, 0, PImage.Width,
                                PImage.Height),
                ImageLockMode.WriteOnly,
                bmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(PImage.Bits,
                0,
                ptr,
                PImage.Width *
                PImage.BytesPerPixel *
                PImage.Height);
            bmap.UnlockBits(bmapdata);
            return bmap;
        }

        private void DrawJoint(int x,int y)
        {
            if (x < 0 || y < 0 || x >= videoimage.Width || y >= videoimage.Height) return;
            // Calculate index of pixel
            int pt = (x + y * videoimage.Width) * videoimage.BytesPerPixel;
            // Draw
            for (int i = 0; i < 3; ++i) videoimage.Bits[pt + i] = 0xFF;
            int stride = videoimage.Width * videoimage.BytesPerPixel;
            if (x >= 1)
            {
                pt -= 4;
                for (int i = 0; i < 3; ++i) videoimage.Bits[pt + i] = 0xFF;
                if (y >= 1)
                {
                    pt -= stride;
                    for (int i = 0; i < 3; ++i) videoimage.Bits[pt + i] = 0xFF;
                    pt += stride;
                }
                if (y < videoimage.Height-1)
                {
                    pt += stride;
                    for (int i = 0; i < 3; ++i) videoimage.Bits[pt + i] = 0xFF;
                    pt -= stride;
                }
                pt += 4;
            }
            if (x < videoimage.Width-1)
            {
                pt += 4;
                for (int i = 0; i < 3; ++i) videoimage.Bits[pt + i] = 0xFF;
                if (y >= 1)
                {
                    pt -= stride;
                    for (int i = 0; i < 3; ++i) videoimage.Bits[pt + i] = 0xFF;
                    pt += stride;
                }
                if (y < videoimage.Height - 1)
                {
                    pt += stride;
                    for (int i = 0; i < 3; ++i) videoimage.Bits[pt + i] = 0xFF;
                    pt -= stride;
                }
            }
        }
        #endregion
    }
}
