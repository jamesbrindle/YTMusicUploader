using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Put any control you don't want to be picked up by the dirty tracker here
    /// </summary>
    public static class DirtyTrackerControlIgnoreList
    {
        public static readonly string[] ControlNames =
        {
            "RollBack",
        };
    }

    /// <summary>
    /// Tracks control changes. Good if you want to be able to tell if something needs saving or not - It fires on the changing event of the control without
    /// taking into account the value of say a text box... Use the 'ValueDrivenDirtyTracker' instead to enable that option
    /// </summary>
    public class EventDrivenDirtyTracker
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly Form _frmTracked;
#pragma warning restore IDE0052 // Remove unread private members

        public EventDrivenDirtyTracker(Form frm)
        {
            _frmTracked = frm;
            AssignHandlersForControlCollection(frm.Controls);
        }

        // property denoting whether the tracked form is clean or dirty
        public bool IsDirty { get; set; }

        // methods to make dirty or clean
        public void SetAsDirty()
        {
            IsDirty = true;
        }

        public void SetAsClean()
        {
            IsDirty = false;
        }

        // event handlers
        private void DirtyTracker_ValueChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        // recursive routine to inspect each control and assign handlers accordingly
        private void AssignHandlersForControlCollection(
            Control.ControlCollection coll)
        {
            foreach (Control c in coll)
            {
                // don't bother is control is part of the 'ignore' list
                if (!DirtyTrackerControlIgnoreList.ControlNames.Where(n => n == c.Name).Any())
                {
                    // Text Boxes

                    if (c is TextBox)
                    {
                        (c as TextBox).TextChanged
                            += DirtyTracker_ValueChanged;
                    }

                    // Check Boxes

                    if (c is CheckBox)
                    {
                        (c as CheckBox).CheckedChanged
                            += DirtyTracker_ValueChanged;
                    }

                    // Radio Buttons

                    if (c is RadioButton)
                    {
                        (c as RadioButton).CheckedChanged
                            += DirtyTracker_ValueChanged;
                    }

                    // Combo Boxes

                    if (c is ComboBox)
                    {
                        (c as ComboBox).SelectedIndexChanged
                            += DirtyTracker_ValueChanged;
                    }

                    // Data grid view

                    if (c is DataGridView)
                    {
                        (c as DataGridView).CellValueChanged
                            += DirtyTracker_ValueChanged;
                    }

                    // List view

                    if (c is ListView)
                    {
                        (c as ListView).ItemSelectionChanged
                            += DirtyTracker_ValueChanged;
                    }

                    if (c is ListView)
                    {
                        (c as ListView).SelectedIndexChanged
                            += DirtyTracker_ValueChanged;
                    }

                    // ... apply for other desired input types similarly ...

                    // recurively apply to inner collections
                    if (c.HasChildren)
                    {
                        AssignHandlersForControlCollection(c.Controls);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Tracks control changes. Good if you want to be able to tell if something needs saving or not - It fires on the changing event while also taking into account
    /// the initial values (of say a text box)
    /// </summary>
    public class ValueDrivenDirtyTracker
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly Form _frmTracked;
#pragma warning restore IDE0052 // Remove unread private members
        private ControlDirtyTrackerCollection _controlsTracked;

        // property denoting whether the tracked form is clean or dirty;
        // used if the full list of dirty controls isn't necessary
        public bool IsDirty
        {
            get
            {
                List<Control> dirtyControls
                    = _controlsTracked.GetListOfDirtyControls();

                return (dirtyControls.Count > 0);
            }
        }


        // public method for accessing the list of currently
        // "dirty" controls
        public List<Control> GetListOfDirtyControls()
        {
            return _controlsTracked.GetListOfDirtyControls();
        }


        // establish the form as "clean" with whatever current
        // control values exist
        public void SetAsClean()
        {
            _controlsTracked.MarkAllControlsAsClean();
        }


        // initialize in the constructor by assigning controls to track
        public ValueDrivenDirtyTracker(Form frm)
        {
            _frmTracked = frm;
            _controlsTracked = new ControlDirtyTrackerCollection(frm);
        }
    }

    public class ControlDirtyTracker
    {
        private readonly Control _control;
        private string _cleanValue;

        // read only properties
        public Control Control { get { return _control; } }
        public string CleanValue { get { return _cleanValue; } }

        // constructor establishes the control and uses its current 
        // value as "clean"
        public ControlDirtyTracker(Control ctl)
        {
            // if the control type is not one that is supported, 
            // throw an exception
            if (ControlDirtyTracker.IsControlTypeSupported(ctl))
            {
                _control = ctl;
                _cleanValue = GetControlCurrentValue();
            }
            else
            {
                throw new NotSupportedException(
                      string.Format(
                       "The control type for '{0}' "
                         + "is not supported by the ControlDirtyTracker class."
                        , ctl.Name)
                      );
            }
        }


        // static class utility method; return whether or not the control type 
        // of the given control is supported by this class;
        // developers may modify this to extend support for other types
        public static bool IsControlTypeSupported(Control ctl)
        {
            // list of types supported
            if (ctl is TextBox)
            {
                return true;
            }

            if (ctl is CheckBox)
            {
                return true;
            }

            if (ctl is RadioButton)
            {
                return true;
            }

            if (ctl is ComboBox)
            {
                return true;
            }

            if (ctl is ListBox)
            {
                return true;
            }

            if (ctl is DataGridView)
            {
                return true;
            }

            if (ctl is ListView)
            {
                return true;
            }

            // ... add additional types as desired ...

            // not a supported type
            return false;
        }


        // private method to determine the current value (as a string) 
        // of the control;
        // developers may modify this to extend support for other types
        private string GetControlCurrentValue()
        {
            if (_control is TextBox)
            {
                return (_control as TextBox).Text;
            }

            if (_control is CheckBox)
            {
                return (_control as CheckBox).Checked.ToString();
            }

            if (_control is ComboBox)
            {
                return (_control as ComboBox).Text;
            }

            if (_control is RadioButton)
            {
                return (_control as RadioButton).Checked.ToString();
            }

            if (_control is ListBox)
            {
                // for a listbox, create a list of the selected indexes
                StringBuilder val = new StringBuilder();
                ListBox lb = (_control as ListBox);
                ListBox.SelectedIndexCollection coll = lb.SelectedIndices;

                for (int i = 0; i < coll.Count; i++)
                {
                    val.AppendFormat("{0};", coll[i]);
                }

                return val.ToString();
            }

            if (_control is DataGridView view)
            {
                // for a listbox, create a list of the selected indexes
                StringBuilder sb = new StringBuilder();
                foreach (DataGridViewRow row in view.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        sb.Append(cell.Value);
                        sb.Append(',');
                    }
                    sb.Remove(sb.Length - 1, 1); // Removes the last delimiter 
                    sb.AppendLine();
                }

                return sb.ToString();
            }

            if (_control is ListView view1)
            {
                // for a listbox, create a list of the selected indexes
                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem row in view1.Items)
                {
                    sb.Append(row.Text);
                    sb.Append(',');

                    sb.Remove(sb.Length - 1, 1); // Removes the last delimiter 
                    sb.AppendLine();
                }

                return sb.ToString();
            }

            // ... add additional types as desired ...

            return "";
        }

        // method to establish the the current control value as "clean"
        public void EstablishValueAsClean()
        {
            _cleanValue = GetControlCurrentValue();
        }

        // determine if the current control value is considered "dirty"; 
        // i.e. if the current control value is different than the one
        // remembered as "clean"
        public bool DetermineIfDirty()
        {
            // compare the remembered "clean value" to the current value;
            // if they are the same, the control is still clean;
            // if they are different, the control is considered dirty.
            return (
              string.Compare(
                _cleanValue, GetControlCurrentValue(), false
                ) != 0
            );
        }
    }

    public class ControlDirtyTrackerCollection : List<ControlDirtyTracker>
    {
        // constructors
        public ControlDirtyTrackerCollection() : base() { }
        public ControlDirtyTrackerCollection(Form frm) : base()
        {
            // initialize to the controls on the passed in form
            AddControlsFromForm(frm);
        }

        // utility method to add the controls from a Form to this collection
        public void AddControlsFromForm(Form frm)
        {
            AddControlsFromCollection(frm.Controls);
        }

        // recursive routine to inspect each control and add to the 
        // collection accordingly
        public void AddControlsFromCollection(Control.ControlCollection coll)
        {
            foreach (Control c in coll)
            {
                if (!DirtyTrackerControlIgnoreList.ControlNames.Where(n => n == c.Name).Any())
                {

                    // if the control is supported for dirty tracking, add it
                    if (ControlDirtyTracker.IsControlTypeSupported(c))
                    {
                        this.Add(new ControlDirtyTracker(c));
                    }

                    // recurively apply to inner collections
                    if (c.HasChildren)
                    {
                        AddControlsFromCollection(c.Controls);
                    }
                }
            }
        }

        // loop through all controls and return a list of those that are dirty
        public List<Control> GetListOfDirtyControls()
        {
            List<Control> list = new List<Control>();

            foreach (ControlDirtyTracker c in this)
            {
                if (c.DetermineIfDirty())
                {
                    list.Add(c.Control);
                }
            }

            return list;
        }

        // mark all the tracked controls as clean
        public void MarkAllControlsAsClean()
        {
            foreach (ControlDirtyTracker c in this)
            {
                c.EstablishValueAsClean();
            }
        }
    }
}