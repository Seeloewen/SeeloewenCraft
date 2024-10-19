using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SeeloewenCraft.entity
{
    public partial class Player
    {
        public Canvas cvsBody = new Canvas();
        public Canvas cvsHead = new Canvas();
        public Canvas cvsLeftLeg = new Canvas();
        public Canvas cvsRightLeg = new Canvas();
        public Canvas cvsLeftArm = new Canvas();
        public Canvas cvsRightArm = new Canvas();

        private AnimationTimeline anLeftLeg;
        private AnimationTimeline anRightLeg;
        private AnimationTimeline anRightArm;
        private AnimationTimeline anLeftArm;

        private bool movingHorizontally = false;
        private bool animationRunning = false;

        private int legMaxSwing = 45;
        private int armMaxSwing = 45;

        public const double ANIMATION_SPEED = 400;
        public const double SPRINT_MULTIPLIER = 1.5;

        public void InitAnimations()
        {
            //Setup canvasses
            cvsLeftLeg.Height = 38;
            cvsLeftLeg.Width = 17;
            cvsLeftLeg.Background = Images.PlayerLegLeft.GetTexture();

            cvsRightLeg.Height = 38;
            cvsRightLeg.Width = 17;
            cvsRightLeg.Background = Images.PlayerLegRight.GetTexture();

            cvsRightArm.Height = 35;
            cvsRightArm.Width = 17;
            cvsRightArm.Background = Images.PlayerArmRight.GetTexture();

            cvsLeftArm.Height = 35;
            cvsLeftArm.Width = 17;
            cvsLeftArm.Background = Images.PlayerArmLeft.GetTexture();

            cvsBody.Height = 35;
            cvsBody.Width = 17;
            cvsBody.Background = Images.PlayerBody.GetTexture();

            cvsHead.Height = 22;
            cvsHead.Width = 23;
            cvsHead.Background = Images.PlayerHead.GetTexture();

            //Setup canvas locations
            Canvas.SetTop(cvsLeftLeg, 57);

            Canvas.SetTop(cvsRightLeg, 57);
            Canvas.SetZIndex(cvsRightLeg, 2);

            Canvas.SetTop(cvsRightArm, 22);
            Canvas.SetZIndex(cvsRightArm, 2);

            Canvas.SetTop(cvsLeftArm, 22);

            Canvas.SetTop(cvsBody, 22);
            Canvas.SetZIndex(cvsBody, 1);

            Canvas.SetTop(cvsHead, 0);
            Canvas.SetLeft(cvsHead, -3);

            texture.Children.Add(cvsLeftLeg);
            texture.Children.Add(cvsRightLeg);
            texture.Children.Add(cvsRightArm);
            texture.Children.Add(cvsLeftArm);
            texture.Children.Add(cvsBody);
            texture.Children.Add(cvsHead);

            //Create transforms and animations
            cvsRightLeg.RenderTransform = new RotateTransform();
            cvsLeftLeg.RenderTransform = new RotateTransform();
            cvsRightArm.RenderTransform = new RotateTransform();
            cvsLeftArm.RenderTransform = new RotateTransform();

            cvsRightLeg.RenderTransformOrigin = new Point(0.5, 0);
            cvsLeftLeg.RenderTransformOrigin = new Point(0.5, 0);
            cvsRightArm.RenderTransformOrigin = new Point(0.5, 0);
            cvsLeftArm.RenderTransformOrigin = new Point(0.5, 0);

            anRightLeg = CreateAnimation(cvsRightLeg, 0, -legMaxSwing, ANIMATION_SPEED);
            anLeftLeg = CreateAnimation(cvsLeftLeg, 0, legMaxSwing, ANIMATION_SPEED);
            anRightArm = CreateAnimation(cvsRightArm, 0, -armMaxSwing, ANIMATION_SPEED);
            anLeftArm = CreateAnimation(cvsLeftArm, 0, armMaxSwing, ANIMATION_SPEED);

            anRightLeg.Completed += (sender, eventArgs) =>
            {
                animationRunning = false;
            };
        }

        public void DoMovementAnimation()
        {
            if (movingHorizontally && !animationRunning)
            {
                //Start the animations
                BeginAnimation(cvsRightLeg, anRightLeg);
                BeginAnimation(cvsLeftLeg, anLeftLeg);
                BeginAnimation(cvsRightArm, anRightArm);
                BeginAnimation(cvsLeftArm, anLeftArm);

                //Turn the animations in the opposite direction so the part swings from left to right correctly
                TurnAnimation(anRightLeg as DoubleAnimation);
                TurnAnimation(anLeftLeg as DoubleAnimation);
                TurnAnimation(anRightArm as DoubleAnimation);
                TurnAnimation(anLeftArm as DoubleAnimation);

                animationRunning = true;
            }
        }

        public void TurnAnimation(DoubleAnimation animation)
        {
            animation.To = -animation.To;
        }

        public AnimationTimeline CreateAnimation(UIElement element, double from, double to, double duration)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = true,
            };

            return animation;
        }

        public void BeginAnimation(UIElement element, AnimationTimeline animation)
        {
            //Set the duration based on whether the player sprints or not
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(pressedSprint ? ANIMATION_SPEED / SPRINT_MULTIPLIER : ANIMATION_SPEED));

            //Begin the animation
            RotateTransform rotateTransform = element.RenderTransform as RotateTransform;
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }
    }
}
