namespace TnCode.TnCodeApp.Data.Events
{
    public enum DataSetChange
    {
        Added,
        AddedFromR,
        Removed,
        Updated
    }

    public class DataSetEventArgs
    {
        public IDataSet Data { get; set; }

        public DataSetChange Modification { get; set; }
    }
}
