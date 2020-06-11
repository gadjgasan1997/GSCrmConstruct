namespace GSCrmLibrary.Configuration
{
    public class Table
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public string BaseType { get; set; }
    }

    public class BusinessComponent
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public string BaseType { get; set; }
    }

    public class Applet
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public string BaseType { get; set; }
    }

    public class DataBUSFactory
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public string BaseType { get; set; }
    }

    public class BUSUIFactory
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public string BaseType { get; set; }
    }

    public class AppletController
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public string BaseType { get; set; }
    }

    public class BusinessComponentController
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public string BaseType { get; set; }
    }

    public class ScreenController
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public object BaseType { get; set; }
    }

    public class ViewCshtml
    {
        public string Path { get; set; }
        public object Namespace { get; set; }
        public object BaseType { get; set; }
    }

    public class Dll
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public object BaseType { get; set; }
    }

    public class Migration
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public object BaseType { get; set; }
    }

    public class Context
    {
        public string Path { get; set; }
        public string Namespace { get; set; }
        public object BaseType { get; set; }
    }

    public class EntitiesConfigModel
    {
        public Table Table { get; set; }
        public BusinessComponent BusinessComponent { get; set; }
        public Applet Applet { get; set; }
        public DataBUSFactory DataBUSFactory { get; set; }
        public BUSUIFactory BUSUIFactory { get; set; }
        public AppletController AppletController { get; set; }
        public BusinessComponentController BusinessComponentController { get; set; }
        public ScreenController ScreenController { get; set; }
        public ViewCshtml ViewCshtml { get; set; }
        public Dll Dll { get; set; }
        public Migration Migration { get; set; }
        public Context Context { get; set; }
    }
}
