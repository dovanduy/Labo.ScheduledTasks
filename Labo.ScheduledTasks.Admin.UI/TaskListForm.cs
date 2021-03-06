﻿namespace Labo.ScheduledTasks.Admin.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Labo.Mvp.Core.View;
    using Labo.ScheduledTasks.Admin.Presentation.Presenters;
    using Labo.ScheduledTasks.Admin.Presentation.Views;
    using Labo.ScheduledTasks.Core.Model;

    public partial class TaskListForm : Form, ITaskListView
    {
        private IList<ScheduledTask> m_ScheduledTasks;

        public TaskListPresenter Presenter { get; set; }

        public IView ParentView { get; set; }

        object IView.Presenter
        {
            get { return Presenter; }
            set { Presenter = value as TaskListPresenter; }
        }

        public string Caption
        {
            get { return Text; }
            set { Text = value; }
        }

        public bool TaskServiceRunning
        {
            set
            {
                RefreshTaskServiceButtons(value);
            }
        }

        public TaskListForm()
        {
            InitializeComponent();

            dgvTasks.CellDoubleClick += dgvTasks_CellDoubleClick;
        }

        private void dgvTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                IList<ScheduledTask> tasks = scheduleTaskBindingSource.DataSource as IList<ScheduledTask>;
                if (tasks != null && e.RowIndex < tasks.Count)
                {
                    Presenter.OpenEditTaskView(tasks[e.RowIndex].Id);
                }
            }
        }

        public IList<ScheduledTask> ScheduledTasks
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("value");
                }

                m_ScheduledTasks = value;

                scheduleTaskBindingSource.ResetBindings(false);
                scheduleTaskBindingSource.DataSource = m_ScheduledTasks;
            }
        }

        public void OnLoad()
        {
            Presenter.LoadScheduledTaskList();
        }

        public void InitView()
        {
            Presenter.LoadScheduledTaskList();
        }

        private void btnStartTaskService_Click(object sender, EventArgs e)
        {
            Presenter.StartTaskService();
        }

        private void btnStopTaskService_Click(object sender, EventArgs e)
        {
            Presenter.StopTaskService();
        }

        private void RefreshTaskServiceButtons(bool taskServiceIsRunning)
        {
            btnStartTaskService.Enabled = !taskServiceIsRunning;
            btnStopTaskService.Enabled = taskServiceIsRunning;
        }

        private void tmrTaskList_Tick(object sender, EventArgs e)
        {
            Presenter.LoadScheduledTaskList();
        }
    }
}
