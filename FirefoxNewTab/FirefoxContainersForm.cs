using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace FirefoxNewTab
{
    using static User32;
    public partial class FirefoxContainersForm : Form
    {
        private IReadOnlyCollection<FirefoxContainerOption> firefoxContainers;
        private IReadOnlyCollection<string> firefoxContainerNames;

        public IntPtr FirefoxHandle { get; internal set; }

        public FirefoxContainersForm()
        {
            InitializeComponent();

            SetContainerData();
        }

        private void SetContainerData()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            this.firefoxContainers = new ReadOnlyCollection<FirefoxContainerOption>(configuration.GetSection("FirefoxContainers").Get<FirefoxContainerOption[]>());
            firefoxContainerNames = new ReadOnlyCollection<string>(this.firefoxContainers.Select(x => x.Name).ToList());
        }

        private int SelectedContainerFirefoxShortcutNumber(string selectedItem)
        {
            return this.firefoxContainers.First(x => x.Name == selectedItem).Index;
        }

        private void FirefoxContainersForm_Load(object sender, EventArgs e)
        {
            var firefoxPosition = GetWindowRect(FirefoxHandle);
            this.Location = new Point(firefoxPosition.Left + 185, firefoxPosition.Top + 100);
            this.filterTextBox.Focus();
            this.filterTextBox.KeyDown += FilterTextBox_KeyDown;
            this.filterTextBox.TextChanged += FilterTextBox_TextChanged;
            this.ContainersListBox.DataSource = this.firefoxContainerNames;
        }

        private void FilterTextBox_TextChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void MoveInList(int position)
        {
            var currentCount = this.ContainersListBox.Items.Count;
            if (currentCount > 0)
            {
                var newIndex = this.ContainersListBox.SelectedIndex + position;
                if (newIndex >= 0 && newIndex < currentCount)
                {
                    this.ContainersListBox.SelectedIndex = newIndex;
                }
            }
        }

        private void FilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                SetForegroundWindow(this.FirefoxHandle);
            }
            else if (e.KeyCode == Keys.Down)
            {
                MoveInList(1);
            }
            else if (e.KeyCode == Keys.Up)
            {
                MoveInList(-1);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                OpenSelectedContainer();
            }
        }

        private void SendFirefoxShortcut(int ind)
        {
            // PostKeyMessage did not work here so using SendKeys.Send
            SendKeys.Send($"^(+({ind}))");
        }

        private void OpenSelectedContainer()
        {
            if (this.ContainersListBox.Items.Count > 0)
            {
                var selectedContainer = (string)this.ContainersListBox.SelectedItem;
                var firefoxInd = SelectedContainerFirefoxShortcutNumber(selectedContainer);

                SetForegroundWindow(this.FirefoxHandle);
                SendFirefoxShortcut(firefoxInd);
                
                this.Close();
            }
        }

        private void RefreshList()
        {
            var filterText = this.filterTextBox.Text;
            var items = this.ContainersListBox.Items.Cast<string>().ToList();
            var newItems = firefoxContainerNames.Where(x => x.Contains(filterText, StringComparison.CurrentCultureIgnoreCase)).ToList();
            var sameList = Enumerable.SequenceEqual(items, newItems);

            if (!sameList)
            {
                var selectedItem = (string)this.ContainersListBox.SelectedItem;
                this.ContainersListBox.DataSource = newItems;
                this.ContainersListBox.ClearSelected();
                if (newItems.Contains(selectedItem))
                {
                    this.ContainersListBox.SelectedItem = selectedItem;
                }
                else
                {
                    if (newItems.Count > 0)
                    {
                        this.ContainersListBox.SelectedIndex = 0;
                    }
                }
            }
        }
    }
}
