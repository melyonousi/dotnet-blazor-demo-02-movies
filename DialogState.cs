namespace Movies
{
    public class DialogState
    {
        public bool IsDialogOpen { get; private set; }

        public event Action OnChange;

        public void ShowDialog()
        {
            IsDialogOpen = true;
            NotifyStateChanged();
        }

        public void CloseDialog()
        {
            IsDialogOpen = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
