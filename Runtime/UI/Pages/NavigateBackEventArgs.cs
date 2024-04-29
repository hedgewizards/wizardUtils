namespace WizardUtils.UI.Pages
{
    public class NavigateBackEventArgs
    {
        public NavigateBackEventArgs(bool instant = false)
        {
            Instant = instant;
        }

        public bool Instant { get; private set; }
    }
}
