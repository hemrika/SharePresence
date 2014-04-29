// -----------------------------------------------------------------------
// <copyright file="FFMpegProcessWrapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Video
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.SharePoint;
    using System.Text.RegularExpressions;

    public interface IFFMpegProcessWrapper
    {
        //void Execute();
        //string VideoName { get; set; }
        //string Identifier { get; set; }
        void GetVideoPoster(VideoFile video);
        void GetVideoInfo(VideoFile video);
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FFMpegProcessWrapper : IFFMpegProcessWrapper
    {
        internal VideoSettings settings = null;

        public FFMpegProcessWrapper()
        {
            settings = new VideoSettings();
            if (SPContext.Current != null)
            {
                settings = settings.Load();
            }

        }
        public FFMpegProcessWrapper(SPContext context)
        {
            settings = new VideoSettings();
            settings.Context = context;
            settings = settings.Load();
        }

        internal void GetVideoPoster(VideoFile video)
        {
            if (settings == null)
            {
                settings = new VideoSettings();
                settings = settings.Load();
            }

            StringBuilder argsBuilder = new StringBuilder();

            argsBuilder.AppendFormat(" -i \"{0}\"", Path.Combine(settings.InputVideoPath, video.Path));
            argsBuilder.AppendFormat(" -ss {0}", settings.StartTimeOffset);
            argsBuilder.Append(" -vframes 1");
            argsBuilder.Append(" -f image2");
            argsBuilder.AppendFormat(" -y \"{0}\"", Path.Combine(settings.OutputImagePath, video.UniqueId.ToString() + ".jpg"));

            /*
            ProcessStartInfo oInfo = new ProcessStartInfo();
            oInfo.FileName = string.Format("\"{0}\"", Path.Combine(settings.FFMpegPath, "ffmpeg.exe"));
            //oInfo.WorkingDirectory = Path.GetDirectoryName(this.FFmpegPath);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            using (Process ffmpegProcess = System.Diagnostics.Process.Start(oInfo))
            {
                using (StreamReader srOutput = ffmpegProcess.StandardError)
                {
                    System.Text.StringBuilder output = new System.Text.StringBuilder();

                    using (StreamReader objStreamReader = ffmpegProcess.StandardError)
                    {
                        System.Text.StringBuilder sbOutPut = new StringBuilder();

                        while (!ffmpegProcess.WaitForExit(settings.ProcessTimeout * 1000))
                        {
                            sbOutPut.Append(objStreamReader.ReadToEnd().ToString());
                        }

                        if (sbOutPut.Length == 0)
                        {
                            sbOutPut.Append(objStreamReader.ReadToEnd().ToString());
                        }

                        if (ffmpegProcess.ExitCode == 0)
                        {
                            ffmpegProcess.Close();
                            if (objStreamReader != null)
                            {
                                objStreamReader.Close();
                            }
                        }
                        else
                        {
                            ffmpegProcess.Close();
                            if (objStreamReader != null)
                            {
                                objStreamReader.Close();
                            }
                        }
                        //return sbOutPut.ToString();
                    }

                }
            }
            */

            // Create a process to generate the preview image.
            using (Process ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo.FileName = string.Format("\"{0}\"", Path.Combine(settings.FFMpegPath, "ffmpeg.exe"));
                ffmpegProcess.StartInfo.Arguments = argsBuilder.ToString();
                ffmpegProcess.StartInfo.UseShellExecute = false;
                ffmpegProcess.StartInfo.RedirectStandardOutput = true;
                ffmpegProcess.StartInfo.RedirectStandardError = true;

                // Execute the process.
                ffmpegProcess.Start();

                ffmpegProcess.WaitForExit(settings.ProcessTimeout * 1000);

                // Kill the process if necessary.
                if (ffmpegProcess.HasExited == false)
                {
                    ffmpegProcess.Kill();
                }
            }
        }

        internal void GetVideoInfo(VideoFile video)
        {
            if (settings == null)
            {
                settings = new VideoSettings();
                settings = settings.Load();
            }

            string output = string.Empty;

            // Create the ffmpeg.exe command line.
            StringBuilder argsBuilder = new StringBuilder();

            argsBuilder.AppendFormat(" -i \"{0}\"", Path.Combine(settings.InputVideoPath, video.Path));

            /*
            ProcessStartInfo oInfo = new ProcessStartInfo();
            oInfo.FileName = string.Format("\"{0}\"", Path.Combine(settings.FFMpegPath, "ffmpeg.exe"));
            //oInfo.WorkingDirectory = Path.GetDirectoryName(this.FFmpegPath);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;
            */
            /*
            using (Process ffmpegProcess = System.Diagnostics.Process.Start(oInfo))
            {
                using (StreamReader srOutput = ffmpegProcess.StandardError)
                {
                    System.Text.StringBuilder output = new System.Text.StringBuilder();

                    using (StreamReader objStreamReader = ffmpegProcess.StandardError)
                    {
                        System.Text.StringBuilder sbOutPut = new StringBuilder();

                        while (!ffmpegProcess.WaitForExit(settings.ProcessTimeout * 1000))
                        {
                            sbOutPut.Append(objStreamReader.ReadToEnd().ToString());                            
                        }

                        if (sbOutPut.Length == 0)
                        {
                            sbOutPut.Append(objStreamReader.ReadToEnd().ToString());
                        }

                        if (ffmpegProcess.ExitCode == 0)
                        {
                            ffmpegProcess.Close();
                            if (objStreamReader != null)
                            {
                                objStreamReader.Close();
                            }
                        }
                        else
                        {
                            ffmpegProcess.Close();
                            if (objStreamReader != null)
                            {
                                objStreamReader.Close();
                            }
                        }

                        string soutput = sbOutPut.ToString();
                        if (!string.IsNullOrEmpty(soutput))
                        {
                            video.RawInfo = soutput;
                            video.Duration = ExtractDuration(video.RawInfo);
                            video.BitRate = ExtractBitrate(video.RawInfo);
                            video.RawAudioFormat = ExtractRawAudioFormat(video.RawInfo);
                            video.AudioFormat = ExtractAudioFormat(video.RawAudioFormat);
                            video.RawVideoFormat = ExtractRawVideoFormat(video.RawInfo);
                            video.VideoFormat = ExtractVideoFormat(video.RawVideoFormat);
                            video.Width = ExtractVideoWidth(video.RawInfo);
                            video.Height = ExtractVideoHeight(video.RawInfo);
                            video.infoGathered = true;
                        }
                        else
                        {
                            video.infoGathered = false;
                        }

                    }

                }
            }
            */
            
            using (Process ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo.FileName = string.Format("\"{0}\"", Path.Combine(settings.FFMpegPath, "ffmpeg.exe"));
                ffmpegProcess.StartInfo.Arguments = argsBuilder.ToString();
                ffmpegProcess.StartInfo.UseShellExecute = false;
                ffmpegProcess.StartInfo.RedirectStandardOutput = true;
                ffmpegProcess.StartInfo.RedirectStandardError = true;

                ffmpegProcess.Start();

                ffmpegProcess.WaitForExit(settings.ProcessTimeout * 1000);

                // Kill the process if necessary.
                if (ffmpegProcess.HasExited == false)
                {
                    ffmpegProcess.Kill();
                }
                output = ffmpegProcess.StandardError.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(output))
            {
                video.RawInfo = output;
                video.Duration = ExtractDuration(video.RawInfo);
                video.BitRate = ExtractBitrate(video.RawInfo);
                video.RawAudioFormat = ExtractRawAudioFormat(video.RawInfo);
                video.AudioFormat = ExtractAudioFormat(video.RawAudioFormat);
                video.RawVideoFormat = ExtractRawVideoFormat(video.RawInfo);
                video.VideoFormat = ExtractVideoFormat(video.RawVideoFormat);
                video.Width = ExtractVideoWidth(video.RawInfo);
                video.Height = ExtractVideoHeight(video.RawInfo);
                video.infoGathered = true;
            }
            else
            {
                video.infoGathered = false;
            }

        }

        private TimeSpan ExtractDuration(string rawInfo)
        {
            TimeSpan t = new TimeSpan(0);
            Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);

            if (m.Success)
            {
                string duration = m.Groups[1].Value;
                string[] timepieces = duration.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4)
                {
                    t = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
                }
            }

            return t;
        }

        private double ExtractBitrate(string rawInfo)
        {
            Regex re = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            double kb = 0.0;
            if (m.Success)
            {
                Double.TryParse(m.Groups[1].Value, out kb);
            }
            return kb;
        }

        private string ExtractRawAudioFormat(string rawInfo)
        {
            string a = string.Empty;
            Regex re = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                a = m.Value;
            }
            return a.Replace("Audio: ", "");
        }

        private string ExtractAudioFormat(string rawAudioFormat)
        {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Audio: ", "");
        }

        private string ExtractRawVideoFormat(string rawInfo)
        {
            string v = string.Empty;
            Regex re = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                v = m.Value;
            }
            return v.Replace("Video: ", ""); ;
        }

        private string ExtractVideoFormat(string rawVideoFormat)
        {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Video: ", "");
        }

        private int ExtractVideoWidth(string rawInfo)
        {
            int width = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out width);
            }
            return width;
        }

        private int ExtractVideoHeight(string rawInfo)
        {
            int height = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[2].Value, out height);
            }
            return height;
        }

        void IFFMpegProcessWrapper.GetVideoPoster(VideoFile video)
        {
            GetVideoPoster(video);
        }

        void IFFMpegProcessWrapper.GetVideoInfo(VideoFile video)
        {
            GetVideoInfo(video);
        }
    }
}
