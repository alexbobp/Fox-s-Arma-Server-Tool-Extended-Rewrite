﻿using AutoUpdaterDotNET;
using FASTER.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace FASTER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _instance;
        public  bool    InstallSteamCmd = false;
        private bool    _cancelled;
        private Process _oProcess = new Process();

        public MainWindow()
        {
            Initialized += MainWindow_Initialized;
            Properties.Options.Default.Reload();
            InitializeComponent();
            IWindowDragBar.MouseDown += WindowDragBar_MouseDown;
            Properties.Options.Default.PropertyChanged += Default_PropertyChanged;
            
            //this.Loaded += MainWindow_Initialized;
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;

            IMessageDialogClose.Click += IMessageDialogClose_Click;
            ISteamUserBox.LostFocus += ISteamSettings_Changed;
            ISteamPassBox.LostFocus += ISteamSettings_Changed;
            IServerDirBox.LostFocus += ISteamSettings_Changed;
            ISteamDirBox.LostFocus += ISteamSettings_Changed;
            IServerBranch.LostFocus += ISteamSettings_Changed;
            IServerDirBox.TextChanged += IServerDirBox_TextChanged;
            IToolsDialog.MouseLeftButtonUp += IToolsDialog_MouseLeftButtonUp;
            ISteamUpdaterTabSelect.Selected += MenuItem_Selected;
            ISteamModsTabSelect.Selected += MenuItem_Selected;
            ISettingsTabSelect.Selected += MenuItem_Selected;
            IAboutTabSelect.Selected += MenuItem_Selected;
            ILocalModsTabSelect.Selected += MenuItem_Selected;
            IToolsDialog.KeyUp += IToolsDialog_KeyUp;
            IMessageDialog.KeyUp += IMessageDialog_KeyUp;
            ISteamGuardDialog.KeyUp += ISteamGuardDialog_KeyUp;
            INewServerProfileDialog.KeyUp += INewServerProfileDialog_KeyUp;
            MouseDown += IDialog_LostFocus;
            ISteamOutputBox.MouseLeftButtonDown  += IDialog_LostFocus;

            try
            {
                if (Properties.Options.Default.checkForAppUpdates)
                {
                    AutoUpdater.ReportErrors = true;
                    AutoUpdater.LetUserSelectRemindLater = false;
                    AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Minutes;
                    AutoUpdater.RemindLaterAt = 1;
                    AutoUpdater.RunUpdateAsAdmin = true;
                    AutoUpdater.Start("https://raw.githubusercontent.com/Foxlider/Fox-s-Arma-Server-Tool-Extended-Rewrite/master/FASTER_Version.xml");
                }
            }
            catch
            {
                MessageBox.Show(
                    @"There is a problem reaching update server please check your internet connection and try again later.",
                    @"Update check failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Properties.Options.Default.Save();
        }

        /// <summary>
        ///     Gets the one and only instance.
        /// </summary>
        public static MainWindow Instance => _instance ??= new MainWindow();

        #region event handlers
        
        #region Custom Window Bar Click events
        private void WindowCloseButton_Click(object sender, RoutedEventArgs e)
        { Close(); }

        private void WindowMinimizeButton_Click(object sender, RoutedEventArgs e)
        { WindowState = WindowState.Minimized; }

        private void IWindowMaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
            }
        }

        private void WindowDragBar_MouseDown(object sender, MouseButtonEventArgs e)
        { DragMove(); }

        private void ToolsButton_Click(object sender, RoutedEventArgs e)
        {
            IToolsDialog.IsOpen = true;
            BlurEffect bme = new BlurEffect();
            MainGrid.Effect = bme;
        }
        #endregion
        
        #region WindowEvents
        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            Functions.CheckSettings();
            LoadServerProfiles();
            LoadSteamUpdaterSettings();
        }

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            //FIX for issue #22 : not necessary
            //CheckAdmin();

            LoadSteamUpdaterSettings();

            if (InstallSteamCmd)
            { InstallSteam(); }
        }

        private void MainWindow_Closing(object sender, EventArgs e)
        {
            //UpdateSteamUpdaterSettings();
            Properties.Options.Default.Save();

            //if(Properties.Options.Default.clearSettings)
            //{ Properties.Options.Default.Reset(); }
        }
        #endregion

        // Opens folder select dialog when clicking certain buttons
        private void DirButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (Equals(sender, ISteamDirButton))
            {
                string path = SelectFolder(Properties.Options.Default.steamCMDPath);
                if (path != null)
                {
                    ISteamDirBox.Text = path;
                    ISteamDirBox.Focus();
                }
            }
            else if (Equals(sender, IServerDirButton))
            {
                string path = SelectFolder(Properties.Options.Default.serverPath);
                if (path != null)
                {
                    IServerDirBox.Text = path;
                    IServerDirBox.Focus();
                }
            }
        }
        // Handles when a user presses the cancel button
        private void ISteamUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string steamCmd = ISteamDirBox.Text + @"\steamcmd.exe";

            var branch = IServerBranch.Text == "Stable" ? "233780" : "107410 -beta development";

            var steamCommand = "+login " + ISteamUserBox.Text + " " + ISteamPassBox.Password + " +force_install_dir \"" + IServerDirBox.Text + "\" +app_update " + branch + " validate +quit";
            
            RunSteamCommand(steamCmd, steamCommand, "server");
        }

        private void ISteamCancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _oProcess?.Kill();
                _cancelled = true;
            }
            catch (Exception ex)
            { MessageBox.Show($"CancelUpdateButton - An exception occurred:\n{ex.Message}", "Error"); }
        }

        private void NewServerProfileButton_Click(object sender, RoutedEventArgs e)
        {
            INewServerProfileDialog.IsOpen = true;
            BlurEffect bme = new BlurEffect();
            MainGrid.Effect = bme;
        }

        private void INewServerProfileDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                INewServerProfileDialog.IsOpen = false;
                MainGrid.Effect = null;
                INewProfileName.Text = string.Empty;
            }
        }

        private void IDialog_LostFocus(object sender, MouseButtonEventArgs e)
        {
            if (!ISteamGuardDialogContent.IsMouseOver && ISteamGuardDialog.IsOpen)
            {
                ISteamGuardDialog.IsOpen = false;
                MainGrid.Effect = null;
                ISteamGuardCode.Text     = string.Empty;
            }
            if (!IToolsDialogContent.IsMouseOver && IToolsDialog.IsOpen)
            {
                IToolsDialog.IsOpen = false;
                MainGrid.Effect = null;
            }
            if (!ImessageDialogContent.IsMouseOver && IMessageDialog.IsOpen)
            {
                IMessageDialog.IsOpen = false;
                MainGrid.Effect = null;
            }
            if (!INewServerProfileDialogContent.IsMouseOver && INewServerProfileDialog.IsOpen)
            {
                INewServerProfileDialog.IsOpen = false;
                MainGrid.Effect = null;
            }
        }

        private void ISteamGuardDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ISteamGuardDialog.IsOpen = false;
                MainGrid.Effect = null;
                ISteamGuardCode.Text     = string.Empty;
            }
        }

        private void IMessageDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                IMessageDialog.IsOpen = false;
                MainGrid.Effect = null;
            }
        }

        private void IMessageDialogClose_Click(object sender, RoutedEventArgs e)
        {
            IMessageDialog.IsOpen = false;
            MainGrid.Effect       = null;
        }

        private void IToolsDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                IToolsDialog.IsOpen = false;
                MainGrid.Effect = null;
            }
        }

        private void ICreateProfileButton_Click(object sender, RoutedEventArgs e)
        {
            INewProfileName.Text = INewProfileName.Text.Trim();
            if (string.IsNullOrEmpty(INewProfileName.Text))
            {
                IMessageDialog.IsOpen = true;
                BlurEffect bme = new BlurEffect();
                MainGrid.Effect = bme;
                IMessageDialogText.Text = "Please use a suitable profile name.";
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Wait;
                var profileName = INewProfileName.Text;
                INewServerProfileDialog.IsOpen = false;
                MainGrid.Effect = null;
                ServerCollection.AddServerProfile(profileName, "_" + Functions.SafeName(profileName));
                INewProfileName.Text = string.Empty;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        //Handles when any menu item is selected
        private void MenuItem_Selected(object sender, RoutedEventArgs e)
        {
            var menus = new List<ListBox> { IMainMenuItems, IServerProfilesMenu, IOtherMenuItems };
            ListBoxItem lbItem = sender as ListBoxItem;
            foreach (var list in menus)
            {
                foreach (ListBoxItem item in list.Items)
                {
                    if (item.Name != lbItem?.Name)
                    { item.IsSelected = false; } 
                } 
            }

            foreach (TabItem item in IMainContent.Items)
            {
                if (item.Name == lbItem?.Name.Replace("Select", ""))
                { IMainContent.SelectedItem = item; } 
            }
        }

        private void InstallSteamCmd_Click(object sender, RoutedEventArgs e)
        {
            IToolsDialog.IsOpen = false;
            MainGrid.Effect = null;
            InstallSteam();
        }

        private void OpenArmaServerLocation_Click(object sender, RoutedEventArgs e)
        {
            IToolsDialog.IsOpen = false;
            MainGrid.Effect = null;
            if (!string.IsNullOrEmpty(IServerDirBox.Text) && Directory.Exists(IServerDirBox.Text))
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo { Arguments = IServerDirBox.Text, FileName = "explorer.exe" };
                    Process.Start(startInfo);
                }
                catch
                { MessageBox.Show($" Could not open {IServerDirBox.Text}"); }
            }
            else
            { MessageBox.Show($"{IServerDirBox.Text} Directory does not exist!"); }
        }
        private void OpenSteamCmdLocation_Click(object sender, RoutedEventArgs e)
        {
            IToolsDialog.IsOpen = false;
            MainGrid.Effect = null;
            if (!string.IsNullOrEmpty(ISteamDirBox.Text) && Directory.Exists(ISteamDirBox.Text))
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo { Arguments = ISteamDirBox.Text, FileName = "explorer.exe" };
                    Process.Start(startInfo);
                }
                catch
                { MessageBox.Show($" Could not open {ISteamDirBox.Text}"); }
            }
            else
            { MessageBox.Show($"{ISteamDirBox.Text} Directory does not exist!"); }
        }

        private void IToolsDialog_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            IToolsDialog.IsOpen = false;
            MainGrid.Effect = null;
        }

        private void IServerDirBox_TextChanged(object sender, RoutedEventArgs e)
        { Properties.Options.Default.serverPath = IServerDirBox.Text; }

        private void ISubmitCode_Click(object sender, RoutedEventArgs e)
        {
            var oStreamWriter = _oProcess.StandardInput;
            Dispatcher?.Invoke(() =>
            { oStreamWriter.Write(ISteamGuardCode.Text + "\n"); });
            ISteamGuardDialog.IsOpen = false;
            MainGrid.Effect = null;
        }

        private void ISteamSettings_Changed(object sender, RoutedEventArgs e)
        { UpdateSteamUpdaterSettings(); }
        #endregion

        //FIX for issue #22 not necessary
        //private void CheckAdmin()
        //{
        //    try
        //    {
        //        using WindowsIdentity identity = WindowsIdentity.GetCurrent();
        //        WindowsPrincipal principal = new WindowsPrincipal(identity);
        //        if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
        //        {
        //            var dialogResult =
        //                MessageBox.Show(
        //                    "Application must be run as administrator",
        //                    "Error",
        //                    MessageBoxButton.OK,
        //                    MessageBoxImage.Error);
        //            //if (dialogResult == MessageBoxResult.OK)
        //            //    Close();
        //        }
        //    }
        //    catch (Exception e)
        //    { throw new ApplicationException("Unable to determine administrator or root status", e); }
        //}

        public void LoadServerProfiles()
        {
            if (Properties.Options.Default.Servers != null)
            {
                var currentProfiles = Properties.Options.Default.Servers;
                IServerProfilesMenu.Items.Clear();

                for (int i = IMainContent.Items.Count - 4; i <= 0; i++)
                    IMainContent.Items.RemoveAt(i);

                foreach (var profile in currentProfiles.ServerProfiles)
                {
                    ListBoxItem newItem = new ListBoxItem
                    {
                        Name    = profile.SafeName,
                        Content = profile.DisplayName
                    };

                    IServerProfilesMenu.Items.Add(newItem);

                    newItem.Selected += MenuItem_Selected;

                    var duplicate = false;

                    foreach (TabItem tab in IMainContent.Items)
                    {
                        if (profile.SafeName == tab.Name)
                            duplicate = true;
                    }

                    if (!duplicate)
                    {
                        var tabControls = new ServerProfile(profile);

                        TabItem newTab = new TabItem
                        {
                            Name    = profile.SafeName,
                            Content = tabControls,
                            Header  = profile.SafeName
                        };

                        IMainContent.Items.Add(newTab);
                    }
                }
            }
        }

        public bool ReadyToUpdate()
        {
            return !string.IsNullOrEmpty(ISteamDirBox.Text) 
                && !string.IsNullOrEmpty(ISteamUserBox.Text) 
                && !string.IsNullOrEmpty(ISteamPassBox.Password) 
                && !string.IsNullOrEmpty(IServerDirBox.Text) 
                && File.Exists(Properties.Options.Default.steamCMDPath + "\\steamcmd.exe");
        }

        // Opens Folder select dialog and returns selected path
        public string SelectFolder(string default_folder = "")
        {
            var dlg = new CommonOpenFileDialog
            {
                Title                     = "Select the folder",
                IsFolderPicker            = true,
                AddToMostRecentlyUsedList = false,
                InitialDirectory          = default_folder,
                DefaultDirectory          = default_folder,
                AllowNonFileSystemItems   = false,
                EnsureFileExists          = true,
                EnsurePathExists          = true,
                EnsureReadOnly            = false,
                EnsureValidNames          = true,
                Multiselect               = false,
                ShowPlacesList            = true
            };

            return dlg.ShowDialog() == CommonFileDialogResult.Ok 
                ? dlg.FileName 
                : null;
        }

        private void UpdateSteamUpdaterSettings()
        {
            Properties.Options.Default.steamCMDPath  = ISteamDirBox.Text;
            Properties.Options.Default.steamUserName = ISteamUserBox.Text;
            Properties.Options.Default.steamPassword = Encryption.Instance.EncryptData(ISteamPassBox.Password);
            Properties.Options.Default.serverPath    = IServerDirBox.Text;
            Properties.Options.Default.serverBranch  = IServerBranch.Text;
        }

        private void LoadSteamUpdaterSettings()
        {
            ISteamDirBox.Text      = Properties.Options.Default.steamCMDPath;
            ISteamUserBox.Text     = Properties.Options.Default.steamUserName;
            ISteamPassBox.Password = Encryption.Instance.DecryptData(Properties.Options.Default.steamPassword);
            IServerDirBox.Text     = Properties.Options.Default.serverPath;
            IServerBranch.Text     = Properties.Options.Default.serverBranch;
        }

        private void InstallSteam()
        {
            if (string.IsNullOrEmpty(ISteamDirBox.Text))
            {
                Instance.IMessageDialog.IsOpen   = true;
                Instance.IMessageDialogText.Text = "Please make sure you have set a valid path for SteamCMD.";
            }
            else if (!File.Exists(Properties.Options.Default.steamCMDPath + "\\steamcmd.exe"))
            {
                IMessageDialog.IsOpen = true;
                BlurEffect bme = new BlurEffect();
                MainGrid.Effect = bme;
                IMessageDialogText.Text = "Steam CMD will now download and start the install process. If prompted please enter your Steam Guard " +
                                          "Code.\n\nYou will receive this by email from steam. When this is all complete type \'quit\' to finish.";
                ISteamOutputBox.Document.Blocks.Clear();
                ISteamOutputBox.AppendText("Installing SteamCMD");
                ISteamOutputBox.AppendText("\nFile Downloading...");
                const string url      = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
                string       fileName = Properties.Options.Default.steamCMDPath + "\\steamcmd.zip";
                if (!Directory.Exists(Properties.Options.Default.steamCMDPath)) Directory.CreateDirectory(Properties.Options.Default.steamCMDPath);
                WebClient client = new WebClient();
                client.DownloadFileCompleted += SteamDownloadCompleted;
                client.DownloadFileAsync(new Uri(url), fileName);
            }
            else
            {
                Instance.IMessageDialog.IsOpen = true;
                BlurEffect bme = new BlurEffect();
                MainGrid.Effect = bme;
                Instance.IMessageDialogText.Text = "SteamCMD already appears to be installed.\n\nPlease delete all files in the selected folder to reinstall.";
            }
        }

        private void SteamDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ISteamOutputBox.AppendText(Environment.NewLine + "Download Finished");

            var steamPath = Properties.Options.Default.steamCMDPath;
            string zip = steamPath + "\\steamcmd.zip";

            ISteamOutputBox.AppendText("\nUnzipping...");
            ZipFile.ExtractToDirectory(zip, steamPath);
            ISteamOutputBox.AppendText("\nInstalling...");
            RunSteamCommand(steamPath + "\\steamcmd.exe", "+login anonymous +quit", "install");

            File.Delete(zip);
        }

        private static bool   _runLog;
        private static object _runLogLock = new object();
        private static int threadSlept;
        private void UpdateTextBox(string text)
        {
            if (_oProcess != null)
            {
                Dispatcher?.Invoke(() =>
                {
                    ISteamOutputBox.AppendText(text + "\n");
                    ISteamOutputBox.ScrollToEnd();
                });

                if (text.StartsWith("Logging in user") && text.Contains("to Steam"))
                {
                    _runLog = true;
                    Thread t = new Thread(() =>
                    {
                        threadSlept = 0;
                        bool _localRunThread;
                        do
                        {
                            Thread.Sleep(500);
                            threadSlept += 500;
                            lock (_runLogLock)
                            { _localRunThread = _runLog; }
                        }
                        while (_localRunThread && threadSlept < 10000);
                        if (_localRunThread)
                        {
                            Dispatcher?.Invoke(() =>
                            {
                                ISteamGuardDialog.IsOpen = true;
                                BlurEffect bme = new BlurEffect();
                                MainGrid.Effect = bme;
                            });
                        }
                    });
                    t.Start();
                }

                if (text.Contains("Logged in OK"))
                {
                    lock (_runLogLock)
                    { _runLog = false; }
                }

                if (text.StartsWith("Retrying..."))
                { threadSlept = 0; }

                if (text.EndsWith("..."))
                {
                    Dispatcher?.Invoke(() =>
                    {
                        ISteamOutputBox.AppendText(Environment.NewLine);
                    });
                }

                if (text.Contains("Two-factor code") )
                {
                    Dispatcher?.Invoke(() =>
                    {
                        ISteamGuardDialog.IsOpen = true;
                        BlurEffect bme = new BlurEffect();
                        MainGrid.Effect = bme;
                    });
                }

                if (text.Contains("Update state"))
                {
                    int    counter  = text.IndexOf(":", StringComparison.Ordinal);
                    string progress = text.Substring(counter + 2, 2);
                    int    progressValue;
                    if (progress.Contains(".")) { int.TryParse(progress.Substring(0, 1), out progressValue); }
                    else { int.TryParse(progress,                                        out progressValue); }

                    Dispatcher?.Invoke(() =>
                    {
                        ISteamProgressBar.IsIndeterminate = false;
                        ISteamProgressBar.Value = progressValue;
                    });
                }

                if (text.Contains("Success"))
                {
                    Dispatcher?.Invoke(() =>
                    {
                        ISteamProgressBar.Value = 100;
                    });
                }

                if (text.Contains("Timeout"))
                {
                    Dispatcher?.Invoke(() =>
                    {
                        Instance.IMessageDialog.IsOpen = true;
                        BlurEffect bme = new BlurEffect();
                        MainGrid.Effect = bme;
                        Instance.IMessageDialogText.Text = "A Steam Download timed out. You may have to download again when task is complete.";
                    });
                }

            }
        }

        //private void ProcessOutputCharacters(StreamReader streamReader)
        //{
        //    int    outputCharInt;
        //    char   outputChar;
        //    string line = String.Empty;

        //    while (!streamReader.EndOfStream)
        //    {
        //        outputCharInt = streamReader.Read();
        //        if ((outputCharInt != -1))
        //        {
        //            outputChar = ((char)(outputCharInt));
        //            if (((outputCharInt == 10)
        //              || (outputCharInt == 13)))
        //            {
        //                if (!string.IsNullOrEmpty(line))
        //                {
        //                    if (!line.Contains("\\src\\common\\contentmanifest.cpp (650) : Assertion Failed: !m_bIsFinalized*"))
        //                    { UpdateTextBox(line); } 
        //                }
        //                line = String.Empty;
        //            }
        //            else if ((line.Length > 7))
        //            {
        //                if ((line.Substring((line.Length - 3)) == " .."))
        //                {
        //                    line = (Environment.NewLine + line);
        //                    UpdateTextBox(line);
        //                    line = String.Empty;
        //                }
        //                else if ((line.Substring((line.Length - 6)) == "bytes)"))
        //                {
        //                    line = (Environment.NewLine + line);
        //                    UpdateTextBox(line);
        //                    line = String.Empty;
        //                }
        //                else
        //                {
        //                    line = (line + outputChar);
        //                }
        //            }
        //            else
        //            {
        //                line = (line + outputChar);
        //            }
        //        }
        //    }
        //}

        public async void RunSteamCommand(string steamCmd, string steamCommand, string type, List<string> modIds = null)
        {
            if (ReadyToUpdate())
            {
                _oProcess = new Process();
                ISteamProgressBar.Value = 0;
                ISteamCancelButton.IsEnabled = true;
                ISteamUpdateButton.IsEnabled = false;
                IMainMenuItems.SelectedItem = null;

                var controls = FindVisualChildren<Control>(ISteamModsTab);
                var enumerable = controls.ToList();
                foreach (Control control in enumerable)
                { control.IsEnabled = false; }

                IMainContent.SelectedItem = ISteamUpdaterTab;
                var tasks = new List<Task>();

                ISteamProgressBar.IsIndeterminate = true;

                switch (type)
                {
                    case "addon":
                        ISteamOutputBox.Document.Blocks.Clear();
                        ISteamOutputBox.AppendText("Starting SteamCMD to update Addon" + Environment.NewLine + Environment.NewLine);
                        break;
                    case "server:":
                        ISteamOutputBox.Document.Blocks.Clear();
                        ISteamOutputBox.AppendText("Starting SteamCMD to update Server" + Environment.NewLine);
                        break;
                    case "install":
                        ISteamOutputBox.AppendText("Proceeding with install" + Environment.NewLine);
                        break;
                }

                tasks.Add(Task.Run(() =>
                {
                    _oProcess.StartInfo.FileName               = steamCmd;
                    _oProcess.StartInfo.Arguments              = steamCommand;
                    _oProcess.StartInfo.UseShellExecute        = false;
                    _oProcess.StartInfo.WindowStyle            = ProcessWindowStyle.Hidden;
                    _oProcess.StartInfo.CreateNoWindow         = true;
                    _oProcess.StartInfo.RedirectStandardOutput = true;
                    _oProcess.StartInfo.RedirectStandardError  = true;
                    _oProcess.StartInfo.RedirectStandardInput  = true;
                    _oProcess.EnableRaisingEvents              = true;

                    //Not realtime for some reason
                    //_oProcess.OutputDataReceived += ProcessOutputEvent;
                    //_oProcess.ErrorDataReceived += ProcessOutputEvent;

                    _oProcess.Start();

                    //NOTES
                    // SteamCMD's behaviour is quite odd. It seems like it does not keep on writing new lines but is almost constantly editing lines
                    // Editing lines does not trigger the OutputDataReceived event and nor does the Input header
                    // (When asking for user input, the programmer can add a header specifying what to enter and it is not picked by the event until the user entered its text)

                    ProcessOutputCharacters(_oProcess.StandardError);
                    ProcessOutputCharacters(_oProcess.StandardOutput);

                    //TO USE WITH _oProcess.DataReceived ONLY
                    //_oProcess.BeginErrorReadLine();
                    //_oProcess.BeginOutputReadLine();

                    _oProcess.WaitForExit();
                }));

                await Task.WhenAll(tasks);

                if (_cancelled)
                {
                    _cancelled = false;
                    ISteamProgressBar.IsIndeterminate = false;
                    ISteamProgressBar.Value = 0;

                    ISteamOutputBox.Document.Blocks.Clear();
                    ISteamOutputBox.AppendText("Process Canceled");

                    _oProcess.Close();
                    _oProcess = null;
                    CheckModUpdatesComplete(modIds);
                }
                else
                {
                    ISteamOutputBox.AppendText("SteamCMD Exited" + Environment.NewLine);
                    ISteamOutputBox.ScrollToEnd();
                    ISteamProgressBar.IsIndeterminate = false;
                    ISteamProgressBar.Value = 100;
                    BlurEffect bme = new BlurEffect();
                    switch (type)
                    {
                        case "addon":
                            CheckModUpdatesComplete(modIds);
                            break;
                        case "server:":
                            Instance.IMessageDialog.IsOpen = true;
                            MainGrid.Effect = bme;
                            Instance.IMessageDialogText.Text = "Server Installed/ Updated.";
                            break;
                        case "install":
                            Instance.IMessageDialog.IsOpen = true;
                            MainGrid.Effect = bme;
                            Instance.IMessageDialogText.Text = "SteamCMD Installed.";
                            break;
                    }
                }
                ISteamCancelButton.IsEnabled = false;
                ISteamUpdateButton.IsEnabled = true;

                foreach (Control control in enumerable)
                { control.IsEnabled = true; }

            }
            else
            {
                IMessageDialog.IsOpen = true;
                BlurEffect bme = new BlurEffect();
                MainGrid.Effect = bme;
                IMessageDialogText.Text = "Please check that SteamCMD is installed and that all fields are correct: \n\n\n"
                                        + "   -  Steam Dir\n\n"
                                        + "   -  User Name & Pass\n\n"
                                        + "   -  Server Dir";
            }
        }

        private void ProcessOutputCharacters(StreamReader output)
        {
            while (!output.EndOfStream)
            {
                string line = output.ReadLine();
                if (line != null && !line.Contains("\\src\\common\\contentmanifest.cpp (650) : Assertion Failed: !m_bIsFinalized*"))
                { UpdateTextBox(line); }
            }
        }

        //private void ProcessOutputEvent(object sender, DataReceivedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(e.Data))
        //    {
        //        if (!e.Data.Contains("\\src\\common\\contentmanifest.cpp (650) : Assertion Failed: !m_bIsFinalized*"))
        //        { UpdateTextBox(e.Data); }
        //    }
        //}
        
        private static void CheckModUpdatesComplete(IReadOnlyCollection<string> modIds)
        {
            if (modIds != null)
            {
                foreach (var modID in modIds)
                {
                    var modToUpdate = Properties.Options.Default.steamMods.SteamMods.Find(m => m.WorkshopId.ToString() == modID);
                    var steamCmdOutputText = Functions.StringFromRichTextBox(Instance.ISteamOutputBox);

                    if (steamCmdOutputText.Contains("ERROR! Timeout downloading"))
                    { modToUpdate.Status = "Download Not Complete"; }
                    else
                    {
                        string modTempPath = Properties.Options.Default.steamCMDPath + @"\steamapps\workshop\downloads\107410\" + modID;
                        string modPath = Properties.Options.Default.steamCMDPath + @"\steamapps\workshop\content\107410\" + modID;

                        if (Directory.Exists(modTempPath))
                            modToUpdate.Status = "Download Not Complete";
                        else if (Directory.GetFiles(modPath).Length != 0)
                        {
                            modToUpdate.Status = "Up to Date";
                            var nx = new DateTime(1970, 1, 1);
                            var ts = DateTime.UtcNow - nx;

                            modToUpdate.LocalLastUpdated = (int)ts.TotalSeconds;
                        }
                    }
                }
                Properties.Options.Default.Save();
            }
        }


        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T dependencyObject)
                    {
                        yield return dependencyObject;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
