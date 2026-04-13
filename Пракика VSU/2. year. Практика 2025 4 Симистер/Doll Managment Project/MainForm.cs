using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Doll_Managment_Project
{
    public partial class MainForm : Form
    {
        private readonly DollRenderer dollRenderer;
        private readonly SoundManager soundManager;
        private readonly CursorManager cursorManager;
        private readonly Cursor customCursor;

        public MainForm()
        {
            InitializeComponent();
            dollRenderer = new DollRenderer();
            dollRenderer = new DollRenderer();
            soundManager = new SoundManager();
            cursorManager = new CursorManager();
            customCursor = cursorManager.GetCustomCursor();
            colorDialog = new ColorDialog();

            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseClick += pictureBox_MouseClick;
            pictureBox.MouseMove += pictureBox_MouseMove;

            pictureBox.Image = Properties.Resources.girldoll;
            this.Cursor = Cursors.Default;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            dollRenderer.DrawDoll(e.Graphics, pictureBox.Width, pictureBox.Height);
        }
        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point imagePoint = new Point(
                (int)(e.X * (double)dollRenderer.ImageWidth / pictureBox.Width),
                (int)(e.Y * (double)dollRenderer.ImageHeight / pictureBox.Height));

            bool success = dollRenderer.AddSparkle(imagePoint);

            if (success)
                soundManager.PlaySuccessSound();
            else
                soundManager.PlayFailureSound();

            pictureBox.Invalidate(); // Refresh to draw the sparkle

        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point imagePoint = new Point(
                (int)(e.X * (double)dollRenderer.ImageWidth / pictureBox.Width),
                (int)(e.Y * (double)dollRenderer.ImageHeight / pictureBox.Height));

            Point[] shirtPoints = new Point[]
            {
                new Point(458, 249),  // upper left
                new Point(565, 253),  // upper right
                new Point(598, 563),  // lower right
                new Point(423, 563)   // lower left
            };
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(shirtPoints);
                if (path.IsVisible(imagePoint))
                    pictureBox.Cursor = customCursor;
                else
                    pictureBox.Cursor = Cursors.Default;
                
            }
        }
        private void btnShirtColor_Click_1(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                dollRenderer.ShirtColor = colorDialog.Color;
                pictureBox.Invalidate();
                soundManager.PlaySuccessSound();
            }
            else
                soundManager.PlayFailureSound();
        }
        private void btnSkirtColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                dollRenderer.SkirtColor = colorDialog.Color;
                pictureBox.Invalidate();
                soundManager.PlaySuccessSound();
            }
            else
                soundManager.PlayFailureSound();
        }
        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void comboFacewear_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool sound_success = true;
            switch (comboFacewear.SelectedItem.ToString())
            {
                case "None":
                    dollRenderer.SetFacewear(FacewearType.None);
                    break;
                case "Sun Glasses":
                    dollRenderer.SetFacewear(FacewearType.Glasses);
                    break;
                case "Ski Mask":
                    dollRenderer.SetFacewear(FacewearType.SkiMask);
                    break;
                case "Hockey Mask":
                    dollRenderer.SetFacewear(FacewearType.HockeyMask);
                    break;
                case "Cap":
                    dollRenderer.SetFacewear(FacewearType.Cap);
                    break;
                case "Glasses":
                    dollRenderer.SetFacewear(FacewearType.newglass);
                    break;
                case "No-facewear":
                    dollRenderer.SetFacewear(FacewearType.nofacwear);
                    break;
                default:
                    sound_success = false;
                    break;
            }

            pictureBox.Invalidate();

            if (sound_success)
                soundManager.PlaySuccessSound();
            else
                soundManager.PlayFailureSound();
        }
        private void btnsunglasses_Click(object sender, EventArgs e)
        {
            dollRenderer.SetFacewear(FacewearType.Glasses);
            pictureBox.Invalidate();
            soundManager.PlaySuccessSound();
        }
        private void btnglasses_Click(object sender, EventArgs e)
        {
            dollRenderer.SetFacewear(FacewearType.newglass);
            pictureBox.Invalidate();
            soundManager.PlaySuccessSound();
        }
        private void btnnone_Click(object sender, EventArgs e)
        {
            dollRenderer.SetFacewear(FacewearType.None);
            pictureBox.Invalidate();
            soundManager.PlaySuccessSound();
        }
        private void btnskimask_Click(object sender, EventArgs e)
        {
            dollRenderer.SetFacewear(FacewearType.SkiMask);
            pictureBox.Invalidate();
            soundManager.PlaySuccessSound();
        }
        private void btnfacewear_Click(object sender, EventArgs e)
        {
            dollRenderer.SetFacewear(FacewearType.nofacwear);
            pictureBox.Invalidate();
            soundManager.PlaySuccessSound();
        }
        private void btncap_Click(object sender, EventArgs e)
        {
            dollRenderer.SetFacewear(FacewearType.Cap);
            pictureBox.Invalidate();
            soundManager.PlaySuccessSound();
        }
        private void btnhockymask_Click(object sender, EventArgs e)
        {
            dollRenderer.SetFacewear(FacewearType.HockeyMask);
            pictureBox.Invalidate();
            soundManager.PlaySuccessSound();
        }
        private void cmboSparkle_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (cmboSparkle.SelectedItem.ToString())
            {
                case "Diamond":
                    dollRenderer.SelectedSparkleShape = SparkleShape.Diamond;
                    break;
                case "Circle":
                    dollRenderer.SelectedSparkleShape = SparkleShape.Circle;
                    break;
                case "Heart":
                    dollRenderer.SelectedSparkleShape = SparkleShape.Heart;
                    break;
            }
        }

        private void btnDeleteRhinestones_Click_1(object sender, EventArgs e)
        {
            bool removed = dollRenderer.ClearSparkles();

            if (removed)
            {
                pictureBox.Invalidate();
                soundManager.PlaySuccessSound();
            }
            else
            {
                soundManager.PlayFailureSound();
                MessageBox.Show("No sparkles to remove.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
