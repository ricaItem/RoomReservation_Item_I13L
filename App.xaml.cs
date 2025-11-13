namespace RoomReservation_Item_I13L
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "RoomReservation_Item_I13L" };
        }
    }
}
