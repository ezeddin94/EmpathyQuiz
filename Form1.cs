using ImageQuizz.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageQuizz
{
    public partial class ImageQuizzForm : Form
    {
        List<string> filteredFiles;
        int counter = -1;
        bool isPlaying = false;
        string _name;
        RadioButton tempRadioButton;
        UserData user = new UserData();
        public int LastViewedImage { get; private set; }

        public ImageQuizzForm()
        {
            InitializeComponent();
            imageViewer.Visible = false;
            filteredFiles = Directory.GetFiles("Images", "*.*")
                .Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("gif")
                || file.ToLower().EndsWith("png") || file.ToLower().EndsWith("bmp")).ToList();
            imageViewer.SizeMode = PictureBoxSizeMode.Zoom;
            tempRadioButton = new RadioButton();
            unvisibleAnsower();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (isPlaying == false)
            {
                if (textBox_Name.TextLength > 0)
                {
                    user.UserName = textBox_Name.Text;
                    user.PicValuation = new List<PicValuation>();
                    _name = textBox_Name.Text;
                    visibleAnsower();
                    counter = 0;
                    imageViewer.Visible = true;
                    button_Start.Text = "Stop";
                    button_Start.BackColor = Color.Red;
                    isPlaying = true;
                    imageViewer.Image = Image.FromFile(filteredFiles[counter]);
                   // System.IO.File.WriteAllText("data.txt", string.Empty);
                }
                else
                {
                    MessageBox.Show("Please enter your name");
                }
                // imageViewer.Image = resizeImage(Image.FromFile(filteredFiles[counter]), new Size(imageViewer.Width,imageViewer.Height));
            }
            else
            {
                unvisibleAnsower();
                imageViewer.Visible = false;
                button_Start.Text = "Play";
                button_Start.BackColor = Color.Green;
                isPlaying = false;
            }
        }

        private void Ansowerd(object sender, EventArgs e)
        {
            tempRadioButton = sender as RadioButton;
            string value = "";
            bool isChecked = tempRadioButton.Checked;

            if (isChecked)
            {
                value = tempRadioButton.Text;
                if (counter >= 0)
                {
                    bool shouldBeChanged = false;
                    try
                    {
                        String json = File.ReadAllText("data.txt");
                        UserData listUserAnswers = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(json);
                        if (listUserAnswers != null)
                        {
                            if (listUserAnswers.UserName == _name)
                            {
                                foreach (var obj in listUserAnswers.PicValuation)
                                {
                                    if (obj.Id == counter && obj.ImageName == Path.GetFileName(filteredFiles[counter].ToString()))
                                    {
                                        shouldBeChanged = true;
                                        obj.value = int.Parse(value);
                                    }
                                }
                            }
                        }
                        else
                        {
                            listUserAnswers = new UserData();
                            listUserAnswers.PicValuation = new List<PicValuation>();
                        }

                        if (shouldBeChanged)
                        {
                            var json11 = JsonConvert.SerializeObject(listUserAnswers, Formatting.Indented);
                            File.WriteAllText("data.txt", json11);
                        }

                        else
                        {
                            PicValuation picValuation = new PicValuation
                            {
                                Id = counter,
                                ImageName = Path.GetFileName(filteredFiles[counter].ToString()),
                                value = int.Parse(value),
                                Time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),
                            };
                            user.PicValuation.Add(picValuation);
                            var json11 = JsonConvert.SerializeObject(user, Formatting.Indented);
                            File.WriteAllText("data.txt", json11);
                        }
                    }
                    catch (Exception ex)
                    {
                        shouldBeChanged = false;
                    }
                }
            }
        }

        private void unvisibleAnsower()
        {
            button_Start.BackColor = Color.Green;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            radioButton4.Visible = false;
            radioButton5.Visible = false;
            radioButton6.Visible = false;
            radioButton7.Visible = false;
            radioButton8.Visible = false;
            radioButton9.Visible = false;
            radioButton10.Visible = false;
            radioButton11.Visible = false;
            radioButton12.Visible = false;
            button_Next.Visible = false;
            button_previous.Visible = false;
            button_Result.Visible = false;
            textBox_Name.Visible = true;
            label_Name.Visible = true;
            label1.Visible = false;
            textBox_Name.Text = "";
        }

        private void visibleAnsower()
        {
            tempRadioButton.Checked = false;
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton3.Visible = true;
            radioButton4.Visible = true;
            radioButton5.Visible = true;
            radioButton6.Visible = true;
            radioButton7.Visible = true;
            radioButton8.Visible = true;
            radioButton9.Visible = true;
            radioButton10.Visible = true;
            radioButton11.Visible = true;
            radioButton12.Visible = true;
            button_Next.Visible = true;
            button_previous.Visible = true;
            textBox_Name.Visible = false;
            label_Name.Visible = false;
            label1.Visible = true;
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            counter += 1;
            if (counter >= filteredFiles.Count)
            {
                button_Result.Visible = true;
                imageViewer.Visible = false;
                button_Next.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
                radioButton5.Visible = false;
                radioButton6.Visible = false;
                radioButton7.Visible = false;
                radioButton8.Visible = false;
                radioButton9.Visible = false;
                radioButton10.Visible = false;
                radioButton11.Visible = false;
                radioButton12.Visible = false;
            }

            else
            {
                try
                {
                    if (tempRadioButton.IsHandleCreated)
                    {
                        tempRadioButton.Invoke((Action)(() =>
                        {
                            tempRadioButton.Checked = false;
                        }));
                    }
                }
                catch (Exception ex)
                {
                }

                //imageViewer.Image = resizeImage(Image.FromFile(filteredFiles[counter]), new Size(405, 1000));
                imageViewer.Image = Image.FromFile(filteredFiles[counter]);
                LastViewedImage = counter;
                RestoreCheckedRadioButton();
            }
        }

        private void button_previous_Click(object sender, EventArgs e)
        {
            counter -= 1;
            if (counter >= 0)
            {
                try
                {
                    tempRadioButton.Invoke((Action)(() =>
                    {
                        tempRadioButton.Checked = false;
                    }));
                }
                catch
                {

                }
                //imageViewer.Image = resizeImage(Image.FromFile(filteredFiles[counter]), new Size(405, 1000));
                RestoreCheckedRadioButton();
                button_Next.Visible = true;
                button_Result.Visible = false;
                label_Result.Visible = false;
                imageViewer.Visible = true;
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                radioButton4.Visible = true;
                radioButton5.Visible = true;
                radioButton6.Visible = true;
                radioButton7.Visible = true;
                radioButton8.Visible = true;
                radioButton9.Visible = true;
                radioButton10.Visible = true;
                radioButton11.Visible = true;
                radioButton12.Visible = true;
                imageViewer.Image = Image.FromFile(filteredFiles[counter]);
                LastViewedImage = counter;
            }
            else
            {
                button_Result.Visible = false;
                imageViewer.Visible = true;
                button_Next.Visible = true;
                counter = LastViewedImage;
            }
        }

        private void RestoreCheckedRadioButton()
        {
            String json = File.ReadAllText("data.txt");
            UserData listUserAnswers = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(json);
            if (listUserAnswers != null)
            {
                if (listUserAnswers.UserName == _name)
                {
                    List<int> vs = new List<int>();
                    foreach (var obj in listUserAnswers.PicValuation)
                    {
                        foreach (RadioButton rBtn in this.Controls.OfType<RadioButton>())
                        {
                            if (obj.Id == counter && rBtn.Text == obj.value.ToString())
                            {
                                rBtn.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void button_Result_Click(object sender, EventArgs e)
        {
            label_Result.Visible = true;
            String json = File.ReadAllText("data.txt");
            UserData listUserAnswers = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(json);
            if (listUserAnswers != null)
            {
                if (listUserAnswers.UserName == _name)
                {
                    List<int> ListInt = new List<int>();
                    double Result = 0;
                    foreach (var obj in listUserAnswers.PicValuation)
                    {
                        ListInt.Add(obj.value);
                    }

                    foreach (var i in ListInt)
                    {
                        Result += i;
                    }

                    Result = Result / ListInt.Count;
                    label_Result.Text = Result.ToString();
                }
            }
        }
    }
}
