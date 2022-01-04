namespace SharpStatistics.EasyERAddIn.Functions.Controls
{
    /// <summary>
    /// Simple object for checked item in function controls list
    /// </summary>
    public class VariableCheckedListItem
    {
        /// <summary>
        /// Display Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is Checked
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="check"></param>
        public VariableCheckedListItem(string name,bool check)
        {
            Name = name;
            IsChecked = check;
        }
    }
}
