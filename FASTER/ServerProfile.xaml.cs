﻿using FASTER.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Path = System.IO.Path;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace FASTER
{
    /// <summary>
    /// Interaction logic for ServerProfile.xaml
    /// </summary>
    public partial class ServerProfile : UserControl
    {
        private readonly string _safeName;
        private readonly string _profilesPath = Properties.Options.Default.serverPath + "\\Servers\\";
        private string _replace;

        private void ServerProfile_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateModsList();
            UpdateMissionsList();
        }


        private void ServerProfile_Initialized(object sender, EventArgs e)
        {
            UpdateModsList();
            UpdateMissionsList();
        }

        public ServerProfile(Models.ServerProfile profile)
        {
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call
            _safeName = profile.SafeName;

            IDisplayName.Content = profile.DisplayName;
            IServerName.Text = profile.ServerName;
            IExecutable.Text = profile.Executable;
            IPassword.Text = profile.Password;
            IAdminPassword.Text = profile.AdminPassword;
            IMaxPlayers.Text = profile.MaxPlayers.ToString();
            IPort.Text = profile.Port.ToString();
            IHeadlessClientEnabled.IsChecked = profile.HeadlessClientEnabled;
            IHeadlessIps.Text = profile.HeadlessIps;
            ILocalClients.Text = profile.LocalClients;
            INoOfHeadlessClients.Value = profile.NoOfHeadlessClients;
            ILoopback.IsChecked = profile.Loopback;
            IUpnp.IsChecked = profile.Upnp;
            INetlog.IsChecked = profile.Netlog;
            // IAutoRestartEnabled.IsChecked = profile.AutoRestartEnabled
            // IDailyRestartAEnabled.IsChecked = profile.DailyRestartAEnabled
            // IDailyRestartA.SelectedTime = profile.DailyRestartA
            // IDailyRestartBEnabled.IsChecked = profile.DailyRestartBEnabled
            // IDailyRestartB.Text = profile.DailyRestartB
            IVotingEnabled.IsChecked = profile.VotingEnabled;
            IVotingMinPlayers.Text = profile.VotingMinPlayers.ToString();
            IVotingThreshold.Text = profile.VotingThreshold.ToString();
            IAllowFilePatching.Text = profile.AllowFilePatching.ToString();
            IVerifySignatures.Text = profile.VerifySignatures.ToString();
            IRequiredBuildEnabled.IsChecked = profile.RequiredBuildEnabled;
            IRequiredBuild.Text = profile.RequiredBuild.ToString();
            IKickDuplicates.IsChecked = profile.KickDuplicates;
            IVonEnabled.IsChecked = profile.VonEnabled;
            ICodecQuality.Value = profile.CodecQuality;
            IServerConsoleLogEnabled.IsChecked = profile.ServerConsoleLogEnabled;
            IPidEnabled.IsChecked = profile.PidEnabled;
            IRankingEnabled.IsChecked = profile.RankingEnabled;
            IRptTimestamp.Text = profile.RptTimestamp;
            IMotd.Text = profile.Motd;
            IMotdDelay.Text = profile.MotdDelay.ToString();
            IManualMissions.IsChecked = profile.ManualMissions;
            IMissionConfig.Text = profile.MissionsClass;
            IPersistentBattlefield.IsChecked = profile.PersistentBattlefield;
            IAutoInit.IsChecked = profile.AutoInit;
            IDifficultyPreset.Text = profile.DifficultyPreset;
            IReducedDamage.IsChecked = profile.ReducedDamage;
            IGroupIndicators.Text = profile.GroupIndicators;
            IFriendlyNameTags.Text = profile.FriendlyNameTags;
            IEnemyNameTags.Text = profile.EnemyNameTags;
            IDetectedMines.Text = profile.DetectedMines;
            IMultipleSaves.IsChecked = profile.MultipleSaves;
            IThirdPerson.IsChecked = profile.ThirdPerson;
            IWeaponInfo.Text = profile.WeaponInfo;
            IStanceIndicator.Text = profile.StanceIndicator;
            IStaminaBar.IsChecked = profile.StaminaBar;
            ICameraShake.IsChecked = profile.CameraShake;
            IVisualAids.IsChecked = profile.VisualAids;
            IMapContentFriendly.IsChecked = profile.MapContentFriendly;
            IMapContentEnemy.IsChecked = profile.MapContentEnemy;
            IMapContentMines.IsChecked = profile.MapContentMines;
            ICommands.Text = profile.Commands;
            IVonId.IsChecked = profile.VonId;
            IKilledBy.IsChecked = profile.KilledBy;
            IWaypoints.Text = profile.Waypoints;
            ICrosshair.IsChecked = profile.Crosshair;
            IAutoReporting.IsChecked = profile.AutoReporting;
            IScoreTable.IsChecked = profile.ScoreTable;
            ITacticalPing.IsChecked = profile.TacticalPing;
            IMapPing.IsChecked = profile.MapPing;
            IAiAccuracy.Text = profile.AiAccuracy.ToString();
            IAiSkill.Text = profile.AiSkill.ToString();
            IAiPreset.Text = profile.AiPreset.ToString();
            IMaxPacketLossEnabled.IsChecked = profile.MaxPacketLossEnabled;
            IMaxPacketLoss.Text = profile.MaxPacketLoss.ToString();
            IDisconnectTimeOutEnabled.IsChecked = profile.DisconnectTimeoutEnabled;
            IDisconnectTimeOut.Text = profile.DisconnectTimeout.ToString();
            IKickOnSlowNetworkEnabled.IsChecked = profile.KickOnSlowNetworkEnabled;
            IKickOnSlowNetwork.Text = profile.KickOnSlowNetwork;
            ITerrainGrid.Text = profile.TerrainGrid.ToString();
            IViewDistance.Text = profile.ViewDistance.ToString();
            IMaxPingEnabled.IsChecked = profile.MaxPingEnabled;
            IMaxPing.Text = profile.MaxPing.ToString();
            IMaxDesyncEnabled.IsChecked = profile.MaxDesyncEnabled;
            IMaxDesync.Text = profile.MaxDesync.ToString();
            IMaxCustomFileSize.Text = profile.MaxCustomFileSize.ToString();
            IMaxPacketSize.Text = profile.MaxPacketSize.ToString();
            IMinBandwidth.Text = profile.MinBandwidth.ToString();
            IMaxBandwidth.Text = profile.MaxBandwidth.ToString();
            IMaxMessagesSend.Text = profile.MaxMessagesSend.ToString();
            IMaxSizeNonguaranteed.Text = profile.MaxSizeNonguaranteed.ToString();
            IMaxSizeGuaranteed.Text = profile.MaxSizeGuaranteed.ToString();
            IMinErrorToSend.Text = profile.MinErrorToSend.ToString();
            IMinErrorToSendNear.Text = profile.MinErrorToSendNear.ToString();
            ICpuCount.Text = profile.CpuCount;
            IMaxMem.Text = profile.MaxMem;
            IExtraParams.Text = profile.ExtraParams;
            IAdminUids.Text = profile.AdminUids;
            IEnableHyperThreading.IsChecked = profile.EnableHyperThreading;
            IFilePatching.IsChecked = profile.FilePatching;
            IServerCommandPassword.Text = profile.ServerCommandPassword;
            IDoubleIdDetected.Text = profile.DoubleIdDetected;
            IOnUserConnected.Text = profile.OnUserConnected;
            IOnUserDisconnected.Text = profile.OnUserDisconnected;
            IOnHackedData.Text = profile.OnHackedData;
            IOnDifferentData.Text = profile.OnDifferentData;
            IOnUnsignedData.Text = profile.OnUnsignedData;
            IRegularCheck.Text = profile.RegularCheck;
            IServerModsList.SelectedValue = profile.ServerMods;
            IClientModsList.SelectedValue = profile.ClientMods;
            IHeadlessModsList.SelectedValue = profile.HeadlessMods;
            IMissionCheckList.SelectedValue = profile.Missions;
            IBattleEye.IsChecked = profile.BattleEye;

            IServerActionButtons.SelectionChanged += IActionButtons_SelectionChanged;
            IServerActionButtons.SelectionChanged += IServerActionButtons_SelectionChanged;
            Loaded += ServerProfile_Loaded;
            Initialized += ServerProfile_Initialized;
            
            ToggleUi(IHeadlessClientEnabled);
            // ToggleUi(IAutoRestartEnabled)
            ToggleUi(IVonEnabled);
            ToggleUi(IVotingEnabled);
            ToggleUi(IServerConsoleLogEnabled);
            ToggleUi(IPidEnabled);
            ToggleUi(IRankingEnabled);
            ToggleUi(IManualMissions);

        }


        private void IActionButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                Thread.Sleep(600);
                Dispatcher?.Invoke(() =>
                {
                    ((ListBox) sender).SelectedItem = null;
                });
            });
            thread.Start();
        }


        private void IProfileNameEditSave_Click(object sender, RoutedEventArgs e)
        {
            var oldName = IDisplayName.Content.ToString();
            var newName = IProfileDisplayNameEdit.Text;

            if (ServerCollection.RenameServerProfile(oldName, newName))
            {
                MainWindow.Instance.IMainContent.Items.RemoveAt(MainWindow.Instance.IMainContent.SelectedIndex);
                MainWindow.Instance.LoadServerProfiles();
                MainWindow.Instance.IMainContent.SelectedIndex = MainWindow.Instance.IMainContent.Items.Count - 1;
            }
            else
            {
                MessageBox.Show("Could not rename Server Profile. \nPlease try again.");
            }
        }

        private void IServerActionButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ISaveProfile.IsSelected)
            { UpdateProfile(); }
            if (IRenameProfile.IsSelected)
            { ShowRenameInterface(true); }
            if (IDeleteProfile.IsSelected)
            {
                ServerCollection.DeleteServerProfile(_safeName);
                MainWindow.Instance.IMainContent.SelectedIndex = 0;


                var tabs = MainWindow.Instance.IMainContent.Items;
                var menus = MainWindow.Instance.IServerProfilesMenu.Items;
                var menu = new ListBoxItem();
                var tab = new TabItem();

                foreach (ListBoxItem m in menus)
                {
                    if (m.Name == _safeName)
                    { menu = m; }
                }

                foreach (TabItem t in tabs)
                {
                    if (t.Name == _safeName)
                    { tab = t; }
                }

                MainWindow.Instance.IMainContent.Items.Remove(tab);
                MainWindow.Instance.IServerProfilesMenu.Items.Remove(menu);

                MainWindow.Instance.IServerProfilesMenu.SelectedIndex = -1;
                MainWindow.Instance.IMainMenuItems.SelectedIndex = 0;

            }
            if (ILaunchServer.IsSelected)
            {
                if (ReadyToLaunch(IDisplayName.Content.ToString()))
                {
                    UpdateProfile();
                    LaunchServer();
                }
            }

            var thread = new Thread(() =>
            {
                Thread.Sleep(600);
                Dispatcher?.Invoke(() =>
                {
                    ((ListBox)sender).SelectedItem = null;
                });
            });
            thread.Start();
        }

        private void ToggleUi(object uiElement, RoutedEventArgs e = null)
        {
            switch ((uiElement as Control)?.Name)
            {
                case "IManualMissions":
                    if (IManualMissions.IsChecked ?? false)
                    {
                        IMissionAutoGrid.Visibility   = Visibility.Collapsed;
                        IMissionManualGrid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        IMissionAutoGrid.Visibility   = Visibility.Visible;
                        IMissionManualGrid.Visibility = Visibility.Collapsed;
                    }

                    break;
                case "IMaxDesyncEnabled":
                    if (IMaxDesyncEnabled.IsChecked ?? false) { IMaxDesync.IsEnabled = true; }
                    else { IMaxDesync.IsEnabled                                      = false; }

                    break;
                case "IMaxPingEnabled":
                    if (IMaxPingEnabled.IsChecked ?? false) { IMaxPing.IsEnabled = true; }
                    else { IMaxPing.IsEnabled                                    = false; }

                    break;
                case "IKickOnSlowNetworkEnabled":
                    if (IKickOnSlowNetworkEnabled.IsChecked ?? false) { IKickOnSlowNetwork.IsEnabled = true; }
                    else { IKickOnSlowNetwork.IsEnabled                                              = false; }

                    break;
                case "IDisconnectTimeOutEnabled":
                    if (IDisconnectTimeOutEnabled.IsChecked ?? false) { IDisconnectTimeOut.IsEnabled = true; }
                    else { IDisconnectTimeOut.IsEnabled                                              = false; }

                    break;
                case "IMaxPacketLossEnabled":
                    if (IMaxPacketLossEnabled.IsChecked ?? false) { IMaxPacketLoss.IsEnabled = true; }
                    else { IMaxPacketLoss.IsEnabled                                          = false; }

                    break;
                case "IPersistentBattlefield":
                    if (IPersistentBattlefield.IsChecked ?? false) { IAutoInit.IsEnabled = true; }
                    else { IAutoInit.IsEnabled                                           = false; }

                    break;
                case "IHeadlessClientEnabled":
                    if (IHeadlessClientEnabled.IsChecked ?? false)
                    {
                        IHcIpGroup.IsEnabled           = true;
                        IHcSliderGroup.IsEnabled       = true;
                        IHeadlessClientEnabled.ToolTip = "Disable HC";
                    }
                    else
                    {
                        IHcIpGroup.IsEnabled           = false;
                        IHcSliderGroup.IsEnabled       = false;
                        IHeadlessClientEnabled.ToolTip = "Enable HC";
                    }

                    break;
                case "IVonEnabled":
                    if (IVonEnabled.IsChecked ?? false)
                    {
                        IVonGroup.IsEnabled = true;
                        IVonEnabled.ToolTip = "Disable VON";
                    }
                    else
                    {
                        IVonGroup.IsEnabled = false;
                        IVonEnabled.ToolTip = "Enable VON";
                    }

                    break;
                case "IVotingEnabled":
                    if (IVotingEnabled.IsChecked ?? false)
                    {
                        IVotingMinPlayers.IsEnabled = true;
                        IVotingThreshold.IsEnabled  = true;
                        IVotingEnabled.ToolTip      = "Disable Voting";
                    }
                    else
                    {
                        IVotingMinPlayers.IsEnabled = false;
                        IVotingThreshold.IsEnabled  = false;
                        IVotingEnabled.ToolTip      = "Enable Voting";
                    }

                    // Case "IAutoRestartEnabled"
                    //     If IAutoRestartEnabled.IsChecked Then
                    //         IDailyRestartAEnabled.IsEnabled = True
                    //         IDailyRestartBEnabled.IsEnabled = True
                    //         IAutoRestartEnabled.ToolTip = "Disable Auto Restart"
                    //     Else
                    //         IDailyRestartAEnabled.IsEnabled = False
                    //         IDailyRestartBEnabled.IsEnabled = False
                    //         IAutoRestartEnabled.ToolTip = "Enable Auto Restart"
                    //     End If
                    break;
                case "IServerConsoleLogEnabled":
                    if (IServerConsoleLogEnabled.IsChecked ?? false)
                    {
                        IServerConsoleLog.IsEnabled = true;
                        IConsoleLogButton.IsEnabled = true;
                    }
                    else
                    {
                        IServerConsoleLog.IsEnabled = false;
                        IConsoleLogButton.IsEnabled = false;
                    }

                    break;
                case "IPidEnabled":
                    if (IPidEnabled.IsChecked ?? false)
                    {
                        IPidLog.IsEnabled    = true;
                        IPidButton.IsEnabled = true;
                    }
                    else
                    {
                        IPidLog.IsEnabled    = false;
                        IPidButton.IsEnabled = false;
                    }

                    break;
                case "IRankingEnabled":
                    if (IRankingEnabled.IsChecked ?? false)
                    {
                        IRankingLog.IsEnabled    = true;
                        IRankingButton.IsEnabled = true;
                    }
                    else
                    {
                        IRankingLog.IsEnabled    = false;
                        IRankingButton.IsEnabled = false;
                    }

                    break;
                case "IRequiredBuildEnabled":
                    if (IRequiredBuildEnabled.IsChecked ?? false) { IRequiredBuild.IsEnabled = true; }
                    else { IRequiredBuild.IsEnabled                                          = false; }

                    // Case "IDailyRestartAEnabled"
                    //     If IDailyRestartAEnabled.IsChecked Then
                    //         IDailyRestartA.IsEnabled = True
                    //     Else
                    //         IDailyRestartA.IsEnabled = False
                    //     End If
                    // Case "IDailyRestartBEnabled"
                    //     If IDailyRestartBEnabled.IsChecked Then
                    //         IDailyRestartB.IsEnabled = True
                    //     Else
                    //         IDailyRestartB.IsEnabled = False
                    //     End If
                    break;
            }
        }


        private void IServerFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                Title                     = "Select the arma server executable",
                IsFolderPicker            = false,
                AddToMostRecentlyUsedList = false,
                InitialDirectory          = Properties.Options.Default.serverPath,
                DefaultDirectory          = Properties.Options.Default.serverPath,
                AllowNonFileSystemItems   = false,
                EnsureFileExists          = true,
                EnsurePathExists          = true,
                EnsureReadOnly            = false,
                EnsureValidNames          = true,
                Multiselect               = false,
                ShowPlacesList            = true
            };
            dialog.Filters.Add(new CommonFileDialogFilter("Arma 3 Server Executable", ".exe"));
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (dialog.FileName != null)
                { IExecutable.Text = dialog.FileName; }
                else
                { MessageBox.Show("Please enter a valid arma3server executable location"); }
            }
        }


        private void IResetPerf_Click(object sender, RoutedEventArgs e)
        {
            IMaxCustomFileSize.Text    = "160";
            IMaxPacketSize.Text        = "1400";
            IMinBandwidth.Text         = "131072";
            IMaxBandwidth.Text         = "10000000000";
            IMaxMessagesSend.Text      = "128";
            IMaxSizeGuaranteed.Text    = "256";
            IMaxSizeNonguaranteed.Text = "512";
            IMinErrorToSend.Text       = "0.001";
            IMinErrorToSendNear.Text   = "0.01";
        }


        private void IMissionsRefresh_Click(object sender, RoutedEventArgs e)
        { UpdateMissionsList(); }


        private void IModsRefresh_Click(object sender, RoutedEventArgs e)
        { UpdateModsList(); }


        private void ModsAll_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Control)?.Name)
            {
                case "IServerModsAll":
                    foreach (CheckBox cb in IServerModsList.Items)
                    { cb.IsChecked = true; }
                    break;
                case "IClientModsAll":
                    foreach (CheckBox cb in IClientModsList.Items)
                    { cb.IsChecked = true; }
                    break;
                case "IHeadlessModsAll":
                    foreach (CheckBox cb in IHeadlessModsList.Items)
                    { cb.IsChecked = true; }
                    break;
                case "IAllMissionsButton":
                    foreach (CheckBox cb in IMissionCheckList.Items)
                    { cb.IsChecked = true; }
                    break;
            }
        }

        private void ModsNone_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Control)?.Name)
            {
                case "IServerModsNone":
                    foreach (CheckBox cb in IServerModsList.Items)
                    { cb.IsChecked = false; }
                    break;
                case "IClientModsNone":
                    foreach (CheckBox cb in IClientModsList.Items)
                    { cb.IsChecked = false; }
                    break;
                case "IHeadlessModsNone":
                    foreach (CheckBox cb in IHeadlessModsList.Items)
                    { cb.IsChecked = false; }
                    break;
                case "INoMissionsButton":
                    foreach (CheckBox cb in IMissionCheckList.Items)
                    { cb.IsChecked = false; }
                    break;
            }
        }


        private void IOpenRpt_Click(object sender, RoutedEventArgs e)
        { OpenLastFile(Path.Combine(_profilesPath, Functions.SafeName(IDisplayName.Content.ToString())), "*.rpt"); }

        private void IOpenNetLog_Click(object sender, RoutedEventArgs e)
        { OpenLastFile(Path.Combine(Environment.ExpandEnvironmentVariables("%LocalAppData%"), "Arma 3"), "netlog-*.log"); }

        private void IOpenRanking_Click(object sender, RoutedEventArgs e)
        { OpenLastFile(Path.Combine(_profilesPath, Functions.SafeName(IDisplayName.Content.ToString())), "ranking.log"); }

        private void IOpenConsoleLog_Click(object sender, RoutedEventArgs e)
        { OpenLastFile(Path.Combine(_profilesPath, Functions.SafeName(IDisplayName.Content.ToString())), "server_console*.log"); }

        private void IDeleteRpt_Click(object sender, RoutedEventArgs e)
        { DeleteAllFiles(Path.Combine(_profilesPath, Functions.SafeName(IDisplayName.Content.ToString())), "*.rpt"); }

        private void IDeleteNetLog_Click(object sender, RoutedEventArgs e)
        { DeleteAllFiles(Path.Combine(Environment.ExpandEnvironmentVariables("%LocalAppData%"), "Arma 3"), "netlog-*.log"); }

        private void UpdateProfile()
        {
            try
            {
                object profileName = Functions.SafeName(IDisplayName.Content.ToString());
                string path = _profilesPath;
                string profilePath = path
                                   + (profileName + "\\");
                if (!Directory.Exists(path))
                { Directory.CreateDirectory(path); }

                if (!Directory.Exists(profilePath))
                { Directory.CreateDirectory(profilePath); }

                if (!File.Exists(profilePath + (profileName + "_config.cfg")))
                {
                    var cfg = File.Create(profilePath + (profileName + "_config.cfg"));
                    cfg.Close();
                }

                if (!File.Exists(profilePath + (profileName + "_basic.cfg")))
                {
                    var cfg = File.Create(profilePath + (profileName + "_basic.cfg"));
                    cfg.Close();
                }

            }
            catch (Exception)
            { /*ignored*/}

            var profile = Properties.Options.Default.Servers.ServerProfiles.Find(p => p.SafeName == _safeName);
            profile.DisplayName = IDisplayName.Content.ToString();
            profile.ServerName = IServerName.Text;
            profile.Executable = IExecutable.Text;
            profile.Password = IPassword.Text;
            profile.AdminPassword = IAdminPassword.Text;
            profile.MaxPlayers = int.Parse(IMaxPlayers.Text);
            profile.Port = int.Parse(IPort.Text);
            profile.HeadlessClientEnabled = IHeadlessClientEnabled.IsChecked?? false;
            profile.HeadlessIps = IHeadlessIps.Text;
            profile.LocalClients = ILocalClients.Text;
            profile.NoOfHeadlessClients = (int)INoOfHeadlessClients.Value;
            profile.Loopback = ILoopback.IsChecked ?? false;
            profile.Upnp = IUpnp.IsChecked ?? false;
            profile.Netlog = INetlog.IsChecked ?? false;
            // profile.AutoRestartEnabled = IAutoRestartEnabled.IsChecked
            // profile.DailyRestartAEnabled = IDailyRestartAEnabled.IsChecked
            // If IDailyRestartA.SelectedTime IsNot Nothing Then
            //     profile.DailyRestartA = IDailyRestartA.SelectedTime
            // End If
            // profile.DailyRestartBEnabled = IDailyRestartBEnabled.IsChecked
            // If IDailyRestartB.SelectedTime IsNot Nothing Then
            //     profile.DailyRestartB = IDailyRestartB.SelectedTime
            // End If
            profile.VotingEnabled = IVotingEnabled.IsChecked ?? false;
            profile.VotingMinPlayers = int.Parse(IVotingMinPlayers.Text);
            profile.VotingThreshold = decimal.Parse(IVotingThreshold.Text);
            profile.AllowFilePatching = int.Parse(IAllowFilePatching.Text);
            profile.VerifySignatures = int.Parse(IVerifySignatures.Text);
            profile.RequiredBuildEnabled = IRequiredBuildEnabled.IsChecked ?? false;
            //TODO Check this
            //profile.RequiredBuild = IRequiredBuild.Text;
            profile.KickDuplicates = IKickDuplicates.IsChecked ?? false;
            profile.VonEnabled = IVonEnabled.IsChecked ?? false;
            profile.CodecQuality = int.Parse(ICodecQuality.Value.ToString());
            profile.ServerConsoleLogEnabled = IServerConsoleLogEnabled.IsChecked ?? false;
            profile.PidEnabled = IPidEnabled.IsChecked ?? false;
            profile.RankingEnabled = IRankingEnabled.IsChecked ?? false;
            profile.RptTimestamp = IRptTimestamp.Text;
            profile.Motd = IMotd.Text;
            profile.MotdDelay = int.Parse(IMotdDelay.Text);
            profile.ManualMissions = IManualMissions.IsChecked ?? false;
            profile.MissionsClass = IMissionConfig.Text;
            profile.PersistentBattlefield = IPersistentBattlefield.IsChecked ?? false;
            profile.AutoInit = IAutoInit.IsChecked ?? false;
            profile.DifficultyPreset = IDifficultyPreset.Text;
            profile.ReducedDamage = IReducedDamage.IsChecked ?? false;
            profile.GroupIndicators = IGroupIndicators.Text;
            profile.FriendlyNameTags = IFriendlyNameTags.Text;
            profile.EnemyNameTags = IEnemyNameTags.Text;
            profile.DetectedMines = IDetectedMines.Text;
            profile.MultipleSaves = IMultipleSaves.IsChecked ?? false;
            profile.ThirdPerson = IThirdPerson.IsChecked ?? false;
            profile.WeaponInfo = IWeaponInfo.Text;
            profile.StanceIndicator = IStanceIndicator.Text;
            profile.StaminaBar = IStaminaBar.IsChecked ?? false;
            profile.CameraShake = ICameraShake.IsChecked ?? false;
            profile.VisualAids = IVisualAids.IsChecked ?? false;
            profile.MapContentFriendly = IMapContentFriendly.IsChecked ?? false;
            profile.MapContentEnemy = IMapContentEnemy.IsChecked ?? false;
            profile.MapContentMines = IMapContentMines.IsChecked ?? false;
            profile.Commands = ICommands.Text;
            profile.VonId = IVonId.IsChecked ?? false;
            profile.KilledBy = IKilledBy.IsChecked ?? false;
            profile.Waypoints = IWaypoints.Text;
            profile.Crosshair = ICrosshair.IsChecked ?? false;
            profile.AutoReporting = IAutoReporting.IsChecked ?? false;
            profile.ScoreTable = IScoreTable.IsChecked ?? false;
            profile.TacticalPing = ITacticalPing.IsChecked ?? false;
            profile.MapPing = IMapPing.IsChecked ?? false;
            profile.AiAccuracy = double.Parse(IAiAccuracy.Text);
            profile.AiSkill = double.Parse(IAiSkill.Text);
            profile.AiPreset = int.Parse(IAiPreset.Text);
            profile.MaxPacketLossEnabled = IMaxPacketLossEnabled.IsChecked ?? false;
            profile.MaxPacketLoss = int.Parse(IMaxPacketLoss.Text);
            profile.DisconnectTimeoutEnabled = IDisconnectTimeOutEnabled.IsChecked ?? false;
            profile.DisconnectTimeout = int.Parse(IDisconnectTimeOut.Text);
            profile.KickOnSlowNetworkEnabled = IKickOnSlowNetworkEnabled.IsChecked ?? false;
            profile.KickOnSlowNetwork = IKickOnSlowNetwork.Text;
            profile.TerrainGrid = int.Parse(ITerrainGrid.Text);
            profile.ViewDistance = int.Parse(IViewDistance.Text);
            profile.MaxPingEnabled = IMaxPingEnabled.IsChecked ?? false;
            profile.MaxPing = int.Parse(IMaxPing.Text);
            profile.MaxDesyncEnabled = IMaxDesyncEnabled.IsChecked ?? false;
            profile.MaxDesync = int.Parse(IMaxDesync.Text);
            profile.MaxCustomFileSize = int.Parse(IMaxCustomFileSize.Text);
            profile.MaxPacketSize = int.Parse(IMaxPacketSize.Text);
            profile.MinBandwidth = double.Parse(IMinBandwidth.Text);
            profile.MaxBandwidth = double.Parse(IMaxBandwidth.Text);
            profile.MaxMessagesSend = int.Parse(IMaxMessagesSend.Text);
            profile.MaxSizeNonguaranteed = int.Parse(IMaxSizeNonguaranteed.Text);
            profile.MaxSizeGuaranteed = int.Parse(IMaxSizeGuaranteed.Text);
            profile.MinErrorToSend = double.Parse(IMinErrorToSend.Text);
            profile.MinErrorToSendNear = double.Parse(IMinErrorToSendNear.Text);
            profile.CpuCount = ICpuCount.Text;
            profile.MaxMem = IMaxMem.Text;
            profile.ExtraParams = IExtraParams.Text;
            profile.AdminUids = IAdminUids.Text;
            profile.EnableHyperThreading = IEnableHyperThreading.IsChecked ?? false;
            profile.FilePatching = IFilePatching.IsChecked ?? false;
            profile.ServerCommandPassword = IServerCommandPassword.Text;
            profile.DoubleIdDetected = IDoubleIdDetected.Text;
            profile.OnUserConnected = IOnUserConnected.Text;
            profile.OnUserDisconnected = IOnUserDisconnected.Text;
            profile.OnHackedData = IOnHackedData.Text;
            profile.OnDifferentData = IOnDifferentData.Text;
            profile.OnUnsignedData = IOnUnsignedData.Text;
            profile.RegularCheck = IRegularCheck.Text;
            profile.ServerMods = "";
            foreach (CheckBox addon in IServerModsList.Items)
            {
                if (!profile.ServerMods.Contains((string)addon.Content) && (addon.IsChecked ?? false) )
                { profile.ServerMods += addon.Content + ";"; }
            }
            profile.ClientMods = "";
            foreach (CheckBox addon in IClientModsList.Items)
            {
                if (!profile.ClientMods.Contains((string)addon.Content) && (addon.IsChecked ?? false))
                { profile.ClientMods += addon.Content + ";"; }
            }
            profile.HeadlessMods = "";
            foreach (CheckBox addon in IHeadlessModsList.Items)
            {
                if (!profile.HeadlessMods.Contains((string)addon.Content) && (addon.IsChecked ?? false))
                { profile.HeadlessMods += addon.Content + ";"; }
            }
            profile.Missions = "";
            foreach (CheckBox addon in IMissionCheckList.Items)
            {
                if (!profile.Missions.Contains((string)addon.Content) && (addon.IsChecked ?? false))
                { profile.Missions += addon.Content + ";"; }
            }
            profile.BattleEye = IBattleEye.IsChecked ?? false;
            Properties.Options.Default.Save();
        }

        private void WriteConfigFiles(string profile)
        {
            string profilePath = Properties.Options.Default.serverPath;
            profile = Functions.SafeName(profile);

            string config        = Path.Combine(profilePath, "Servers", profile, $"{profile}_config.cfg");
            string basic         = Path.Combine(profilePath, "Servers", profile, $"{profile}_basic.cfg");
            string serverProfile = Path.Combine(profilePath, "Servers", profile, "users", profile, $"{profile}.Arma3Profile");

            Directory.CreateDirectory(Path.Combine(profilePath, "Servers", profile, "users", profile));

            #region CONFIG FILE CREATION
            var von = !IVonEnabled.IsChecked ?? true;
            List<string> configLines = new List<string>
            {
                $"passwordAdmin = \"{IAdminPassword.Text}\";",
                $"password = \"{IPassword.Text}\";",
                $"serverCommandPassword = \"{IServerCommandPassword.Text}\";",
                $"hostname = \"{IServerName.Text}\";",
                $"maxPlayers = {IMaxPlayers.Text};",
                $"kickduplicate = {(IKickDuplicates.IsChecked != null && (bool) IKickDuplicates.IsChecked ? "1" : "0")};",
                $"upnp = {(IUpnp.IsChecked                    != null && (bool) IUpnp.IsChecked ? "1" : "0")};",
                $"allowedFilePatching = {IAllowFilePatching.Text};",
                $"verifySignatures = {IVerifySignatures.Text};",
                $"disableVoN = {(von ? "1" : "0")};",
                $"vonCodecQuality = {(int) ICodecQuality.Value};",
                "vonCodec = 1;",
                $"BattlEye = {(IBattleEye.IsChecked               != null && (bool) IBattleEye.IsChecked ? "1" : "0")};",
                $"persistent = {(IPersistentBattlefield.IsChecked != null && (bool) IPersistentBattlefield.IsChecked ? "1" : "0")};",
                "motd[]= {"
            };


            var lines = Functions.GetLinesCollectionFromTextBox(IMotd);
            foreach (var line in lines)
            {
                _replace = line;
                _replace = _replace.Replace("\r", "").Replace("\n", "");

                configLines.Add(lines.IndexOf(_replace) == lines.Count - 1 ? $"\t\"{_replace}\"" : $"\t\"{_replace}\",");
            }

            configLines.Add("};");
            configLines.Add($"motdInterval = {IMotdDelay.Text};");
            var headless = IHeadlessIps.Text.Replace(",", "\",\"");
            var local    = ILocalClients.Text.Replace(",", "\",\"");

            if (IHeadlessClientEnabled.IsChecked ?? false)
            {
                configLines.Add("headlessClients[] = {\"" + headless + "\"};");
                configLines.Add("localClient[] = {\"" + local        + "\"};");
            }

            if (IVotingEnabled.IsChecked ?? false)
            {
                configLines.Add("allowedVoteCmds[] = {};");
                configLines.Add("allowedVotedAdminCmds[] = {};");
                configLines.Add($"voteMissionPlayers = {IVotingMinPlayers.Text};");
                configLines.Add($"voteThreshold = {double.Parse(IVotingThreshold.Text)/100.0};");
            }
            else
            {
                configLines.Add("voteMissionPlayers = 1;");
                configLines.Add("voteThreshold = 0;");
            }

            if (ILoopback.IsChecked ?? false) { configLines.Add("loopback = True;"); }

            if (IDisconnectTimeOutEnabled.IsChecked ?? false) { configLines.Add($"disconnectTimeout = {IDisconnectTimeOut.Text};"); }

            if (IMaxDesyncEnabled.IsChecked ?? false) { configLines.Add($"maxdesync = {IMaxDesync.Text};"); }

            if (IMaxPingEnabled.IsChecked ?? false) { configLines.Add($"maxping = {IMaxPing.Text};"); }

            if (IMaxPacketLossEnabled.IsChecked ?? false) { configLines.Add($"maxpacketloss = {IMaxPacketLoss.Text};"); }

            if (IKickOnSlowNetworkEnabled.IsChecked ?? false)
            {
                switch (IKickOnSlowNetwork.Text)
                {
                    case "Log":
                        configLines.Add("kickClientsOnSlowNetwork[] = { 0, 0, 0, 0 };");
                        break;
                    case "Log & Kick":
                        configLines.Add("kickClientsOnSlowNetwork[] = { 1, 1, 1, 1 };");
                        break;
                }
            }

            if (IServerConsoleLogEnabled.IsChecked ?? false) { configLines.Add("logFile = \"server_console.log\";"); }

            if (IRequiredBuildEnabled.IsChecked ?? false) { configLines.Add($"requiredBuild = {IRequiredBuild.Text};"); }

            configLines.Add($"doubleIdDetected = \"{IDoubleIdDetected.Text}\";");
            configLines.Add($"onUserConnected = \"{IOnUserConnected.Text}\";");
            configLines.Add($"onUserDisconnected = \"{IOnUserDisconnected.Text}\";");
            configLines.Add($"onHackedData = \"{IOnHackedData.Text}\";");
            configLines.Add($"onDifferentData = \"{IOnDifferentData.Text}\";");
            configLines.Add($"onUnsignedData = \"{IOnUnsignedData.Text}\";");
            configLines.Add($"regularCheck = \"{IRegularCheck.Text}\";");
            configLines.Add("admins[]= {");
            var admins = IAdminUids.Text.Split('\n');
            foreach (var line in admins)
            {
                _replace = line;
                _replace = _replace.Replace("\r", "").Replace("\n", "");
                configLines.Add(lines.IndexOf(_replace) == lines.Count - 1 ? $"\t\"{_replace}\"" : $"\t\"{_replace}\",");
            }

            configLines.Add("};");
            configLines.Add($"timeStampFormat = \"{IRptTimestamp.Text}\";");
            if (IManualMissions.IsChecked ?? false)
            {
                var missionLines = Functions.GetLinesCollectionFromTextBox(IMissionConfig);
                foreach (var line in missionLines)
                { configLines.Add(line); }
            }
            else
            {
                configLines.Add("class Missions {");
                var difficulty = IDifficultyPreset.Text;
                if (string.IsNullOrEmpty(difficulty))
                { difficulty = "Regular"; }

                foreach (CheckBox mission in IMissionCheckList.Items)
                {
                    if (mission.IsChecked ?? false)
                    {
                        configLines.Add($"\tclass Mission_{IMissionCheckList.Items.IndexOf(mission) + 1} " + "{");
                        configLines.Add($"\t\ttemplate = \"{mission.Content}\";");
                        configLines.Add($"\t\tdifficulty = \"{difficulty}\";");
                        configLines.Add("\t};");
                    }
                }
                configLines.Add("};");
            }

            if (IEnableAdditionalParams.IsChecked ?? false)
            {
                var moreParams = Functions.GetLinesCollectionFromTextBox(IAdditionalParams);
                foreach (var line in moreParams)
                { configLines.Add(line); }
            }

            // "drawingInMap = 0;"
            // "forceRotorLibSimulation = 0;"
            // "forcedDifficulty = ""regular"";"
            // "missionWhitelist[] = {""intro.altis""};"

            File.WriteAllLines(config, configLines);
            #endregion

            #region BASIC FILE CREATION
            List<string> basicLines = new List<string>
            {
                "adapter = -1;",
                "3D_Performance=1;",
                "Resolution_W = 0;",
                "Resolution_H = 0;",
                "Resolution_Bpp = 32;",
                $"terrainGrid = {ITerrainGrid.Text};",
                $"viewDistance = {IViewDistance.Text};",
                "Windowed = 0;",
                $"MaxMsgSend = {IMaxMessagesSend.Text};",
                $"MaxSizeGuaranteed = {IMaxSizeGuaranteed.Text};",
                $"MaxSizeNonguaranteed = {IMaxSizeNonguaranteed.Text};",
                $"MinBandwidth = {IMinBandwidth.Text};",
                $"MaxBandwidth = {IMaxBandwidth.Text};",
                $"MinErrorToSend = {IMinErrorToSend.Text};",
                $"MinErrorToSendNear = {IMinErrorToSendNear.Text};",
                $"MaxCustomFileSize = {IMaxCustomFileSize.Text};",
                "class sockets{maxPacketSize = " + IMaxPacketSize.Text + ";};"
            };
            File.WriteAllLines(basic, basicLines);
            #endregion

            #region PROFILE FILE CREATION
            List<string> profileLines = new List<string>
            {
                $"difficulty = \"{IDifficultyPreset.Text}\";",
                "class DifficultyPresets {",
                "\tclass CustomDifficulty {",
                "\t\tclass Options {",
                $"\t\t\treduceDamage={IReducedDamage.IsChecked};",
                $"\t\t\tgroupIndicators={IGroupIndicators.Text};",
                $"\t\t\tfriendlyTags={IFriendlyNameTags.Text};",
                $"\t\t\tenemyTags={IEnemyNameTags.Text};",
                $"\t\t\tdetectedMines={IDetectedMines.Text};",
                $"\t\t\tcommands={ICommands.Text};",
                $"\t\t\twaypoints={IWaypoints.Text};",
                $"\t\t\ttacticalPing={ITacticalPing.IsChecked};",
                $"\t\t\tmapContentPing={IMapPing.IsChecked};",
                $"\t\t\tweaponInfo={IWeaponInfo.Text};",
                $"\t\t\tstanceIndicator={IStanceIndicator.Text};",
                $"\t\t\tstaminaBar={IStaminaBar.IsChecked};",
                $"\t\t\tweaponCrosshair={ICrosshair.IsChecked};",
                $"\t\t\tvisionAid={IVisualAids.IsChecked};",
                $"\t\t\tthirdPersonView={IThirdPerson.IsChecked};",
                $"\t\t\tcameraShake={ICameraShake.IsChecked};",
                $"\t\t\tscoreTable={IScoreTable.IsChecked};",
                $"\t\t\tdeathMessages={IKilledBy.IsChecked};",
                $"\t\t\tvonID={IVonId.IsChecked};",
                $"\t\t\tmapContentFriendly={IMapContentFriendly.IsChecked};",
                $"\t\t\tmapContentEnemy={IMapContentEnemy.IsChecked};",
                $"\t\t\tmapContentMines={IMapContentMines.IsChecked};",
                $"\t\t\tautoReport={IAutoReporting.IsChecked};",
                $"\t\t\tmultipleSaves={IMultipleSaves.IsChecked};",
                "\t\t};",
                "",
                $"\t\taiLevelPreset={IAiPreset.Text};",
                "",
                "\t\tclass CustomAILevel {",
                $"\t\t\tskillAI={IAiSkill.Text};",
                $"\t\t\tprecisionAI={IAiAccuracy.Text};",
                "\t\t};",
                "\t};",
                "};"
            };
            var profileNever   = profileLines.Select(s => s.Replace("Never", "0")).ToList();
            var profileLimited = profileNever.Select(s => s.Replace("Limited Distance", "1")).ToList();
            var profileFade    = profileLimited.Select(s => s.Replace("Fade Out", "1")).ToList();
            var profileAlways  = profileFade.Select(s => s.Replace("Always", "2")).ToList();
            var profileTrue    = profileAlways.Select(s => s.Replace("True", "1")).ToList();
            var profileFalse   = profileTrue.Select(s => s.Replace("False", "0")).ToList();
            File.WriteAllLines(serverProfile, profileFalse);
            #endregion  
        }

        private void UpdateMissionsList()
        {
            var          currentMissions = IMissionCheckList.Items;
            List<string> newMissions     = new List<string>();
            var          checkedMissions = new List<CheckBox>();
            var          profile         = Properties.Options.Default.Servers.ServerProfiles.Find(p => p.SafeName == _safeName);

            if (profile != null)
            {
                foreach (var mission in profile.Missions.Split(';'))
                {
                    if (checkedMissions.FirstOrDefault(c => (string) c.Content == mission.Replace(";", "")) == null && !string.IsNullOrWhiteSpace(mission))
                        checkedMissions.Add(new CheckBox { Content = mission.Replace(";", ""), IsChecked = true });
                }
            }

            IMissionCheckList.Items.Clear();

            if (Directory.Exists(Path.Combine(Properties.Options.Default.serverPath, "mpmissions")))
            {
                newMissions.AddRange(Directory.GetFiles(Path.Combine(Properties.Options.Default.serverPath, "mpmissions"), "*.pbo")
                                              .Select(mission => mission.Replace(Path.Combine(Properties.Options.Default.serverPath, "mpmissions") + "\\", "")));

                foreach (var mission in newMissions.ToList())
                {
                    if (currentMissions.Contains(mission))
                    { newMissions.Remove(mission); }
                }

                

                foreach (var mission in newMissions.ToList())
                {
                    var checkedMission = checkedMissions.FirstOrDefault(m => (string)m.Content == mission.Replace(".pbo", ""))?.IsChecked ?? false;
                    IMissionCheckList.Items.Add(new CheckBox { Content = mission.Replace(".pbo", "") , IsChecked = checkedMission});
                }
                
                IMissionCheckList.SelectedValue = checkedMissions;
            }
        }

        private void UpdateModsList()
        {
            var          currentMods = IServerModsList.Items;
            List<string> newMods     = new List<string>();

            var profile           = Properties.Options.Default.Servers.ServerProfiles.Find(p => p.SafeName == _safeName);
            var checkedServerMods = new List<CheckBox>();
            var checkedHcMods = new List<CheckBox>();
            var checkedClientMods = new List<CheckBox>();
            
            if (profile != null)
            { 
                foreach (var mod in profile.ServerMods.Split(';'))
                {
                    if (checkedServerMods.FirstOrDefault(c => (string) c.Content == mod.Replace(";", "")) == null && !string.IsNullOrWhiteSpace(mod))
                        checkedServerMods.Add(new CheckBox { Content = mod.Replace(";", ""), IsChecked = true });
                }
                foreach (var mod in profile.HeadlessMods.Split(';'))
                {
                    if (checkedHcMods.FirstOrDefault(c => (string)c.Content == mod.Replace(";", "")) == null && !string.IsNullOrWhiteSpace(mod))
                        checkedHcMods.Add(new CheckBox { Content = mod.Replace(";",             ""), IsChecked = true });
                }
                foreach (var mod in profile.ClientMods.Split(';'))
                {
                    if (checkedClientMods.FirstOrDefault(c => (string)c.Content == mod.Replace(";", "")) == null && !string.IsNullOrWhiteSpace(mod))
                        checkedClientMods.Add(new CheckBox { Content = mod.Replace(";",             ""), IsChecked = true });
                }
            }

            IServerModsList.Items.Clear();
            IClientModsList.Items.Clear();
            IHeadlessModsList.Items.Clear();

            if (Directory.Exists(Properties.Options.Default.serverPath))
            {
                newMods.AddRange(Directory.GetDirectories(Properties.Options.Default.serverPath, "@*")
                                          .Select(addon => addon.Replace(Properties.Options.Default.serverPath + @"\", "")));

                foreach (var folder in Properties.Options.Default.localModFolders) { newMods.AddRange(Directory.GetDirectories(folder, "@*")); }

                foreach (var addon in newMods.ToList())
                {
                    if (currentMods.Contains(addon))
                        newMods.Remove(addon);
                }

                foreach (var addon in newMods.ToList())
                {
                    var serverCheck = checkedServerMods.FirstOrDefault(m => (string) m.Content == addon)?.IsChecked ?? false;
                    var clientCheck = checkedClientMods.FirstOrDefault(m => (string)m.Content == addon)?.IsChecked ?? false;
                    var hcCheck = checkedHcMods.FirstOrDefault(m => (string)m.Content == addon)?.IsChecked ?? false;
                    IServerModsList.Items.Add(new CheckBox { Content = addon, IsChecked = serverCheck });
                    IClientModsList.Items.Add(new CheckBox { Content = addon, IsChecked = clientCheck  });
                    IHeadlessModsList.Items.Add(new CheckBox { Content = addon, IsChecked = hcCheck });
                }

                IServerModsList.SelectedValue   = checkedServerMods;
                IClientModsList.SelectedValue   = checkedClientMods;
                IHeadlessModsList.SelectedValue = checkedHcMods;
            }
            else
            { MessageBox.Show("Please install game before continuing."); }
        }
        
        private static void OpenLastFile(string path, string filter)
        {
            try
            {
                var dir = new DirectoryInfo(path);
                var file = dir.EnumerateFiles(filter).OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
                if (file != null)
                {
                    try
                    { Process.Start(file.FullName); }
                    catch (Exception)
                    { /*ignored*/}
                }
                else
                {
                    MainWindow.Instance.IMessageDialog.IsOpen = true;
                    MainWindow.Instance.IMessageDialogText.Text = "Cannot Open - File Not Found \n\nIf Opening PID file make sure Server is running.";
                }
            }
            catch (Exception)
            {
                MainWindow.Instance.IMessageDialog.IsOpen = true;
                MainWindow.Instance.IMessageDialogText.Text = "Cannot Open - File Not Found \n\nIf Opening PID file make sure Server is running.";
            }
        }

        private static void DeleteAllFiles(string path, string filter)
        {
            var dir = new DirectoryInfo(path);
            var files = dir.EnumerateFiles(filter);
            var i = 0;
            foreach (var file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception )
                { /* ignored */}
                i += 1;
            }
            MainWindow.Instance.IMessageDialog.IsOpen = true;
            MainWindow.Instance.IMessageDialogText.Text = $"Deleted {i} files.";
        }

        private void LaunchServer()
        {
            string profileName = Functions.SafeName(IDisplayName.Content.ToString());
            string profilePath = _profilesPath + profileName + "\\";
            string configs = profilePath + profileName;
            bool start = true;
            string serverMods = IServerModsList.Items.Cast<CheckBox>()
                                               .Where(addon => addon.IsChecked ?? false)
                                               .Aggregate<CheckBox, string>(null, (current, addon) => current + (addon.Content + ";"));

            string playerMods = IClientModsList.Items.Cast<CheckBox>()
                                               .Where(addon => addon.IsChecked ?? false)
                                               .Aggregate<CheckBox, string>(null, (current, addon) => current + (addon.Content + ";"));

            try
            { WriteConfigFiles(profileName); }
            catch (Exception)
            {
                MainWindow.Instance.IMessageDialog.IsOpen = true;
                MainWindow.Instance.IMessageDialogText.Text = "Config files in use elsewhere - make sure server is not running.";
                start = false;
            }

            if (start)
            {
                var commandLine = "-port=" + IPort.Text;
                commandLine = commandLine + " \"-config=" + configs + "_config.cfg\"";
                commandLine = commandLine + " \"-cfg=" + configs + "_basic.cfg\"";
                commandLine = commandLine + " \"-profiles=" + profilePath + "\"";
                commandLine = commandLine + " -name=" + profileName;
                commandLine = commandLine + " \"-mod=" + playerMods + "\"";
                commandLine = commandLine + " \"-serverMod=" + serverMods + "\"";
                if (IEnableHyperThreading.IsChecked ?? false)
                { commandLine += " -enableHT"; }

                if (IFilePatching.IsChecked ?? false)
                { commandLine += " -filePatching"; }

                if (INetlog.IsChecked ?? false)
                { commandLine += " -netlog"; }

                if (IRankingEnabled.IsChecked ?? false)
                { commandLine = commandLine + " -ranking=Servers\\" + Functions.SafeName(IDisplayName.Content.ToString()) + "\\" + "ranking.log"; }

                if (IPidEnabled.IsChecked ?? false)
                { commandLine = commandLine + " -pid=Servers\\" + Functions.SafeName(IDisplayName.Content.ToString()) + "\\" + "pid.log"; }

                if (IAutoInit.IsChecked ?? false)
                { commandLine += " -autoInit"; }

                if (!string.IsNullOrEmpty(IMaxMem.Text))
                { commandLine = commandLine + " \"-maxMem=" + IMaxMem.Text + "\""; }

                if (!string.IsNullOrEmpty(ICpuCount.Text))
                { commandLine = commandLine + " \"-cpuCount=" + ICpuCount.Text + "\""; }

                if (!string.IsNullOrEmpty(IExtraParams.Text))
                { commandLine = commandLine + " " + IExtraParams.Text; }

                Clipboard.SetText(commandLine);
                ProcessStartInfo sStartInfo = new ProcessStartInfo(IExecutable.Text, commandLine);
                Process sProcess = new Process { StartInfo = sStartInfo };
                sProcess.Start();
                if (IHeadlessClientEnabled.IsChecked ?? false)
                {
                    for (int hc = 1; hc <= INoOfHeadlessClients.Value; hc++)
                    {
                        string hcCommandLine = "-client -connect=127.0.0.1 -password=" + IPassword.Text + " -profiles=" + profilePath + " -nosound -port=" + IPort.Text;
                        string hcMods = IHeadlessModsList.SelectedItems.Cast<CheckBox>()
                                                         .Where(addon => addon.IsChecked ?? false)
                                                         .Aggregate<CheckBox, string>(null, (current, addon) => current + (addon.Content + ";"));

                        hcCommandLine = hcCommandLine + " \"-mod=" + hcMods + "\"";
                        Clipboard.SetText(hcCommandLine);
                        ProcessStartInfo hcStartInfo = new ProcessStartInfo(IExecutable.Text, hcCommandLine);
                        Process hcProcess = new Process { StartInfo = hcStartInfo };
                        hcProcess.Start();
                    }
                }
            }
        }

        private bool ReadyToLaunch(string profile)
        {
            profile = Functions.SafeName(profile);
            if (!ProfileFilesExist(profile))
            {
                MainWindow.Instance.IMessageDialog.IsOpen   = true;
                MainWindow.Instance.IMessageDialogText.Text = "The profile does not exist in the game files.";
                return false;
            }

            if (!IExecutable.Text.Contains("arma3server") && !IExecutable.Text.EndsWith(".exe"))
            {
                MainWindow.Instance.IMessageDialog.IsOpen   = true;
                MainWindow.Instance.IMessageDialogText.Text = "Please select a valid Arma 3 Sever Executable.";
                return false;
            }

            if (!File.Exists(IExecutable.Text))
            {
                MainWindow.Instance.IMessageDialog.IsOpen   = true;
                MainWindow.Instance.IMessageDialogText.Text = "Arma 3 Server Executable does not exist. Please reselect correct file.";
                return false;
            }
            return true;
        }

        private static bool ProfileFilesExist(string profile)
        {
            string path = Properties.Options.Default.serverPath;
            if (!Directory.Exists(Path.Combine(path, "Servers", profile)))
            { return false; }

            if (!File.Exists(Path.Combine(path, "Servers", profile, $"{profile}_config.cfg")))
            { return false; }

            if (!File.Exists(Path.Combine(path, "Servers", profile, $"{profile}_basic.cfg")))
            { return false; }

            return true;
        }

        private void ShowRenameInterface(bool show)
        {
            if (show)
            {
                IProfileDisplayNameEdit.Text = IDisplayName.Content.ToString();
                IDisplayName.Visibility = Visibility.Collapsed;
                IBattleEye.Visibility = Visibility.Collapsed;
                IProfileNameEdit.Visibility = Visibility.Visible;
                IProfileDisplayNameEdit.Background = new SolidColorBrush(Color.FromArgb(100, 0, 100, 255));
                IProfileDisplayNameEdit.SelectAll();
                return;
            }
            IProfileDisplayNameEdit.Text = string.Empty;
            IDisplayName.Visibility = Visibility.Visible;
            IBattleEye.Visibility = Visibility.Visible;
            IProfileNameEdit.Visibility = Visibility.Collapsed;
        }

    }
}
