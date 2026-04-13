
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Doll_Managment_Project.Properties;

namespace Doll_Managment_Project
{
    public class SoundManager
    {
        private SoundPlayer successSound;
        private SoundPlayer failureSound;

        public SoundManager()
        {
            try
            {
                // Load success sound
                var successStream = new MemoryStream();
                Resources.Success.CopyTo(successStream);
                successStream.Position = 0;
                successSound = new SoundPlayer(successStream);

                // Load failure sound
                var failureStream = new MemoryStream();
                Resources.failed.CopyTo(failureStream);
                failureStream.Position = 0;
                failureSound = new SoundPlayer(failureStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sounds: {ex.Message}", "Sound Error");
            }
        }

        public void PlaySuccessSound()
        {
            try
            {
                successSound?.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not play success sound: {ex.Message}", "Sound Error");
                SystemSounds.Beep.Play();
            }
        }

        public void PlayFailureSound()
        {
            try
            {
                failureSound?.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not play failure sound: {ex.Message}", "Sound Error");
                SystemSounds.Beep.Play();
            }
        }
    }
}
