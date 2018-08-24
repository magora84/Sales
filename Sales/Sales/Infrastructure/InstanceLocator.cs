
namespace Sales.Infrastructure
{
    using ViewModels;
    
    //la clase locator instancia la MainViewModel
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator() {
            this.Main = new MainViewModel();
        }
    }
}
